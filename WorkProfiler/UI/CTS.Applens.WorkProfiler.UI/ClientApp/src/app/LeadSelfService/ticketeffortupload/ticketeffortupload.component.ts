// Copyright (c) Applens. All Rights Reserved.
import { Component, OnInit, ChangeDetectorRef, TemplateRef } from '@angular/core';
import { TicketEffortUploadServices } from '../Service/ticketeffortupload.service';
import { ErrorLogServices } from '../Service/errorlog.service';
import { SpinnerService } from './../../common/services/spinner.service';
import { HeaderService } from 'src/app/Layout/services/header.service';
import { MasterDataModel } from '../../Layout/models/header.models';
import { MessageService } from 'primeng/api';
import { Constants } from 'src/app/common/constants/constants';
import { TranslateService } from '@ngx-translate/core';
import { AppSettingsConfig } from 'src/app/app.settings.config';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { SearchTicketService } from 'src/app/searchticket/Service/SearchTicket.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { DatePipe } from  '@angular/common';
import { Routes, RouterModule, ActivatedRoute, Params, Router } from '@angular/router';
import { LayoutService } from 'src/app/common/services/layout.service';

@Component({
  selector: 'app-ticketeffortupload',
  templateUrl: './ticketeffortupload.component.html',
  styleUrls: ['./ticketeffortupload.component.css'],
  providers: [MessageService,DatePipe]
})
export class TicketeffortuploadComponent implements OnInit {
  id:number;
  public projectListData = [];
  public scopeCustomerData = [];
  public projectTilePercentage = [];
  public scopeData = [];
  public scopeDataList = [];
  public scopeCustData = [];
  public employeeID;
  public employeeName;
  public customerID;
  public projectID;
  public isCognizant;
  public isDaily;
  public isEffortConfigured;
  public debtunClassifiedlFG: FormGroup = new FormGroup({});
  public showRadiobuttons: boolean = false;
  public ispureApp: boolean = true;
  public onboardingPercentage : number  = 0;
  //Flag
  public itsmConfigFlag;
  public warningFlag: boolean = false;
  public errorFlag: boolean = false;
  //Show-Hide
  public ticketUploadShow: number = 1;
  public isTicketDescriptionOptedShow: boolean = false;
  public defaultTicketMessageShow: boolean = false;
  public defaultEffortMessageShow: boolean = false;
  public errITSMShow: boolean = false;
  public warningShow: boolean = false;
  public buttonDisable: boolean = false;
  public copyTPath: boolean = false;
  public copyEPath: boolean = false;
  public autoclassificationShow: boolean = false;
  public errorMessageShow: boolean = false;
  public successMessageShow: boolean = false;
  public errITSMMandateColumn: boolean = false;
  public errITSMPercentage: boolean = false;
  public errITSMManualOrAuto: boolean = false;
  public autoClassificationMultilingual: boolean = false;
  public autoClassification: boolean = false;
  public ismaintenancedevtest: boolean = false;
  public isCIS: boolean = false;
  public IsApp: boolean = true;
  public EffortDivVisibility: boolean = true;
  public EffortDivVisibilitywarningShow: boolean = false;
  public DebtunClassifiedVisibility: boolean = false;
  //Message
  public warningMessage: string;
  public errITSMMessage: string;
  public spanTicketPath: string;
  public spanEffortPath: string;
  public spanTicketUsers: string;
  public spanEffortUsers: string;
  public autoClassificationMessage: string = null;
  public errorMessage: string;
  public successMessage: string;
  public scopeValues = '';
  public debtShow: boolean = false;
  public IsDebtUnClassifiedApplicableforSAAS: boolean = false;
  public showDebtUnClassifiedtab: boolean = false;
  public currentyear: number;
  public startingyear: number;
  public maxclosedatefrom: Date;
  public maxclosedateto: Date;
  public minclosedatefrom: Date;
  public myFiles = [];
  public minclosedateto: Date;
  public MasterProjectList = [];
  public SubBusinessClusterMapping = [];
  public MasterHierarcyList = [];
  public HierarchyDropdownList = [];
  public ApplicationTowerList = [];
  public ProjectList = [];
  public RowHiearchyList = [];
  public HcmSupervisorList=[];
  public SaveHcmSupervisorList=[];
  public ApplicationTowerids : string;
  public HighlightBox: boolean = false;
  public displayDebtUnclassifiedPop:boolean=false;
  excelName: string;
  modalRef: BsModalRef;
  config = {
    backdrop: true,
    keyboard: false,
    ignoreBackdropClick: true
  };
 
  public ProjectName: string;
  public accountname : string;
  public SelectedFromDate: FormControl = new FormControl();
  public SelectedToDate: FormControl = new FormControl();
  public EsaProjectId: string;
  public GetispureApp: any;
  public SelectedProjectName=[];
  public projectval:string;
  public esaprojvalue : string;
  displayExemptedMsg=false;
  exemptedMsg:string;
  hiddendata: any;
 


  constructor(private TicketEffortUploadServices: TicketEffortUploadServices,
    private ErrorLogServices: ErrorLogServices, private messageService: MessageService,
    private spinner: SpinnerService, private headerService: HeaderService, private translate: TranslateService,
    private searchTicketService: SearchTicketService,private cdref: ChangeDetectorRef,private fb: FormBuilder,
    private readonly modalService: BsModalService, private readonly datePipe : DatePipe,private router:ActivatedRoute,
    private layoutService:LayoutService) {
  }

