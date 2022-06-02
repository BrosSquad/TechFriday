using System.ComponentModel.DataAnnotations;

namespace FastEndpoints.Example.Options;

public class MongoOptions
{
  public const string Section = "Mongo";

  [Required]
  public string Host {get;set;} = "localhost";

  [Required]
  public string AppName {get;set;} = default!;

  [Required]
  public int Port {get;set;} = 27017;

  [Required]
  [Range(10, 1024)]
  public int ConnectTimeoutMs {get;set;} = 300;

  [Required]
  public int MaxPoolSize {get;set;} = 200;

  [Required]
  public int MinPoolSize {get;set;} = 4;

  [Required]
  public int MaxIdleTimeMs {get;set;} = 300;

  public string ConnectionString
  {
    get => $"mongodb://{Host}:{Port}/?connectTimeoutMS={ConnectTimeoutMs}&maxPoolSize={MaxPoolSize}&minPoolSize={MinPoolSize}&maxIdleTimeMS={MaxIdleTimeMs}&appName={AppName}";
  }
}
