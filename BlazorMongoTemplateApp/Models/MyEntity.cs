using Engine.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using BlazorMongoTemplateApp.Models.CustomAttribute;
using BlazorMongoTemplateApp.Pages;
using Microsoft.AspNetCore.Components.Forms;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorMongoTemplateApp.Models
{
    /// <summary>
    /// Entity class Test
    /// </summary>
    public class MyEntity : Entity
    {
        //[Required]
        //[Range(0.0, 10.0, ErrorMessage = "Value must be between {1} and {2}")]
        [BsonElement("numeric")]
        public int Numeric { get; set; }
        
        //[Required]
        //[MaxLength(10)]
        //[IsUnique]
        [BsonElement("data")]
        public string Data { get; set; }

        [BsonElement("now")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Now { get; set; }

        public IBrowserFile Fichier { get; set; }

        public byte[] DataBytes { get; set; }
    }
}
