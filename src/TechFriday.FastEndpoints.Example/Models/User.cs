using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FastEndpoints.Example.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;

    [BsonElement("firstName")]
    public string FirstName { get; init; } = default!;

    [BsonElement("lastName")]
    public string LastName { get; init; } = default!;

    [BsonElement("email")]
    public string Email { get; init; } = default!;

    [BsonElement("role")]
    public string Role { get; init; } = default!;

    [BsonElement("password")]
    [JsonIgnore]
    public string Password { get; init; } = default!;
}
