using Engine.Model;
using System;

namespace BlazorMongoTemplateApp.Models
{
    /// <summary>
    /// Entity class Test
    /// </summary>
    public class MyEntity : Entity
    {
        public int Numeric { get; set; }
        public string Data { get; set; }
        public DateTime Now { get; set; }
    }
}
