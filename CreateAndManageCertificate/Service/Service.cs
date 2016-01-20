using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using CommunicationContract;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Service
{
    public class Service : IContract
    {
        private static int currID = 0;
        private int secondsLeft = 10;

        public void PingServer(TimeSpan t)
        {
            currID++;
            TimeSpan now = DateTime.Now.TimeOfDay;
            string[] lines = { currID + " : " + now.ToString() + " : " + t.ToString() };
            System.IO.File.AppendAllLines(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString() + "\\textFile.txt", lines);
            Console.WriteLine("Client pinged me.");
        }

        public void EstablishConnection()
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Service";
                eventLog.WriteEntry("Client made a connection to the service.", EventLogEntryType.Information);

                while (true)
                {
                    Thread.Sleep(1000);
                    if (--secondsLeft == 0)
                    {
                        eventLog.WriteEntry("Client disconnected from the service.", EventLogEntryType.Information);
                        break;
                    }
                }
            }
        }
    }
}
