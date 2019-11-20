using System;
using System.Management;

namespace Jaroszek.ProofOfConcept.MonitorOfRunningProcesses
{
    class Program
    {

        private const string PROCESS_NAME = "AMHMSQL.exe";

        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to exit");

            GetRunningProcesses();
        }


        private static void GetRunningProcesses()
        {
            ManagementEventWatcher startWatch = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            startWatch.EventArrived += new EventArrivedEventHandler(startWatch_EventArrived);
            startWatch.Start();
            ManagementEventWatcher stopWatch = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
            stopWatch.EventArrived += new EventArrivedEventHandler(stopWatch_EventArrived);
            stopWatch.Start();
            while (!Console.KeyAvailable) System.Threading.Thread.Sleep(50);
            startWatch.Stop();
            stopWatch.Stop();
        }
        static void stopWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            var name = e.NewEvent.Properties["ProcessName"].Value;
            if (name.ToString().ToUpper() == PROCESS_NAME.ToUpper())
            {
                Console.WriteLine("Process stopped: {0}", name);
            }
        }

        static void startWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            var name = e.NewEvent.Properties["ProcessName"].Value;
            if (name.ToString().ToUpper() == PROCESS_NAME.ToUpper())
            {
                Console.WriteLine("Process started: {0}", name);
            }
        }

    }
}
