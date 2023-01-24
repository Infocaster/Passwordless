using System;

namespace Infocaster.Umbraco.Passwordless.Client.Models;

public class VerifyResponse
{
    public bool success { get; set; }
    public string userId { get; set; } = null!;
    public DateTime timestamp { get; set; }
    public string rpid { get; set; } = null!;
    public string origin { get; set; } = null!;
    public string device { get; set; } = null!;
    public string country { get; set; } = null!;
    public string? nickname { get; set; }
    public DateTime expiresAt { get; set; }
}