  ngOnInit(): void {
    this.currentyear = new Date().getFullYear();
    this.startingyear = new Date().getFullYear() - 5;
    this.maxclosedatefrom = new Date();
    this.maxclosedateto = new Date();
    this.spinner.show();

    this.router.params.subscribe((params:Params)=>{
      this.id=params['id']
    })

    this.headerService.masterDataEmitter.subscribe((masterData: MasterDataModel) => {
      if (masterData != null) {
        this.hiddendata = masterData.hiddenFields;
        this.employeeID = masterData.hiddenFields.employeeId;
        this.customerID = masterData.selectedCustomer.customerId;
        this.isCognizant = masterData.hiddenFields.isCognizant;
        this.scopeCustomerData = masterData.hiddenFields.lstScope;
        this.projectTilePercentage = masterData.hiddenFields.lstProjectPercentage;
        this.employeeName = masterData.hiddenFields.employeeName;
        this.accountname=masterData.selectedCustomer.customerName;
        this.isDaily = masterData.hiddenFields.isDaily == 1 ? true : false;
        this.isEffortConfigured = masterData.hiddenFields.isEffortConfigured == 1 ? true : false;
        this.IsDebtUnClassifiedApplicableforSAAS = AppSettingsConfig.settings.IsDebtUnClassifiedApplicableforSAAS;
        this.HcmSupervisorList=masterData.hiddenFields.lstHCMSupervisorlist;
        const Params = {
          EmployeeID: this.employeeID,
          CustomerID: this.customerID,
          MenuRole:this.id==1?'Analyst':'Lead'
        };

        this.TicketEffortUploadServices.ProjectDetails(Params).subscribe(x => {
          if (x.userDetails.length > 0) {
            this.projectListData = x.userDetails;
            this.autoClassificationMessage = x.autoClassificationMessage;
            this.projectID = this.projectListData[0].projectId;
            this.ticketUploadShow = 1;
            this.GetITSMConfigDetails();
            this.SelectedProjectName= masterData.hiddenFields.lstProjectUserID;
            this.SetProject(this.projectID);
            this.EsaProjectId = masterData.hiddenFields.lstProjectUserID.filter(x => x.projectId == this.projectID)[0].esaProjectId;
          }
          else {
            this.projectListData = [];
            this.projectID = null;
            this.copyTPath = false;
            this.copyEPath = false;
            this.autoclassificationShow = false;
            this.errITSMShow = false;
            this.warningShow = false;
            this.isTicketDescriptionOptedShow = false;
            this.defaultTicketMessageShow = false;
            this.defaultEffortMessageShow = false;
            this.warningFlag = false;
            this.spinner.hide();
          }
        });
      }
    });
    this.RowHiearchyList = [
      { "label": "ApplicationTower", "formcontrolname": "SelectedApplication", "List": this.ProjectList, "isinfra": false }];
    
  }

  isValid(value: any): boolean {
    return (value != null && value != undefined);
  }

  CreateFormGroup(ConstrolsList) {
    this.cdref.detectChanges();
    this.debtunClassifiedlFG = new FormGroup({});
    this.debtunClassifiedlFG = this.fb.group({
      SelectedFromDate: this.SelectedFromDate,
      SelectedToDate: this.SelectedToDate,
      SelectedHierarchy0: [""],
      SelectedHierarchy1: [""],
      SelectedHierarchy2: [""],
      SelectedApplication: [""]
    })
  }

  debtRBChange(e) {
    this.spinner.show();
    let ispureApp = e.target;
    if (ispureApp.value == "0") {
      this.ispureApp = false;
      this.GetHierarchyOnTabChange();
    }
    else {
      this.ispureApp = true;
      this.GetHierarchyOnTabChange();
    }
    this.SelectedToDate.reset();
    this.SelectedFromDate.reset();
  }
  public closedatefromchange(SelectedFromDate): void {
    let startDate = SelectedFromDate.value;
    let endDate = '';
    if (startDate != null) {
      this.setdate_startend(startDate, endDate);
    }
  }
  public closedatetochange(SelectedToDate): void {
    let startDate = '';
    let endDate = SelectedToDate.value;
    if (endDate != null) {
      this.setdate_startend(startDate, endDate);
    }
  }
  public setdate_startend(startDate, endDate) {
    if (startDate == '' && endDate != '') {
      let SelectedDate = new Date(endDate);
      this.maxclosedatefrom = SelectedDate;
    }
    else if (endDate == '' && startDate != '') {
      let SelectedDate = new Date(startDate);
      this.minclosedateto = SelectedDate;

    }
  }
  debtunclassified() {
    let DebtPercentage = 0;
    if(this.projectTilePercentage.filter(c =>  c.tileId == 10 && c.projectId == this.projectID).length>0){
      DebtPercentage = this.projectTilePercentage.filter(c =>  c.tileId == 10 && c.projectId == this.projectID)[0].tileProgressPercentage;
    }
    this.scopeData = this.scopeCustomerData.filter(x => x.projectId == this.projectID);
    if((this.IsDebtUnClassifiedApplicableforSAAS && this.isCognizant == 1) || this.isCognizant == 0){
      if (this.scopeData.filter(y => y.scope == 2 || y.scope == 3).length > 0 && DebtPercentage == 100 && this.onboardingPercentage == 100) {
        this.debtShow = true;}
      else{
        this.debtShow = false;
      }
    }
    this.opentab();
  }

