using System.ComponentModel.DataAnnotations;

namespace Infocaster.Umbraco.Passwordless.Configuration;

public class PasswordlessOptions
{
    [Required]
    public string PrivateKey { get; set; } = null!;

    [Required]
    public string PublicKey { get; set; } = null!;
}
