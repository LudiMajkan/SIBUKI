using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CustomWindowsEventLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEventLogger logger = new CustomEventLogger();

            logger.WriteToLog("Warning test 1", EventLogEntryType.Warning);
            logger.WriteToLog("FailureAudit test 1", EventLogEntryType.FailureAudit);
            logger.WriteToLog("Error test 1", EventLogEntryType.Error);
            logger.WriteToLog("Information test 1", EventLogEntryType.Information);
            logger.WriteToLog("SuccessAudit test 1", EventLogEntryType.SuccessAudit);
        }
    }
}
