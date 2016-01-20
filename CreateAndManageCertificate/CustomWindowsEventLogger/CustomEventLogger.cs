using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CustomWindowsEventLogger
{
    public class CustomEventLogger
    {
        public CustomEventLogger()
        {
            CreateCustomLog();
        }

        public void CreateCustomLog()
        {
            if (!EventLog.Exists("CreateAndManageCertificate"))
            {
                EventLog.CreateEventSource("CreateAndManageCertificate", "CreateAndManageCertificate");
            }
        }

        public void DeleteLog()
        {
            if (EventLog.Exists("CreateAndManageCertificate"))
            {
                EventLog.DeleteEventSource("CreateAndManageCertificate");
            }
        }

        public void WriteToLog(string message, EventLogEntryType entryType)
        {
            if (!EventLog.SourceExists("CreateAndManageCertificate"))
            {
                CreateCustomLog();
            }

            EventLog log = new EventLog("CreateAndManageCertificate");
            log.Source = "CreateAndManageCertificate";

            log.WriteEntry(message, entryType);
        }

    }
}
