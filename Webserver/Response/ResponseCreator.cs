using System.Text;
using Humanizer;

namespace Webserver.Response
{
    public class ResponseCreator : IResponseCreator
    {
        private const string LineDelimiter = "\r\n";

        public byte[] Create(ResponseData responseData)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(responseData.Version);
            stringBuilder.Append(' ');
            stringBuilder.Append((int)responseData.StatusCode);
            stringBuilder.Append(' ');
            stringBuilder.Append(responseData.StatusCode.Humanize(LetterCasing.Title));
            stringBuilder.Append(LineDelimiter);

            if (responseData.Headers != null)
            {
                foreach (var (key, value) in responseData.Headers)
                {
                    stringBuilder.Append(key);
                    stringBuilder.Append(": ");
                    stringBuilder.Append(value);
                    stringBuilder.Append(LineDelimiter);
                }
            }

            stringBuilder.Append(LineDelimiter);

            var responseBytes = Encoding.ASCII.GetBytes(stringBuilder.ToString());

            if (responseData.Body == null)
            {
                return responseBytes;
            }

            var finalResponseBytes = new byte[responseBytes.Length + responseData.Body.Length];

            for (var i = 0; i < responseBytes.Length; i++)
            {
                finalResponseBytes[i] = responseBytes[i];
            }

            for (var i = 0; i < responseData.Body.Length; i++)
            {
                finalResponseBytes[i + responseBytes.Length] = responseData.Body[i];
            }

            return finalResponseBytes;
        }
    }
}
