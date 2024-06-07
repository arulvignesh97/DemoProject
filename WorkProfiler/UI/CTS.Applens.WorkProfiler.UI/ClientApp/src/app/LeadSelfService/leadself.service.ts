// Copyright (c) Applens. All Rights Reserved.
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AppSettingsConfig } from '../app.settings.config';
import { AppConfig } from '../common/Config/config';
import { Constants } from '../common/constants/constants';
import { ApproveDebtModel, DebtReviewUpload, DropdownModel } from './Models/LoadSelfServiceModel';

@Injectable({
  providedIn: 'root'
})
export class LeadselfService {

  apiType: string = null;
  apiURL: string = null;
  CauseCodeDropdown: DropdownModel[] = [];
  ResolutionCodeDropdown: DropdownModel[] = [];
  DebtOverrideReviewDropdown: DropdownModel[] = [];
  ResidualMasterDropdown: DropdownModel[] = [];
  AvoidableMasterDropdown: DropdownModel[] = [];
  

  constructor(private http: HttpClient,private config: AppConfig) {
    this.apiURL = AppSettingsConfig.settings.API;
   }

getProducts() {
  return this.http.get<any>('assets/products.json')
  .toPromise()
  .then(res =>  res.data as any[])
  .then(data => data);
}

getDebtOverrideAndDictionaryData(StartDate: string, EndDate: string, CustomerID: number,
  EmployeeID: string, ProjectID: number, ReviewStatus: number)
{
  var searchModel = {
    StartDate: StartDate,
    EndDate: EndDate,
    CustomerID: CustomerID,
    EmployeeID: EmployeeID,
    ProjectID: ProjectID,
    ReviewStatus: ReviewStatus
  }
  const headerOptions = new HttpHeaders({
    'Content-Type': 'application/json'
  });
  const requestOptions = { headers: headerOptions };
  this.apiType = "DebtOverideAndDataDictionary/DebtOverRideReviewDict"
  const url = `${this.apiURL}${this.apiType}`;
  return this.http.post(url, searchModel, requestOptions).pipe(map((response) => {
    return response;
  }));
}
GetDebtclassification(): Observable<any>
{
  this.apiType = "DebtOverideAndDataDictionary/GetDebtclassification";
  const url = `${this.apiURL}${this.apiType}`;
  return this.http.get(url).pipe(map((response) => {
    return response;
  }));
  
}

GetCausecode(ProjectID: number): Observable<any>
{
  const params = new HttpParams()
    .set('ProjectID', ProjectID.toString());
  let headers = new HttpHeaders();
  headers.append('Content-Type', 'application/json');
  this.apiType = "DebtOverideAndDataDictionary/GetCausecode";
  const url = `${this.apiURL}${this.apiType}`;
  return this.http.get(url,{headers:headers,params:params}).pipe(map((response) => {
    return response;
  }));
}

GetAvoidableFlag(): Observable<any>
{
  this.apiType = "DebtOverideAndDataDictionary/GetAvoidableFlag";
  const url = `${this.apiURL}${this.apiType}`;
  return this.http.get(url).pipe(map((response) => {
    return response;
  }));
}

GetResolutioncode(ProjectID: number): Observable<any>
{
  this.apiType = "DebtOverideAndDataDictionary/GetResolutioncode";
  const params = new HttpParams()
    .set('ProjectID', ProjectID.toString());
  let headers = new HttpHeaders();
  headers.append('Content-Type', 'application/json');
  const url = `${this.apiURL}${this.apiType}`;
  return this.http.get(url,{headers:headers,params:params}).pipe(map((response) => {
    return response;
  }));
}
GetResidualDebt(): Observable<any>
{
  this.apiType = "DebtOverideAndDataDictionary/GetResidualDebt";
  const url = `${this.apiURL}${this.apiType}`;
  return this.http.get(url).pipe(map((response) => {
    return response;
  }));
}
DownloadFile(searchModel): Observable<any> {
  const headerOptions = new HttpHeaders({
    'Content-Type': 'application/json'
  });
  const requestOptions = { headers: headerOptions };
    this.apiType = 'DebtOverideAndDataDictionary/DownloadDebtOverrideTemplate';
    const url = `${this.apiURL}${this.apiType}`;
    return this.http.post(url, searchModel,{responseType: 'blob', observe: 'response'}).pipe(map(response => {
        return response;
    }));
  }
  fileChange(files: File,debtReviewUpload : DebtReviewUpload): Observable<any>
    {
    let formData: FormData = new FormData();  
    formData.append('files',files);
    formData.append('debtReviewUpload', JSON.stringify(debtReviewUpload));
    this.apiType = "DebtOverideAndDataDictionary/DebtReviewUpload";
    let headers = new HttpHeaders();
    headers.append("Content-Type", "multipart/form-data");  
    headers.append('Accept', 'application/json');  
    const url = `${this.apiURL}${this.apiType}`;
    return this.http.post(url, formData, { headers: headers}).pipe(map((response) => {
      return response;
  },
  function(error) { return error.json();}
  )
);
    }

    ApproveSelectedData(approveDebtModel: ApproveDebtModel)
    {
      const headerOptions = new HttpHeaders({
        'Content-Type': 'application/json'
      });
      const requestOptions = { headers: headerOptions };
      this.apiType = "DebtOverideAndDataDictionary/ApproveTicketsByTicketId";
      const url = `${this.apiURL}${this.apiType}`;
      return this.http.post(url, approveDebtModel, requestOptions).pipe(map((response) => {
        return response;
      }));
    }

    GetDropDownValuesProjectPortfolio(employeeID: string, customerID: number)
    {
      this.apiType = "DebtOverideAndDataDictionary/GetDropDownValuesProjectPortfolio";
      const params = new HttpParams()
        .set('employeeID', employeeID).set('customerID', customerID.toString());
      let headers = new HttpHeaders();
      headers.append('Content-Type', 'application/json');
      const url = `${this.apiURL}${this.apiType}`;
      return this.http.get(url,{headers:headers,params:params}).pipe(map((response) => {
        return response;
      }));
    }
}
