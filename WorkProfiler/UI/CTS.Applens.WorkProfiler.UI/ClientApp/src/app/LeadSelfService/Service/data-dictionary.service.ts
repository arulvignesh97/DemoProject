// Copyright (c) Applens. All Rights Reserved.
import { AppSettingsConfig } from './../../app.settings.config';
import { AppConfig } from './../../common/Config/config';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Constants } from 'src/app/common/constants/constants';

@Injectable({
  providedIn: 'root'
})
export class DataDictionaryService {

datadictionary: any;
apiURL: string;

constructor(private http: HttpClient, private config: AppConfig) {
  this.apiURL = AppSettingsConfig.settings.API;
  this.datadictionary = this.config.setting.dataDictionary;
}

getHttpParams(param): HttpParams {
  let httpparams = new HttpParams();
  Object.keys(param).forEach(x => {
    httpparams = httpparams.set(x, param[x]);
  });
  return httpparams;
}

GetGriddetails(param: any): Observable<any> {
  return this.http.post(
    this.apiURL +
    this.datadictionary.controllerName + this.datadictionary.GetGriddetails,
    param
  );
}

ProjectDebtDetails(param: any): Observable<any> {
  const getParams = this.getHttpParams(param);
  return this.http.get(
   this.apiURL +
   this.datadictionary.controllerName + this.datadictionary.ProjectDebtDetails,
   {params: getParams}
 );
}

GetDropDownValuesDataDictionary(param: any): Observable<any> {
  return this.http.post(
    this.apiURL +
    this.datadictionary.controllerName + this.datadictionary.GetDropDownValuesDataDictionary,
    param
  );
}
GetApplicationdetails(projectId: any): Observable<any> {
  //const getParams = this.getHttpParams(param);
  return this.http.get(
    this.apiURL +
    this.datadictionary.controllerName + this.datadictionary.GetApplicationdetails + projectId,
   // {params: getParams}
  );
}
GetDropDownValuesProjectPortfolio(param: any): Observable<any> {
  const getParams = this.getHttpParams(param);
  return this.http.get(
   this.apiURL +
   this.datadictionary.controllerName + this.datadictionary.GetDropDownValuesProjectPortfolio,
   {params: getParams}
 );
}
GetResolutioncode(param: any): Observable<any> {
  const getParams = this.getHttpParams(param);
  return this.http.get(
    this.apiURL +
    this.datadictionary.controllerName + this.datadictionary.GetResolutioncode,
    {params: getParams}
  );
}

GetCausecode(param: any): Observable<any> {
  const getParams = this.getHttpParams(param);
  return this.http.get(
    this.apiURL +
    this.datadictionary.controllerName + this.datadictionary.GetCausecode,
    {params: getParams}
  );
}
GetDebtclassification(): Observable<any> {
  return this.http.get(
    this.apiURL +
    this.datadictionary.controllerName + this.datadictionary.GetDebtclassification
  );
}

GetAvoidableFlag(): Observable<any> {
  return this.http.get(
    this.apiURL +
    this.datadictionary.controllerName + this.datadictionary.GetAvoidableFlag
  );
}

GetResidualDebt(): Observable<any> {
  return this.http.get(
    this.apiURL +
    this.datadictionary.controllerName + this.datadictionary.GetResidualDebt
  );
}

Getreasonforresidual(param: any): Observable<any> {
  const getParams = this.getHttpParams(param);
  return this.http.get(
    this.apiURL +
    this.datadictionary.controllerName + this.datadictionary.Getreasonforresidual,
    {params: getParams}
  );
}

AddApplicationDetails(param: any): Observable<any> {
  return this.http.post(
  this.apiURL +
  this.datadictionary.controllerName + this.datadictionary.AddApplicationDetails,
  param
);
}

SaveDataDictionaryByID(param: any): Observable<any> {
  return this.http.post(
  this.apiURL +
  this.datadictionary.controllerName + this.datadictionary.SaveDataDictionaryByID,
  param
);
}

AddReasonResidualAndCompDate(param: any): Observable<any> {
  return this.http.post(
  this.apiURL +
  this.datadictionary.controllerName + this.datadictionary.AddReasonResidualAndCompDate,
  param
);
}
UpdateSignOffDate(param: any): Observable<any> {
  return this.http.post(
  this.apiURL +
  this.datadictionary.controllerName + this.datadictionary.UpdateSignOffDate,
  param
);
}
DataDictionaryUploadByProject(data, ProjectID, EmployeeID): Observable<any> {
  const qstring = '?ProjectID=' + ProjectID + '&EmployeeID=' + EmployeeID;
  return this.http.post(
  this.apiURL +
  this.datadictionary.controllerName + this.datadictionary.DataDictionaryUploadByProject + qstring,
  data,
);
}

DeleteDataDictionaryByID(param: any): Observable<any> {
  return this.http.post(
  this.apiURL +
  this.datadictionary.controllerName + this.datadictionary.DeleteDataDictionaryByID,
  param
);
}
GetResidualDetail(param: any): Observable<any> {
  const getParams = this.getHttpParams(param);
  return this.http.get(
    this.apiURL +
    this.datadictionary.controllerName + this.datadictionary.ResidualDetail,
    {params: getParams}
  );
}
GetDDErrorLogData(param: any): Observable<any> {
  const getParams = this.getHttpParams(param);
  return this.http.get(
    this.apiURL +
    this.datadictionary.controllerName + this.datadictionary.GetDDErrorLogData,
    {params: getParams}
  );
}

GetConflictPatternDetailsForDownload(param: any): Observable<any> {
  const getParams = this.getHttpParams(param);
  return this.http.get(
    this.apiURL +
    this.datadictionary.controllerName + this.datadictionary.GetConflictPatternDetailsForDownload,
    {params: getParams}
  );
}
DownloadDataDictionary(param: any): Observable<any> {
  const getParams = this.getHttpParams(param);
  return this.http.get(
    this.apiURL +
    this.datadictionary.controllerName + this.datadictionary.DownloadDataDictionary,
    {params: getParams}
  );
}
DownloadTemplate(param: any): Observable<any> {
  const headerOptions = new HttpHeaders({
    'Content-Type': 'application/json',
    Accept: Constants.ExcelType
  });
  const requestOptions = { headers: headerOptions, responseType: 'blob' as 'blob'};
  return this.http.post
    (
      this.apiURL +
      this.datadictionary.controllerName +
      this.datadictionary.DownloadTemplate, param, requestOptions
    );
}
DownloadPatternTemplate(param: any): Observable<any> {
  const headerOptions = new HttpHeaders({
    'Content-Type': 'application/json',
    Accept: Constants.ExcelType
  });
  const requestOptions = { headers: headerOptions, responseType: 'blob' as 'blob'};
  return this.http.post
    (
      this.apiURL +
      this.datadictionary.controllerName +
      this.datadictionary.Download, param, requestOptions
    );
}
}
