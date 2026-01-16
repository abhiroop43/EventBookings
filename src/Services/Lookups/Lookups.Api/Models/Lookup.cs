using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Lookups.Api.Models;

public class Lookup
{
    [BsonId] public ObjectId Id { get; set; }

    [BsonElement("key")] public string Key { get; set; }

    [BsonElement("value")] public string Value { get; set; }

    [BsonElement("type")] public string Type { get; set; }

    [BsonElement("children")] public List<Lookup> Children { get; set; } = [];
}