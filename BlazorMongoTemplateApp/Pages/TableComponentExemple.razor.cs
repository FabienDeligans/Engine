using System;
using System.Collections.Generic;
using System.Linq;
using BlazorMongoTemplateApp.Component;

namespace BlazorMongoTemplateApp.Pages
{
    public partial class TableComponentExemple
    {
        private TableComponent<Truc> ChildComponentTruc { get; set; }
        public List<Truc> CustomList { get; set; }

        protected override void OnInitialized()
        {
            CustomList = new List<Truc>();
            for (var i = 0; i < 500; i++)
            {
                CustomList.Add(new Truc()
                {
                    Nombre = new Random().Next(0, 11),
                    Name = RandomString(10),
                });
            }
        }

        private static readonly Random Random = new();

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
