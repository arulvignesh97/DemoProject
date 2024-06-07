using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// class Used for view all screen
    /// </summary>
    public class SpDebtMLPatternValidationModelForViewAll
    {
        private List<SpDebtMLPatternValidationModel> existingPatternsModel = new List<SpDebtMLPatternValidationModel>();
        /// <summary>
        /// This list is used to get exisiting patterns
        /// </summary>
        public List<SpDebtMLPatternValidationModel> ExistingPatternsModel
        {
            get
            {
                return existingPatternsModel;
            }
            set
            {
                existingPatternsModel = value;
            }
        }

        private List<SpDebtMLPatternValidationModel> newPatternsModel = new List<SpDebtMLPatternValidationModel>();
        /// <summary>
        /// This list is used to get new patterns model
        /// </summary>
        public List<SpDebtMLPatternValidationModel> NewPatternsModel
        {
            get
            {
                return newPatternsModel;
            }
            set
            {
                newPatternsModel = value;
            }
        }


    }
}
