using System;
using System.Threading.Tasks;
using Infocaster.Umbraco.Passwordless.Authentication;
using Infocaster.Umbraco.Passwordless.Client;
using Infocaster.Umbraco.Passwordless.Client.Models;
using Infocaster.Umbraco.Passwordless.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.BackOffice.Security;

namespace Infocaster.Umbraco.Passwordless;

public class PasswordlessApiController
    : UmbracoAuthorizedApiController
{
    private readonly IPasswordlessEndpoints _passwordlessEndpoints;
    private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;
    private readonly IOptionsMonitor<PasswordlessOptions> _options;
    private readonly IBackOfficeUserManager _backOfficeUserManager;

    public PasswordlessApiController(IPasswordlessEndpoints passwordlessEndpoints,
                                     IBackOfficeSecurityAccessor backOfficeSecurityAccessor,
                                     IOptionsMonitor<PasswordlessOptions> options,
                                     IBackOfficeUserManager backOfficeUserManager)
    {
        _passwordlessEndpoints = passwordlessEndpoints;
        _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
        _options = options;
        _backOfficeUserManager = backOfficeUserManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult PublicKey()
    {
        // Only return public key here. Be careful when making changes.
        //    Do not return the private key to the client
        return Ok(_options.CurrentValue.PublicKey);
    }

    [HttpGet]
    public async Task<IActionResult> RegisterAsync()
    {
        var user = _backOfficeSecurityAccessor.BackOfficeSecurity?.CurrentUser;
        if (user is null) throw new InvalidOperationException("Cannot register when no user is logged in");

        var userId = user.Id;
        var userName = user.Username;
        var displayName = user.Name;

        var response = await _passwordlessEndpoints.RegisterAsync(new RegisterRequest
        {
            displayName = displayName,
            userId = userId,
            username = userName,
        });

        return Ok(response.Value);
    }

    [HttpPost]
    public async Task<IActionResult> FinishRegister()
    {
        var user = _backOfficeSecurityAccessor.BackOfficeSecurity?.CurrentUser;
        if (user is null) throw new InvalidOperationException("Cannot register when no user is logged in");

        var backofficeUserIdentity = await _backOfficeUserManager.GetUserAsync(User);

        await _backOfficeUserManager.AddLoginAsync(backofficeUserIdentity, new UserLoginInfo("Umbraco." + PasswordlessLoginProviderOptions.SchemeName,user.Id.ToString() , user.Name));

        return Ok();
    }
}
