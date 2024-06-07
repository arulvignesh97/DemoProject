// Copyright (c) Applens. All Rights Reserved.
export class Constants {

  public static imageData = 'data:image/Bmp;base64,';
  public static dateFormat = 'MM/dd/yyyy';
  public static dateTimeFormat = 'MM/dd/yyyy hh:mm:ss';
  public static dateInputFormat = 'MM/DD/YYYY';
  public static dateTimeZoneFormat = 'MM/dd/yyyy hh:mm:ss a';
  public static dateInputFormatApprove = 'dd/MM/yyyy';
  public static ExcelType = 'application/vnd.ms-excel';
  public static hiddenfields = 'hiddenfields';
  public static lifeSpan = 5000;
  public static rowsPerPage = [100,200,300,400];

  public static DataDictionaryDeleteWarning = 'Please choose atleast one checkbox';
  public static greenBg = 'clsBgGreen';
  public static redBg = 'clsBgRed';
  public static greyBg = 'clsBgGrey';
  public static mode = 'GetApplications';
  public static toggleIconOn = 'fa fa-toggle-on';
  public static toggleiconOff = 'fa fa-toggle-off';

  public static EffortFileName = 'EffortDetails.xlsx';
  public static ColumnMappingNotDoneInITSMConfiguration = 'Column Mapping has not been done in ITSM Configuration.';
  public static ColumnMappingNotDoneInITSMConfiguration1 = 'Column Mapping has not been done in ITSM Configuration';
  public static ProblemWithDownload = 'Problem With Download';
  public static FileDoesNotExists = 'File does not exists';
  public static PleaseUploadValidTemplate = 'Please upload Valid template, valid file is .xlsx';
  public static DumpUploadFailed = 'Dump Upload Failed.Please check e-mail.';
  public static TemplateNotMatchWithITSM = 'Template is not matching with ITSM configuration Column mapping. Please upload valid template.';
  public static UploadedSuccessfully = 'Uploaded successfully';
  public static Successfullydone = 'Successfullydone';
  public static UploadedSuccessfullyCheckErrorLog = 'Uploaded successfully. Please check error log for failed tickets.';
  public static FileFormat = '.xlsx';
  public static FileFormatxl = '.xlsm';
  public static MoreRecords = 'Timesheet with more than 2000 records cannot be uploaded through UI';
  public static emptyTemplate = 'Upload failed.Template should not be empty';
  public static exemptedMessage='Selected project has got exemption from using AppLens, Please reach out to your project manager for further queries';
  
  public static Ticketdetails ='TicketDetails_';  
  public static datadic ='DataDictionary_';
  //SearchTicket
  public static ExcelMacroType = 'application/vnd.ms-excel.sheet.macroEnabled.12';
  public static ExcelExtension = '.xlsx';
  public static SearchTicket = 'SearchTicket';
  public static MandatoryMsg = 'Choose at least one project';
}
export class AppRoutes {
  public static timesheetEntry = 'timesheet-entry';
  public static datadictionary = 'data-dictionary';
  public static ticketeffortupload = 'ticketeffortupload/:id';
  public static errorlog = 'errorlog/:id';
  public static debtreview = 'debt-review';
  public static basemeasures = 'basemeasures';
  public static approveunfreeze = 'approveunfreeze';
}
