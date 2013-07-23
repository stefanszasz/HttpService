using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using Nancy;
using Newtonsoft.Json;
using System.Timers;
using Service.Nancy.App;

namespace BernalService
{
    public class LoginModule : NancyModule
    {
        private static readonly Dictionary<string, DoorStatus> DoorsStatuses = new Dictionary<string, DoorStatus>(2);

        private static readonly Dictionary<string, int> doorTimes = new Dictionary<string, int>(2);

        private static readonly System.Timers.Timer timer = new Timer(1000);

        private ElapsedEventHandler openingDelegate;
        private ElapsedEventHandler closingDelegate;
        private int doorOpeningTimeInSeconds = 10;

        public LoginModule()
        {

            if (!DoorsStatuses.ContainsKey("11223344"))
            {
                DoorStatus doorStatus1 = new DoorStatus();
                doorStatus1.D0902[0] = 1;
                doorStatus1.D0902[1] = 1;

                DoorsStatuses.Add("11223344", doorStatus1);

                DoorStatus doorStatus2 = new DoorStatus();
                doorStatus2.D0902[0] = 1;
                doorStatus2.D0902[1] = 1;

                DoorsStatuses.Add("1321312", doorStatus2);

                DoorStatus doorStatus3 = new DoorStatus();
                doorStatus3.D0902[0] = 1;
                doorStatus3.D0902[1] = 1;

                DoorsStatuses.Add("666666", doorStatus3);

                //door opening & closing default times
                doorTimes["11223344"] = doorOpeningTimeInSeconds;
                doorTimes["1321312"] = doorOpeningTimeInSeconds;
                doorTimes["666666"] = doorOpeningTimeInSeconds;

                timer.Start();

            }

            Get["/"] = Root;

            Post["/bernal_user/0/id"] = Login;

            Post["bernal_gta/0/gtas/get"] = DoorList;

            Post["bernal_gta/{key}/hostcontroller/set/ventilation"] = SetVentilation;

            Post["/bernal_gta/{key}/opstatus"] = DoorStatus;

            Post["/bernal_gta/{key}/hostcontroller/set/light"] = SetLight;

            Post["/bernal_gta/{key}/hostcontroller/set/relay"] = SetRelay;

            Post["/bernal_gta/{key}/hostcontroller/set/door"] = SetDoor;
        }

        public string Root(dynamic @params)
        {
            return "Merje";
        }

        public string Login(dynamic @params)
        {
            using (var reader = new StreamReader(Request.Body))
            {
                string readToEnd = reader.ReadToEnd();
                Console.WriteLine(readToEnd);
                return
                    "{ \"success\": true, \"user\": {\"Key\": 233, \"Value\": { \"Name\": \"someName\", \"Roles\": [\"Role\"], \"EMail\": \"email@email.com\", \"Phone\": \"122-334-44\", \"Address\": \"address\", \"Comment\": \"comment...\", \"WebAccess\": true } } }";
            }
        }

        public string DoorList(dynamic @params)
        {
            return "{\"Addresses\": [{\"Key\": 11223344,\"Value\": {\"__type\": \"GTAInfo:#bernal.gta.RemoteControllerService\",\"Connection\": 1,\"Address\": \"Goethe Straße 7; 12345 Mustern\",\"Name\": \" Fleischerei Hofeinfahrt \",\"Comment\": \"G501-D\"}},{\"Key\": 1321312,\"Value\": {\"__type\": \"GTAInfo:#bernal.gta.RemoteControllerService\",\"Connection\": 1,\"Address\": \"Goethe Straße 7; 12345 Mustern\",\"Name\": \" Fleischerei Haupttor \",\"Comment\": \"G501-D\"}},{\"Key\": 666666,\"Value\": {\"__type\": \"GTAInfo:#bernal.gta.RemoteControllerService\",\"Connection\": 1,\"Address\": \"Middle-earth to the East of Anduin\",\"Name\": \" Mordoor\",\"Comment\": \"My precious\"}}]}";
        }

        public string DoorStatus(dynamic @params)
        {
            string key = @params.key;
            string status = JsonConvert.SerializeObject(DoorsStatuses[key]);
            return status;
        }

        public string SetDoor(dynamic @params)
        {
            string key = @params.key;
            int doorState = DoorsStatuses[key].D0802[0];

            switch (doorState)
            {
                case 0: Opening(key); break;// DoorsStatuses[key].D0802[0] = 1; break;//return DoorStatuses.CLOSED;


                case 1: { timer.Elapsed -= openingDelegate; DoorsStatuses[key].D0802[0] = 3; } break; //from DoorStatuses.OPENING to STOPED;
                case 2: { timer.Elapsed -= openingDelegate; DoorsStatuses[key].D0802[0] = 3; } break; //from DoorStatuses.OPENING to STOPED;

                case 3: Opening(key); break; //DoorsStatuses[key].D0802[0] = 1; //from DoorStatuses.STOPED to OPENING;

                case 4: Opening(key); break; //DoorsStatuses[key].D0802[0] = 1; //from DoorStatuses.STOPED to OPENING;

                case 5: Closing(key); break; //DoorsStatuses[key].D0802[0] = 6; //from DoorStatuses.OPEN to CLOSING;

                case 6: { timer.Elapsed -= closingDelegate; DoorsStatuses[key].D0802[0] = 3; } break; //from DoorStatuses.CLOSING to STOPED
                case 7: { timer.Elapsed -= closingDelegate; DoorsStatuses[key].D0802[0] = 3; } break; //from DoorStatuses.CLOSING to STOPED
                case 8: { timer.Elapsed -= closingDelegate; DoorsStatuses[key].D0802[0] = 3; } break; //from DoorStatuses.CLOSING to STOPED
                case 9: { timer.Elapsed -= closingDelegate; DoorsStatuses[key].D0802[0] = 3; } break; //from DoorStatuses.CLOSING to STOPED

                case 11: Opening(key); break; //DoorsStatuses[key].D0802[0] = 1; break;//from DoorStatuses.STOPED to OPENING;

                case 12: { doorTimes[key] = 5; Opening(key); } break; //DoorsStatuses[key].D0802[0] = 1; break;//from DoorStatuses.VENTILATING to CLOSING;

                case 13: { timer.Elapsed -= openingDelegate; DoorsStatuses[key].D0802[0] = 3; } break;//from DoorStatuses.OPENING to STOPED;
                case 14: { timer.Elapsed -= openingDelegate; DoorsStatuses[key].D0802[0] = 3; } break;//from DoorStatuses.OPENING to STOPED;

                case 15: break; //return DoorStatuses.REVERSEING;
                case 16: break; //return DoorStatuses.LEARNING;
                default: break;
            }

            return "{ \"status\":0,\"data\":[] }";
        }


