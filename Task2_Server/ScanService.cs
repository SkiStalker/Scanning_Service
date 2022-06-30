using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Globalization;

namespace Task2_Kapserskiy
{
    class ScanService: IDisposable
    {
        Dictionary<int, ScanTask> scanTasks = new Dictionary<int, ScanTask>();
        public ScanService()
        {

        }
        public void Dispose()
        {
            foreach (KeyValuePair<int, ScanTask> task in scanTasks)
            {
                task.Value.Alive = false;
            }
            scanTasks.Clear();
        }


        public ScanDto AddTask(ScanRequest? scanRequest)
        {
            if (scanRequest == null)
                return new ScanDto(message: "Incorrect request");

            if (scanRequest.Argument != null)
            {
                int id = scanTasks.Count;
                ScanTask curScanTask = new ScanTask(id, scanRequest.Argument);
                scanTasks.Add(id, curScanTask);

                new Thread(curScanTask.StartScan).Start();

                return new ScanDto(message: "Scan task was created with ID:", id: id);
            }
            return new ScanDto(message: "Null scanning argument");
        }

        public ScanDto GetTask(ScanRequest? scanRequest)
        {
            if (scanRequest == null)
                return new ScanDto(message: "Incorrect request");
            if (scanRequest.Argument != null)
            {
                if (int.TryParse(scanRequest.Argument, System.Globalization.NumberStyles.Integer, CultureInfo.CurrentCulture, out int id))
                {
                    ScanTask? scanTask = null;
                    if (scanTasks.TryGetValue(id, out scanTask))
                    {
                        if (scanTask.Status == ScanTask.STATUS.Complete)
                        {
                            return scanTask.ScanDto;
                        }
                        else
                        {
                            return new ScanDto(message: "Scan task in progress, please wait");
                        }
                    }
                    else
                    {
                        return new ScanDto(message: "Cannot find task by id");
                    }
                }
                else
                {
                    return new ScanDto(message: "Incorrect id");
                }
            }
            return new ScanDto(message: "Null status argument");
        }
    }
}