  opentab(){
    let Scopecount = this.scopeData.filter(y => y.scope == 2 || y.scope == 3).length;
    let scopelist = this.scopeData.filter(y => y.scope == 2 || y.scope == 3);
    if(Scopecount  == 2){
      this.showRadiobuttons = true;
      this.ispureApp = true;
    }
    else{
      this.showRadiobuttons = false;
    }
    if(Scopecount == 1){
      if(scopelist.filter(x=>x.scope == 2).length> 0){
        this.ispureApp = true;
      }
      else{
        this.ispureApp = false;
      }
    }
  }
  effortUpload(): void{
    this.autoClassification = false;
    this.autoClassificationMultilingual = false;
    if (this.projectID != undefined) {
      this.spinner.show();
      if (this.itsmConfigFlag = 'Y') {
        let frmData: FormData;
        frmData = this.AppendFileToFormData(event);
        this.TicketEffortUploadServices.TicketEffortUpload(frmData).subscribe(x => {

          if (x.result == Constants.emptyTemplate) {
            this.setErrorMessage('Upload failed - Template should not be empty & Ticket ID/User ID is mandatory');
          }
          else if (x.result == Constants.MoreRecords) {
            this.setErrorMessage('Timesheet with more than 2000 records cannot be uploaded through UI');
          }
          else if (x.result == Constants.ColumnMappingNotDoneInITSMConfiguration1) {
            this.setErrorMessage('Column Mapping Has Not Been Done In ITSM Configuration');
          }
          else if (x.result == Constants.PleaseUploadValidTemplate) {
            this.setErrorMessage('Upload a valid template in the same format & file name as downloaded');
          }
          else if (x.result == Constants.DumpUploadFailed) {
            this.setErrorMessage('Dump Upload Failed Please Check Email');
          }
          else if (x.result == Constants.TemplateNotMatchWithITSM) {
            this.setErrorMessage('Template Is Not Matching With ITSM Configuration Column Mapping Please Upload Valid Template');
          }
          else if (x.result == Constants.Successfullydone) {
            this.setSuccessMessage('Uploaded Successfully');
          }
          else if (x.result == Constants.UploadedSuccessfullyCheckErrorLog) {
            this.setSuccessMessage('Uploaded Successfully Please Check ErrorLog For Failed Tickets');
          }
          else if (x.result.toUpperCase().search("SUCCESS") > 0) {
            this.setSuccessMessage(x.result);
          }
          else {
            this.errorMessageShow = true;
            this.errorMessage = x.result;
            setTimeout(() => {
              this.errorMessageShow = false;
              this.errorMessage = '';
            }, Constants.lifeSpan);
          }
          if (x.result == Constants.Successfullydone || x.result == Constants.UploadedSuccessfullyCheckErrorLog) {
            this.autoclassificationShow = true;
            if (x.isMultilingual) {
              this.autoClassificationMultilingual = true;
            }
            else {
              this.autoClassification = true;
            }
          }
          else {
            this.autoclassificationShow = false;
          }
          this.spinner.hide();
        });
      }
    }
    else {
      this.setErrorMessage('PleaseSelectProjectFromDropdown');
    }
  }

  DownloadDebtUnClassifiedTickets() {
    
    this.ApplicationTowerids="";
    this.spinner.show();
    this.GetispureApp = this.ispureApp;
    let sDate = this.datePipe.transform(this.debtunClassifiedlFG.get('SelectedFromDate').value,'MM/dd/yyyy');
    let eDate =  this.datePipe.transform(this.debtunClassifiedlFG.get('SelectedToDate').value,'MM/dd/yyyy');
    let Value = (this.debtunClassifiedlFG.get('SelectedFromDate').value && this.debtunClassifiedlFG.get('SelectedToDate').value);
    if (Value=="" || Value==null || Value==0 || Value==undefined){
      this.setErrorMessage('Capture mandatory fields and try again');
      this.SelectedFromDate.markAsDirty();
      this.SelectedToDate.markAsDirty();
      this.spinner.hide();
    }
    else {
      if (this.debtunClassifiedlFG.value.SelectedApplication !=null && this.debtunClassifiedlFG.value.SelectedApplication.length>0) {
        let applist = [];
        for (let i = 0; i < this.debtunClassifiedlFG.value.SelectedApplication.length; i++) {
          applist.push(this.debtunClassifiedlFG.value.SelectedApplication[i].value);
        }
        for (var i = 0; i < applist.length; i++) {
          this.ApplicationTowerids += applist[i] + ',';
        }

      }
      else if (this.debtunClassifiedlFG.get('SelectedHierarchy0').value == null ||
        this.debtunClassifiedlFG.get('SelectedHierarchy0').value.length == 0) {
        this.ApplicationTowerids = "";
      }
      else{
        this.DownloadClassifiedTickets();
      }
      for(let i=0;i<this.projectListData.length;i++){
        if(this.projectListData[i].projectId==this.projectID){
          this.ProjectName=this.projectListData[i].projectName;
        }
      }
      this.HighlightBox = false;
      var Filename = this.EsaProjectId + "-" + this.ProjectName + "-" + "Debt Unclassified Tickets";
      const Params = {
        EsaProjectID: this.EsaProjectId,
        ProjectName: this.ProjectName,
        ProjectID: this.projectID,
        ClosedDateFrom: sDate,
        ClosedDateTo: eDate,
        AppTowerID: this.ApplicationTowerids,
        ispureApp: this.GetispureApp,
        FileName: Filename,
      };
      this.TicketEffortUploadServices.DownloadDebtUnClassifiedTickets(Params).subscribe(x => { 
        if (x.size != 0)
        {
          const blob = new Blob([x], { type: Constants.ExcelType });
          const url = window.URL.createObjectURL(blob);
          const downloadTag = document.createElement('a');
          downloadTag.download = Filename + Constants.FileFormatxl;
          downloadTag.href = url;
          downloadTag.click();
        //this.downloaddebtfile(x);
        this.spinner.hide();
      }
        else
        {
          this.setErrorMessage('No Records Found');
          this.spinner.hide();
        }
      });
    }

  }
  importFile(event) {
    this.spinner.show();
    this.GetispureApp = this.ispureApp;
    this.myFiles = [];
    if (event !== '') {
      for (var i = 0; i < event.target.files.length; i++) {
        this.myFiles.push(event.target.files[i]);
      }
    }
    const frmData = new FormData();
    frmData.append('projectID', this.projectID);
    frmData.append('ispureApp', this.GetispureApp);
    frmData.append('UserId', this.employeeID);
    for (var i = 0; i < this.myFiles.length; i++) {
      frmData.append('file' + i, this.myFiles[i]);
    }

    if (event.target.files[0].type === Constants.ExcelMacroType) {
      this.TicketEffortUploadServices.UploadforDebtunclassifiedTicket(frmData).subscribe(x => {
        
        if (x) {
          if (x == 'Uploaded Successfully.') {
            this.setSuccessMessage('Uploaded successfully.Debt information is not updated for the tickets which are already met with grace period');
          }
          else {
            this.setErrorMessage(x);
          }
        }
        else {
          this.setErrorMessage('Invalid Template');
        }
        this.spinner.hide();
      });
    }
    else {
      this.setErrorMessage('Invalid Template');
      this.spinner.hide();
    }
  }

