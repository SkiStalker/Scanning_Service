using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;

namespace Task2_Kapserskiy
{
    [Serializable]
    class ScanDto
    {
        public ScanDto(string? message = null, int? id = null, int? processedFiles = null, int? jSDetects = null,
            int? rMRFDetects = null, int? rundll32Detects = null, int? errors = null, TimeSpan? processingTime = null, string? directory = null)
        {
            Message = message;
            ID = id;
            ProcessedFiles = processedFiles;
            JSDetects = jSDetects;
            RMRFDetects = rMRFDetects;
            Rundll32Detects = rundll32Detects;
            Errors = errors;
            ProcessingTime = processingTime;
            Directory = directory;
        }
        public string? Message { get; set; }
        public string? Directory { get; set; }
        public int? ID { get; set; }
        public int? ProcessedFiles { get; set; }
        public int? JSDetects { get; set; }
        public int? RMRFDetects { get; set; }
        public int? Rundll32Detects { get; set; }
        public int? Errors { get; set; }
        public TimeSpan? ProcessingTime {get; set;}

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (Message != null)
                stringBuilder.AppendLine(Message);

            if (ID != null)
                stringBuilder.AppendLine(ID.ToString());

            if (Directory != null)
                stringBuilder.AppendLine("Directory: " + Directory);

            if (ProcessedFiles != null)
                stringBuilder.AppendLine("Processed files: " + ProcessedFiles);

            if (JSDetects != null)
                stringBuilder.AppendLine("JS detects: " + JSDetects);

            if (RMRFDetects != null)
                stringBuilder.AppendLine("rm -rf detects: " + RMRFDetects);

            if (Rundll32Detects != null)
                stringBuilder.AppendLine("Rundll32 detects: " + RMRFDetects);

            if (Errors != null)
                stringBuilder.AppendLine("Errors: " + Errors);

            if (ProcessingTime != null)
                stringBuilder.AppendLine("Exection time:" + ProcessingTime);

            return stringBuilder.ToString();
        }
    }
}
