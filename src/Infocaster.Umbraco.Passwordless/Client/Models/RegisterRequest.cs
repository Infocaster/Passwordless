using System.ComponentModel.DataAnnotations;

namespace Infocaster.Umbraco.Passwordless.Client.Models;

public class RegisterRequest
{
    [Required]
    public int? userId { get; set; }

    [Required]
    public string? username { get; set; }

    [Required]
    public string? displayName { get; set; }
}
