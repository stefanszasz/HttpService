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
    }
}