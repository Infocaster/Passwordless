using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Infocaster.Umbraco.Passwordless.Client.Models;
using Newtonsoft.Json;

namespace Infocaster.Umbraco.Passwordless.Client;


// For requests without response
public interface IRequestSerializer<in TRequest>
{
    Task<HttpContent> SerializeRequestAsync(TRequest request);
    Task<object?> DeserializeErrorAsync(HttpStatusCode statusCode, string body);
}

// for requests with response
public interface IRequestSerializer<in TRequest, TResponse>
    : IRequestSerializer<TRequest>
{
    Task<TResponse> DeserializeResponseAsync(string body);
}

public class JsonRequestSerializer<TRequest>
    : IRequestSerializer<TRequest>
{
    public Task<object?> DeserializeErrorAsync(HttpStatusCode statusCode, string body)
    {
        return Task.FromResult<object?>(JsonConvert.DeserializeObject<PasswordlessError>(body));
    }

    public Task<HttpContent> SerializeRequestAsync(TRequest request)
    {
        // Hacky fix: standard media type header is not recognised by OneWelcome, so set it explicitly
        string body = JsonConvert.SerializeObject(request, new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Formatting = Formatting.Indented
        });
        StringContent result = new(body);
        result.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return Task.FromResult<HttpContent>(result);
    }
}

public class JsonRequestSerializer<TRequest, TResponse>
    : JsonRequestSerializer<TRequest>, IRequestSerializer<TRequest, TResponse>
{

    public Task<TResponse> DeserializeResponseAsync(string body)
    {

        return Task.FromResult(JsonConvert.DeserializeObject<TResponse>(body)!);
    }
}

public class JsonRequestStringResultSerializer<TRequest>
    : JsonRequestSerializer<TRequest>, IRequestSerializer<TRequest, string>
{
    public Task<string> DeserializeResponseAsync(string body)
    {
        return Task.FromResult(body);
    }
}