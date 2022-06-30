using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Task2_Kapserskiy
{
    class ScanTask
    {
        const string JSAttention = @"<script>evil_script()</script>";
        const string RMAttention = @"rm -rf %userprofile%\Documents";
        const string RUNDLLAttention = @"Rundll32 sus.dll SusEntry";
        public enum STATUS
        {
            Processing,
            Complete
        }

        public ScanDto ScanDto { get; private set; }
        public STATUS Status { get; private set; }
        public string Dir { get; private set; }

        public int Id { get; private set; }
        public bool Alive { get; set; }

        public ScanTask(int id, string dir)
        {
            dir = Environment.ExpandEnvironmentVariables(dir);
            ScanDto = new ScanDto(directory: dir, processedFiles: 0, jSDetects: 0, rMRFDetects: 0, rundll32Detects: 0, errors: 0);
            Id = id;
            Dir = dir;
            Alive = true;
        }

        int FindJSScript(StreamReader fileStream)
        {
            int res = 0;
            string? line = "";
            while ((line = fileStream.ReadLine()) != null)
            {
                if (!Alive)
                    break;

                if (line.Contains(JSAttention))
                {
                    res++;
                }
            }
            return res;
        }

        /// <summary>
        /// Find rm attention or rundll attention
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns>(find attention type, count attentions)</returns>
        (int, int) FindRMORRundll(StreamReader fileStream)
        {
            string? line = "";
            int rmcnt = 0;
            int rdcnt = 0;
            while ((line = fileStream.ReadLine()) != null)
            {
                if (!Alive)
                    break;

                if (line.Contains(RMAttention))
                {
                    if (rdcnt != 0)
                        return (-1, -1);
                    rmcnt++;
                }
                else if (line.Contains(RUNDLLAttention))
                {
                    if (rmcnt != 0)
                        return (-1, -1);
                    rdcnt++;
                }
            }
            if (rmcnt > rdcnt)
                return (1, rmcnt);
            else
                return (2, rdcnt);
        }
        public void StartScan()
        {
            Status = STATUS.Processing;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (Directory.Exists(Dir))
            {
                foreach (string file in Directory.GetFiles(Dir))
                {
                    if (!Alive)
                        break;

                    try
                    {
                        StreamReader fileStream = new StreamReader(file);
                        if (Path.GetExtension(file) == ".js")
                        {
                            int cnt = 0;
                            if ((cnt = FindJSScript(fileStream)) != 0)
                            {
                                ScanDto.JSDetects += cnt;
                            }
                        }
                        else
                        {
                            (int tp, int cnt) = FindRMORRundll(fileStream);
                            if (tp == 1)
                            {
                                ScanDto.RMRFDetects += cnt;
                            }
                            else if (tp == 2)
                            {
                                ScanDto.Rundll32Detects += cnt;
                            }
                            else if (tp == -1)
                            {
                                ScanDto.Errors++;
                            }
                        }
                        fileStream.Close();
                    }
                    catch
                    {
                        ScanDto.Errors++;
                    }
                    ScanDto.ProcessedFiles++;
                }
            }
            else
            {
                ScanDto.Message = "Directory is not exist";
                ScanDto.Errors++;
            }
            stopwatch.Stop();
            ScanDto.ProcessingTime = stopwatch.Elapsed;
            Status = STATUS.Complete;
        }

    }
}
