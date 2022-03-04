using Engine.Model;

namespace BlazorMongoTemplateApp.Models
{
    public class Picture : Entity
    {
        public string Name { get; set; }
        public string File { get; set; }
    }
}
