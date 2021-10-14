namespace Webserver.HTTPHeaders
{
    public class ContentTypeHeaderProvider : IContentTypeHeaderProvider
    {
        public string Provide(string fileName)
        {
            var extension = GetExtension(fileName);
            return extension switch
            {
                "gif" or "png" or "tiff" => $"image/{extension}",
                "jpeg" or "jpg" => "image/jpeg",
                "csv" or "css" or "xml" => $"text/{extension}; charset=utf-8",
                "html" or "htm" => "text/html; charset=utf-8",
                "txt" or _ => "text/plain; charset=utf-8"
            };
        }

        private static string GetExtension(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return "";
            }

            var indexOfLastDot = fileName.LastIndexOf('.');
            return indexOfLastDot < 0 ? "" : fileName[(indexOfLastDot + 1)..];
        }
    }
}
