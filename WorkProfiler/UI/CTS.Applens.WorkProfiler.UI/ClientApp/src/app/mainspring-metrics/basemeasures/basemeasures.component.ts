// Copyright (c) Applens. All Rights Reserved.
import { Component, OnInit } from '@angular/core';
import { HeaderService } from 'src/app/Layout/services/header.service';
import { MasterDataModel } from '../../Layout/models/header.models';
import { TranslateService } from '@ngx-translate/core';
import { Constants } from 'src/app/common/constants/constants';
import { SpinnerService } from '../../common/services/spinner.service';
import { MainspringMetricsService } from './../Service/mainspring-metrics.service';
import { DatePipe } from '@angular/common';
import { LayoutService } from 'src/app/common/services/layout.service';
import { AppSettingsConfig } from 'src/app/app.settings.config';
@Component({
  selector: 'app-basemeasures',
  templateUrl: './basemeasures.component.html',
  styleUrls: ['./basemeasures.component.css'],
  providers: [DatePipe]
})
export class BasemeasuresComponent implements OnInit {
  public userId: string;
  public customerId: number;
  odcRestricted: string;
  divBaseNoteSummary: boolean = false;
  notes: boolean = false;
  odcRadiobtn: boolean = false;
  OdcNotes: boolean = false;
  LoadfactorCircle: boolean = false;
  progresstile: boolean = false;
  public loadFactorDialog: boolean;
  divrecord: boolean = false;
  loadsearchbase: boolean = false;
  divBaseNoteSummaryCircle: boolean = false;
  divMainspringBaseMeasureMarquee: boolean = false;
  divMainspringBaseMeasureMain: boolean = false;
  divgrdBaseMeasures: boolean;
  divgrdBaseMeasuresODC: boolean = false;
  divgrdTicketSummaryODC: boolean = false;
  projects: any[];
  selectedProjects: any;
  defaultselection: string[] = [];
  ddlReportingPeriod: any[];
  selectedReportingPeriod: any;
  ddlservice: any[];
  selectedddlservice: any[] = [];
  ddlmetrics: any[];
  selectedddlmetrics: any[] = [];
  rbBaseMeasurechk: string;
  rbTicketSummarychk: string;
  topfiltersMps: boolean = false;
  ddlfrequency: any[];
  selectedfrequency: any;
  ddlmetricslist : any[] = [];
  topfiltersMpsTicketSummary: boolean = false;
  public btnRow: boolean = false;
  public frequency: boolean = false;
  public reportingPeriod: boolean = false;
  public services: boolean = false;
  public divMetrics: boolean = false;
  mpsCurrentPage: string = "BaseMeasure";
  divDateNoteSummary: boolean = false;
  ddlprojectdropdown: boolean = false;
  basemeasuresUserDefinedModel: any[] = [];
  baseMeasureProgressAvailableCount: any;
  baseMeasureProgressTotalCount: any;
  progressPercentage: any;
  basemeasuresOdcModel: any[] = [];
  basemeasureTicketSummaryOdc: any[] = [];
  completedCount = 0;
  totalCount = 0;
  screenType: any;
  loadFactor: number;
  erroralert: boolean = false;
  barOptions = {};
  countClicked = false;
  toggleIcon = 'fa fa-arrow-circle-left';
  isError = false;
  isSuccess = false; 
  isFactorError = false;
  errorMessage = '';
  successMessage = '';
  odcrestrictedUserdefined : string;
  displayExemptedMsg=false;
  exemptedMsg:string;
  projectLength: any;
  exemptedMsgMain: string;
  ddexempted=false;
  hiddendata: any;

  constructor(private headerService: HeaderService, private translate: TranslateService,
    private spinner: SpinnerService, private basemeasureservice: MainspringMetricsService,
    private datePipe: DatePipe,
    private layoutService:LayoutService) { }

