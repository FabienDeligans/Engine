using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using BlazorMongoTemplateApp.Database;
using BlazorMongoTemplateApp.Models.TestModelCommande;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp;
using PdfSharp.Charting;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using Document = MigraDoc.DocumentObjectModel.Document;
using VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment;


namespace BlazorMongoTemplateApp.Pages
{
    public partial class PDF_Test
    {

        private string Text { get; set; } = Faker.Lorem.Sentence(200);

        public Dictionary<string, List<string>> Datas { get; set; }

        public List<Commande> Commandes { get; set; }

        #region Build Fake Datas
        private void GenerateFakeDatas()
        {
            using var context = ContextFactory.MakeContext();

            var depot = new Depot()
            {
                Libelle = $"depot-1",
                Adresse = $"depot-1",
                CodePostal = $"depot-1",
                Ville = $"depot-1",
            };
            context.Insert(depot);

            var article = new ArticleCommande()
            {
                Libelle = "Sac 10 kg",
                PrixUnitaire = 7,
                QtMini = 10
            };
            context.Insert(article);

            var client = new Client()
            {
                Nom = Faker.Name.First(),
                Prenom = Faker.Name.Last(),
                Ecurie = Faker.Company.Name(),
                Adresse = Faker.Address.StreetAddress(),
                CodePostal = Faker.Address.ZipCode(),
                Ville = Faker.Address.City(),
                Telephone = Faker.RandomNumber.Next(1000000000, 2147483647).ToString(),
                Email = Faker.Internet.Email(),
                DepotId = depot.Id,
                JourDeTournee = Faker.Enum.Random<JourDeTournee>(),
                Abonnement = Faker.Boolean.Random()
            };
            context.Insert(client);

            var datesPossibles = GetAllDayOfWeek(DateTime.Now.Date, new DateTime(2022, 12, 31), (int)client.JourDeTournee);
            foreach (var datesPossible in datesPossibles)
            {
                if (client.Abonnement)
                {
                    var commande = new Commande()
                    {
                        ClientId = client.Id,
                        ArticleId = article.Id,
                        PrixUnitaire = article.PrixUnitaire,
                        DepotId = client.DepotId,
                        QuantiteFacture = Faker.RandomNumber.Next(10, 99),
                        DateDeLivraisonPrevue = datesPossible.Date,
                        EtatFacture = EtatFacture._
                    };
                    commande.QuantiteOfferte = (int)(commande.QuantiteFacture >= 10 ? Math.Floor((double)commande.QuantiteFacture / 10) : 0);

                    context.Insert(commande);
                }
                else
                {
                    var commande = new Commande()
                    {
                        ClientId = client.Id,
                        ArticleId = article.Id,
                        PrixUnitaire = article.PrixUnitaire,
                        DepotId = client.DepotId,
                        QuantiteFacture = Faker.RandomNumber.Next(10, 99),
                        DateDeLivraisonPrevue = datesPossible.Date,
                        EtatFacture = EtatFacture._
                    };

                    context.Insert(commande);
                }
            }
        }
        private List<DateTime> GetAllDayOfWeek(DateTime start, DateTime end, int jour)
        {
            var datesToReturn = new List<DateTime>();

            var tempDate = start;

            while (tempDate <= end)
            {
                if ((int)tempDate.DayOfWeek == jour)
                {
                    datesToReturn.Add(tempDate);
                }

                tempDate = tempDate.AddDays(1.0);
            }

            return datesToReturn;
        }
        #endregion
        protected override async Task OnInitializedAsync()
        {
            using var context = ContextFactory.MakeContext();
            context.DropDatabase();

            GenerateFakeDatas();

            Datas = new Dictionary<string, List<string>>
            {
                {"Expediteur", new List<string>()},
                {"Destinataire", new List<string>()},
                {"Boucle", new List<string>()},
                {"Pied", new List<string>()}
            };


            var queryCommandes = context.QueryCollection<Commande>();
            Commandes = new List<Commande>();

            foreach (var commande in queryCommandes.ToList())
            {
                Commandes.Add(await context.GetEntityWithForeignKey<Commande>(commande));
            }
        }


