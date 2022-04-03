namespace Engine.Model
{
    public interface IEntity
    {
        public string Id { get; set; }
        public bool IsDisabled { get; set; }
    }
}
