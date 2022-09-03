using System.Collections.Generic;
using System.Linq;
using BlazorMongoTemplateApp.Database;
using Engine.CustomAttribute;
using Engine.Database;
using Engine.Model;
using MongoDB.Bson.Serialization.Attributes;
using MudBlazor;
using MudBlazor.Utilities;

namespace BlazorMongoTemplateApp.Pages
{
    public class Model : Entity
    {
        [BsonId(IdGenerator = typeof(IdGenerator<Model>))]
        public override string Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; } = "";
        public bool IsDragOver { get; set; }
    }

    public partial class DragAndDrop
    {
        public List<Model> Models { get; set; } = new List<Model>();


        protected override void OnInitialized()
        {
            for (var i = 0; i < 55; i++)
            {
                Model m = new()
                {
                    Order = i,
                    Name = $"Item {i}"
                };
                Models.Add(m);
            }

            using var context = ContextFactory.MakeContext();
            context.DropCollection<Model>();
            context.InsertAll(Models);

            Models = context.QueryCollection<Model>().ToList();
        }
        
        /// <summary>
        /// mudblazor
        /// </summary>
        private MudDropContainer<DropItem> _container;

        private void ItemUpdated(MudItemDropInfo<DropItem> dropItem)
        {
            dropItem.Item.Selector = dropItem.DropzoneIdentifier;

            var indexOffset = dropItem.DropzoneIdentifier switch
            {
                "2" => _serverData.Count(x => x.Selector == "1"),
                _ => 0,
            };

            _serverData.UpdateOrder(dropItem, item => item.Order, indexOffset);
        }

        private List<DropItem> _dropzoneItems = new();

        private List<DropItem> _serverData = new()
        {
            new DropItem() { Order = 0, Name = "Item 1", Selector = "1" },
            new DropItem() { Order = 1, Name = "Item 2", Selector = "1" },
            new DropItem() { Order = 2, Name = "Item 3", Selector = "1" },
            new DropItem() { Order = 3, Name = "Item 4", Selector = "1" },
            new DropItem() { Order = 4, Name = "Item 5", Selector = "1" },
            new DropItem() { Order = 5, Name = "Item 6", Selector = "1" },
            new DropItem() { Order = 6, Name = "Item 7", Selector = "2" },
            new DropItem() { Order = 7, Name = "Item 8", Selector = "2" },
            new DropItem() { Order = 8, Name = "Item 9", Selector = "2" },
            new DropItem() { Order = 9, Name = "Item 10", Selector = "2" },
        };

        private void RefreshContainer()
        {
            //update the binding to the container
            StateHasChanged();

            //the container refreshes the internal state
            _container.Refresh();
        }

        private void LoadServerData()
        {
            List<DropItem> newdata = new List<DropItem>();

            foreach (var item in _serverData.OrderBy(x => x.Order))
            {
                newdata.Add(item);
            }

            _dropzoneItems = newdata;
            RefreshContainer();
        }

        private void SaveData() => _serverData = _dropzoneItems;

        private void Reset()
        {
            _dropzoneItems = new();
            _serverData = new()
            {
                new DropItem() { Order = 0, Name = "Item 1", Selector = "1" },
                new DropItem() { Order = 1, Name = "Item 2", Selector = "1" },
                new DropItem() { Order = 2, Name = "Item 3", Selector = "1" },
                new DropItem() { Order = 3, Name = "Item 4", Selector = "1" },
                new DropItem() { Order = 4, Name = "Item 5", Selector = "1" },
                new DropItem() { Order = 5, Name = "Item 6", Selector = "1" },
                new DropItem() { Order = 6, Name = "Item 7", Selector = "1" },
                new DropItem() { Order = 7, Name = "Item 8", Selector = "1" },
                new DropItem() { Order = 8, Name = "Item 9", Selector = "1" },
                new DropItem() { Order = 9, Name = "Item 10", Selector = "1" },

                new DropItem() { Order = 10, Name = "Item 11", Selector = "1" },
                new DropItem() { Order = 11, Name = "Item 12", Selector = "1" },
                new DropItem() { Order = 12, Name = "Item 13", Selector = "1" },
                new DropItem() { Order = 13, Name = "Item 14", Selector = "1" },
                new DropItem() { Order = 14, Name = "Item 15", Selector = "1" },
                new DropItem() { Order = 15, Name = "Item 16", Selector = "1" },
                new DropItem() { Order = 16, Name = "Item 17", Selector = "1" },
                new DropItem() { Order = 17, Name = "Item 18", Selector = "1" },
                new DropItem() { Order = 18, Name = "Item 19", Selector = "1" },
                new DropItem() { Order = 19, Name = "Item 10", Selector = "1" },

                new DropItem() { Order = 20, Name = "Item 21", Selector = "1" },
                new DropItem() { Order = 21, Name = "Item 22", Selector = "1" },
                new DropItem() { Order = 22, Name = "Item 23", Selector = "1" },
                new DropItem() { Order = 23, Name = "Item 24", Selector = "1" },
                new DropItem() { Order = 24, Name = "Item 25", Selector = "1" },
                new DropItem() { Order = 25, Name = "Item 26", Selector = "1" },
                new DropItem() { Order = 26, Name = "Item 27", Selector = "2" },
                new DropItem() { Order = 27, Name = "Item 28", Selector = "2" },
                new DropItem() { Order = 28, Name = "Item 29", Selector = "2" },
                new DropItem() { Order = 29, Name = "Item 20", Selector = "2" },
            };

            RefreshContainer();
        }

        public class DropItem
        {
            public string Name { get; init; }
            public string Selector { get; set; }
            public int Order { get; set; }
        }

    }
}
