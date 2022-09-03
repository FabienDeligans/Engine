using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorMongoTemplateApp.Pages
{
    public partial class TestKanban
    {
        private List<Article> Articles { get; set; }
        private List<ArticleEmplacement> ArticleEmplacements { get; set; }
        private List<string> Cols { get; set; } = new List<string>();
        private List<int> Rows { get; set; } = new List<int>();
        private List<int> Positions { get; set; } = new List<int>();
        private string NewCol { get; set; }

        protected override void OnInitialized()
        {
            GenerateData();
            BuildCols();
            BuildRows();
            BuildPosition();
        }

        private void BuildPosition()
        {
            Positions.AddRange(new[] { 1, 2, 3 });
        }

        private void BuildRows()
        {
            Rows.AddRange(new[] { 1, 2, 3, 4, 5 });
        }

        private void BuildCols()
        {
            Cols.AddRange(new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD" });
        }

        private static readonly Random Random = new();
        private static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        private void GenerateData()
        {
            Articles = new List<Article>();
            for (var i = 1; i < 200; i++)
            {
                Articles.Add(new Article
                {
                    IdArticle = "ID" + i,
                    Libelle = RandomString(5)
                });
            }
        }


        private void AddNewCol()
        {
            if (!string.IsNullOrEmpty(NewCol))
            {
                Cols.Add(NewCol);
            }

            NewCol = "";
            StateHasChanged();
        }
    }

    public class ArticleEmplacement
    {
        public string IdArticle { get; set; }
        public string Emplacement { get; set; }
    }
    public class Article
    {
        public string IdArticle { get; set; }
        public string Libelle { get; set; }

    }
}
