using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2_Kapserskiy
{
    class ScanTerminal
    {
        public string ScanUtilPath { get; private set; }
        public string ScanServicePath { get; private set; }

        Process scanServiceProcess;
        Process scanUtilProcess;
        StreamWriter scanServiceInput;
        StreamWriter scanUtilInput;
        public ScanTerminal(string scanUtilPath, string scanServicePath)
        {
            Process curProcess = Process.GetCurrentProcess();

            scanServiceProcess = StartApp(scanServicePath);
            scanUtilProcess = StartApp(scanUtilPath);

            scanServiceInput = scanServiceProcess.StandardInput;
            scanUtilInput = scanUtilProcess.StandardInput;

            ScanUtilPath = scanUtilPath;
            ScanServicePath = scanServicePath;
        }

        Process StartApp(string path)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(path)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true
            };

            Process process = new Process() { StartInfo = processStartInfo };
            process.OutputDataReceived += ExternalDataRecievedEventHandler;
            process.Start();
            process.BeginOutputReadLine();
            return process;
        }

        static void ExternalDataRecievedEventHandler(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data) == false)
            {
                Console.WriteLine(e.Data);
            }
        }


        public bool WriteToTerminal(string? buf)
        {
            if (buf == null)
            {
                return true;
            }
            string[] parts = buf.Split(' ');
            if (parts.Length == 0)
            {
                return true;
            }
            switch (parts[0])
            {
                case "scan_service":
                    {
                        scanServiceInput.WriteLine("start");
                        break;
                    }
                case "scan_util":
                    {
                        scanUtilInput.WriteLine(parts[1] + " " + parts[2]);
                        break;
                    }
                case "exit":
                    {
                        scanServiceInput.WriteLine("stop");
                        scanServiceInput.WriteLine("exit");
                        scanUtilInput.WriteLine("exit");
                        return false;
                    }
                default:
                    {
                        Console.WriteLine("Incorrect command");
                        break;
                    }
            }
            return true;
        }
    }
}
