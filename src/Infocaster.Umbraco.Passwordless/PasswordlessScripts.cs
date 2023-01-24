using Umbraco.Cms.Core.WebAssets;

namespace Infocaster.Umbraco.Passwordless;

public class PasswordlessMainScript
    : JavaScriptFile
{
    public PasswordlessMainScript()
        : base("/App_Plugins/Passwordless/main.controller.js")
    { }
}

public class PasswordlessLoginScript
    : JavaScriptFile
{
    public PasswordlessLoginScript()
        : base("/App_Plugins/Passwordless/login.directive.js")
    { }
}

public class PasswordlessRegisterScript
    : JavaScriptFile
{
    public PasswordlessRegisterScript()
        : base("/App_Plugins/Passwordless/register.directive.js")
    { }
}

public class PasswordlessServiceScript
    : JavaScriptFile
{
    public PasswordlessServiceScript()
        : base("/App_Plugins/Passwordless/passwordless.service.js")
    { }
}