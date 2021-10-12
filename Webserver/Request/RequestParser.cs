using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Webserver.Exceptions;

namespace Webserver.Request
{
    public class RequestParser : IRequestParser
    {
        private readonly byte _carriageReturnByte;
        private readonly byte _lineFeedByte;

        public RequestParser()
        {
            var bytes = Encoding.ASCII.GetBytes("\r\n");
            _carriageReturnByte = bytes[0];
            _lineFeedByte = bytes[1];
        }

        public RequestData Parse(byte[] requestBytes)
        {
            if (requestBytes == null || requestBytes.Length == 0)
            {
                throw new ServerException(HttpStatusCode.BadRequest);
            }

            var requestedBytesSpan = requestBytes;

            var firstLineBytes = ExtractLine(requestedBytesSpan, 0);
            var firstLine = Encoding.ASCII.GetString(firstLineBytes);


            var (methodName, target, version) = ParseFirstLine(firstLine);

            if (string.IsNullOrWhiteSpace(firstLine))
            {
                throw new ServerException(HttpStatusCode.BadRequest);
            }

            var headers = new Dictionary<string, string>();

            string headerLine;
            var startIndex = firstLine.Length + 2;
            do
            {
                var headerLineBytes = ExtractLine(requestedBytesSpan, startIndex);
                headerLine = Encoding.ASCII.GetString(headerLineBytes);
                startIndex += headerLine.Length + 2;

                if (string.IsNullOrEmpty(headerLine))
                {
                    break;
                }

                UpdateHeadersDictionary(headers, headerLine);
            } while (!string.IsNullOrEmpty(headerLine));

            byte[] body = null;
            if (startIndex < requestedBytesSpan.Length)
            {
                body = requestedBytesSpan[startIndex..];
            }


            return new RequestData(methodName, target, version, headers, body);
        }

        private Span<byte> ExtractLine(Span<byte> bytes, int startIndex)
        {
            try
            {
                for (var i = startIndex; i < bytes.Length; i++)
                {
                    if (bytes[i] == _carriageReturnByte && bytes[i + 1] == _lineFeedByte)
                    {
                        return bytes[startIndex..i];
                    }
                }

                throw new ServerException(HttpStatusCode.BadRequest);
            }
            catch
            {
                throw new ServerException(HttpStatusCode.BadRequest);
            }
        }

        private static (string methodName, string target, string version) ParseFirstLine(string line)
        {
            var parts = line.Split(' ');
            if (parts.Length != 3)
            {
                throw new ServerException(HttpStatusCode.BadRequest);
            }

            return (parts[0], parts[1], parts[2]);
        }

        private static void UpdateHeadersDictionary(IDictionary<string, string> headers, string headerLine)
        {
            var indexOfSeparator = headerLine.IndexOf(':');
            if (indexOfSeparator < 0)
            {
                throw new ServerException(HttpStatusCode.BadRequest);
            }

            var headerName = headerLine[..indexOfSeparator].Trim();
            var headerValue = headerLine[(indexOfSeparator + 1)..].Trim();

            if (headers.ContainsKey(headerName))
            {
                headers[headerName] = headerValue;
            }
            else
            {
                headers.Add(headerName, headerValue);
            }
        }
    }
}
