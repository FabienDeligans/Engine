using System.Collections.Generic;
using System.Linq;
using BlazorMongoTemplateApp.Database;
using Engine.CustomAttribute;
using Engine.Model;
using MongoDB.Bson.Serialization.Attributes;

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
            for (var i = 0; i < 1000; i++)
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

        private void HandleDrop(Model landingModel)
        {
            using var context = ContextFactory.MakeContext();

            //landing model -> where the drop happened
            if (draggingModel is null) return;

            //keep the original order for later
            int originalOrderLanding = landingModel.Order;
            
            //increase model uned by 1
            Models.Where(x => x.Order >= landingModel.Order).ToList().ForEach(x => x.Order++);
            draggingModel.Order = originalOrderLanding;//replace landing model
            int ii = 0;
            foreach (var model in Models.OrderBy(x => x.Order).ToList())
            {
                model.Order = ii++;//keep the numbers from 0 to size-1
                model.IsDragOver = false;//remove drag over.
                context.UpdateEntity(model);
            }

            Models = context.QueryCollection<Model>().ToList();
        }

        private Model? draggingModel;//the model that is being dragged

    }
}