  ngOnInit(): void {
    

    this.headerService.masterDataEmitter.subscribe((masterData: MasterDataModel) => {

      if (masterData != null) {
        this.hiddendata = masterData.hiddenFields;
        this.projects = [];       
        this.userId = masterData.hiddenFields.employeeId;
        this.customerId = masterData.selectedCustomer.customerId;
        var data = {
          "employeeId": this.userId,
          "customerId": this.customerId
        };
        this.basemeasureservice.MpsFilters(data).subscribe(x => {
          this.projectLength=x.length;
          if (x != null && x.length > 0) {
            for (var i = 0; i < x.length; i++) {
              this.projects.push({
                "value": Number(x[i].projectId),
                "label": x[i].projectName
              });
            }
            this.selectedProjects = this.projects[0].value;
          }
          this.screenType = 'overallfilters';

          this.MpsFilters(this.selectedProjects, this.screenType);
          this.projectChange(this.selectedProjects);     

        });  
        
      }
    });

  }
  public loadFactorPopup(param: boolean): void {
    const params = {
      ProjectID: this.selectedProjects,
      MetricName: 'Load Factor',
      ReportPeriodID:  Number(this.selectedReportingPeriod),
    };
    this.basemeasureservice.GetBaseMeasureValueLoadFactor(params)
    .subscribe(data => {
      this.loadFactor = data;
      this.loadFactorDialog = param;
    });    
  }
  public loadsearchBaseMeasurePopup(param: boolean): void {
    this.loadsearchbase = param;
  }
  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;
  }
  MpsFilters(projectId, screentypes) {
    this.spinner.show();
    var params = {
      ProjectID: projectId
    };
    this.basemeasureservice.GetBaseMeasureFiltermainspringavailability(params).subscribe(x => {
      if (x != null) {
        this.odcRestricted = x.isODCRestricted;
        if (x.isMainSpringConfigured == 'Y' && x.isODCRestricted == 'N') {
          this.odcRadiobtn = false;
          this.LoadfactorCircle = false;
          this.notes = true;
          this.OdcNotes = false;
          this.divBaseNoteSummary = true;
          this.progresstile = true;
          this.divBaseNoteSummaryCircle = true;
          this.divgrdBaseMeasures = true;
          this.divgrdBaseMeasuresODC = false;
          this.divMainspringBaseMeasureMarquee = false;
          this.divMainspringBaseMeasureMain = true;
          this.btnRow = true;
          this.frequency = true;
          this.reportingPeriod = true;
          this.services = true;
          this.divMetrics = true;
          this.rbBaseMeasurechk = 'Y';
          this.rbTicketSummarychk = 'Y';
          this.topfiltersMps = true;
          this.BindFilterData(x);
        }
        else if (x.isMainSpringConfigured == 'Y' && x.isODCRestricted == 'Y') {
          if (this.mpsCurrentPage == "BaseMeasure" && screentypes == "TicketSummary") {
            this.odcRadiobtn = true;
            this.ddlprojectdropdown = true;
            this.LoadfactorCircle = false;
            this.rbBaseMeasurechk = 'N';
            this.rbTicketSummarychk = 'N';
            this.topfiltersMpsTicketSummary = true;
            this.OdcNotes = true;
            this.notes = false;
            this.progresstile = true;
            this.divBaseNoteSummary = true;
            this.divBaseNoteSummaryCircle = true;
            this.divgrdBaseMeasures = false;
            this.divMainspringBaseMeasureMarquee = false;
            this.divMainspringBaseMeasureMain = true;
            this.divgrdBaseMeasuresODC = true;
            this.btnRow = true;
            this.frequency = true;
            this.reportingPeriod = true;
            this.services = true;
            this.divMetrics = true;
            this.BindFilterData(x);
          }
          else if (this.mpsCurrentPage == "BaseMeasure") {
            this.odcRadiobtn = true;
            this.ddlprojectdropdown = true;
            this.LoadfactorCircle = false;
            this.rbBaseMeasurechk = 'Y';
            this.rbTicketSummarychk = 'Y';
            this.topfiltersMpsTicketSummary = false;
            this.topfiltersMps = true;
            this.OdcNotes = true;
            this.notes = false;
            this.progresstile = true;
            this.divBaseNoteSummary = true;
            this.divBaseNoteSummaryCircle = true;
            this.divgrdBaseMeasures = false;
            this.divgrdBaseMeasuresODC = true;
            this.divMainspringBaseMeasureMarquee = false;
            this.divMainspringBaseMeasureMain = true;
            this.btnRow = true;
            this.frequency = true;
            this.reportingPeriod = true;
            this.services = true;
            this.divMetrics = true;
            this.LoadFactorProject();
            this.BindFilterData(x);
          }
        }
        else {
          this.topfiltersMps = true;
          this.LoadfactorCircle = false;
          this.divDateNoteSummary = false;
          this.odcRadiobtn = false;
          this.progresstile = false;
          this.divBaseNoteSummary = false;
          this.divBaseNoteSummaryCircle = false;
          this.divMainspringBaseMeasureMarquee = true;
          this.btnRow = false;
          this.frequency = false;
          this.reportingPeriod = false;
          this.services = false;
          this.divMetrics = false;
          if (this.mpsCurrentPage == "BaseMeasure") {
            this.divgrdBaseMeasures = false;
            this.divgrdBaseMeasuresODC = false;
          }
          this.notes = false;
          this.OdcNotes = false;
          this.progresstile = false;
          this.ddlprojectdropdown = true;
          this.spinner.hide();
        }
      }
    });
  }
  BindFilterData(x){
          this.ddlfrequency = [];
          this.selectedfrequency = 0;
           
              this.ddlfrequency.push({
                "value": x.frequencyId,
                "label": x.frequencyName
              });
            this.selectedfrequency = this.ddlfrequency[0].value;
                     
          this.ddlReportingPeriod = [];    
          this.selectedReportingPeriod = [];
           
              this.ddlReportingPeriod.push({
                "value": x.reportPeriodId,
                "label": x.reportPeriod
              });
            this.selectedReportingPeriod = this.ddlReportingPeriod[0].value;
          
          this.ddlservice = [];
          this.selectedddlservice = [];
          if(x.lstservice.length > 0)
          {
            for (var i = 0; i < x.lstservice.length; i++) {
              this.ddlservice.push({
                "value": x.lstservice[i].serviceId,
                "label": x.lstservice[i].serviceName
              });
              this.selectedddlservice.push(this.ddlservice[i].value);
            }
            this.divBaseNoteSummaryCircle = true;   
            this.progresstile = true;            
            this.isError = false;
          }
          else 
          {
            this.divgrdBaseMeasures = false;
            this.divgrdBaseMeasuresODC = false;
            this.divgrdTicketSummaryODC = false;  
              
            this.divBaseNoteSummaryCircle = false;   
            this.progresstile = false;
            this.ddlmetrics = [];  
            this.selectedddlmetrics = [];
            this.translate.get('Thereisnoserviceavailableforthisproject')
              .subscribe(message => {
                this.isError = true;
                this.errorMessage = message;            
            });  
          }  
          this.spinner.hide();  
          this.ddlmetrics = [];  
          this.selectedddlmetrics = [];
          if(x.lstMetrics.length > 0)
          {
            for (var i = 0; i < x.lstMetrics.length; i++) {
              this.ddlmetrics.push({
                value: x.lstMetrics[i].metricId,
                label: x.lstMetrics[i].metricName,
                serviceId:x.lstMetrics[i].serviceId
              });         
            }
            this.ddlmetricslist = this.ddlmetrics;
            this.Bindmetricvalues(this.selectedddlservice,true);
          }  
  }
  odcradioChange(odcType) {
    if (odcType == 'Y') { 
      this.rbBaseMeasurechk = 'Y';
      this.rbTicketSummarychk = 'Y';
      this.screenType = 'overallfilters';
      this.MpsFilters(this.selectedProjects, this.screenType);
      this.loadsearchBaseMeasurePopup(false);
    }
    else {
      this.rbBaseMeasurechk = 'N';
      this.rbTicketSummarychk = 'N';
      if (this.mpsCurrentPage == "BaseMeasure" && this.odcRestricted == "Y" && this.rbTicketSummarychk) {
        this.screenType = 'TicketSummary';
        this.MpsFilters(this.selectedProjects, this.screenType);
        this.loadsearchBaseMeasurePopup(false);
      }
    }
  }

  LoadFactorProject() {
    var data = {
      ProjectID: this.selectedProjects,
      MetricName: "Load Factor"
    };
    this.basemeasureservice.GetBaseMeasureLoadFactorProject(data).subscribe(x => {
      if (x == true) {
        this.LoadfactorCircle = true;
      } else {
        this.LoadfactorCircle = false;
      }
    });
  }
  SaveLoadFactor(): void {
    if (this.loadFactor) {
    const params = {
      ProjectID: Number(this.selectedProjects),
      MetricName: "Load Factor", 
      ReportPeriodID:  Number(this.selectedReportingPeriod),
      LoadFactor: Number(this.loadFactor)
    };
    this.basemeasureservice.SaveLoadFactor(params)
    .subscribe(data => {
      if (data = '2') {
        this.setSuccessMessage('LoadFactorValueUpdatedSuccessFully');
        this.loadFactorDialog = false;
      }
      else if (data = '1') {
        this.setSuccessMessage('LoadFactorValueInsertedSuccessFully');   
        this.loadFactorDialog = false;    
      }
      else {
        this.setFactorErrorMessage('Cantabletoperformtheoperation');       
      }
      });
    } else {
      this.setFactorErrorMessage('Pleasefillthemandatoryfield');     
    }
  }

  ddlservicechange() {
    if (this.screenType == "TicketSummary") {
    
      if (this.isInvalid(this.selectedddlservice)) {
          this.loadsearchbase = false;  
          this.setErrorMessage('Definevaluesformandatedfiltertoseetheresults');
          this.divgrdTicketSummaryODC = false;
      }
    }
    else if (this.mpsCurrentPage == "BaseMeasure" || this.mpsCurrentPage == "BaseMeasureReport") {
        if (this.isInvalid(this.selectedddlservice)) {              
              this.selectedddlmetrics = [];
              this.ddlmetrics = [];
            if (this.isInvalid(this.selectedddlmetrics)) {
                if (this.mpsCurrentPage == "BaseMeasure") {
                  this.divgrdTicketSummaryODC = false;
                  this.divgrdBaseMeasuresODC = false;
                  this.divgrdBaseMeasures = false;
                  this.setErrorMessage('Definevaluesformandatedfiltertoseetheresults');
                }
                else if (this.mpsCurrentPage == "BaseMeasureReport") {
                  //CCAP Fix
                    
                }
            }
        }
        else {       
          this.Bindmetricvalues(this.selectedddlservice,false);                
     }
    }

    else {
        if (this.isInvalid(this.selectedddlservice)) {
            this.loadsearchbase = false;  
            this.setErrorMessage('Definevaluesformandatedfiltertoseetheresults');
            
        }
        else {
          this.MpsMetricsSearch(this.screenType);
        }
    }    
  }
  Bindmetricvalues(selectedservice,refresh){
    this.ddlmetrics=[];
    this.selectedddlmetrics=[];
    for (var i = 0; i <  selectedservice.length; i++) {
     for (var j = 0; j < this.ddlmetricslist.length; j++) {
      if(selectedservice[i]==this.ddlmetricslist[j].serviceId)
      {
       this.ddlmetrics.push({
       value: this.ddlmetricslist[j].value,
       label: this.ddlmetricslist[j].label
      });  
      }       
    }
  }
 this.ddlmetrics.forEach(r => this.selectedddlmetrics.push(r.value)); 
 if(refresh == true){
   this.MpsMetricsSearch(this.screenType);
  }
  this.spinner.hide();
  }
  ddlmetricschange() {
    if (((this.mpsCurrentPage == "BaseMeasure") || (this.mpsCurrentPage == "BaseMeasureReport")) 
    && (this.isInvalid(this.selectedddlmetrics))) {
      this.divgrdTicketSummaryODC = false;     
      this.divgrdBaseMeasures = false;
    }
  }
  MpsMetricsSearch(screenTypes) {

    if (this.selectedfrequency != null && this.selectedfrequency != ""
      && this.selectedddlservice != null && this.selectedddlservice.length > 0
      && this.selectedReportingPeriod != null && this.selectedReportingPeriod != "") {
      if (screenTypes == "TicketSummary") {
        this.divgrdBaseMeasures = false;
        this.divgrdBaseMeasuresODC = false;
        this.divgrdTicketSummaryODC = true;
        this.divMetrics = false;
        this.LoadTicketSummaryODC(true);
      }
      if (this.mpsCurrentPage == "BaseMeasure") {
        if (this.selectedddlmetrics != null && this.selectedddlmetrics.length > 0) {
          if (this.odcRestricted == 'N') {
            this.divgrdBaseMeasures = true;
            this.divgrdBaseMeasuresODC = false;
            this.divgrdTicketSummaryODC = false;
            this.divMetrics = true;
            this.odcrestrictedUserdefined = 'user defined';
            this.GetBaseMeasureProjectwiseSearchUserDefined(true);            
          }
          else if (this.odcRestricted == 'Y' && this.rbBaseMeasurechk == 'Y') {
            this.divgrdBaseMeasures = false;
            this.divgrdBaseMeasuresODC = true;
            this.divgrdTicketSummaryODC = false;
            this.divMetrics = true;
            this.odcrestrictedUserdefined = 'ODC';
            this.GetBaseMeasureProjectwiseSearchODC(true);
          }
        }
        else {
            this.divgrdTicketSummaryODC = false;
            this.divgrdBaseMeasuresODC = false;
            this.divgrdBaseMeasures = false;
          this.setErrorMessage('Definevaluesformandatedfiltertoseetheresults');
        }
      }
    }
    else {
      this.divgrdTicketSummaryODC = false;
      this.divgrdBaseMeasuresODC = false;
      this.divgrdBaseMeasures = false;
      this.setErrorMessage('Definevaluesformandatedfiltertoseetheresults');
    }   
  }
  projectChange(val) {
      this.displayExemptedMsg=this.hiddendata.lstProjectUserID
      .filter(x=>x.projectId==val && x.isExempted==true).length>0
      ?true:false;
      if(!this.displayExemptedMsg){
        this.ddexempted=false;
        this.onProjectChange(val);     
       }
      else{
        if(this.projects[0].value==val){
          this.ddexempted=this.displayExemptedMsg;
          this.exemptedMsgMain=Constants.exemptedMessage
        }
        else{
          this.ddexempted=!this.displayExemptedMsg;
        }
      this.exemptedMsg=Constants.exemptedMessage;
      }   
  }

  onProjectChange(val){
    this.spinner.show();
    this.selectedProjects =val;
    this.MpsFilters(val, 'overallfilters');
    this.loadsearchBaseMeasurePopup(false);
  }
  MpsMetricsSearchBtn() {    
    this.screenType = 'overallfilters';
    if (this.odcRestricted == 'Y' && this.rbTicketSummarychk === 'N') {
      this.screenType = 'TicketSummary';
    }
    this.MpsMetricsSearch(this.screenType);
    this.loadsearchBaseMeasurePopup(false);
  }
  GetBaseMeasureProjectwiseSearchUserDefined(showSpinner: boolean) {
    if (showSpinner) {
      this.spinner.show();
    }
    var params = {
      ProjectID: this.selectedProjects,
      FrequencyID: this.selectedfrequency,
      ServiceIDs: this.selectedddlservice.join(','),
      MetricsIDs: this.selectedddlmetrics.join(','),
      ReportFrequencyID: this.selectedReportingPeriod,
      BaseMeasureSystemDefinedOrUserDefined: this.odcrestrictedUserdefined
    };
    this.basemeasureservice.GetBaseMeasureProjectwiseSearchUserDefinedList(params).subscribe(x => {
      this.basemeasuresUserDefinedModel = [];
      this.baseMeasureProgressAvailableCount = 0;
      if (x != null) {
        if (x.mainspringAvailabilityModels != null && x.mainspringAvailabilityModels.length > 0) {

          this.basemeasuresUserDefinedModel  = JSON.parse(JSON.stringify(this.convertDateValue(x.mainspringAvailabilityModels)));
        } else {
          this.setErrorMessage('NoMatchingRecordsFound');
        }
        this.baseMeasureProgressAvailableCount = x.baseMeasureProgressAvailableCount;
        this.baseMeasureProgressTotalCount = x.baseMeasureProgressTotalCount;
        this.progressPercentage = x.progressPercentage;
        this.totalCount = this.baseMeasureProgressTotalCount ?? 0;
        this.completedCount = this.baseMeasureProgressAvailableCount ?? 0;
        this.progressbarUpdate();
      }
      this.spinner.hide();
    });
  }
  GetBaseMeasureProjectwiseSearchODC(showSpinner: boolean) {
    if(showSpinner) {
      this.spinner.show();
      }
    var params = {
      ProjectID: this.selectedProjects,
      FrequencyID: this.selectedfrequency,
      ServiceIDs: this.selectedddlservice.join(','),
      MetricsIDs: this.selectedddlmetrics.join(','),
      ReportFrequencyID: this.selectedReportingPeriod,
      BaseMeasureSystemDefinedOrUserDefined: this.odcrestrictedUserdefined
    };
    this.basemeasureservice.GetBaseMeasureProjectwiseSearchODCList(params).subscribe(x => {
      if (x != null) {
        if (x.mainspringAvailabilityModels != null && x.mainspringAvailabilityModels.length > 0) {
          this.basemeasuresOdcModel = this.convertDateValue(x.mainspringAvailabilityModels); 
        } else {
          this.setErrorMessage('NoMatchingRecordsFound');
        }
        this.completedCount = x.baseMeasureProgressAvailableCount ?? 0;
        this.totalCount = x.mainspringAvailabilityModels.length;
        this.progressPercentage = x.progressPercentage;
        this.progressbarUpdate();
      }
      this.spinner.hide();
    });
  }
  LoadTicketSummaryODC(showSpinner: boolean) {
    if(showSpinner) {
    this.spinner.show();
    }
    this.divgrdTicketSummaryODC = true;
    this.divgrdBaseMeasuresODC = false;
    this.divgrdBaseMeasures = false;
    var parms = {
      ProjectID: this.selectedProjects,
      FrequencyID: this.selectedfrequency,
      ServiceIDs: this.selectedddlservice.join(','),
      ReportFrequencyID: this.selectedReportingPeriod
    };
    this.basemeasureservice.GetTicketSummeryBaseMeasureODCList(parms).subscribe(x => {
      this.basemeasureTicketSummaryOdc = [];
      if (x != null && x.mainspringAvailabilityModels != null && x.mainspringAvailabilityModels.length > 0) {
        this.basemeasureTicketSummaryOdc = x.mainspringAvailabilityModels; 
      } else {
        this.setErrorMessage('NoMatchingRecordsFound');
      }
      this.completedCount = x.completedCount ?? 0;
      this.totalCount = x.mainspringAvailabilityModels.length;
      this.progressPercentage = x.progressPercent ?? 0;     
      this.spinner.hide();
    });
  }
  progressbarUpdate() {
    this.barOptions = {
      barType: 'linear',
      color: '#0e90d2',
      secondColor: '#D3D3D3',
      progress: this.progressPercentage,
      linear: {
        depth: 22,
        stripped: true,
        active: true,
        label: {
          enable: false,
          value: 'Linear Bar',
          color: '#fff',
          fontSize: 12,
          showPercentage: true,
        }
      }

    }
  }

  convertDateValue(models: any): any {    
    if (models) {
    models.forEach(x => {
     if (x.uomDataType === 'Date') {
       x.baseMeasureValue = this.getDate(x.baseMeasureValue);
     }
   });  
    }
   return models;
  }
  setSuccessMessage(pattern: string): void {
    this.translate.get(pattern)
          .subscribe(message => {
            this.isSuccess = true;
            this.successMessage = message;
            setTimeout(() => {
              this.isSuccess = false;
              this.successMessage = '';
            }, Constants.lifeSpan);
    });    
  }
  setErrorMessage(pattern): void {
    this.translate.get(pattern)
          .subscribe(message => {
            this.isError = true;
            this.errorMessage = message;
            setTimeout(() => {
             this.isError = false;
             this.errorMessage = '';
            }, Constants.lifeSpan);
    });    
  }
  setErrorMessageWithoutTime(pattern: string): void {
    this.translate.get(pattern)
          .subscribe(message => {
            this.isError = true;
            this.errorMessage = message;           
    });    
  }
  setFactorErrorMessage(pattern: string): void {
    this.translate.get(pattern)
          .subscribe(message => {
            this.isFactorError = true;
            this.errorMessage = message;
            setTimeout(() => {
             this.isFactorError = false;
             this.errorMessage = '';
            }, Constants.lifeSpan);
    });    
  }
  toggleCount(): void {
    if (this.countClicked) {
      this.countClicked = false;
      this.toggleIcon = 'fa fa-arrow-circle-left';
    } else {
      this.countClicked = true;
      this.toggleIcon = 'fa fa-arrow-circle-right';
    }
  }
  getDate(date: any) {
    if (date && date !== '') {
    return this.datePipe.transform(new Date(date), Constants.dateFormat);
    }
  }
  isInvalid(value: any): boolean {
    return (value === null || value === undefined || value.length === 0);
  }
}
