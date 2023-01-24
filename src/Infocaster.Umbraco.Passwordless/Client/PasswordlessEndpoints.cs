using System.Net.Http;
using System.Threading.Tasks;
using Infocaster.Umbraco.Passwordless.Client.Models;

namespace Infocaster.Umbraco.Passwordless.Client;

public interface IPasswordlessEndpoints
{
    Task<PasswordlessResult<string>> RegisterAsync(RegisterRequest request);
    Task<PasswordlessResult<VerifyResponse>> VerifyAsync(VerifyRequest request);
}

public class PasswordlessEndpoints : IPasswordlessEndpoints
{
    private readonly IPasswordlessClient _client;

    public PasswordlessEndpoints(IPasswordlessClient client)
    {
        _client = client;
    }

    public Task<PasswordlessResult<string>> RegisterAsync(RegisterRequest request)
    {
        return _client.SendWithResultAsync("/register/token", HttpMethod.Post, new JsonRequestStringResultSerializer<RegisterRequest>(), request);
    }

    public Task<PasswordlessResult<VerifyResponse>> VerifyAsync(VerifyRequest request)
    {
        return _client.SendWithResultAsync("/signin/verify", HttpMethod.Post, CreateSerializer<VerifyRequest, VerifyResponse>(), request);
    }

    private IRequestSerializer<TRequest> CreateSerializer<TRequest>() => new JsonRequestSerializer<TRequest>();
    private IRequestSerializer<TRequest, TResponse> CreateSerializer<TRequest, TResponse>() => new JsonRequestSerializer<TRequest, TResponse>();
}
