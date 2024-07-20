using System.ComponentModel.DataAnnotations;

namespace LocalLens.WebApi.Constants.Configurations;

public class GoogleAuthConfig
{
    public const string Key = "GoogleAuthSettings";

    [Required]
    public string[] AppIds { get; set; }
}
