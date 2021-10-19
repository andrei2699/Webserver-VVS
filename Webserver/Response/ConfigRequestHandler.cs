using System;
using System.Text.Json;
using Webserver.Config;

namespace Webserver.Response
{
    public class ConfigRequestHandler : IConfigRequestHandler
    {
        private readonly IServerEvents _serverEvents;

        public ConfigRequestHandler(IServerEvents serverEvents)
        {
            _serverEvents = serverEvents;
        }

        public string Handle(string requestDataBody)
        {
            var configRequest = JsonSerializer.Deserialize<ConfigRequest>(requestDataBody);
            if (configRequest == null || string.IsNullOrEmpty(configRequest.action) ||
                string.IsNullOrEmpty(configRequest.value))
            {
                throw new ArgumentException();
            }

            switch (configRequest.action)
            {
                case "change_state":
                {
                    try
                    {
                        var status = (ServerState)int.Parse(configRequest.value);

                        switch (status)
                        {
                            case ServerState.Stopped:
                            {
                                _serverEvents.ChangeState(status);
                                return JsonSerializer.Serialize(new ConfigResponse(true, "stopping server"));
                            }
                            case ServerState.Running:
                            {
                                _serverEvents.ChangeState(status);
                                return JsonSerializer.Serialize(new ConfigResponse(true, "status changed to running"));
                            }
                            case ServerState.Maintenance:
                            {
                                _serverEvents.ChangeState(status);
                                return JsonSerializer.Serialize(new ConfigResponse(true,
                                    "status changed to maintenance"));
                            }
                            default:
                                return JsonSerializer.Serialize(new ConfigResponse(false, "invalid status"));
                        }
                    }
                    catch
                    {
                        return JsonSerializer.Serialize(new ConfigResponse(false, "invalid status"));
                    }
                }

                case "change_path":
                {
                    _serverEvents.ChangeFilePath(configRequest.value);
                    return JsonSerializer.Serialize(new ConfigResponse(true, "path changed"));
                }
            }

            return JsonSerializer.Serialize(new ConfigResponse(false, "invalid command"));
        }

        private record ConfigRequest(string action, string value);

        private record ConfigResponse(bool success, string message);
    }
}
