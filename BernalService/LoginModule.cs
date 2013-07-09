using System;
using System.IO;
using System.Collections.Generic;
using Nancy;
using Newtonsoft.Json;

namespace BernalService
{
    public class LoginModule : NancyModule
    {
        static readonly Dictionary<string, DoorStatus> DoorsStatuses = new Dictionary<string, DoorStatus>(2);

        public LoginModule()
        {
            if (!DoorsStatuses.ContainsKey("11223344"))
            {
                DoorStatus doorStatus1 = new DoorStatus();
                doorStatus1.D0902[0] = 0;
                doorStatus1.D0902[1] = 0;

                DoorsStatuses.Add("11223344", doorStatus1);

                DoorStatus doorStatus2 = new DoorStatus();
                doorStatus2.D0902[0] = 0;
                doorStatus2.D0902[1] = 0;

                DoorsStatuses.Add("1321312", doorStatus2);
            }

            Get["/"] = Root;
            Post["/Login"] = param =>
            {
                var es = Login(param.userName, param.password);
                var response = (Response)es;
                response.ContentType = "application/json";
                return response;
            };

            Post["bernal_gta/0/gtas/get"] = param =>
                {
                    string doorList = DoorList();
                    var response = (Response)doorList;
                    response.ContentType = "application/json";
                    return response;
                };

            Post["bernal_gta/{key}/hostcontroller/set/ventilation"] = SetVentilation;

            Post["bernal_gta/0/gtas/get"] = param =>
            {
                string doorList = DoorList();
                var response = (Response)doorList;
                response.ContentType = "application/json";
                return response;
            };

            Post["/bernal_gta/{key}/opstatus"] = param =>
            {
                string doorList = DoorStatus("11223344");
                var response = (Response)doorList;
                response.ContentType = "application/json";
                return response;
            };

            Post["/bernal_gta/11223344/hostcontroller/set/light"] = param =>
            {
                string doorList = DoorStatus("11223344");
                var response = (Response)doorList;
                response.ContentType = "application/json";
                return response;
            };

            Post["/bernal_gta/1321312/hostcontroller/set/light"] = param =>
            {
                var stat = SetLight("1321312");
                var response = (Response)stat;
                response.ContentType = "application/json";
                return response;
            };


            Post["/bernal_gta/11223344/hostcontroller/set/relay"] = param =>
            {
                string doorList = DoorStatus("11223344");
                var response = (Response)doorList;
                response.ContentType = "application/json";
                return response;
            };

            Post["/bernal_gta/1321312/hostcontroller/set/relay"] = param =>
            {
                var stat = SetLight("1321312");
                var response = (Response)stat;
                response.ContentType = "application/json";
                return response;
            };
        }

        public string SetVentilation(dynamic @params)
        {
            string value = @params.key;
            return "{\"status\":0,\"data\":[]}";
        }

        public string Root(dynamic @params)
        {
            return "";
        }

        public string Login(string userName, string password)
        {
            using (var reader = new StreamReader(Request.Body))
            {
                string readToEnd = reader.ReadToEnd();
                Console.WriteLine(readToEnd);
                return
                    "{ \"success\": true, \"user\": {\"Key\": 233, \"Value\": { \"Name\": \"someName\", \"Roles\": \"Role\", \"EMail\": \"email@email.com\", \"Phone\": \"122-334-44\", \"Address\": \"address\", \"Comment\": \"comment...\", \"WebAccess\": true } } }";
            }
        }

        public string DoorList()
        {
            return "{\"Addresses\":[{\"Key\": 11223344,\"Value\":{\"__type\": \"GTAInfo:#bernal.gta.RemoteControllerService\",\"Connection\": 1,\"Address\": \"Goethe Straße 7; 12345 Mustern\",\"Name\": \" Fleischerei Hofeinfahrt \",\"Comment\": \"G501-D\"}},{\"Key\": 1321312,\"Value\":{\"__type\": \"GTAInfo:#bernal.gta.RemoteControllerService\",\"Connection\": 1,\"Address\": \"Goethe Straße 7; 12345 Mustern\",\"Name\": \" Fleischerei Haupttor \",\"Comment\": \"G501-D\"}}]}";
        }

        public string DoorStatus(dynamic @params)
        {
            string status = JsonConvert.SerializeObject(DoorsStatuses[@params.key]);
            return status;
        }

        public string SetLight(string key)
        {
            if (DoorsStatuses[key].D0903[0] == 0 && DoorsStatuses[key].D0903[1] == 0)
            {
                DoorsStatuses[key].D0903[0] = 1;
                DoorsStatuses[key].D0903[1] = 1;
            }
            else
            {
                DoorsStatuses[key].D0903[0] = 0;
                DoorsStatuses[key].D0903[0] = 0;
            }

            return "{ \"status\":0,\"data\":[] }";
        }

        public string SetRelay(string key)
        {
            if (DoorsStatuses[key].D0904[0] == 0 && DoorsStatuses[key].D0904[1] == 0)
            {
                DoorsStatuses[key].D0904[0] = 1;
                DoorsStatuses[key].D0904[1] = 1;
            }
            else
            {
                DoorsStatuses[key].D0904[0] = 0;
                DoorsStatuses[key].D0904[0] = 0;
            }

            return "{ \"status\":0,\"data\":[] }";
        }
    }
}