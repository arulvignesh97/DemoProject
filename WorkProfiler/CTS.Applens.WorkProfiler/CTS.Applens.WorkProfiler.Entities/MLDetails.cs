using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Models
{
    /// <summary>
    /// This class holds ML Details informations
    /// </summary>
    public class MLDetails
    {
        /// <summary>
        /// Gets or sets StartDate
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// Gets or sets EndDate
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// Gets or sets IsAutoClassified
        /// </summary>
        public string IsAutoClassified { get; set; }
        /// <summary>
        /// Gets or sets IsAutoClassified
        /// </summary>
        public string IsMLSignoff { get; set; }
        /// <summary>
        /// Gets or sets IsAutoClassified
        /// </summary>
        public int? OptionalFieldId { get; set; }
        /// <summary>
        /// Gets or sets lstOptionalFields
        /// </summary>
        public List<ProjOptionalFields> LstOptionalFields { get; set; }
        /// <summary>
        /// Gets or sets Validation info
        /// </summary>
        public ILValidationResult Result{ get; set; }
}
    /// <summary>
    /// This class holds MLDetailsParam details
    /// </summary>
    public class MLDetailsParam
    {
        /// <summary>
        /// Gets or sets ProjectID
        /// </summary>
        public Int64 ProjectId { get; set; }
        /// <summary>
        /// Gets or sets SupportTypeID
        /// </summary>
        public Int32 SupportTypeId { get; set; }
        /// <summary>
        /// Gets or sets AssociateID
        /// </summary>
        [MaxLength(50)]
        public string AssociateId { get; set; }
        /// <summary>
        /// Gets or sets DateFrom
        /// </summary>
        [MaxLength(100)]
        public string DateFrom { get; set; }
        /// <summary>
        /// Gets or sets DateTo
        /// </summary>
        [MaxLength(100)]
        public string  DateTo { get; set; }
        /// <summary>
        /// Gets or sets OptFieldProjID
        /// </summary>
        [MaxLength(100)]
        public string OptFieldProjId { get; set; }
        /// <summary>
        /// Gets or sets UserID
        /// </summary>
        [MaxLength(400)]
        public string UserId { get; set; }

    }
}