  EffortDownload() {
    if (this.projectID != undefined) {
      this.spinner.show();
      let result;
      let esaProjectID;
      if (!(this.ismaintenancedevtest && this.isCIS)) {
        this.IsApp = this.ismaintenancedevtest ? true : false;
      }
      const Params = {
        Filename: Constants.EffortFileName,
        IsCognizant: this.isCognizant.toString(),
        ProjectID: this.projectID,
        IsApp: this.IsApp
      };
      this.TicketEffortUploadServices.ExportExcelClick(Params).subscribe(x => {
      this.downloadeffortfile(x);
      });
      this.spinner.hide();
    }
    else {
      this.setErrorMessage('PleaseSelectProjectFromDropdown');
    }
  }

  TicketDownload() {
    if (this.projectID != undefined) {
      this.spinner.show();
      if (this.itsmConfigFlag = 'Y') {
        let result;
        let filename;
        const Params = {
          EmployeeID: this.employeeID,
          ProjectID: this.projectID
        };
        this.TicketEffortUploadServices.DownloadTicketTemplate(Params).subscribe(x => {
          this.downloadfile(x);
        //   if (x.excelPath == Constants.ColumnMappingNotDoneInITSMConfiguration || x.excelPath == Constants.ProblemWithDownload) {
        //     if (x.excelPath == Constants.ColumnMappingNotDoneInITSMConfiguration) {
        //       this.setErrorMessage('ColumnMappingHasNotBeenDoneInITSMConfiguration');
        //     }
        //     else {
        //       this.setErrorMessage('ProblemWithDownload');
        //     }
        //   }
        //  else {
        //     // const Params = {
        //     //   Filename: result
        //     // };
        //     // this.TicketEffortUploadServices.Download(Params).subscribe(x => {
        //   //  });
        //   }
        });
      }
      this.spinner.hide();
    }
    else {
      this.setErrorMessage('PleaseSelectProjectFromDropdown');
    }
  }
  downloadeffortfile(data: any) {
    const blob = new Blob([data], { type: 'application/vnd.ms-excel' });
    this.excelName =  this.EsaProjectId + '_' + Constants.EffortFileName;
    const url = window.URL.createObjectURL(blob);
    const noice = document.createElement('a');
    noice.download = this.excelName;
    noice.href = url;
    noice.click();
  }

  downloadfile(data: any) {
    const chknumber2=2;
    const blob = new Blob([data], { type: 'application/vnd.ms-excel' });
    var today = new Date();
    var dd = String(today.getDate()).padStart(chknumber2, '0');
    var MM = String(today.getMonth() + 1).padStart(chknumber2, '0');
    var yyyy = today.getFullYear();
    var hh = today.getHours();
    var mm = today.getMinutes();
    var ss = today.getSeconds();
    var td = yyyy + '_' + MM + '_' + dd + '_' + hh + '_' + mm + '_' + ss;
    this.excelName =  Constants.Ticketdetails + this.employeeID + '_' + td + '.xlsx';
    const url = window.URL.createObjectURL(blob);
    const noice = document.createElement('a');
    noice.download = this.excelName;
    noice.href = url;
    noice.click();
  }

  TicketUpload(event) {
    this.autoClassification = false;
    this.autoClassificationMultilingual = false;
    if (this.projectID != undefined) {
      this.spinner.show();
      if (this.itsmConfigFlag = 'Y') {
        let frmData: FormData;
        frmData = this.AppendFileToFormData(event);
        for(let i=0;i<this.HcmSupervisorList.length;i++)
        {
          this.SaveHcmSupervisorList=this.HcmSupervisorList.filter(x=>x.projectID==this.projectID);
        }
        let supervisorlist=JSON.stringify(this.SaveHcmSupervisorList);
        frmData.append('hcmList',supervisorlist);

        this.TicketEffortUploadServices.TicketExcelUpload(frmData).subscribe(x => {
          if (x.result == Constants.ColumnMappingNotDoneInITSMConfiguration1) {
            this.setErrorMessage('ColumnMappingHasNotBeenDoneInITSMConfiguration');
          }
          else if (x.result == Constants.PleaseUploadValidTemplate) {
            this.setErrorMessage('PleaseUploadValidTemplateValidFileIs.xlsx');
          }
          else if (x.result == Constants.DumpUploadFailed) {
            this.setErrorMessage('DumpUploadFailedPleaseCheckEmail');
          }
          else if (x.result == Constants.TemplateNotMatchWithITSM) {
            this.setErrorMessage('TemplateIsNotMatchingWithITSMConfigurationColumnMappingPleaseUploadValidTemplate');
          }
          else if (x.result == Constants.UploadedSuccessfully) {
            this.setSuccessMessage('UploadedSuccessfully');
          }
          else if (x.result == Constants.UploadedSuccessfullyCheckErrorLog) {
            this.setSuccessMessage('UploadedSuccessfullyPleaseCheckErrorLogForFailedTickets');
          }
          else {
            this.errorMessageShow = true;
            this.errorMessage = x.result;
            setTimeout(() => {
              this.errorMessageShow = false;
              this.errorMessage = '';
            }, Constants.lifeSpan);
          }
          if (x.result == Constants.UploadedSuccessfully || x.result == Constants.UploadedSuccessfullyCheckErrorLog) {
            this.autoclassificationShow = true;
            if (x.isMultilingual) {
              this.autoClassificationMultilingual = true;
            }
            else {
              this.autoClassification = true;
            }
          }
          else {
            this.autoclassificationShow = false;
          }
          this.spinner.hide();
        });
      }
    }
    else {
      this.setErrorMessage('PleaseSelectProjectFromDropdown');
    }
  }

