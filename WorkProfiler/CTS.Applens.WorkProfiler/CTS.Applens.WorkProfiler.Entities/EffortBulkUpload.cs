using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Models
{
    public class EffortBulkUpload
    {
        public Int32 ProjectID { get; set; }
        public string EsaProjectID { get; set; }
        public string SharePathName { get; set; }

        public bool IsCognizant { get; set; }

        public bool IsEffortTrackActivityWise { get; set; }
        public bool IsDaily { get; set; }        

    }

    public class EfforUploadTracker
    {
        public Int64 ID { get; set; }

        public string EmployeeID { get; set; }

        public string ProjectID { get; set; }

        public string EffortUploadDumpFileName { get; set; }

        public string EffortUploadErrorDumpFile { get; set; }

        public string Status { get; set; }

        public string FilePickedTime { get; set; }

        public string APIRequestedTime { get; set; }

        public string APIRespondedTime { get; set; }
        public bool IsActive { get; set; }

        public string Remarks { get; set; }
    }
}
