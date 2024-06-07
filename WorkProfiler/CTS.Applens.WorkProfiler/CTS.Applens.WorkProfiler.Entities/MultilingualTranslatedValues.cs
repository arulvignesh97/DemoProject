﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTS.Applens.WorkProfiler.Models
{
    public class MultilingualTranslatedValues
    {
        /// <summary>
        /// Gets or Sets TimeTickerID
        /// </summary>
        public long TimeTickerId { get; set; }
        /// <summary>
        /// Gets or Sets ProjectID
        /// </summary>
        public long ProjectId { get; set; }
        /// <summary>
        /// Gets or Sets IsTicketDescriptionUpdated
        /// </summary>
        public bool IsTicketDescriptionUpdated { get; set; }
        /// <summary>
        /// Gets or Sets TicketDescription
        /// </summary>
        public string TicketDescription { get; set; }
        /// <summary>
        /// Gets or Sets HasTicketDescriptionError
        /// </summary>
        public bool HasTicketDescriptionError { get; set; }
        /// <summary>
        /// Gets or Sets TranslatedTicketdescription
        /// </summary>
        public string TranslatedTicketDescription { get; set; } = string.Empty;
        /// <summary>
        /// Gets or Sets IsResolutionRemarksUpdated
        /// </summary>
        public bool IsResolutionRemarksUpdated { get; set; }
        /// <summary>
        /// Gets or Sets ResolutionRemarks
        /// </summary>
        public string ResolutionRemarks { get; set; }
        /// <summary>
        /// Gets or Sets HasResolutionRemarksError
        /// </summary>
        public bool HasResolutionRemarksError { get; set; }
        /// <summary>
        /// Gets or Sets TranslatedResolutionRemarks
        /// </summary>
        public string TranslatedResolutionRemarks { get; set; } = string.Empty;
        /// <summary>
        /// Gets or Sets IsTicketSummaryUpdated
        /// </summary>
        public bool IsTicketSummaryUpdated { get; set; }
        /// <summary>
        /// Gets or Sets HasTicketSummaryError
        /// </summary>
        public bool HasTicketSummaryError { get; set; }
        /// <summary>
        /// Gets or Sets TicketSummary
        /// </summary>
        public string TicketSummary { get; set; }
        /// <summary>
        /// Gets or Sets TranslatedTicketSummary
        /// </summary>
        public string TranslatedTicketSummary { get; set; } = string.Empty;
        /// <summary>
        /// Gets or Sets IsCommentsUpdated
        /// </summary>
        public bool IsCommentsUpdated { get; set; }
        /// <summary>
        /// Gets or Sets HasCommentsError
        /// </summary>
        public bool HasCommentsError { get; set; }
        /// <summary>
        /// Gets or Sets Comments
        /// </summary>
        public string Comments { get; set; }
        /// <summary>
        /// Gets or Sets TranslatedComments
        /// </summary>
        public string TranslatedComments { get; set; } = string.Empty;
        /// <summary>
        /// Gets or Sets IsFlexField1Updated
        /// </summary>
        public bool IsFlexField1Updated { get; set; }
        /// <summary>
        /// Gets or Sets HasFlexField1Error
        /// </summary>
        public bool HasFlexField1Error { get; set; }
        /// <summary>
        /// Gets or Sets FlexField1
        /// </summary>
        public string FlexField1 { get; set; }
        /// <summary>
        /// Gets or Sets TranslatedFlexField1
        /// </summary>
        public string TranslatedFlexField1 { get; set; } = string.Empty;
        /// <summary>
        /// Gets or Sets IsFlexField2Updated
        /// </summary>
        public bool IsFlexField2Updated { get; set; }
        /// <summary>
        /// Gets or Sets HasFlexField2Error
        /// </summary>
        public bool HasFlexField2Error { get; set; }
        /// <summary>
        /// Gets or Sets FlexField2
        /// </summary>
        public string FlexField2 { get; set; }
        /// <summary>
        /// Gets or Sets TranslatedFlexField2
        /// </summary>
        public string TranslatedFlexField2 { get; set; } = string.Empty;
        /// <summary>
        /// Gets or Sets IsFlexField3Updated
        /// </summary>
        public bool IsFlexField3Updated { get; set; }
        /// <summary>
        /// Gets or Sets HasFlexField3Error
        /// </summary>
        public bool HasFlexField3Error { get; set; }
        /// <summary>
        /// Gets or Sets FlexField3
        /// </summary>
        public string FlexField3 { get; set; }
        /// <summary>
        /// Gets or Sets TranslatedFlexField3
        /// </summary>
        public string TranslatedFlexField3 { get; set; } = string.Empty;
        /// <summary>
        /// Gets or Sets IsFlexField4Updated
        /// </summary>
        public bool IsFlexField4Updated { get; set; }
        /// <summary>
        /// Gets or Sets HasFlexField4Error
        /// </summary>
        public bool HasFlexField4Error { get; set; }
        /// <summary>
        /// Gets or Sets FlexField4
        /// </summary>
        public string FlexField4 { get; set; }
        /// <summary>
        /// Gets or Sets TranslatedFlexField4
        /// </summary>
        public string TranslatedFlexField4 { get; set; } = string.Empty;
        /// <summary>
        /// Gets or Sets IsCategoryUpdated
        /// </summary>
        public bool IsCategoryUpdated { get; set; }
        /// <summary>
        /// Gets or Sets Category
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// Gets or Sets HasCategoryError
        /// </summary>
        public bool HasCategoryError { get; set; }
        /// <summary>
        /// Gets or Sets TranslatedCategory
        /// </summary>
        public string TranslatedCategory { get; set; } = string.Empty;
        /// <summary>
        /// Gets or Sets IsTypeUpdated
        /// </summary>
        public bool IsTypeUpdated { get; set; }
        /// <summary>
        /// Gets or Sets HasTypeError
        /// </summary>
        public bool HasTypeError { get; set; }
        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Gets or Sets TranslatedType
        /// </summary>
        public string TranslatedType { get; set; } = string.Empty;
        /// <summary>
        /// Gets or Sets Languages
        /// </summary>
        public List<string> Languages { get; set; }
        /// <summary>
        /// Gets or Sets Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Gets or Sets SupportTypeID
        /// </summary>
        public int SupportTypeId { get; set; }

    }
}