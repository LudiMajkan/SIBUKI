using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using CommunicationContract;
using System.IO;

namespace Service
{
    public class Service : IContract
    {
        private static int currID = 0;
        public void PingServer(TimeSpan t)
        {
            currID++;
            TimeSpan now = DateTime.Now.TimeOfDay;
            string[] lines = { currID + " : " + now.ToString() + " : " + t.ToString()};
            //System.IO.File.WriteAllLines(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString() + "\\textFile.txt", lines);
            System.IO.File.AppendAllLines(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString() + "\\textFile.txt", lines);
            Console.WriteLine("Service pinged me.");
        }
    }
}
