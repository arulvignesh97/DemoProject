using CTS.Applens.WorkProfiler.Models;
using System;
using System.Collections.Generic;
using TimeSheet = CTS.Applens.WorkProfiler.DAL.TimeSheetRepository;
namespace CTS.Applens.WorkProfiler.Repository
{
    /// <summary>
    /// Class file for holding the Business Logics for Timesheet related functionalities
    /// </summary>
    public class TimeSheetRepository
    {
        /// <summary>
        /// GetAssignessOrDefaulters
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public List<AssignessOrDefaulters> GetAssignessOrDefaulters(ApprovalUnfreezeInputParams2 InputParameter)
        {
            try
            {
                return new TimeSheet().GetAssignessOrDefaulters(InputParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetTimeSheetData
        /// </summary>
        /// <param name="InputParameter"></param>
        /// <returns></returns>
        public List<TimeSheetData> GetTimeSheetData(ApprovalUnfreezeInputParams InputParameter)
        {
            try
            {
                return new TimeSheet().GetTimeSheetData(InputParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetTimeSheetDataDaily
        /// </summary>
        /// <param name="InputParameter"></param>
        /// <returns></returns>
        public List<TimeSheetDataDaily> GetTimeSheetDataDaily(ApprovalUnfreezeInputParams InputParameter)
        {
            try
            {
                return new TimeSheet().GetTimeSheetDataDaily(InputParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To UpdateTimeSheetData
        /// </summary>
        /// <param name="lstApproveUnfreezeTimesheet"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public bool UpdateTimeSheetData(List<ApproveUnfreezeTimesheet> lstApproveUnfreezeTimesheet, Int64 CustomerID, bool isDaily, string userid, string access)
        {
            try
            {
                return new TimeSheet().UpdateTimeSheetData(lstApproveUnfreezeTimesheet, CustomerID, isDaily,userid, access);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetCalendarData
        /// </summary>
        /// <param name="InputParameter"></param>
        /// <returns></returns>
        public List<CalendarViewData> GetCalendarData(ApprovalUnfreezeInputParams2 InputParameter)
        {
            try
            {
                return new TimeSheet().GetCalendarData(InputParameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetTicketDetailsForApprovalUnfreeze
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="SubmitterId"></param>
        /// <returns></returns>
        public List<TicketDetails> GetTicketDetailsForApprovalUnfreeze(int CustomerID,
            string FromDate, string ToDate, string SubmitterId,string TsApproverId)
        {
            try
            {
                return new TimeSheet().GetTicketDetailsForApprovalUnfreeze(CustomerID, FromDate, ToDate, SubmitterId, TsApproverId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetTicketDetailsPopUp
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="SubmitterId"></param>
        /// <returns></returns>
        public List<TicketDetails> GetTicketDetailsPopUp(int CustomerID, string FromDate, string ToDate,
            string SubmitterId,string TsApproverId)
        {
            try
            {
                return new TimeSheet().GetTicketDetailsPopUp(CustomerID, FromDate, ToDate, SubmitterId, TsApproverId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method Is Used To GetTicketDetailsForDownload
        /// </summary>
        /// <param name="lstobject"></param>
        /// <param name="DestinationTemplateFileName"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public byte[] GetTicketDetailsForDownload(List<TicketDetails> lstobject, string DestinationTemplateFileName,
            int CustomerID, bool IsCognizant, bool IsADMApplicableforCustomer)
        {
            try
            {
                return new TimeSheet().GetTicketDetailsForDownload(lstobject, DestinationTemplateFileName, CustomerID, IsCognizant, IsADMApplicableforCustomer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
