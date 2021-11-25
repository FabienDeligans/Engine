using Engine.Database;

namespace EngineTests.Base
{
    public class ContextFactoryTest : IContextFactoryBase
    {
        public static string ConnectionString { get; } = "mongodb://127.0.0.1:27017";
        private static string DatabaseName { get; } = "BaseDeTest";

        public static BaseContext MakeContext()
        {
            return new BaseContext(ConnectionString, DatabaseName);
        }
    }
}
