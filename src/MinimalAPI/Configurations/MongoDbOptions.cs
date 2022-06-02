using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Configurations;

public class MongoDbOptions
{
    public const string Section = "MongoDb";

    [Required]
    public string Database {get; set;} = default!;

    [Required]
    public string Host {get; set;} = default!;
}