  AppendFileToFormData(event) {
    const inputFiles = [];
    if (event !== '') {
      for (let i = 0; i < event.target.files.length; i++) {
        inputFiles.push(event.target.files[i]);
      }
    }
    const formData = new FormData();
    formData.append('accountname', this.projectval);
    formData.append('esaprojectid', this.esaprojvalue);
    formData.append('employeeID', this.employeeID);
    formData.append('projectID', this.projectID);
    formData.append('customerID', this.customerID);
    formData.append('isCognizant', this.isCognizant);
    formData.append('employeeName', this.employeeName);
    formData.append('isDaily', this.isDaily);
    formData.append('isEffortConfigured', this.isEffortConfigured);
    if (this.isCognizant == 1) {
      if (!(this.ismaintenancedevtest && this.isCIS)) {
        this.IsApp = this.ismaintenancedevtest ? true : false;
      }
      formData.append('isApp', this.IsApp ? "True" : "False");
    }
    else {
      formData.append('isApp', "True");
    }
    const fileconcatname = "file";
    for (let i = 0; i < inputFiles.length; i++) {
      formData.append(fileconcatname + i.toString(), inputFiles[i]);
    }
    return formData;
  }

  CopyPath(id: any) {
    const selBox = document.createElement('textarea');
    selBox.style.position = 'fixed';
    selBox.style.left = '0';
    selBox.style.top = '0';
    selBox.style.opacity = '0';
    selBox.value = id;
    document.body.appendChild(selBox);
    selBox.focus();
    selBox.select();
    document.execCommand('copy');
    document.body.removeChild(selBox);
    this.setSuccessMessage('SharePathCopied');
  }

  GetITSMConfigDetails() {
    if (this.isCognizant == 1) {
      this.scopeValues = '';
      this.scopeData = this.scopeCustomerData.filter(x => x.projectId == this.projectID);
      if (this.scopeData.length > 0) {
        for (let i = 0; i < this.scopeData.length; i++) {
          if (this.scopeData[i].scope == 1) {
            this.scopeValues = this.scopeData[i].scopeName + '/';
          }
          if (this.scopeData[i].scope == 2) {
            this.scopeValues += this.scopeData[i].scopeName + '/';
          }
          if (this.scopeData[i].scope == 4) {
            this.scopeValues += this.scopeData[i].scopeName + '/';
          }
        }
        this.scopeValues = this.scopeValues.slice(0, -1);
      }
    }
    else if (this.isCognizant == 0) {
      this.scopeValues = '';
      this.scopeCustData = this.projectListData.filter(x => x.projectId == this.projectID);
      this.scopeDataList = this.scopeCustomerData.filter(x => x.projectId == this.projectID);
      if (this.scopeDataList.length > 0) {
        for (let i = 0; i < this.scopeDataList.length; i++) {
          if (this.scopeDataList[i].scope == 1) {
            this.scopeValues = this.scopeDataList[i].scopeName + '/';
          }
          if (this.scopeDataList[i].scope == 2) {
            this.scopeValues += this.scopeDataList[i].scopeName + '/';
          }
          if (this.scopeDataList[i].scope == 4) {
            this.scopeValues += this.scopeDataList[i].scopeName + '/';
          }
        }
        this.scopeValues = this.scopeValues.slice(0, -1);
      }
    }
    this.CheckITSM();
  }

  CheckITSM() {
    //Reset
    this.itsmConfigFlag = null;
    this.warningFlag = false;
    this.errorFlag = false;
    this.isTicketDescriptionOptedShow = false;
    this.defaultTicketMessageShow = false;
    this.defaultEffortMessageShow = false;
    this.errITSMShow = false;
    this.warningShow = false;
    this.buttonDisable = false;
    this.copyTPath = false;
    this.copyEPath = false;
    this.errITSMMandateColumn = false;
    this.errITSMPercentage = false;
    this.errITSMManualOrAuto = false;

    const Params = {
      CustomerID: this.customerID,
      ProjectID: this.projectID
    };
    this.TicketEffortUploadServices.CheckITSM(Params).subscribe(x => {
      this.itsmConfigFlag = x.responce;
      if (x.mandateColumn != "1") {
        this.errITSMMandateColumn = true;
        this.buttonDisable = true;
        this.errorFlag = true;
      }
      else if (x.percentage != 100) {
        this.errITSMPercentage = true;
        this.buttonDisable = true;
        this.errorFlag = true;
      }
      else if (x.manualOrAuto == "A") {
        this.errITSMManualOrAuto = true;
        this.buttonDisable = true;
        this.errorFlag = true;
      }
      else if (x.isTicketDescriptionOpted == false) {
        this.isTicketDescriptionOptedShow = true;
        this.buttonDisable = false;
      }
      else {
        this.defaultTicketMessageShow = true;
        this.buttonDisable = false;
      }
      this.defaultEffortMessageShow = true;
      this.GetConfigDetails();
    });
  }

