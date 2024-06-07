using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Models.Work_Items
{
    public class WorkItemModel
    {
        public List<MandateAttributes> LstMandateAttributes { get; set; }
        public List<NameIDModel> LstPriority { get; set; }
        public List<NameIDModel> LstStatus { get; set; }
    }
}
