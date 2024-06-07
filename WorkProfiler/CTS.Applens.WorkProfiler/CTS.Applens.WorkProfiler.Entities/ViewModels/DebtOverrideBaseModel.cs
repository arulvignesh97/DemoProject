using CTS.Applens.WorkProfiler.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CTS.Applens.WorkProfiler.Entities.ViewModels
{
    public class DebtOverRideReviewModel
    {
        public string EmployeeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<DebtOverrideReview> DebtOverRideReviews { get; set; }
    }
    public class SearchModel
    {
        [DataType(DataType.Text)]
        public string StartDate { get; set; }
        [DataType(DataType.Text)]
        public string EndDate { get; set; }
        [DataType(DataType.Text)]
        public Int64 CustomerID { get; set; }
        [DataType(DataType.Text)]
        public string EmployeeID { get; set; }
        [DataType(DataType.Text)]
        public long ProjectID { get; set; }
        [DataType(DataType.Text)]
        public int ReviewStatus { get; set; }
        [DataType(DataType.Text)]
        public int isCognizant { get; set; }
    }
    public class DebtReviewUpload
    {
        [DataType(DataType.Text)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Text)]
        public DateTime CloseDate { get; set; }
        [DataType(DataType.Text)]
        public long ProjectID { get; set; }
        [DataType(DataType.Text)]
        public int IsCognizant { get; set; }
        [DataType(DataType.Text)]
        public int ReviewStatus { get; set; }
        [DataType(DataType.Text)]
        public string EmployeeID { get; set; }
    }
    public class DebtReviewPostModel
    {
        public List<IFormFile> files { get; set; }

        [MaxLength(int.MaxValue)]  
        public string debtReviewUpload { get; set; }
    }

    public class ApproveDebtModel
    {
        [DataType(DataType.Text)]
        public string ticketDetails { get; set; }
        [DataType(DataType.Text)]
        public string EmployeeID { get; set; }
        [DataType(DataType.Text)]
        public long ProjectID { get; set; }
    }
}
