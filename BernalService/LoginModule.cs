using System;
using System.IO;
using Nancy;

namespace BernalService
{
    public class LoginModule : NancyModule
    {
        public LoginModule()
        {
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
        }

        public string SetVentilation(dynamic @params)
        {
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
    }
}