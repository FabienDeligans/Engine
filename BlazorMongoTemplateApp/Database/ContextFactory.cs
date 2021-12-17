using Engine.Database;

namespace BlazorMongoTemplateApp.Database
{
    // TODO Build a context factory to connect database
    /// <summary>
    /// Class to connect database
    /// Inherit IContextFactoryBase
    /// </summary>
    public class ContextFactory : IContextFactoryBase
    {
        private static string ConnectionString { get; } = "mongodb://127.0.0.1:27017";
        private static string DatabaseName { get; } = "BaseDeTest";

        public static BaseContext MakeContext()
        {
            return new BaseContext(ConnectionString, DatabaseName);
        }
    }
}
