using System;
using System.IO;
using System.Management;
using System.Text;

namespace Jaroszek.ProofOfConcept.MonitorOfRunningProcesses
{
    public class RunningServices
    {
        private const string PROCESS_NAME = "AMHMSQL.exe";
        private const string SAVE_FILE_NAME = "ManagementObjectSearcher_services.csv";

        public static void GetRuningServices()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();

            string format = "{0},{1},{2},{3},{4}";

            // Header line
            sb.AppendFormat(format, "DisplayName",
                "ServiceName",
                "Status",
                "ProcessId",
                "PathName");
            sb.AppendLine();

            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("SELECT * FROM Win32_Service");

            var serviceList = searcher.Get();

            foreach (ManagementObject result in serviceList)
            {

                if (result["Name"].ToString() == PROCESS_NAME)
                {
                    sb1.AppendFormat(format, result["DisplayName"],
                        result["Name"],
                        result["State"],
                        result["ProcessId"],
                        result["PathName"]
                    );
                    sb1.AppendLine();

                    Console.WriteLine(sb1.ToString());
                }


                sb.AppendFormat(format, result["DisplayName"],
                    result["Name"],
                    result["State"],
                    result["ProcessId"],
                    result["PathName"]
                );
                sb.AppendLine();
            }

            File.WriteAllText(
                SAVE_FILE_NAME,
                sb.ToString()
            );
        }
    }
}