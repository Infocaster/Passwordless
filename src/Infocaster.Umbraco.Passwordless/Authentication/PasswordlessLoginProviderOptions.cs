using Microsoft.Extensions.Options;

using Umbraco.Cms.Web.BackOffice.Security;

namespace Infocaster.Umbraco.Passwordless.Authentication;

public class PasswordlessLoginProviderOptions
    : IConfigureNamedOptions<BackOfficeExternalLoginProviderOptions>
{
    public const string SchemeName = "Passwordless";

    public void Configure(string name, BackOfficeExternalLoginProviderOptions options)
    {
        if (name != "Umbraco." + SchemeName)
        {
            return;
        }

        Configure(options);
    }

    public void Configure(BackOfficeExternalLoginProviderOptions options)
    {
        options.CustomBackOfficeView = "~/App_Plugins/Passwordless/main.html";
        options.AutoLinkOptions = new ExternalSignInAutoLinkOptions();
    }
}
