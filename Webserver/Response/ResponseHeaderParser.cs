using System.Collections.Generic;
using System.Text;
using Humanizer;

namespace Webserver.Response
{
    public class ResponseHeaderParser : IResponseHeaderParser
    {
        private const string LineDelimiter = "\r\n";

        public byte[] Parse(ResponseStatusLine responseStatusLine, IDictionary<string, string> headers)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(responseStatusLine.Version);
            stringBuilder.Append(' ');
            stringBuilder.Append((int)responseStatusLine.StatusCode);
            stringBuilder.Append(' ');
            stringBuilder.Append(responseStatusLine.StatusCode.Humanize(LetterCasing.Title));
            stringBuilder.Append(LineDelimiter);

            if (headers != null)
            {
                foreach (var (key, value) in headers)
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
