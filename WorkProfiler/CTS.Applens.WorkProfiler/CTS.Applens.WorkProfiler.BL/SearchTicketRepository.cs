namespace CTS.Applens.WorkProfiler.Repository
{
    using CTS.Applens.WorkProfiler.Models.SearchTicket;
    using Models.SearchTicket;
    using System;
    using System.Collections.Generic;
    using SearchTicket = CTS.Applens.WorkProfiler.DAL.SearchTicketRepository;
    /// <summary>
    /// The Class SearchTicketRepository holds methods to interact with database for 
    /// Search Tickets related functionalities.
    /// </summary>
    public class SearchTicketRepository
    {
        /// <summary>
        /// Method to get all active Ticket Types mapped to project.
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns>List of active Ticket Types</returns>
        public List<TicketTypes> GetTicketTypes(SearchTicketTypeParameter objSearchTicketType)
        {
            try
            {
                return new SearchTicket().GetTicketTypes(objSearchTicketType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        /// <summary>
        /// Method to get all active Ticket Statuses mapped to project.
        /// </summary>
        /// <param name="ProjectID">Project ID</param>
        /// <returns>List of all active Ticket Statuses</returns>
        public List<TicketStatus> GetTicketStatus(ProjectIDs searchTicketParameters)
        {
            try
            {
                return new SearchTicket().GetTicketStatus(searchTicketParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to get Search Ticket Details based on search parameters.
        /// </summary>
        /// <param name="searchTicketParameters">Search Ticket Parameters</param>
        /// <returns>List of Search Ticket Details.</returns>
        public List<SearchTicketDetails> GetSearchTickets(
            SearchTicketParameters searchTicketParameters, byte[] aesKey, string encryptionEnabled)
        {
            try
            {
                return new SearchTicket().GetSearchTickets(searchTicketParameters, aesKey, encryptionEnabled);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to get the Business Cluster Data.        
        /// </summary>
        /// <param name="customerID">Customer ID</param>
        /// <param name="associateID">Associate ID</param>
        /// <returns>Business Cluster Data</returns>
        public ProjectBusinessCluster GetBusinessClusterData(string customerID, string associateID)
        {
            try
            {
                return new SearchTicket().GetBusinessClusterData(customerID, associateID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DownloadSearchTicket(SearchTicketParameters searchTicketParameters, byte[] aesKey, string encryptionEnabled)
        {
            if (searchTicketParameters != null)
            {
                return new SearchTicket().DownloadSearchTicket(searchTicketParameters, aesKey, encryptionEnabled, "SearchTicket", "SearchTicketData");
            }
            else
            {
                return null;
            }
        }
    }
}

