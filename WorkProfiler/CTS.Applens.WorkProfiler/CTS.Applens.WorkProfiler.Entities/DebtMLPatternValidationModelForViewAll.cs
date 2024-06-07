using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class used for view all functionality
    /// </summary>
    public class DebtMLPatternValidationModelForViewAll
    {
        private List<DebtMLPatternValidationModel> existingPatterns = new List<DebtMLPatternValidationModel>();
        /// <summary>
        /// Used to get and set exisiting patterns in ViewAll
        /// </summary>
        public List<DebtMLPatternValidationModel> ExistingPatterns
        {
            get
            {
                return existingPatterns;
            }
            set
            {
                existingPatterns = value;
            }
        }

    }
}
