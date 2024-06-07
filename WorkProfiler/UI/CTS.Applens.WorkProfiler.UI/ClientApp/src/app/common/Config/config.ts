// Copyright (c) Applens. All Rights Reserved.
import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';

@Injectable()

export class AppConfig {
  private _config: { [key: string]: any };

  constructor() {

    this._config = {
      DefaultController: 'Values/',
      httpOptions: {
        headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Access-Control-Allow-Origin': 'http://localhost:4200/' }),
        crossDomain: true, withCredentials: true
      },
      analystSelfService: {
        controllerName: 'TicketingModule/',
        GetProjectName: 'GetProjectName',
        GetCurrentTimeofTimeZones: 'GetCurrentTimeofTimeZones',
        GetAssigneNameByProjectID: 'GetAssigneNameByProjectID'
      },
      userDetails: {
        controllerName: 'TimeSheet/',
        GetHiddenDetails: 'Index'
      },
      dataDictionary: {
        controllerName: 'DebtOverideAndDataDictionary/',
        GetGriddetails: 'GetGriddetails',
        GetDropDownValuesDataDictionary: 'GetDropDownValuesDataDictionary',
        ProjectDebtDetails: 'ProjectDebtDetails',
        GetApplicationdetails: 'GetApplicationdetails/',
        GetResolutioncode: 'GetResolutioncode',
        GetCausecode: 'GetCausecode',
        GetDebtclassification: 'GetDebtclassification',
        GetAvoidableFlag: 'GetAvoidableFlag',
        GetResidualDebt: 'GetResidualDebt',
        Getreasonforresidual: 'Getreasonforresidual',
        AddApplicationDetails: 'AddApplicationDetails',
        GetDropDownValuesProjectPortfolio: 'GetDropDownValuesProjectPortfolio',
        GetConflictPatternDetailsForDownload: 'GetConflictPatternDetailsForDownload',
        DownloadDataDictionary: 'DownloadDataDictionary',
        Download: 'Download',
        DownloadTemplate: 'DownloadTemplate',
        DeleteDataDictionaryByID: 'DeleteDataDictionaryByID',
        GetDDErrorLogData: 'GetDDErrorLogData',
        AddReasonResidualAndCompDate: 'AddReasonResidualAndCompDate',
        DataDictionaryUploadByProject: 'DataDictionaryUploadByProject',
        SaveDataDictionaryByID: 'SaveDataDictionaryByID',
        ResidualDetail: 'ResidualDetail',
        UpdateSignOffDate: 'UpdateSignOffDate',
        GetTicketRoles: 'GetTicketRoles',
        GetRoles: 'GetRoles'
      },
      ticketeffortupload: {
        controllerName: 'TicketUpload/',
        ProjectDetails: 'ProjectDetails',
        ExportExcelClick: 'ExportExcelClick',
        DownloadEffortUploadTemplate: 'Download_EffortUploadTemplate',
        CheckITSM: 'CheckITSM',
        GetUploadConfigDetails: 'GetUploadConfigDetails',
        DownloadDebtTemplate: 'DownloadDebtTemplate',
        DownloadDebtUnClassifiedTickets: 'DownloadDebtUnClassifiedTickets',
        UploadforDebtUnclassifiedTicket: 'UploadforDebtUnclassifiedTicket',
        DownloadTicketTemplate: 'DownloadTicketTemplate',
        Download: 'Download',
        TicketExcelUpload: 'TicketExcelUpload',
        TriggerEffortUpload: 'TriggerEffortUpload',
        GetOnboardingPercentageDetails: 'GetOnboardingPercentageDetails'
      },
      errorlog: {
        controllerName: 'ErrorLog/',
        ProjectDetails: 'ProjectDetails',
        ErrorLog: 'ErrorLog',
        ExportExcelClick: 'ExportExcelClick',
        Download: 'Download',
        GetConfigDetails: 'GetConfigDetails'
      },
      ticketingmodule: {
        controllerName: 'TicketingModule/',
        GetMandatoryHours: 'MandatoryHours',
        GetEffortChartDetails: 'GetEffortDetailsChartnew',
        GetLanguageDetails: 'GetLanguageDetails',
        SaveLanguageDetails: 'SaveLanguageDetails',
        Getticketdetail: 'GetTicketGrid',
        UpdateWorkItemServiceandStatus: 'UpdateWorkItemServiceandStatus',
        UpdateIsAttributeByTicketId: 'UpdateIsAttributeByTicketId',
        GetAddTicketPopup: 'GetAddTicketPopup',
        GetTicketIdByCustomerID: 'GetTicketIdByCustomerID',
        GetDetailsByProjectID: 'GetDetailsByProjectID',
        GetDetailsAddTicket: 'GetDetailsAddTicket',
        AddTicketDetails: 'AddTicketDetails',
        ChooseUnAssignedTicket: 'ChooseUnAssignedTicket',
        GetNonTicketPopup: 'GetNonTicketPopup',
        SaveNonTicketDetails: 'AddNonTicket',
        SaveData: 'SaveData',
        GetInfoIconDetails: 'GetIconDetail',
        GetHiddenFields: 'GetHiddenFieldsforTM',
        GetTowerDetailsByProjectID: 'GetTowerDetailsByProjectID',
        GetApplicationDetailsByProject: 'GetApplicationDetailsByProject',
        GetTicketStatusByProject: 'GetTicketStatusByProject',
        GetSelectedTicketDetails: 'GetSelectedTicketDetails',
        GetAssignmentGroupByProject: 'GetAssignmentGroupByProject',
        GetWorkItempopup: 'GetWorkItempopup',
        GetDropDownValuesForWorkItem: 'GetDropDownValuesForWorkItem',
        GetDropDownValuesSprint: 'GetDropDownValuesSprint',
        CheckSprintName: 'CheckSprintName',
        CheckWorkItemId: 'CheckWorkItemId',
        SaveSprintDetails: 'SaveSprintDetails',
        AddWorkItem: 'AddWorkItem',
        GetProjectLeadDetails: 'GetProjectLeadDetails',
        GetProjectDetailsforDefaultLanding: 'GetProjectDetailsforDefaultLanding',
        GetDefaultLandingPageDetails: 'GetDefaultLandingPageDetails',
        SaveDefaultLandingPageDetails: 'SaveDefaultLandingPageDetails',
        Deletegriddata: "DeleteAutoAssignedTickets",
        GetUserProfilePicture: 'GetUserProfilePicture',
        GetMyAssociateURL: 'GetMyAssociateURL',
        GetHomePage: 'GetHomePage',
        GetCustomerwiseDefaultPage: 'GetCustomerwiseDefaultPage',
        GetCountErroredTickets: 'GetCountErroredTickets',
        GetNavMenuUrl: 'GetNavMenuUrl',
        GetFooterValue: 'FooterAPI/api/values'
      },
      ticketAttributes: {
        controllerName: 'TicketingModule/',
        GetAttributeDetails: 'GetAttributeDetails',
        PopupAttribute: 'PopupAttribute',
        TicketAttributeDetails: 'TicketAttributeDetails',
        GetResolutionPriority: 'GetResolutionPriority',
        CauseCodeResolutionCode: 'CauseCodeResolutionCode',
        InsertAttributeDetails: 'InsertAttributeDetails',
        CheckIsGracePeriodMet: 'CheckIsGracePeriodMet',
        GetAlgoKeyAndColumn: 'GetAlgoKeyAndColumn',
        NewAlgoClassification: 'NewAlgoClassification'
      },
      approveunfreeze: {
        controllerName: 'TimeSheet/',
        GetAssignessOrDefaulters: 'GetAssignessOrDefaulters',
        GetTimeSheetDataDaily: 'GetTimeSheetDataDaily',
        GetTimeSheetDataWeekly: 'GetTimeSheetDataWeekly',
        GetAssignessDownload: 'GetAssignessDownload',
        UpdateTimeSheetData: 'UpdateTimeSheetData',
        GetTicketDetailsPopUp: 'GetTicketDetailsPopUp',
        GetTicketDetailsForDownload: 'GetTicketDetailsForDownload',
        GetCalendarData: 'GetCalendarData'
      },
      continuouslearning: {
        controllerName: 'ContinuousLearning/',
        DownloadExcel: 'DownloadExcel'
      },
      Constants: {
        ApproveUnfreezeFilename: 'ApproveUnfreeze'
      },
      EffortTracking: {
        controllerName: 'EffortTracking/',
        GetTicketInfoDetails: 'GetTicketInfoDetails',

        ChooseTicketDetails: 'ChooseTicketDetails',
        GetCurrentTimeofTimeZones: 'GetCurrentTimeofTimeZones',
      },
      erroredTickets: {
        controllerName: 'TicketingModule/',
        GetErrorLogTickets: 'GetErrorLogTickets',
        LoadErrorLogGrid: 'LoadErrorLogGrid',
        GetDebtFields: 'GetDebtFields',
        GetErrorLogTicketData: 'GetErrorLogTicketData'
      },
      mainspring: {
        controllerName: 'MainSpring/',
        MpsFilters: 'MPSFilters',
        GetBaseMeasureFiltermainspringavailability: 'GetBaseMeasureFiltermainspringavailability',
        GetBaseMeasureFilterFrequencyList: 'GetBaseMeasureFilterFrequencyList',
        GetBaseMeasureFilterReportingPeriodList: 'GetBaseMeasureFilterReportingPeriodList',
        GetBaseMeasureFilterReportingPeriodListReport: 'GetBaseMeasureFilterReportingPeriodListReport',
        GetBaseMeasureFilterServiceList: 'GetBaseMeasureFilterServiceList',
        GetBaseMeasureFilterMetricsList: 'GetBaseMeasureFilterMetricsList',
        GetTicketSummaryFilterServiceList: 'GetTicketSummaryFilterServiceList',
        GetBaseMeasureProjectwiseSearchUserDefinedList: 'GetBaseMeasureProjectwiseSearchUserDefinedList',
        GetBaseMeasureProjectwiseSearchList : 'GetBaseMeasureProjectwiseSearchList',
        GetBaseMeasureLoadFactorProject: 'GetBaseMeasureLoadFactorProject',
        GetBaseMeasureProjectwiseSearchODCList: 'GetBaseMeasureProjectwiseSearchODCList',
        SaveBaseMeasureData: 'SaveBaseMeasureData',
        SaveUserDefinedBaseMeasure: 'SaveUserDefinedBaseMeasure',
        GetTicketSummeryBaseMeasureODCList: 'GetTicketSummeryBaseMeasureODCList',
        SaveTicketSummaryBaseMeasureODC: 'SaveTicketSummaryBaseMeasureODC',
          SaveLoadFactor: 'SaveLoadFactor',
          GetBaseMeasureValueLoadFactor: 'GetBaseMeasureValueLoadFactor'
      },
      SearchTicket: {
        controllerName: 'SearchTicket/',
        GetSearchTickets: 'GetSearchTickets',
        GetHierarchyList: 'GetHierarchyList',
        GetTicketTypes: 'GetTicketTypes',
        GetTicketStatus: 'GetTicketStatus',
        DownloadSearchTicket: 'DownloadSearchTicket'
      }
    };

  }

  get setting(): { [key: string]: any } {
    return this._config;
  }

  get(key: any): any {
    return this._config[key];
  }

}
