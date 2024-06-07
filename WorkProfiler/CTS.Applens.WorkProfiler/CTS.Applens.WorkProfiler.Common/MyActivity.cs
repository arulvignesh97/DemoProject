using CTS.Applens.Framework;
using CTS.Applens.WorkProfiler.Entities;
using CTS.Applens.WorkProfiler.Entities.Base;
using CTS.Applens.WorkProfiler.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CTS.Applens.WorkProfiler.Common
{
    public class MyActivity
    {
        public string MySaveActivity(MyActivitySourceDto myActivitySources, string access)
        {
            string result = "";
            HttpClientHandler handler = new HttpClientHandler { UseDefaultCredentials = true };
            using var client = new HttpClient(handler);
            string url = new AppSettings().AppsSttingsKeyValues["MyActivityURL"] + ApplicationConstants.MyActivityURLConst;
            client.BaseAddress = new Uri(url);
            bool keyCloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["KeyCloakEnabled"], CultureInfo.CurrentCulture);
            KeyCloakTokenHelper.SetTokenOnHeader(client, access, keyCloakEnabled);
            MyActivitySourceDto myActivitySource = new MyActivitySourceDto
            {
                ActivityDescription = myActivitySources.ActivityDescription,
                WorkItemCode = myActivitySources.WorkItemCode,
                SourceRecordID = myActivitySources.SourceRecordID,
                ActivityTo = myActivitySources.ActivityTo,
                RequestorJson = myActivitySources.RequestorJson,
                ActivityInfo = "",
                RaisedOnDate = myActivitySources.RaisedOnDate,
                Navigation = new AppSettings().AppsSttingsKeyValues["MyActivityNavigation"],
                MailContent = null,
                MailTo = null,
                RaisedByName = "",
                CreatedBy = myActivitySources.CreatedBy

            };
            var json = JsonConvert.SerializeObject(myActivitySource);
            var response = client.PostAsync(new Uri(new SanitizeString(url).Value), new StringContent(json, Encoding.UTF8, "application/json")).Result;
            result = response.IsSuccessStatusCode.ToString(CultureInfo.CurrentCulture);
            return result;
        }
        public List<ActivityConfigurationsModel> GetActivityConfigurations(string access)
        {
            List<ActivityConfigurationsModel> result = new List<ActivityConfigurationsModel>();
            HttpClientHandler handler = new HttpClientHandler { UseDefaultCredentials = true };
            using var client = new HttpClient(handler);
            string url = new AppSettings().AppsSttingsKeyValues["MyActivityURL"] + ApplicationConstants.GETActivityURL;
            client.BaseAddress = new Uri(url);
            bool keyCloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["KeyCloakEnabled"], CultureInfo.CurrentCulture);
            KeyCloakTokenHelper.SetTokenOnHeader(client, access, keyCloakEnabled);
            var response = client.GetAsync(new Uri(url)).Result;
            result = JsonConvert.DeserializeObject<List<ActivityConfigurationsModel>>(response.Content.ReadAsStringAsync().Result);

            return result;
        }
        public string UpdateActivityToExpiry(UpdateActivityToExpiryModel toExpiryModel, string access)
        {
            string result = "";
            HttpClientHandler handler = new HttpClientHandler { UseDefaultCredentials = true };
            using var client = new HttpClient(handler);
            string url = new AppSettings().AppsSttingsKeyValues["MyActivityURL"] + ApplicationConstants.ExpiryActivityURL;
            client.BaseAddress = new Uri(url);
            bool keyCloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["KeyCloakEnabled"], CultureInfo.CurrentCulture);
            KeyCloakTokenHelper.SetTokenOnHeader(client, access, keyCloakEnabled);
            UpdateActivityToExpiryModel expiryModel = new UpdateActivityToExpiryModel
            {
                WorkItemCode = toExpiryModel.WorkItemCode,
                SourceRecordId = toExpiryModel.SourceRecordId,
                ModifiedBy = toExpiryModel.ModifiedBy
            };
            var json = JsonConvert.SerializeObject(expiryModel);
            var response = client.PostAsync(new Uri(new SanitizeString(url).Value), new StringContent(json, Encoding.UTF8, "application/json")).Result;
            result = response.IsSuccessStatusCode.ToString(CultureInfo.CurrentCulture);

            return result;
        }
        public string ExpireBasedOnActivity(ActivityBasedExpiryModel toexpiryModel, string access)
        {
            string result = "";
            HttpClientHandler handler = new HttpClientHandler { UseDefaultCredentials = true };
            using var client = new HttpClient(handler);
            string url = new AppSettings().AppsSttingsKeyValues["MyActivityURL"] + ApplicationConstants.ExpireBasedOnActivityURL;
            client.BaseAddress = new Uri(url);
            bool keyCloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["KeyCloakEnabled"], CultureInfo.CurrentCulture);
            KeyCloakTokenHelper.SetTokenOnHeader(client, access, keyCloakEnabled);
            ActivityBasedExpiryModel expiryModel = new ActivityBasedExpiryModel
            {
                ActivityID = toexpiryModel.ActivityID,
                ActivityTo = toexpiryModel.ActivityTo,
                ModifiedBy = toexpiryModel.ModifiedBy
            };
            var json = JsonConvert.SerializeObject(expiryModel);
            var response = client.PostAsync(new Uri(new SanitizeString(url).Value), new StringContent(json, Encoding.UTF8, "application/json")).Result;
            result = response.IsSuccessStatusCode.ToString(CultureInfo.CurrentCulture);

            return result;
        }
        public List<ExistingAcitivityDetailsModel> GetExistingActivitys(long sourceRecordId, string workitemCode, string access)
        {
            List<ExistingAcitivityDetailsModel> result = new List<ExistingAcitivityDetailsModel>();
            HttpClientHandler handler = new HttpClientHandler { UseDefaultCredentials = true };
            using var client = new HttpClient(handler);
            string baseurl = new AppSettings().AppsSttingsKeyValues["MyActivityURL"];
            string url = Convert.ToString(baseurl + ApplicationConstants.GETExistingActivitysURL + "?sourceRecordId=" + sourceRecordId + "&workitemCode=" + workitemCode);
            client.BaseAddress = new Uri(url);
            bool keyCloakEnabled = Convert.ToBoolean(new AppSettings().AppsSttingsKeyValues["KeyCloakEnabled"], CultureInfo.CurrentCulture);
            KeyCloakTokenHelper.SetTokenOnHeader(client, access, keyCloakEnabled);
           // client.Timeout = TimeSpan.FromMinutes(15);
            var response = client.GetAsync(new Uri(url));
            response.Wait();
            var resultmsg=response.Result;
            result = JsonConvert.DeserializeObject<List<ExistingAcitivityDetailsModel>>(resultmsg.Content.ReadAsStringAsync().Result);
            return result;
        }
        public string GetWorkItemcode(string Workitemname, string ActivityTypecode, string access)
        {
            List<ActivityConfigurationsModel> SaveactivityConfigurations = new List<ActivityConfigurationsModel>();
            SaveactivityConfigurations = new MyActivity().GetActivityConfigurations(access);
            string Workitemcode = "";
            Workitemcode = SaveactivityConfigurations.First(i => i.ModuleCode == "MOD0001" && i.WorkItemName == Workitemname && i.ActivityTypeCode == ActivityTypecode).WorkItemCode;
            return Workitemcode;
        }
        public bool CheckexistingActivity(Int64 UniqueSourceid, string Workitemcode, string access)
        {
            bool CheckSourceidstatus = false;
            List<ExistingAcitivityDetailsModel> existingAcitivities = new List<ExistingAcitivityDetailsModel>();
            existingAcitivities = GetExistingActivitys(UniqueSourceid, Workitemcode, access);
            if (existingAcitivities != null)
            {
                CheckSourceidstatus = existingAcitivities.Any(i => i.SourceRecordID == UniqueSourceid);
            }
            return CheckSourceidstatus;
        }
    }
}