  GetConfigDetails() {
    const Params = {
      ProjectID: this.projectID
    };
    this.ErrorLogServices.GetConfigDetails(Params).subscribe(x => {
      if (x[0].isActive == "1") {
        this.warningFlag = false;
        this.warningShow = false;
      }
      else {
        this.setWarningMessage('Effortbulkuploadisnotenabledforthechosenproject');
        this.warningFlag = !this.displayExemptedMsg;
        this.warningShow = !this.displayExemptedMsg;
      }
      this.GetUploadConfigDetails();
    });
  }

  GetUploadConfigDetails() {
    this.spanTicketPath = '';
    this.spanEffortPath = '';
    this.spanTicketUsers = '';
    this.spanEffortUsers = '';
    const Params = {
      ProjectID: this.projectID,
      EmployeeID: this.employeeID
    };
    this.TicketEffortUploadServices.GetUploadConfigDetails(Params).subscribe(x => {
      if (x.ticketUpload[0] != null && x.ticketUpload[0] != undefined) {
        let arr = x.ticketUpload[0].ticketSharePathUsers.split(';');
        let valtoremove = "0";
        let filteredItems = arr.filter(item => item !== valtoremove);
        let UploadUsers = filteredItems.toString();
        this.spanTicketPath = x.ticketUpload[0].sharePath;
        this.spanTicketUsers = UploadUsers;
        this.spanEffortUsers = UploadUsers;
        this.copyTPath = true;
      }
      else {
        this.copyTPath = false;
      }
      if (x.effortUpload[0] != null && x.effortUpload[0] != undefined) {
        this.spanEffortPath = x.effortUpload[0].sharePathName;
        this.copyEPath = true;
      }
      else {
        this.copyEPath = false;
      }
      this.DefaultTabSelection();
    });
  }

  DefaultTabSelection() {
    if (this.isCognizant == 1 && (this.scopeData.filter(x => (x.scope == "1" || x.scope == "4")).length > 0) && 
    (this.scopeData.filter(x => (x.scope == "2" || x.scope == "3")).length > 0)) {
      this.ShowTicketUpload();
    }
    else if (this.isCognizant == 1 && this.scopeData.filter(x => (x.scope == "1" || x.scope == "4")).length > 0) {
      this.ShowEffortUpload();
    }
    else if (this.isCognizant == 1 && this.scopeData.filter(x => (x.scope == "3")).length > 0) {
      this.ShowEffortUpload();
    }
    else if ((this.isCognizant == 1 && this.scopeData.filter(x => (x.scope == "3")).length > 0)
      || (this.isCognizant == 0 && this.scopeCustData.filter(x => (x.supportTypeId == "2")).length > 0)) {
      this.setWarningMessage('Effortbulkuploadoptioniscurrentlynotenabledfortheproject');
      this.warningFlag = true;
      this.ShowTicketUpload();
    }
    else {
      this.ShowTicketUpload();
    }
  }

  ShowTicketUpload() {
    this.ticketUploadShow = 1;
    if (this.errorFlag == true) {
      this.errITSMShow = true;
    }
    else {
      this.errITSMShow = false;
    }
    this.spinner.hide();
    this.EffortDivVisibilitywarningShow = false;
    this.EffortDivVisibility = true;
  }

  ShowEffortUpload() {
    this.errITSMShow = false;
    if (this.warningFlag == true) {
      this.warningShow = true;
      this.ticketUploadShow = 2;
    }
    else {
      this.warningShow = false;
      this.ticketUploadShow = 2;
    }
    this.spinner.hide();

  }
  OpenDebtUnClassifiedtab(): void {
    this.debtunClassifiedlFG.reset();
    this.errITSMShow = false;
    this.ticketUploadShow = 3;
    this.spinner.hide();
  }
  UploadSelection(val: any) {
    if (val == 1) {
      this.ShowTicketUpload();
    }
    else if (val == 2) {
      this.ShowEffortUpload();
    }
    else if (val == 3) {
      this.OpenDebtUnClassifiedtab();
    }
    else {
      this.ShowTicketUpload();
    }
  }

  SetProject(val: any) {
    this.isCIS = false;
    this.ismaintenancedevtest = false;
    this.spinner.show();
    this.projectID = val;
    this.autoclassificationShow = false;
    this.GetITSMConfigDetails();
    this.projectval= this.SelectedProjectName.filter(x => x.projectId == this.projectID)[0].projectName;
    this.esaprojvalue=this.SelectedProjectName.filter(x => x.projectId == this.projectID)[0].esaProjectId;
    const Params = {
      ProjectID: this.projectID,
      EmployeeID : this.employeeID
    };
    this.TicketEffortUploadServices.GetOnboardingPercentageDetails(Params).subscribe(x => {
      this.onboardingPercentage = x;
      this.debtunclassified();
      this.GetHierarchyValues()
    });
    
    if (this.isCognizant == 1) {
      //cognizant project
      if (this.scopeData.filter(x => x.scope == 1 || x.scope == 2 || x.scope == 4).length > 0) {
        this.ismaintenancedevtest = true;
      } else {
        this.ismaintenancedevtest = false;
      }
      if (this.scopeData.filter(x => x.scope == 3).length > 0) {
        this.isCIS = true;
      } else {
        this.isCIS = false;
      }
    }
    else {
      //customer project
      if (this.scopeCustData.filter(x => x.supportTypeId == '1' || x.supportTypeId == '2' || x.supportTypeId == '4' || x.supportTypeId == '3').length > 0) {
        this.ismaintenancedevtest = true;
      } else {
        this.ismaintenancedevtest = false;
      }
      if (this.scopeCustData.filter(x => x.supportTypeId == '3').length > 0) {
        this.isCIS = true;
      } else {
        this.isCIS = false;
      }
    }
      this.displayExemptedMsg=this.hiddendata.lstProjectUserID
      .filter(x=>x.projectId==val && x.isExempted==true).length>0
      ?true:false;
      if(this.displayExemptedMsg){
      this.ticketUploadShow=1;    
      this.exemptedMsg=Constants.exemptedMessage;
      this.errITSMShow=false;
      this.warningShow=false;
      }
  }

onChangeProject(val:any){
    this.spinner.show();
    this.projectID = val;
    this.autoclassificationShow = false;
    this.GetITSMConfigDetails();
    this.projectval= this.SelectedProjectName.filter(x => x.projectId == this.projectID)[0].projectName;
    this.esaprojvalue=this.SelectedProjectName.filter(x => x.projectId == this.projectID)[0].esaProjectId;
    const Params = {
      ProjectID: this.projectID,
      EmployeeID : this.employeeID
    };
    this.TicketEffortUploadServices.GetOnboardingPercentageDetails(Params).subscribe(x => {
      this.onboardingPercentage = x;
      this.debtunclassified();
      this.GetHierarchyValues()
    });
  }