        void timer_Elapsed_Opening(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void timer_Elapsed_Closing(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Opening(string key)
        {
            DoorsStatuses[key].D0802[0] = 1; //set door status to OPENING

            doorTimes[key] = 10;

            openingDelegate = delegate(Object o, ElapsedEventArgs e)
            {
                doorTimes[key]--;
                if (doorTimes[key] == 0)
                {
                    //t.Stop();
                    //doorTimes[key] = 12;
                    DoorsStatuses[key].D0802[0] = 5; //done opening; set to OPEN
                    timer.Elapsed -= openingDelegate; //disconnect from handler
                }
            };

            timer.Elapsed += openingDelegate;
        }

        private void OpeningToVentilatingPosition(string key)
        {
            DoorsStatuses[key].D0802[0] = 1; //set door status to OPENING

            doorTimes[key] = 5;

            openingDelegate = delegate(Object o, ElapsedEventArgs e)
            {
                doorTimes[key]--;
                if (doorTimes[key] == 0)
                {
                    DoorsStatuses[key].D0802[0] = 12; //done opening; set to VENTILATING
                    timer.Elapsed -= openingDelegate; //disconnect from handler
                }
            };

            timer.Elapsed += openingDelegate;
        }

        private void Closing(string key)
        {
            DoorsStatuses[key].D0802[0] = 6; //set door status to CLOSING

            doorTimes[key] = 0;

            closingDelegate = delegate(Object o, ElapsedEventArgs e)
            {
                doorTimes[key]++;
                if (doorTimes[key] == doorOpeningTimeInSeconds)
                {
                    //t.Stop();
                    //doorTimes[key] = 12;
                    DoorsStatuses[key].D0802[0] = 0; //done closing; set to CLOSED
                    timer.Elapsed -= closingDelegate; //disconnect from handler
                }
            };

            timer.Elapsed += closingDelegate;
        }

        //private void ClosingFromVentilatig(string key)
        //{
        //    doorOpeningTimeInSeconds = 5;

        //    DoorsStatuses[key].D0802[0] = 6; //set door status to CLOSING

        //    closingDelegate = delegate(Object o, ElapsedEventArgs e)
        //    {
        //        doorTimes[key]++;
        //        if (doorTimes[key] == doorOpeningTimeInSeconds)
        //        {
        //            DoorsStatuses[key].D0802[0] = 0; //done closing; set to CLOSED
        //            timer.Elapsed -= closingDelegate; //disconnect from handler
        //        }
        //    };

        //    timer.Elapsed += closingDelegate;
        //}



        public string SetLight(dynamic @params)
        {
            if (DoorsStatuses[@params.key].D0903[0] == 0 && DoorsStatuses[@params.key].D0903[1] == 0)
            {
                DoorsStatuses[@params.key].D0903[0] = 1;
                DoorsStatuses[@params.key].D0903[1] = 1;
            }
            else
            {
                DoorsStatuses[@params.key].D0903[0] = 0;
                DoorsStatuses[@params.key].D0903[1] = 0;
            }

            return "{ \"status\":0,\"data\":[] }";
        }

        public string SetRelay(dynamic @params)
        {
            if (DoorsStatuses[@params.key].D0904[0] == 0 && DoorsStatuses[@params.key].D0904[1] == 0)
            {
                DoorsStatuses[@params.key].D0904[0] = 1;
                DoorsStatuses[@params.key].D0904[1] = 1;
            }
            else
            {
                DoorsStatuses[@params.key].D0904[0] = 0;
                DoorsStatuses[@params.key].D0904[1] = 0;
            }

            return "{ \"status\":0,\"data\":[] }";
        }

        public string SetVentilation(dynamic @params)
        {
            string value = @params.key;

            if (DoorsStatuses[@params.key].D0902[0] == 0 && DoorsStatuses[@params.key].D0902[1] == 0)
            {
                DoorsStatuses[@params.key].D0902[0] = 1;
                DoorsStatuses[@params.key].D0902[1] = 1;

                doorOpeningTimeInSeconds = 5;

                Opening(@params.key);
            }
            else
            {
                DoorsStatuses[@params.key].D0902[0] = 0;
                DoorsStatuses[@params.key].D0902[1] = 0;

                OpeningToVentilatingPosition(@params.key);

            }

            return "{\"status\":0,\"data\":[]}";
        }
    }
}