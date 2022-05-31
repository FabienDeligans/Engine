using System;
using Engine.CustomAttribute;
using Engine.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorMongoTemplateApp.Models;

public class ChatMessage : Entity
{
    [BsonId(IdGenerator = typeof(IdGenerator<ChatMessage>))]
    public override string Id { get; set; }
    
    [ForeignKey(typeof(User))]
    public string FromUserId { get; set; }
    
    [BsonIgnore]
    public User FromUser { get; set; }

    [ForeignKey(typeof(User))]
    public string ToUserId { get; set; }
    
    [BsonIgnore]
    public User ToUser { get; set; }

    public string Message { get; set; }
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime DateTime { get; set; }
}

public class User : Entity
{
    [BsonId(IdGenerator = typeof(IdGenerator<User>))]
    public override string Id { get; set; }

    public string UserName { get; set; }

}