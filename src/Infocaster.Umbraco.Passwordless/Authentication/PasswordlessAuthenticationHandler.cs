using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Infocaster.Umbraco.Passwordless.Client;
using Infocaster.Umbraco.Passwordless.Client.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Web.BackOffice.Security;
using Umbraco.Extensions;

namespace Infocaster.Umbraco.Passwordless.Authentication;

public class PasswordlessAuthenticationHandler
    : RemoteAuthenticationHandler<PasswordlessAuthenticationSchemeOptions>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPasswordlessEndpoints _client;
    private readonly IBackOfficeSignInManager _backOfficeSignInManager;
    private readonly IBackOfficeUserManager _backOfficeUserManager;
    private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;
    private readonly LinkGenerator _linkGenerator;

    public PasswordlessAuthenticationHandler(IOptionsMonitor<PasswordlessAuthenticationSchemeOptions> options,
                                             ILoggerFactory logger,
                                             UrlEncoder encoder,
                                             ISystemClock clock,
                                             IHttpContextAccessor httpContextAccessor,
                                             IPasswordlessEndpoints client,
                                             IBackOfficeSignInManager backOfficeSignInManager,
                                             IBackOfficeUserManager backOfficeUserManager,
                                             IBackOfficeSecurityAccessor backOfficeSecurityAccessor,
                                             LinkGenerator linkGenerator)
        : base(options, logger, encoder, clock)
    {
        _httpContextAccessor = httpContextAccessor;
        _client = client;
        _backOfficeSignInManager = backOfficeSignInManager;
        _backOfficeUserManager = backOfficeUserManager;
        _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
        _linkGenerator = linkGenerator;
    }

    protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
    {
        var httpContext = _httpContextAccessor.GetRequiredHttpContext();
        var token = httpContext.Request.Query["token"].FirstOrDefault();

        if (token is null) return HandleRequestResult.NoResult();

        var response = await _client.VerifyAsync(new VerifyRequest
        {
            token = token
        });

        if (!response.Success)
        {
            return HandleRequestResult.Fail(response.Exception);
        }

        if (response.Value.success)
        {
            const string AuthenticationScheme = "Umbraco." + PasswordlessLoginProviderOptions.SchemeName;

            var currentUser = _backOfficeSecurityAccessor.BackOfficeSecurity?.CurrentUser;

            var originalReturnUrl = httpContext.Request.Query["returnUrl"].FirstOrDefault() ?? "/umbraco";
            var returnUrl = _linkGenerator.GetPathByAction(nameof(PasswordlessController.FinishLogin), "Passwordless", new { returnUrl = originalReturnUrl });
            
            var identityUser = await _backOfficeUserManager.FindByIdAsync(response.Value.userId);

            var properties = _backOfficeSignInManager.ConfigureExternalAuthenticationProperties(AuthenticationScheme, returnUrl, response.Value.userId);
            var principal = await _backOfficeSignInManager.CreateUserPrincipalAsync(identityUser);

            var ticket = new AuthenticationTicket(principal, properties, Constants.Security.BackOfficeExternalAuthenticationType);

            return HandleRequestResult.Success(ticket);
        }

        return HandleRequestResult.NoResult();
    }
}