  setErrorMessage(pattern): void {
    this.translate.get(pattern)
      .subscribe(message => {
        this.errorMessageShow = true;
        this.errorMessage = message;
        setTimeout(() => {
          this.errorMessageShow = false;
          this.errorMessage = '';
        }, Constants.lifeSpan);
      });
  }

  setSuccessMessage(pattern: string): void {
    this.translate.get(pattern)
      .subscribe(message => {
        this.successMessageShow = true;
        this.successMessage = message;
        setTimeout(() => {
          this.successMessageShow = false;
          this.successMessage = '';
        }, Constants.lifeSpan);
      });
  }

  setWarningMessage(pattern: string): void {
    this.translate.get(pattern)
      .subscribe(message => {
        this.warningMessage = message;
      });
  }

  RBChange(evt) {
    let target = evt.target;
    if (target.value == "0") {
      //Infra
      this.IsApp = false;
    }
    else {
      //App
      this.IsApp = true;
    }
    if (target.value == "0" && this.isCognizant == 0) {
      //customer and Infra
      this.setWarningMessage('Effortbulkuploadoptioniscurrentlynotenabledfortheproject');
      this.EffortDivVisibilitywarningShow = true;
      this.EffortDivVisibility = false;
    }
    else if (target.value == "1" && this.isCognizant == 0) {
      //customer and App
      this.EffortDivVisibilitywarningShow = false;
      this.EffortDivVisibility = true;
    }
  }

  showDebtDialog($event: MouseEvent){
    this.displayDebtUnclassifiedPop = true;
    $event.stopPropagation();
  }

  GetHierarchyOnTabChange() {

    const param = {
      customerID: this.customerID,
      associateID: this.employeeID
    };

    this.searchTicketService.GetHierarchyList(param).subscribe(result => {
      this.MasterProjectList = result.projects;
      this.SubBusinessClusterMapping = result.businessCluster;

      if (this.ispureApp == true) {
        this.MasterHierarcyList = this.SubBusinessClusterMapping.filter(x => x.isInfra == false);
        this.MasterHierarcyList = this.MasterHierarcyList.slice(this.MasterHierarcyList.length - 3);
      }
      else if (this.ispureApp == false) {
        this.MasterHierarcyList = this.SubBusinessClusterMapping.filter(x => x.isInfra == true);
        this.MasterHierarcyList = this.MasterHierarcyList.slice(this.MasterHierarcyList.length - 3);
      }
      this.spinner.hide();
      this.Dropdownfunction();

    });
  }

  GetHierarchyValues() {
    const param = {
      customerID: this.customerID,
      associateID: this.employeeID
    };

    this.searchTicketService.GetHierarchyList(param).subscribe(result => {
      this.MasterProjectList = result.projects;
      this.SubBusinessClusterMapping = result.businessCluster;

      if (this.ispureApp == true) {
        this.MasterHierarcyList = this.SubBusinessClusterMapping.filter(x => x.isInfra == false);
        this.MasterHierarcyList = this.MasterHierarcyList.slice(this.MasterHierarcyList.length - 3);
      }
      else if (this.ispureApp == false) {
        this.MasterHierarcyList = this.SubBusinessClusterMapping.filter(x => x.isInfra == true);
        this.MasterHierarcyList = this.MasterHierarcyList.slice(this.MasterHierarcyList.length - 3);
      }
      this.Dropdownfunction();

    });
  }
  Dropdownfunction() {
    this.HierarchyDropdownList = [];
    let SubClusterList = [];
    let finalList = [];
    for (let i = 0; i < this.MasterHierarcyList.length; i++) {
      SubClusterList = [
        {
          "label": this.MasterHierarcyList[i].businessClusterName,
          "formcontrolname": "SelectedHierarchy" + i,
          "List": i == 0 ? this.getDropDownList(this.MasterHierarcyList[i].subBusinessClusterList, 'businessClusterMapId', 'businessClusterBaseName') : [],
          "isinfra": this.SubBusinessClusterMapping[i].isInfra
        },
      ]
      
      this.HierarchyDropdownList.push(SubClusterList[0]);
    }
    this.RowHiearchyList[0].isinfra = this.ispureApp;
    for (let i = 0; i < this.RowHiearchyList.length; i++) {
      this.HierarchyDropdownList.push(this.RowHiearchyList[i]);
    }

    this.CreateFormGroup(this.HierarchyDropdownList);
  }

  getDropDownList(projectDropList, id, name): any {
    projectDropList = this.FilterProjectId(projectDropList);
    let lstlength = projectDropList.length;
    let masterDropDown = [];
    for (let i = 0; i < lstlength; i++) {
      let labelList = projectDropList[i];
      let list = { "label": labelList[name], "value": labelList[id] }
      masterDropDown.push(list);
    }
    let finalArray = [];
    let masterset = new Set(masterDropDown.map(x => x.value));
    masterset.forEach(x => {
      finalArray.push({
        "label": masterDropDown.find(y => y.value == x).label,
        "value": x
      })
    });
    return finalArray.sort((a, b) => {
      if (a.label > b.label) {
        return 1;
      } else if (a.label < b.label) {
        return -1;
      } else {
        return 0;
      }
    });
  
  }



