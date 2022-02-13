using BlazorMongoTemplateApp.Component.Modal.Called;
using BlazorMongoTemplateApp.Database;
using BlazorMongoTemplateApp.Models;

namespace BlazorMongoTemplateApp.Pages.Modal
{
    public partial class CreateUpdateEntity : ModalComponentCalled
    {
        public Exemplaire Exemplaire { get; set; }

        protected override void OnInitialized()
        {
            using var context = ContextFactory.MakeContext();

            Exemplaire = DataId == null ? 
                new Exemplaire() : 
                context.GetEntity<Exemplaire>(DataId);
        }


        public override void Submit()
        {
            ReturnedObject = Exemplaire; 
            base.Submit();
        }
    }
}
