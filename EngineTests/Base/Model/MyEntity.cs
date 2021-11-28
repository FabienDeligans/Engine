using System;
using Engine.Model;

namespace EngineTests.Base.Model
{
    public class MyEntity : Entity
    {
        public string? Name { get; set; }
        public int Numeric { get; set; }
        public DateTime DateTime { get; set; }
    }
}
