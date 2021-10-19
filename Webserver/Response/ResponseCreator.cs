using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Webserver.HTTPHeaders;
using Webserver.IO;
using Webserver.Request;

namespace Webserver.Response
{
    public class ResponseCreator : IResponseCreator
    {
        private readonly IResponseHeaderParser _responseHeaderParser;
        private readonly IFilePathProvider _filePathProvider;
        private readonly IFileReader _fileReader;
        private readonly IContentTypeHeaderProvider _contentTypeHeaderProvider;
        private readonly IConfigRequestHandler _configRequestHandler;

        public ResponseCreator(IResponseHeaderParser responseHeaderParser, IFilePathProvider filePathProvider,
            IFileReader fileReader, IContentTypeHeaderProvider contentTypeHeaderProvider,
            IConfigRequestHandler configRequestHandler)
        {
            _responseHeaderParser = responseHeaderParser;
            _filePathProvider = filePathProvider;
            _fileReader = fileReader;
            _contentTypeHeaderProvider = contentTypeHeaderProvider;
            _configRequestHandler = configRequestHandler;
        }

        public byte[] Create(RequestData requestData)
        {
            if (requestData == null)
            {
                return Create(new ResponseStatusLine("HTTP/1.1", HttpStatusCode.BadRequest));
            }

            if (requestData.Method is not ("GET" or "HEAD"))
            {
                if (requestData.Method is "POST" && requestData.Target == "/config")
                {
                    var headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/json" }
                    };

                    try
                    {
                        var responseString = _configRequestHandler.Handle(Encoding.ASCII.GetString(requestData.Body));
                        return Create(new ResponseStatusLine("HTTP/1.1", HttpStatusCode.OK), headers,
                            Encoding.ASCII.GetBytes(responseString));
                    }
                    catch
                    {
                        return Create(new ResponseStatusLine("HTTP/1.1", HttpStatusCode.BadRequest));
                    }
                }

                return Create(new ResponseStatusLine("HTTP/1.1", HttpStatusCode.MethodNotAllowed));
            }

            try
            {
                var responseStatusLine = new ResponseStatusLine(requestData.Version, HttpStatusCode.OK);

                var headers = new Dictionary<string, string>
                {
                    { "Content-Type", _contentTypeHeaderProvider.Provide(requestData.Target) }
                };

                return Create(responseStatusLine, headers, requestData.Target);
            }
            catch (FileNotFoundException)
            {
                return Create(new ResponseStatusLine(requestData.Version, HttpStatusCode.NotFound));
            }
            catch
            {
                return Create(new ResponseStatusLine(requestData.Version, HttpStatusCode.InternalServerError));
            }
        }

        public byte[] Create(ResponseStatusLine responseStatusLine)
        {
            switch (responseStatusLine.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.MethodNotAllowed:
                case HttpStatusCode.InternalServerError:
                {
                    var headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "text/html; charset=utf-8" }
                    };

                    return Create(responseStatusLine, headers, $"{(int)responseStatusLine.StatusCode}.html");
                }

                default:
                {
                    return _responseHeaderParser.Parse(responseStatusLine);
                }
            }
        }

        private byte[] Create(ResponseStatusLine statusLine, IDictionary<string, string> headers, string path)
        {
            var headerBytes = _responseHeaderParser.Parse(statusLine, headers);

            var filePath = _filePathProvider.Provide(path);

            var contentBytes = _fileReader.Read(filePath);

            var resultBytes = new byte[headerBytes.Length + contentBytes.Length];
            for (var i = 0; i < headerBytes.Length; i++)
            {
                resultBytes[i] = headerBytes[i];
            }

            for (var i = 0; i < contentBytes.Length; i++)
            {
                resultBytes[i + headerBytes.Length] = contentBytes[i];
            }

            return resultBytes;
        }

        private byte[] Create(ResponseStatusLine statusLine, IDictionary<string, string> headers,
            IReadOnlyList<byte> body)
        {
            var headerBytes = _responseHeaderParser.Parse(statusLine, headers);

            var resultBytes = new byte[headerBytes.Length + body.Count];
            for (var i = 0; i < headerBytes.Length; i++)
            {
                resultBytes[i] = headerBytes[i];
            }

            for (var i = 0; i < body.Count; i++)
            {
                resultBytes[i + headerBytes.Length] = body[i];
            }

            return resultBytes;
        }
    }
}
