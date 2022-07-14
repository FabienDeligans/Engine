using Microsoft.AspNetCore.StaticFiles;

namespace Engine.Handler
{
    public static class Handler
    {
        public static string GetTypeMime(string fileName)
        {
            const string DefaultContentType = "application/octet-stream";
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out string contentType))
            {
                contentType = DefaultContentType;
            }
            return contentType;
        }
    }
}