  FilterProjectId(projlist) {
    let filterArray = [];

    for (let i = 0; i < projlist.length; i++) {
      if (projlist[i].projectId == this.projectID) {
        filterArray.push(projlist[i]);
      }
    }
    return filterArray;
  }

  LoadHierarchy(index,name){
    for(let i=index+1;i<=3;i++){
      this.HierarchyDropdownList[i].List=[];
    }
    var AppList =  this.MasterHierarcyList;
    var SubHierarchyList=[];
    var formcontrolnumber = parseInt(name.split(/([0-9]+)/)[1]);
    for (var x = formcontrolnumber + 1; x <= AppList.length; x++) {
      this.debtunClassifiedlFG.get(this.HierarchyDropdownList[x].formcontrolname).reset();
    }
    if (index < (AppList.length - 1)) {
      var FilteredAppList = AppList[index + 1].subBusinessClusterList;
      var SubHierarchyLength = this.debtunClassifiedlFG.get(name).value;
      var HierarchyListFinal = [];

      for (var i = 0; i < SubHierarchyLength.length; i++) {
        let x = FilteredAppList.filter(x => x.parentBusinessClusterMapId ===
          SubHierarchyLength[i].value);
        x.forEach(x => { SubHierarchyList.push(x) });

        if (SubHierarchyList.length > 0) {
          for (var y = 0; y < SubHierarchyList.length; y++) {
            HierarchyListFinal.push(SubHierarchyList[y]);
          }
        }
      }
      this.HierarchyDropdownList[index + 1].List = this.getDropDownList(HierarchyListFinal, 'businessClusterMapId', 'businessClusterBaseName')
    }
    else {
      var FilteredAppList = AppList[index].subBusinessClusterList;
      var appList = FilteredAppList[0].applicationList;
      if (appList.length > 0) {
        var SubHierarchyLength = this.debtunClassifiedlFG.get(name).value;
        var AppListFinal = [];
        var AppFinalList = [];

        for (var i = 0; i < SubHierarchyLength.length; i++) {
          let x = appList.filter(x => x.parentBusinessClusterId ===
            SubHierarchyLength[i].value);
          x.forEach(x=>{SubHierarchyList.push(x)});
        }
        this.HierarchyDropdownList[index + 1].List = this.getDropDownList(SubHierarchyList, 'applicationId', 'applicationName')
      }
    }
  }



  DownloadClassifiedTickets(){
    let DownloadselectedParaLoad = [];
    var remAppList =  this.MasterHierarcyList; //AppList
    var SubHierarchyList=[];
    for(let i=1;i<this.HierarchyDropdownList.length;i++){
      if(this.debtunClassifiedlFG.get(this.HierarchyDropdownList[i].formcontrolname).value == null ||
        this.debtunClassifiedlFG.get(this.HierarchyDropdownList[i].formcontrolname).value.length == 0){
        DownloadselectedParaLoad.push(this.HierarchyDropdownList[i]);
      }

    }
    for(let i=0;i<DownloadselectedParaLoad.length-1;i++){
      var index = parseInt((DownloadselectedParaLoad[i].formcontrolname).split(/([0-9]+)/)[1]);
      var name = DownloadselectedParaLoad[i].formcontrolname;
      var HierarchyListFinal = [];

      if (index < (remAppList.length - 1)) {

        var FilteredAppList = remAppList[index+1].subBusinessClusterList;
        var SubHierarchyLength = DownloadselectedParaLoad[i].List;
        var HierarchyListFinal = [];

        for (var j = 0;j < SubHierarchyLength.length; j++) {
          let x= FilteredAppList.filter(x => x.parentBusinessClusterMapId ===
            SubHierarchyLength[j].value);
          x.forEach(x=>{SubHierarchyList.push(x)});

          if (SubHierarchyList.length > 0) {
            for (var y = 0; y < SubHierarchyList.length; y++) {
              HierarchyListFinal.push(SubHierarchyList[y]);
            }
          }
        }
        DownloadselectedParaLoad[index].List = this.getDropDownList(HierarchyListFinal, 'businessClusterMapId', 'businessClusterBaseName')
      }
      else{
        var FilteredAppList = remAppList[index].subBusinessClusterList;
        var appList = FilteredAppList[0].applicationList;
        if (appList.length > 0) {
          var SubHierarchyLength = DownloadselectedParaLoad[DownloadselectedParaLoad.length-2].List;
          for (var j = 0; j < SubHierarchyLength.length; j++) {
            let x= appList.filter(x => x.parentBusinessClusterId  ===
              SubHierarchyLength[j].value);
            x.forEach(x=>{SubHierarchyList.push(x)});
          }
          DownloadselectedParaLoad[DownloadselectedParaLoad.length-1].List = this.getDropDownList(SubHierarchyList, 'applicationId', 'applicationName')
        }
      }
    }
    this.ApplicationTowerids="";
    var UploadAppTowerParameter=[];
    var AppFinalList = [];
    AppFinalList = DownloadselectedParaLoad.slice(DownloadselectedParaLoad.length-1);
    for(var i=0;i<AppFinalList[0].List.length;i++){
      UploadAppTowerParameter.push(AppFinalList[0].List[i].value);
    }

    for (var i = 0; i < UploadAppTowerParameter.length; i++) {
      if(UploadAppTowerParameter[i]!=undefined){
        this.ApplicationTowerids += UploadAppTowerParameter[i] + ',';
      }
    }

  }
}
