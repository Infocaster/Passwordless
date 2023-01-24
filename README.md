<h3 align="center">
<img height="100" src="https://raw.githubusercontent.com/Infocaster/.github/main/assets/infocaster_nuget_pink.svg">
</h3>

<h1 align="center">
Passwordless backoffice

[![Downloads](https://img.shields.io/nuget/dt/Infocaster.Umbraco.Passwordless?color=ffc800)](https://www.nuget.org/packages/Infocaster.Umbraco.Passwordless/)
[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Infocaster.Umbraco.Passwordless?color=ff0069)](https://www.nuget.org/packages/Infocaster.Umbraco.Passwordless/)
![GitHub](https://img.shields.io/github/license/Infocaster/Passwordless?color=ffc800)

</h1>

Easy and secure login with your fingerprint or face-id using Passwordless

## Requirements
Passwordless backoffice should work on any version of Umbraco 10 and above.
You need to have an account at [the Passwordless service](https://www.passwordless.dev/). Make sure you copy the credentials and store them in a safe place. You'll only receive them once!

## Getting Started
The package is available via NuGet. Visit [the Passwordless backoffice on NuGet](https://www.nuget.org/packages/Infocaster.Umbraco.Passwordless/) for instructions on how to install the package in your website.
You'll need a few configurations to get the package to work:

## Configuration
These configurations are mandatory before you can use the passwordless backoffice login

```json
{
    "Passwordless": {
        "PrivateKey": "[Enter Your passwordless private key here]",
        "PublicKey": "[Enter your passwordless public key here]",
    }
}
```
Obviously you don't put your secrets directly in your appsettings. Use a secret storage like Environment variables or azure keyvault so you don't have your secrets in source control!

## Contributing
This project is currently in prerelease, so I haven't taken the time to write contribution guidelines. Just let me know in [the Umbraco community discord](https://community.umbraco.com/get-involved/community-discord-channel/) or through any issue if you would like to contribute, then I'll help you get setup!

<a href="https://infocaster.net">
<img align="right" height="200" src="https://raw.githubusercontent.com/Infocaster/.github/main/assets/Infocaster_Corner.png?raw=true">
</a>
