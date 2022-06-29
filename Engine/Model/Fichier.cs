using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace Engine.Model
{
    public class Fichier 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public byte[] DataBytes { get; set; }
    }
}
