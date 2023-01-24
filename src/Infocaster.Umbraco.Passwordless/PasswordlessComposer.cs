using System;
using Infocaster.Umbraco.Passwordless.Authentication;
using Infocaster.Umbraco.Passwordless.Client;
using Infocaster.Umbraco.Passwordless.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Extensions;

namespace Infocaster.Umbraco.Passwordless;

public class PasswordlessComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.ConfigureOptions<PasswordlessLoginProviderOptions>();

        builder.AddBackOfficeExternalLogins(logins =>
        {
            logins.AddBackOfficeLogin(authBuilder =>
            {
                authBuilder.AddRemoteScheme<PasswordlessAuthenticationSchemeOptions, PasswordlessAuthenticationHandler>(authBuilder.SchemeForBackOffice(PasswordlessLoginProviderOptions.SchemeName)!, "Passwordless login", pwlOptions =>
                {
                    pwlOptions.CallbackPath = new PathString("/umbraco-pwl-login");
                });
            });
        });

        builder.BackOfficeAssets()
            .Append<PasswordlessMainScript>()
            .Append<PasswordlessLoginScript>()
            .Append<PasswordlessRegisterScript>()
            .Append<PasswordlessServiceScript>();

        builder.Services.AddHttpClient<PasswordlessClient>()
            .ConfigureHttpClient((sp, client) =>
            {
                client.BaseAddress = new Uri("https://apiv2.passwordless.dev");
            });
        builder.Services.AddSingleton<IPasswordlessClient>(sp => sp.GetRequiredService<PasswordlessClient>());
        builder.Services.AddSingleton<IPasswordlessEndpoints, PasswordlessEndpoints>();

        builder.Services.AddOptions<PasswordlessOptions>()
            .Bind(builder.Config.GetSection("Passwordless"))
            .ValidateDataAnnotations();
    }
}
