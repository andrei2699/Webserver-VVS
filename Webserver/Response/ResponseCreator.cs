using System.Text;
using Humanizer;

namespace Webserver.Response
{
    public class ResponseCreator : IResponseCreator
    {
        private const string LineDelimiter = "\r\n";

        public byte[] Create(ResponseDataHeader responseDataHeader)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(responseDataHeader.Version);
            stringBuilder.Append(' ');
            stringBuilder.Append((int)responseDataHeader.StatusCode);
            stringBuilder.Append(' ');
            stringBuilder.Append(responseDataHeader.StatusCode.Humanize(LetterCasing.Title));
            stringBuilder.Append(LineDelimiter);

            if (responseDataHeader.Headers != null)
            {
                foreach (var (key, value) in responseDataHeader.Headers)
                {
                    stringBuilder.Append(key);
                    stringBuilder.Append(": ");
                    stringBuilder.Append(value);
                    stringBuilder.Append(LineDelimiter);
                }
            }

            stringBuilder.Append(LineDelimiter);

            var responseBytes = Encoding.ASCII.GetBytes(stringBuilder.ToString());

            return responseBytes;
        }
    }
}
