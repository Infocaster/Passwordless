using System.ComponentModel.DataAnnotations;

namespace Infocaster.Umbraco.Passwordless.Client.Models;

public class VerifyRequest
{
    [Required]
    public string token { get; set; } = null!;
}
