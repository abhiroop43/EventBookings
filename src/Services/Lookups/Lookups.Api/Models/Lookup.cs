using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Lookups.Api.Models;

public class Lookup
{
    [BsonId] public ObjectId Id { get; set; }

    [BsonElement("key")] public required string Key { get; set; }

    [BsonElement("value")] public required string Value { get; set; }

    [BsonElement("type")] public required string LookupType { get; set; }

    [BsonElement("children")] public List<Lookup> Children { get; set; } = [];

    [BsonElement("createdBy")] public required string CreatedBy { get; set; }
    [BsonElement("createdAt")] public DateTime CreatedAt { get; set; }

    [BsonElement("updatedBy")] public string? UpdatedBy { get; set; }
    [BsonElement("updatedAt")] public DateTime UpdatedAt { get; set; }
}