        private void GeneratePdf()
        {
            PrepareDatas();

            var doc = new Document();
            var section = doc.AddSection();

            foreach (var (bloc, dataList) in Datas)
            {
                if (bloc.Equals("Expediteur"))
                {
                    foreach (var data in dataList)
                    {
                        section.AddParagraph(data);
                    }
                }
                if (bloc.Equals("Destinataire"))
                {
                    foreach (var data in dataList)
                    {
                        var paragraph = doc.LastSection.AddParagraph();
                        paragraph.Format.LeftIndent = "11cm";
                        paragraph.AddText(data);
                    }
                }
            }

            doc.LastSection.AddParagraph();
            doc.LastSection.AddParagraph($"Facture N° xxx");

            var table = doc.LastSection.AddTable();
            table.Borders.Visible = false;
            //table.Format.Shading.Color = Colors.LavenderBlush;
            //table.Shading.Color = Colors.Salmon;
            table.TopPadding = 5;
            table.BottomPadding = 5;

            // Date de Livraison
            var column = table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Right;

            // libelle
            column = table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Right;

            // Qt facturée
            column = table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Right;

            // Qt offerte
            column = table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Right;

            // Qt Livrée
            column = table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Right;

            // Prix Unitaire
            column = table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Right;

            // Total Facturé
            column = table.AddColumn();
            column.Format.Alignment = ParagraphAlignment.Right;

            table.Rows.Height = 15;

            var row = table.AddRow();
            row.VerticalAlignment = VerticalAlignment.Center;

            row.Cells[0].AddParagraph($"Date de Livraison");
            row.Cells[1].AddParagraph($"Libellé");
            row.Cells[2].AddParagraph($"Qt Facturé");
            row.Cells[3].AddParagraph($"Qt Offerte");
            row.Cells[4].AddParagraph($"Qt Livrée");
            row.Cells[5].AddParagraph($"Prix Unitaire");
            row.Cells[6].AddParagraph($"Total Facturé");

            foreach (var commande in Commandes)
            {
                row = table.AddRow();
                row.VerticalAlignment = VerticalAlignment.Center;
                
                row.Cells[0].AddParagraph($"{commande.DateDeLivraisonPrevue.ToShortDateString()}");
                row.Cells[1].AddParagraph($"{commande.ArticleCommande.Libelle}");
                row.Cells[2].AddParagraph($"{commande.QuantiteFacture}");
                row.Cells[3].AddParagraph($"{commande.QuantiteOfferte}");
                row.Cells[4].AddParagraph($"{commande.QuantiteFacture + commande.QuantiteOfferte}");
                row.Cells[5].AddParagraph($"{commande.ArticleCommande.PrixUnitaire} €");
                row.Cells[6].AddParagraph($"{commande.ArticleCommande.PrixUnitaire * commande.QuantiteFacture} €");
            }

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true, PdfFontEmbedding.Always);
            pdfRenderer.Document = doc;

            pdfRenderer.RenderDocument();
            pdfRenderer.PdfDocument.Save("test.pdf");

        }

        private void PrepareDatas()
        {
            Datas["Expediteur"].Add($"Chevarotte SAS");
            Datas["Expediteur"].Add($"31, rue du Viognier");
            Datas["Expediteur"].Add($"30230, RODILHAN");
            Datas["Expediteur"].Add($"Téléphone : 06 50 05 92 88");
            Datas["Expediteur"].Add($"Siret : 88429845600010");
            Datas["Expediteur"].Add($"N° de TVA : FR88884298456");

            var client = Commandes.FirstOrDefault().Client;
            Datas["Destinataire"].Add($"{client.Nom} {client.Prenom}");
            Datas["Destinataire"].Add($"{client.Adresse}");
            Datas["Destinataire"].Add($"{client.CodePostal} {client.Ville}");

        }
    }
}
