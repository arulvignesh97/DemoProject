
namespace CTS.Applens.WorkProfiler.Repository
{
    using Models.ServiceClassification;
    using System;
    using System.Collections.Generic;
    using ServiceClassification = CTS.Applens.WorkProfiler.DAL.ServiceClassificationRepository;
    public class ServiceClassificationRepository
    {
        public List<ServiceClassificationDictionaryDetails> GetDictionaryDetails()
        {
            try
            {
                return new ServiceClassification().GetDictionaryDetails();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ServiceTicketDetails> GetTicketDetails()
        {
            try
            {
                return new ServiceClassification().GetTicketDetails();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ServiceNameUpdation(List<UpdateServiceName> updServiceName,List<ServiceAutoClassifiedTicketDetails> deleteTempTable)
        {
            try
            {
                new ServiceClassification().ServiceNameUpdation(updServiceName, deleteTempTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<UpdateServiceName> ServiceAutoClassificationAPI(List<ServiceTicketDetails> tickketDetails)
        {
            try
            {
                return new ServiceClassification().ServiceAutoClassificationAPI(tickketDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }

    }
}

