namespace Engine.Database
{
    public interface IContextFactoryBase 
    {
        public static string ConnectionString { get; set; }
        public static string DatabaseName{ get; set; }

        public static BaseContext MakeContext()
        {
            return new BaseContext(ConnectionString, DatabaseName); 
        }
    }
}
