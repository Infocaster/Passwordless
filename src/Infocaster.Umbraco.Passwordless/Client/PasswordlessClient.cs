using System.Net.Http;
using System.Threading.Tasks;
using Infocaster.Umbraco.Passwordless.Client.Models;
using Infocaster.Umbraco.Passwordless.Configuration;
using Microsoft.Extensions.Options;

namespace Infocaster.Umbraco.Passwordless.Client;

public interface IPasswordlessClient
{
    Task<PasswordlessResult> SendWithoutResultAsync<TRequest>(string endpoint, HttpMethod method, IRequestSerializer<TRequest> serializer, TRequest? data = null) where TRequest : class;
    Task<PasswordlessResult<TResponse>> SendWithResultAsync<TRequest, TResponse>(string endpoint, HttpMethod method, IRequestSerializer<TRequest, TResponse> serializer, TRequest? data = null) where TRequest : class;
}

public class PasswordlessClient : IPasswordlessClient
{
    private readonly HttpClient _httpClient;

    protected IOptionsMonitor<PasswordlessOptions> Options { get; }

    public PasswordlessClient(HttpClient httpClient, IOptionsMonitor<PasswordlessOptions> options)
    {
        _httpClient = httpClient;
        Options = options;
    }

    /// <inheritdoc />
    public Task<PasswordlessResult<TResponse>> SendWithResultAsync<TRequest, TResponse>(string endpoint, HttpMethod method, IRequestSerializer<TRequest, TResponse> serializer, TRequest? data = null)
        where TRequest : class
    {
        return PasswordlessResult.ExecuteSafelyAsync(async () =>
        {
            HttpContent? body = await SerializeRequestBodyAsync(serializer, data);
            var response = await SendAsync(endpoint, method, body);
            await EnsureSuccessAsync(serializer, response);

            return await serializer.DeserializeResponseAsync(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        });
    }

    /// <inheritdoc />
    public Task<PasswordlessResult> SendWithoutResultAsync<TRequest>(string endpoint, HttpMethod method, IRequestSerializer<TRequest> serializer, TRequest? data = null)
        where TRequest : class
    {
        return PasswordlessResult.ExecuteSafelyAsync(async () =>
        {
            HttpContent? body = await SerializeRequestBodyAsync(serializer, data);
            var response = await SendAsync(endpoint, method, body);
            await EnsureSuccessAsync(serializer, response);
        });
    }

    private static async Task EnsureSuccessAsync<TRequest>(IRequestSerializer<TRequest> serializer, HttpResponseMessage response) where TRequest : class
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorModel = await serializer.DeserializeErrorAsync(response.StatusCode, await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            throw new PasswordlessException("Response indicates failure.", errorModel);
        }
    }

    private static async Task<HttpContent?> SerializeRequestBodyAsync<TRequest>(IRequestSerializer<TRequest> serializer, TRequest? data)
        where TRequest : class
    {
        HttpContent? body = null;
        if (data != null) body = await serializer.SerializeRequestAsync(data);
        return body;
    }

    private async Task<HttpResponseMessage> SendAsync(string endpoint, HttpMethod method, HttpContent? body)
    {
        var message = CreateBaseRequest(endpoint, method, body);
        message.Headers.Add("ApiSecret", Options.CurrentValue.PrivateKey);

        var response = await _httpClient.SendAsync(message, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
        return response;
    }

    private static HttpRequestMessage CreateBaseRequest(string endpoint, HttpMethod method, HttpContent? body)
    {
        var message = new HttpRequestMessage(method, endpoint);
        if (body != null)
        {
            message.Content = body;
        }

        return message;
    }
}
