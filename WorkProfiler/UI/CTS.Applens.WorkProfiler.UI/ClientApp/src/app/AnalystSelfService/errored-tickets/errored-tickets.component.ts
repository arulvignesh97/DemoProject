// Copyright (c) Applens. All Rights Reserved.
import { AfterViewChecked, Component, EventEmitter, HostListener, OnInit, Output, ViewChild } from '@angular/core';
import { HeaderService} from 'src/app/Layout/services/header.service';
import { MasterDataModel } from 'src/app/Layout/models/header.models';
import { ErroredTicketsService } from '../Service/errored-tickets.service';
import { SelectItem } from 'primeng/api';
import { FormBuilder,FormGroup,FormControl } from '@angular/forms';
import { AddTicketService } from '../Service/add-ticket.service';
import { ChooseTicketService } from '../Service/choose-ticket.service';
import { AnalystselfserviceService } from '../analystselfservice.service';
import { BsDatepickerDirective, BsDatepickerModule, DatepickerDateCustomClasses } from 'ngx-bootstrap/datepicker';
import { ErrorTicketFilterPipe } from '../Pipes/error-ticket-filter.pipe';
import { ErroredTicketDetails, ErrorTicketModel, ErrorTicketPostModel, SearchModel } from '../Models/ErrorTicketsModels';
import { DatePipe } from '@angular/common';
import { SpinnerService } from './../../common/services/spinner.service';
import { DynamicgridComponent } from 'src/app/AnalystSelfService/dynamicgrid/dynamicgrid.component';
import { Table } from 'primeng/table';
import { Constants } from 'src/app/common/constants/constants';
import { AppSettingsConfig } from 'src/app/app.settings.config';
import { LayoutService } from 'src/app/common/services/layout.service';

@Component({
  selector: 'app-errored-tickets',
  templateUrl: './errored-tickets.component.html',
  styleUrls: ['./errored-tickets.component.css']
})
export class ErroredTicketsComponent implements OnInit, AfterViewChecked {
  public FromdatePickerConfig: Partial<BsDatepickerModule>;
  hdnCustomerId: string;
  hdnEmployeeId: string;
  hdnScopeLst = [];
  hdnlstscope = [];
  hdnprjlist = [];
  debtList = [];
  ticketList: ErrorTicketModel[] = [];
  ticketListMasterList: ErrorTicketModel[] = [];
  projectList: SelectItem[] = [];
  statusList: SelectItem[] = [];
  priorityList: SelectItem[] = [];
  ticketTypeList: SelectItem[] = [];
  towerList: SelectItem[] = [];
  assignmentGroupList: SelectItem[] = [];
  applicationList: SelectItem[] = [];
  causeList: SelectItem[] = [];
  resolutionList: SelectItem[] = [];
  debtClassificationList: SelectItem[] = [];
  avoidableFlgList: SelectItem[] = [];
  residualFlgList: SelectItem[] = [];
  partialList:  SelectItem[] = [];
  severityList:   SelectItem[] = [];
  supportType: number;
  prjSupportType: number;
  projectId: number;
  counter:number = 0;
  showSearchBox: boolean = false;
  scrHeight: number = 0;
  scrWidth : number = 0;
  itemsPerPage: number = 8;
  currentPage: number = 1;
  totalPages: number = 1;
  previousPage: number = 0;
  itemsPerPageDropdown: string[] = ["8","10","20","30"];
  dateCustomClasses: DatepickerDateCustomClasses[];
  searchData: SearchModel;
  isDevErrorlog: boolean = false;
  isErrorMsg: boolean = false;
  isRecordsFound: boolean = false;
  isTranslationFailure: boolean = false;
  isEncryptionFailure : boolean = false;
  showErrorGrid: boolean = false;
  rowIdArray: number[] = [];
  errorList: ErrorTicketPostModel[] = [];
  isCauseCodeVisible: boolean = true;
  isResolutionCodeVisible: boolean = true;
  isDebtClassificationVisible: boolean = true;
  isAvoidableFlagVisible: boolean = true;
  isResidualtDebtVisible: boolean = true;
  @Output() closeValue = new EventEmitter<boolean>();
  isAppMode: boolean = true;
  errorLogloading = false;
  @ViewChild('errorTicketsGrid') errorTicketsGrid: Table;
  searchTicketId: string;
  searchTicketDescription: string;
  searchApplicationName: string;
  searchTicketType: string;
  searchPriority: string;
  searchStatus: string;
  searchOpenDate: string;
  searchSeverity: string;
  searchCauseCode: string;
  searchResolutionCode: string;
  searchDebtCategory: string;
  searchAvoidable: string;
  searchResidualDebt: string;
  searchIsPartialAutomated: string;
  searchTower: string;
  searchAssignment: string;
  toggleErrorFilter: boolean = true;
  toggleFilter = true;
  toggleicon = Constants.toggleIconOn;
  toggleErroricon = Constants.toggleIconOn;
  maxdate: Date = new Date();
  public datePickerConfig: Partial<BsDatepickerModule>;
  minCompletionDate: Date = new Date();
  displayExemptedMsg=false;
  exemptedMsg:string;


  @ViewChild('dtGrid') dtGrid: Table;
  @ViewChild('baseMeasuredp', { static: false }) datepicker: BsDatepickerDirective;
  

  public erroredTicketsFG: FormGroup = new FormGroup({});
  hiddendata: any;
  constructor(private headerService: HeaderService,
              private erroredTicketsService: ErroredTicketsService,private fb: FormBuilder,
              private addTicketService: AddTicketService, private chooseTicketService: ChooseTicketService,
              private analystselfserviceService: AnalystselfserviceService, private datePipe: DatePipe,
              private spinner: SpinnerService,private dynamicgridComponent: DynamicgridComponent,
              private layoutService:LayoutService ) { 
                this.getScreenSize();
              }
  ngAfterViewChecked(): void {
    if(document.getElementsByClassName('p-datepicker') != undefined){
      for (let index = 0; index < document.getElementsByClassName('p-datepicker').length; index++) {
        document.getElementsByClassName('p-datepicker')[index].setAttribute("style","top: 2px;z-index:999;padding:0;");
        for (let j = 0; j < document.getElementsByClassName('p-datepicker')[index].getElementsByTagName("td").length; j++) {
          document.getElementsByClassName('p-datepicker')[index].getElementsByTagName("td")[j].setAttribute("style","padding:0");
        }
      }
    }
    if(document.getElementsByClassName('p-datepicker p-datepicker-header') != undefined){
      for (let index = 0; index < document.getElementsByClassName('p-datepicker p-datepicker-header').length; index++) {
        document.getElementsByClassName('p-datepicker p-datepicker-header')[index].setAttribute("style","padding:0px;");
      }
    }
    if(document.getElementsByClassName('p-datatable-tbody') != undefined){
      for(let index = 0; index < document.getElementsByClassName('p-datatable-tbody').length; index++){
        document.getElementsByClassName('p-datatable-tbody')[index].setAttribute("style","width: min-content;");
      }
    }
    if(document.getElementsByClassName('p-inputtext') != undefined){
      for(let index = 0; index < document.getElementsByClassName('p-inputtext').length; index++){
        document.getElementsByClassName('p-inputtext')[index].setAttribute("style","padding: 7.5px;padding-top: 8.5px;font-size:0.8rem;");
      }
    }
    if(document.getElementsByClassName('p-dropdown') != undefined){
      for(let index = 0; index < document.getElementsByClassName('p-dropdown').length; index++){
        document.getElementsByClassName('p-dropdown')[index].setAttribute("style","width: 100% !important;");
      }
    }
    if(document.getElementsByClassName('p-paginator-left-content') != undefined){
      for(let index = 0; index < document.getElementsByClassName('p-paginator-left-content').length; index++){
        document.getElementsByClassName('p-paginator-left-content')[index].setAttribute("style","width:50%!important");
      }
    }
    if(document.getElementsByClassName('p-button') != undefined){
      for(let index = 0; index < document.getElementsByClassName('p-button').length; index++){
        if(index == 0){
          document.getElementsByClassName('p-button')[index].setAttribute("style","width:15%!important");
        }
        else{
          document.getElementsByClassName('p-button')[index].setAttribute("style","width:30%!important");
        }
      }
    }
  }

  @HostListener('window:onLoad')
  @HostListener('window:resize')
  getScreenSize() {
        this.scrHeight = window.innerHeight;
        this.scrWidth = window.innerWidth;
  }
  ngOnInit(): void {
    this.datePickerConfig = Object.assign({},
      {
        showWeekNumbers: false,
        dateInputFormat: Constants.dateInputFormat,
        todayHighlight: true,
        maxDate : new Date(),
      });
    this.searchData = {
      searchApplicationName: null,
      searchAvoidable: null,
      searchCauseCode: null,
      searchDebtCategory: null,
      searchIsPartialAutomated: null,
      searchOpenDate: null,
      searchPriority: null,
      searchResidualDebt: null,
      searchResolutionCode: null,
      searchSeverity: null,
      searchStatus: null,
      searchTicketDescription: null,
      searchTicketId: null,
      searchTicketType: null,
      searchAssignment: null,
      searchTower: null
    }
    this.dateCustomClasses = [
      { date:  new Date(), classes: ['backgblue'] }
    ];
    this.FromdatePickerConfig = Object.assign({},
      {
        showWeekNumbers: false,
        dateInputFormat: 'dd/MM/YYYY',
        todayHighlight: true
        });
    this.erroredTicketsFG = this.fb.group({
      projectId: new FormControl(),
      userTimeZoneName: new FormControl(),
      supportTypeId: new FormControl(),
      app: new FormControl(),
      infra: new FormControl(),
    });
    this.headerService.masterDataEmitter.subscribe((masterData: MasterDataModel) => {
      if(masterData != null){
      this.hiddendata = masterData.hiddenFields;
      this.hdnCustomerId = masterData.hiddenFields.customerId;
      this.hdnEmployeeId = masterData.hiddenFields.employeeId;
      this.hdnScopeLst = masterData.hiddenFields.lstScope;
      }
    });
    let errorParams = {
      CustomerId: this.hdnCustomerId,
      EmployeeId: this.hdnEmployeeId
    };
    this.erroredTicketsService.getErrorLogTickets(errorParams).subscribe(x => {
    if(x !=undefined && x.length != 0){
      let prjlst = x.lstProjectsModel;
      this.projectList = this.getDropDownList(prjlst,'projectId','projectName','supportTypeId','userTimeZoneName','projectTimeZoneName');
      this.projectId = this.erroredTicketsFG.get('projectId').value;
      this.showErrorGrid = false;
      if (this.projectId == 0 || this.projectId == null && (prjlst != null && prjlst.length > 0)) {
        this.onDropDownChange(this.projectList[0].value);
      }
    }
    else {
      this.erroredTicketsFG.controls["projectId"].setValue('');
      this.spinner.hide();
    }
  });
  }
  getDropDownList(projectDropList,id,name,masterId,userTimeZoneName,projectTimeZoneName):any{
    if(projectDropList != null && projectDropList.length != 0 && projectDropList != undefined){
      let lstlength = projectDropList.length;
      let masterDropDown = [];
    if(lstlength > 0 && name == "ticketType" && (this.prjSupportType == 1 || this.prjSupportType == 3)){
      let appLst = projectDropList.filter(a => a.supportTypeId == 1);
      let appInfraLst = projectDropList.filter(a => a.supportTypeId == 3);
      let finalLst = appLst.concat(appInfraLst);
      if(finalLst.length > 0){
        projectDropList = [];
        projectDropList = finalLst;
        lstlength = finalLst.length;
      }
    }
    else if(lstlength > 0 && name == "ticketType" && this.prjSupportType == 2){
      let appLst = projectDropList.filter(a => a.supportTypeId == 3);
      let infraLst = projectDropList.filter(a => a.supportTypeId == 2);
      let finalLst = appLst.concat(infraLst);
      if(finalLst.length > 0){
        projectDropList = [];
        projectDropList = finalLst;
        lstlength = finalLst.length;
      }
    }
    else{
      projectDropList;
    }
    for(let i=0; i< lstlength; i++){     
      let labelList = projectDropList[i];
      let list = {"label":labelList[name],"value":labelList[id],
        "masterId": labelList[masterId], "userTimeZoneName": labelList[userTimeZoneName], "projectTimeZoneName": labelList[projectTimeZoneName]};
      masterDropDown.push(list);
    }
    if(id == "ResolutionId" || id == "DebtClassificationId" || id == "SeverityId"){
      masterDropDown.splice(0,1);
    }
    return masterDropDown;
  }
  }
  onDropDownChange(prjId){
    this.ticketList.length = 0;
    this.showErrorGrid = false;
    this.isDevErrorlog = false;
    this.isErrorMsg = false;
    this.isRecordsFound = false;
    this.isTranslationFailure = false;
    this.isEncryptionFailure = false;
    this.erroredTicketsFG.controls["projectId"].setValue(prjId);
    if (this.projectList.filter(x => x.value == prjId)) {
      this.hdnprjlist = this.projectList.filter(x => x.value == prjId);
      this.erroredTicketsFG.controls["supportTypeId"].setValue(this.hdnprjlist[0].masterId);
      this.erroredTicketsFG.controls["userTimeZoneName"].setValue(this.hdnprjlist[0].userTimeZoneName);
      this.supportType = this.erroredTicketsFG.get('supportTypeId').value;
    }
    this.decideScope();
  }
  decideScope() {
    let scope = [];
    let prjId = this.erroredTicketsFG.get('projectId').value;
    for (let i = 0; i < this.hdnScopeLst.length; i++) {
      if (this.hdnScopeLst[i].projectId == prjId) {
        let scopeList = { "projectId": this.hdnScopeLst[i].projectId, "scope": this.hdnScopeLst[i].scope };
        scope.push(scopeList);
      }
    }
    if ((scope.filter(y => (y.scope == 1 || y.scope == 4)).length > 0)
      && (scope.filter(y => (y.scope == 2 || y.scope == 3)).length == 0)) {
      this.isErrorMsg = true;
      this.displayExemptedMsg=this.hiddendata.lstProjectUserID
            .filter(x=>x.projectId==prjId && x.isExempted==true).length>0
            ?true:false;
      if(this.displayExemptedMsg){
        this.exemptedMsg=Constants.exemptedMessage;
      }
      else{
        this.isDevErrorlog = true;
      }
    }
    else {
      this.loadErrorLogGrid(0);
    }

  }
  loadErrorLogGrid(supprtTypeParam){
    this.applicationList = [];
    this.towerList = [];
    this.assignmentGroupList = [];
    this.ticketTypeList = [];
    this.priorityList = [];
    this.statusList = [];
    this.severityList = [];
    this.causeList = [];
    this.resolutionList = [];
    this.debtClassificationList = [];
    this.avoidableFlgList = [];
    this.residualFlgList = [];
    this.partialList = [];
    this.debtList = [];
    this.isErrorMsg = this.displayExemptedMsg;
    this.isRecordsFound = false;
    this.spinner.show();
    this.prjSupportType = 0;
    this.rowIdArray = [];
    if (supprtTypeParam == null || supprtTypeParam == undefined || supprtTypeParam == 'undefined' ||   supprtTypeParam == 0) {
      this.prjSupportType = this.supportType;
    }
    else {
      this.prjSupportType = supprtTypeParam;
    }
    if(this.supportType  == 1){
      (<HTMLInputElement>(document.getElementById("appId"))).disabled = false;
      (<HTMLInputElement>(document.getElementById("appId"))).checked = true;
      (<HTMLInputElement>(document.getElementById("infraId"))).disabled = true;
    }
    else if(this.supportType  == 3){
      (<HTMLInputElement>(document.getElementById("appId"))).disabled = false;
      (<HTMLInputElement>(document.getElementById("infraId"))).disabled = false;
      if((<HTMLInputElement>(document.getElementById("appId"))).checked || supprtTypeParam == undefined || supprtTypeParam == null || supprtTypeParam == ''){
        (<HTMLInputElement>(document.getElementById("appId"))).checked = true;
      }
      else{
        (<HTMLInputElement>(document.getElementById("infraId"))).checked = true;
      }
    }
    else{
      (<HTMLInputElement>(document.getElementById("infraId"))).disabled = false;
      (<HTMLInputElement>(document.getElementById("infraId"))).checked = true;
      (<HTMLInputElement>(document.getElementById("appId"))).disabled = true;
    }
    if (this.supportType == 1) {
      (<HTMLInputElement>(document.getElementById("appId"))).disabled = false;
      (<HTMLInputElement>(document.getElementById("appId"))).checked = true;
      (<HTMLInputElement>(document.getElementById("infraId"))).disabled = true;
    }
    else if (this.supportType == 3) {
      (<HTMLInputElement>(document.getElementById("appId"))).disabled = false;
      (<HTMLInputElement>(document.getElementById("infraId"))).disabled = false;
      if ((<HTMLInputElement>(document.getElementById("appId"))).checked || supprtTypeParam == undefined || supprtTypeParam == null || supprtTypeParam == '') {
        (<HTMLInputElement>(document.getElementById("appId"))).checked = true;
      }
      else {
        (<HTMLInputElement>(document.getElementById("infraId"))).checked = true;
      }
    }
    else {
      (<HTMLInputElement>(document.getElementById("infraId"))).disabled = false;
      (<HTMLInputElement>(document.getElementById("infraId"))).checked = true;
      (<HTMLInputElement>(document.getElementById("appId"))).disabled = true;
    }
    let statusMasterArr = [];
    let priorityMasterArr = [];
    let ticketTypeMasterArr = [];
    let infraTowerDetailsArr = [];
    let assignmentGroupDetailsArr = [];
    this.projectId = this.erroredTicketsFG.get('projectId').value;
    if (this.projectId != null) {
      this.addTicketService
        .GetDetailsByProjectID(this.projectId)
        .subscribe((response: any) => {
          statusMasterArr = response.lstStatusListForAdd;
          priorityMasterArr = response.lstPriorityListForAdd;
          ticketTypeMasterArr = response.lstTicketTypeListForAdd;
          assignmentGroupDetailsArr = response.lstAssignmentGroupDetails;
          infraTowerDetailsArr = response.lstTowerDetails;
        this.statusList =  this.getDropDownList(statusMasterArr,'statusId','statusName','ticketStatusId','' ,'');
        this.priorityList =  this.getDropDownList(priorityMasterArr,'priorityId','priorityName','priorityMasId','','');
        this.ticketTypeList =  this.getDropDownList(ticketTypeMasterArr,'ticketTypeId','ticketType','ticketTypemasId','','');
        this.towerList =  this.getDropDownList(infraTowerDetailsArr,'towerId','tower','','','');
        this.assignmentGroupList =  this.getDropDownList(assignmentGroupDetailsArr,'assignmentGroupId','assignmentGroupName','','','');      
      this.chooseTicketService.GetApplicationDetailsByProject(this.projectId).subscribe(x => {
        this.applicationList =  this.getDropDownList(x,'applicationId','applicationName','','','');
        });
        let errorDebtArgs = {
        ProjectId: this.projectId.toString(),
        SupportTypeId: this.prjSupportType,
        ApplicationId:0
      }
      this.erroredTicketsService.getDebtFields(errorDebtArgs).subscribe(x => {
          this.debtList.push(JSON.parse(x));
          this.causeList =  this.getDropDownList(this.debtList[0].LstCause,'CauseId','CauseName','','','');
          this.resolutionList =  this.getDropDownList(this.debtList[0].LstResolution,'ResolutionId','ResolutionName','','','');
          this.debtClassificationList =  this.getDropDownList(this.debtList[0].LstDebtClassification,'DebtClassificationId','DebtClassificationName','','','');
          this.severityList =  this.getDropDownList(this.debtList[0].LstSeverity,'SeverityId','SeverityName','','','');
          this.partialList =  this.getDropDownList(this.debtList[0].LstMetSLA,'Id','Value','','','');
          let avoidableFlag = {};
          let avoidableFlagArr = [];
          avoidableFlag = { Id: 1, Value: 'Yes' };
          avoidableFlagArr.push(avoidableFlag);
          avoidableFlag = {};
          avoidableFlag = { Id: 2, Value: 'No' };
          avoidableFlagArr.push(avoidableFlag);
          this.avoidableFlgList =  this.getDropDownList(avoidableFlagArr,'Id','Value','','','');
          let residualFlag = {};
          let residualFlagArr = [];
          residualFlag = { Id: 1, Value: 'Yes' };
          residualFlagArr.push(residualFlag);
          residualFlag = {};
          residualFlag = { Id: 2, Value: 'No' };
          residualFlagArr.push(residualFlag);
          this.residualFlgList =  this.getDropDownList(residualFlagArr,'Id','Value','','','');


            let prjId = this.erroredTicketsFG.get('projectId').value;
            this.displayExemptedMsg=this.hiddendata.lstProjectUserID
            .filter(x=>x.projectId==prjId && x.isExempted==true).length>0
            ?true:false;
            if(!this.displayExemptedMsg && !this.isDevErrorlog){
              this.isErrorMsg=false;
              this.showGrid(this.prjSupportType);
            }
            else{
            this.isErrorMsg = true;
            this.exemptedMsg=Constants.exemptedMessage;
            this.spinner.hide()
            this.isRecordsFound=false;
            }        
      });
    });
  }
  }
  setDateText(date){
    if ("0"+"0"+"0"+new Date(date).getFullYear() == "0001" || date == ""){                   
     return "";
    }
    else {
      let attrDate = new Date(date);
      return attrDate;
    }
  }
  showGrid(prjSupportType){
    this.isAppMode = (prjSupportType == 1 || prjSupportType == 3)? true: false;
    this.searchData = {
      searchApplicationName: null,
      searchAvoidable: null,
      searchCauseCode: null,
      searchDebtCategory: null,
      searchIsPartialAutomated: null,
      searchOpenDate: null,
      searchPriority: null,
      searchResidualDebt: null,
      searchResolutionCode: null,
      searchSeverity: null,
      searchStatus: null,
      searchTicketDescription: null,
      searchTicketId: null,
      searchTicketType: null,
      searchAssignment: null,
      searchTower: null
    }
    let param = {
      ProjectId: this.projectId
    }
    this.erroredTicketsService.getTicketDebtDetails(param).subscribe((debtData: any) => {
      if(debtData != null || debtData.length !=0 || debtData!= undefined){
        this.isCauseCodeVisible = (debtData.isDebtEnabled || debtData.causecode);
        this.isResolutionCodeVisible = (debtData.isDebtEnabled || debtData.resolutionCode);
        this.isDebtClassificationVisible = (debtData.isDebtEnabled || debtData.debtClassification);
        this.isAvoidableFlagVisible = (debtData.isDebtEnabled || debtData.avoidableFlag);
        this.isResidualtDebtVisible = (debtData.isDebtEnabled || debtData.residualDebt);
      }
      if(prjSupportType==3){
         prjSupportType=1;
      }
      let paramData = { ProjectId: this.projectId.toString(), EmployeeId: this.hdnEmployeeId , SupportTypeID: prjSupportType}
      this.erroredTicketsService.getErrorLogTicketData(paramData).subscribe((y: any[]) => {
        this.ticketListMasterList = [];
        this.ticketList = [];
        let applicationData: any;
        let priorityData: any;
        let statusData: any;
        let severityData: any;
        let causeCodeData: any;
        let resolutionData: any;
        let debtClassificationData: any;
        let avoidableData: any;
        let residualData: any;
        let towerData: any;
        let assignmentGrpData: any;
        for (let index = 0; index < y.length; index++) {
          if(prjSupportType == 1 && this.applicationList != undefined && y[index].application != "" && y[index].application != null && y[index].application != undefined){
            applicationData = this.applicationList.find(s=>s.label.toLocaleLowerCase().trim().replace(/\s/g,'') == 
            y[index].application.toLocaleLowerCase().trim().replace(/\s/g,''));
          }
          if(applicationData == undefined || applicationData == null || applicationData == 0){
            applicationData ={label: "", value: 0, masterId: "", userTimeZoneName: "", projectTimeZoneName: ""}
          }
          if(y[index].priority != "" && this.priorityList != undefined && y[index].priority != null && y[index].priority != undefined){
            priorityData = this.priorityList.find(a=>a.label.toLocaleLowerCase().trim().replace(/\s/g,'') == 
            y[index].priority.toLocaleLowerCase().trim().replace(/\s/g,''));
          }
          if(y[index].status != "" && this.statusList != undefined && y[index].status != null && y[index].status != undefined){
            statusData = this.statusList.find(b=>b.label.toLocaleLowerCase().trim().replace(/\s/g,'') == 
            y[index].status.toLocaleLowerCase().trim().replace(/\s/g,''));
          }
          if(y[index].severity != "" && this.severityList != undefined && y[index].severity != null && y[index].severity != undefined){
            severityData = this.severityList.find(c=>c.label.toLocaleLowerCase().trim().replace(/\s/g,'') == 
            y[index].severity.toLocaleLowerCase().trim().replace(/\s/g,''));
          }
          if(y[index].causecode != "" && this.causeList != undefined && y[index].causeCode != null && y[index].causeCode != undefined){
            causeCodeData = this.causeList.find(d=>d.label.toLocaleLowerCase().trim().replace(/\s/g,'') == 
            y[index].causeCode.toLocaleLowerCase().trim().replace(/\s/g,''));
          }
          if(y[index].resolutionCode != "" && this.resolutionList != undefined && y[index].resolutionCode != null && y[index].resolutionCode != undefined){
            resolutionData = this.resolutionList.find(e=>e.label.toLocaleLowerCase().trim().replace(/\s/g,'') == 
            y[index].resolutionCode.toLocaleLowerCase().trim().replace(/\s/g,''));
          }
          if(y[index].debtClassification != "" && this.debtClassificationList != undefined && y[index].debtClassification != null && 
          y[index].debtClassification != undefined){
            debtClassificationData = this.debtClassificationList.find(f=>f.label.toLocaleLowerCase().trim().replace(/\s/g,'') == 
            y[index].debtClassification.toLocaleLowerCase().trim().replace(/\s/g,''));
          }
          if(y[index].avoidableFlag != "" && this.avoidableFlgList != undefined && y[index].avoidableFlag != null && y[index].avoidableFlag != undefined){
            avoidableData = this.avoidableFlgList.find(g=>g.label.toLocaleLowerCase().trim().replace(/\s/g,'') == 
            y[index].avoidableFlag.toLocaleLowerCase().trim().replace(/\s/g,''));
          }
          if(y[index].residualDebt != "" && this.residualFlgList != undefined && y[index].residualDebt != null && y[index].residualDebt != undefined){
            residualData = this.residualFlgList.find(h=>h.label.toLocaleLowerCase().trim().replace(/\s/g,'') == 
            y[index].residualDebt.toLocaleLowerCase().trim().replace(/\s/g,''));
          }
          if(prjSupportType == 2 && this.towerList != undefined && y[index].tower != "" && y[index].tower != null && y[index].tower != undefined){
            towerData = this.towerList.find(k=>k.label.toLocaleLowerCase().trim().replace(/\s/g,'') == 
            y[index].tower.toLocaleLowerCase().trim().replace(/\s/g,''));
          }
          if(prjSupportType == 2 && this.assignmentGroupList != undefined && y[index].assignmentGroup != "" && y[index].assignmentGroup != null && 
          y[index].assignmentGroup != undefined){
            assignmentGrpData = this.assignmentGroupList.find(l=>l.label.toLocaleLowerCase().trim().replace(/\s/g,'') == 
            y[index].assignmentGroup.toLocaleLowerCase().trim().replace(/\s/g,''));
          }
          this.ticketListMasterList.push({
            id: index + 1,
            application: y[index].application,
            applicationId: prjSupportType == 1 && this.applicationList != undefined && 
                           this.applicationList.find(f=>f.value == y[index].applicationId) == undefined ? 0:((y[index].applicationId == 0 && 
                            y[index].application != "" && y[index].application != null && 
                            y[index].application != undefined) ? applicationData.value : y[index].applicationId),
            assignee:  y[index].assignee,
            assignmentGroup:  y[index].assignmentGroup,
            assignmentGroupId: prjSupportType == 2 && this.assignmentGroupList != undefined &&
                               this.assignmentGroupList.find(f=>f.value == y[index].assignmentGroupId) == undefined ? 0: y[index].assignmentGroupId,
            avoidableFlag: y[index].avoidableFlag,
            avoidableFlagId:  this.avoidableFlgList != undefined && 
                              this.avoidableFlgList.find(f=>f.value == y[index].avoidableFlagId) == 
                              undefined ? 0:((avoidableData != undefined && y[index].avoidableFlagId != 1 && y[index].avoidableFlagId != 2 && 
                                y[index].avoidableFlag != "" && y[index].avoidableFlag != null && 
                                y[index].avoidableFlag != undefined) ? avoidableData.value : y[index].avoidableFlagId),
            causeCode: y[index].causeCode,
            causeCodeId: this.causeList != undefined && 
                         this.causeList.find(f=>f.value == y[index].causeCodeId) == undefined ? 0: y[index].causeCodeId,
            debtClassification: y[index].debtClassification,
            debtClassificationId:  this.debtClassificationList != undefined && 
                                   this.debtClassificationList.find(f=>f.value == y[index].debtClassificationId) == undefined ? 0: y[index].debtClassificationId,
            employeeId: y[index].employeeId,
            employeeName: y[index].employeeName,
            errorLogTicketDetails: y[index].errorLogTicketDetails,
            externalLoginId:  y[index].externalLoginId,
            isAHTicket:  y[index].isAHTicket,
            isDeleted:  y[index].isDeleted,
            isManual:  y[index].isManual,
            isPartiallyAutomated:  y[index].isPartiallyAutomated,
            isTicketDescriptionModified:  y[index].isTicketDescriptionModified,
            mTicketDescription:  y[index].mTicketDescription,
            openDate:  this.setDateText(new Date(y[index].openDate)) != "" ? this.datePipe.transform(new Date(y[index].openDate), Constants.dateFormat): null,
            openDate2:  new Date(y[index].openDate2).toISOString().slice(0, 16),
            priority:  y[index].priority,
            priorityId: this.priorityList != undefined && 
                        this.priorityList.find(f=>f.value == y[index].priorityId) == undefined ? 0: y[index].priorityId,
            projectId:  y[index].projectId,
            residualDebt:  y[index].residualDebt,
            residualDebtId:  this.residualFlgList != undefined && 
                             this.residualFlgList.find(f=>f.value == y[index].residualDebtId) == undefined ? 0:((residualData != undefined && y[index].residualDebtId != 1 && 
                              y[index].residualDebtId != 2 && y[index].residualDebtId != "" && 
                              y[index].residualDebtId != null && y[index].residualDebt != undefined) ? residualData.value : y[index].residualDebtId),
            resolutionCode:  y[index].resolutionCode,
            resolutionId:   this.resolutionList != undefined && 
                            this.resolutionList.find(f=>f.value == y[index].resolutionId) == undefined ? 0: y[index].resolutionId,
            severity:  y[index].severity,
            severityId:  this.severityList != undefined && 
                         this.severityList.find(f=>f.value == y[index].severityId) == undefined ? 0: y[index].severityId,
            status:  y[index].status,
            statusId:  this.statusList != undefined && 
                       this.statusList.find(f=>f.value == y[index].statusId) == undefined ? 0: y[index].statusId,
            ticketDescription:  y[index].ticketDescription,
            ticketId:  y[index].ticketId,
            ticketType:  y[index].ticketType,
            ticketTypeId:  this.ticketTypeList != undefined && 
                           this.ticketTypeList.find(f=>f.value == y[index].ticketTypeId) == undefined ? 0: y[index].ticketTypeId,
            tower:  y[index].tower,
            towerId:  prjSupportType == 2 && this.towerList != undefined && 
                      this.towerList.find(f=>f.value == y[index].towerId) == undefined ? 0: y[index].towerId
          });
        }
        if( this.ticketListMasterList.length != 0 && this.ticketListMasterList != undefined){
          this.showErrorGrid = true;
          for (let index = 0; index < ((this.ticketListMasterList.length > this.itemsPerPage) ? this.itemsPerPage : this.ticketListMasterList.length); index++) {
            this.ticketList.push(this.ticketListMasterList[index]);
          }
          this.showSearchBox = (this.ticketList.length > 0);
          this.totalPages = (this.ticketList.length % this.itemsPerPage) == 0 ? (this.ticketList.length / this.itemsPerPage) : 
          (Math.trunc(this.ticketList.length / this.itemsPerPage) + 1);
          this.spinner.hide();
        }
        else {
          this.spinner.hide();
          this.showErrorGrid = false;
          this.isErrorMsg = true;
          this.isRecordsFound = true;
        }
      });
    });
  }
  ChangeItemsPerPage(event){
    this.itemsPerPage = +event.target.value;
    this.LoadItemsPerPage(+event.target.value, this.ticketListMasterList);
  }

  LoadItemsPerPage(numberOfItemsPerPage: number, ticketDataList: any[]){
    this.currentPage = 1;
    this.totalPages = (ticketDataList.length % this.itemsPerPage) == 0 ? (ticketDataList.length / this.itemsPerPage): (Math.trunc(ticketDataList.length / this.itemsPerPage) + 1);
    const tempList = [];
    let endIndex = ((ticketDataList.length > numberOfItemsPerPage)?numberOfItemsPerPage:ticketDataList.length);
    for (let index = 0; index < endIndex; index++) {
      tempList.push(ticketDataList[index]);
    }
    this.ticketList = tempList;
  }
  LoadNextPage(){
    const tempList = [];
    if(this.currentPage < this.totalPages){
      this.previousPage = this.currentPage;
      this.currentPage += 1;
      let startIndex = (this.previousPage * this.itemsPerPage);
      let endIndex = startIndex + ((this.ticketList.length > this.itemsPerPage)?this.itemsPerPage:this.ticketList.length);
      endIndex = (endIndex > this.ticketList.length) ? this.ticketList.length : endIndex;
      for (let index = startIndex; index < endIndex; index++) {
        tempList.push(this.ticketList[index]);
      }
      this.ticketList = tempList;
    }
  }
  LoadLastPage(){
    const tempList = [];
    if(this.currentPage < this.totalPages){
      this.previousPage = this.totalPages - 1;
      this.currentPage = this.totalPages;
      let startIndex = (this.previousPage * this.itemsPerPage);
      let endIndex = startIndex + ((this.ticketList.length > this.itemsPerPage)?this.itemsPerPage:this.ticketList.length);
      endIndex = (endIndex > this.ticketList.length) ? this.ticketList.length : endIndex;
      for (let index = startIndex; index < endIndex; index++) {
        tempList.push(this.ticketList[index]);
      }
      this.ticketList = tempList;
    }
  }
  LoadPreviousPage(){
    const tempList = [];
    if(this.currentPage > 1){
      this.previousPage = this.currentPage - 1;
      this.currentPage  = this.currentPage - 1;
      let startIndex = ((this.currentPage - 1) * this.itemsPerPage);
      let endIndex = startIndex + ((this.ticketList.length > this.itemsPerPage)?this.itemsPerPage:this.ticketList.length);
      endIndex = (endIndex > this.ticketList.length) ? this.ticketList.length : endIndex;
      for (let index = startIndex; index < endIndex; index++) {
        tempList.push(this.ticketList[index]);
      }
      this.ticketList = tempList;
    }
  }
  LoadFirstPage(){
    let tempList = [];
    if(this.currentPage > 1){
      this.previousPage = 0;
      this.currentPage = 1;
      let endIndex = ((this.ticketList.length > this.itemsPerPage)?this.itemsPerPage:this.ticketList.length);
      for (let index = 0; index < endIndex; index++) {
        tempList.push(this.ticketList[index]);
      }
      this.ticketList = tempList;
    }
  }
  SearchTicketValue(){
    let masterLists = {
      applicationList: this.applicationList,
      ticketTypeList: this.ticketTypeList,
      priorityList: this.priorityList,
      statusList: this.statusList,
      severityList: this.severityList,
      causeList: this.causeList,
      resolutionList: this.resolutionList,
      debtList: this.debtList,
      avoidableFlgList: this.avoidableFlgList,
      residualFlgList: this.residualFlgList,
      partialList: this.partialList,
      towerList: this.towerList
    };
    let filter = new ErrorTicketFilterPipe();
    let tempErrorTicketList: ErrorTicketModel[] = this.ticketList;
    const tempList: ErrorTicketModel[] = [];
    if(this.ticketList.length == 0){
      tempErrorTicketList = filter.transform(this.ticketListMasterList,this.searchData,masterLists);
    }
    else{
      tempErrorTicketList = filter.transform(this.ticketList,this.searchData,masterLists);
    }
    this.ticketList = tempErrorTicketList;
    if(this.ticketList.length > 0){
      this.currentPage = 1;
      this.previousPage = 0;
      let endIndex = ((this.ticketList.length > this.itemsPerPage)?this.itemsPerPage:this.ticketList.length);
    for (let index = 0; index < endIndex; index++) {
      tempList.push(this.ticketList[index]);
    }
    this.ticketList = tempList;
    }
  }
  validateRow(ticket: ErrorTicketModel){
  if(this.isAppMode){
    return(ticket.ticketTypeId > 0 &&
   ticket.priorityId > 0 &&
   ticket.statusId > 0 &&
   (ticket.openDate != null && ticket.openDate != "") &&
   ticket.applicationId > 0 &&
   (this.isCauseCodeVisible == true && ticket.causeCodeId > 0) && 
   (this.isResolutionCodeVisible == true && ticket.resolutionId >0 ) &&
   (this.isDebtClassificationVisible == true && ticket.debtClassificationId >0) && 
   (this.isAvoidableFlagVisible == true && ticket.avoidableFlagId > 0) &&
   (this.isResidualtDebtVisible  == true && ticket.residualDebtId > 0))
   
  }
  else{
    return(ticket.ticketTypeId > 0 && 
   ticket.priorityId >0 &&
   ticket.statusId >0 &&
   (ticket.openDate != null && ticket.openDate != "") &&
   ticket.assignmentGroupId > 0 &&
   (this.isCauseCodeVisible == true && ticket.causeCodeId > 0) && 
   (this.isResolutionCodeVisible == true && ticket.resolutionId >0 ) &&
   (this.isDebtClassificationVisible == true && ticket.debtClassificationId >0) && 
   (this.isAvoidableFlagVisible == true && ticket.avoidableFlagId > 0) &&
   (this.isResidualtDebtVisible  == true && ticket.residualDebtId > 0) && 
   ticket.towerId > 0 &&
   ticket.ticketDescription != "" && ticket.ticketDescription != null)
  }
  }
  onGridDropChange(event,field,ticket: ErrorTicketModel){
    const isValidRow = this.validateRow(ticket);
    if(!this.rowIdArray.includes(ticket.id) && isValidRow){
      this.rowIdArray.push(ticket.id);
    }
    switch (field) {
        case 'Application':
        this.ticketListMasterList.find(f=>f.id == ticket.id).application = this.applicationList.find(w=>w.value == event.target.value).label;
        this.ticketList.find(f=>f.id == ticket.id).application = this.applicationList.find(w=>w.value == event.target.value).label;
        break;
        case 'TicketType':
        this.ticketListMasterList.find(f=>f.id == ticket.id).ticketType = this.ticketTypeList.find(w=>w.value == event.target.value).label;
        this.ticketList.find(f=>f.id == ticket.id).ticketType = this.ticketTypeList.find(w=>w.value == event.target.value).label;
        break;
        case 'Priority':
          this.ticketListMasterList.find(f=>f.id == ticket.id).priority = this.priorityList.find(w=>w.value == event.target.value).label;
        this.ticketList.find(f=>f.id == ticket.id).priority = this.priorityList.find(w=>w.value == event.target.value).label;
        break;
        case 'Status':
          this.ticketListMasterList.find(f=>f.id == ticket.id).priority = this.statusList.find(w=>w.value == event.target.value).label;
        this.ticketList.find(f=>f.id == ticket.id).priority = this.statusList.find(w=>w.value == event.target.value).label;
        break;
        case 'Severity':
          this.ticketListMasterList.find(f=>f.id == ticket.id).severity = this.severityList.find(w=>w.value == event.target.value).label;
        this.ticketList.find(f=>f.id == ticket.id).severity = this.severityList.find(w=>w.value == event.target.value).label;
        break;
        case 'CauseCode':
          this.ticketListMasterList.find(f=>f.id == ticket.id).causeCode = this.causeList.find(w=>w.value == event.target.value).label;
        this.ticketList.find(f=>f.id == ticket.id).causeCode = this.causeList.find(w=>w.value == event.target.value).label;
        break;
        case 'ResolutionCode':
          this.ticketListMasterList.find(f=>f.id == ticket.id).resolutionCode = this.resolutionList.find(w=>w.value == event.target.value).label;
        this.ticketList.find(f=>f.id == ticket.id).resolutionCode = this.resolutionList.find(w=>w.value == event.target.value).label;
        break;
        case 'DebtCategory':
          this.ticketListMasterList.find(f=>f.id == ticket.id).debtClassification = this.debtClassificationList.find(w=>w.value == event.target.value).label;
        this.ticketList.find(f=>f.id == ticket.id).debtClassification = this.debtClassificationList.find(w=>w.value == event.target.value).label;
        break;
        case 'AvoidableFlag':
          this.ticketListMasterList.find(f=>f.id == ticket.id).avoidableFlag = this.avoidableFlgList.find(w=>w.value == event.target.value).label;
        this.ticketList.find(f=>f.id == ticket.id).debtClassification = this.avoidableFlgList.find(w=>w.value == event.target.value).label;
        break;
        case 'ResidualDebt':
        this.ticketListMasterList.find(f=>f.id == ticket.id).residualDebt = this.residualFlgList.find(w=>w.value == event.target.value).label;
        this.ticketList.find(f=>f.id == ticket.id).residualDebt = this.residualFlgList.find(w=>w.value == event.target.value).label;
        break;
        case 'Tower':
        this.ticketListMasterList.find(f=>f.id == ticket.id).tower = this.towerList.find(w=>w.value == event.target.value).label;
        this.ticketList.find(f=>f.id == ticket.id).tower = this.towerList.find(w=>w.value == event.target.value).label;
        break;
        case 'Assignment':
        this.ticketListMasterList.find(f=>f.id == ticket.id).assignmentGroup = this.assignmentGroupList.find(w=>w.value == event.target.value).label;
        this.ticketList.find(f=>f.id == ticket.id).assignmentGroup = this.assignmentGroupList.find(w=>w.value == event.target.value).label;
        break;
    
      default:
        break;
    }

  }
  ClearFilterData(){   
    this.errorTicketsGrid.clear();
    this.searchApplicationName= null;
      this.searchAvoidable= null;
      this.searchCauseCode= null;
      this.searchDebtCategory= null;
      this.searchIsPartialAutomated= null;
      this.searchOpenDate= null;
      this.searchPriority= null;
      this.searchResidualDebt= null;
      this.searchResolutionCode= null;
      this.searchSeverity= null;
      this.searchStatus= null;
      this.searchTicketDescription= null;
      this.searchTicketId= null;
      this.searchTicketType= null;
      this.searchAssignment= null;
      this.searchTower= null;
  }
  SaveErrorData(isClose: boolean){
    let supportTypeId;
    supportTypeId = this.prjSupportType;
    if (supportTypeId == 3) {
        if ((<HTMLInputElement>(document.getElementById("appId"))).checked == true) {
          supportTypeId = 1;
        }
        else if ((<HTMLInputElement>(document.getElementById("infraId"))).checked == true) {
          supportTypeId = 2;
        }
        else {
          supportTypeId = 1;
        }
      }
      if(this.rowIdArray.length > 0){
      this.rowIdArray.forEach((f: number)=>{
        let data = this.ticketListMasterList.find(q=>q.id == f);
      if(data != undefined){
        this.errorList.push({
          application: data.application,
          applicationId: data.applicationId,
          assignee: data.assignee,
          assignmentGroup: data.assignmentGroup,
          assignmentGroupId: data.assignmentGroupId,
          avoidableFlag: data.avoidableFlag,
          avoidableFlagId: data.avoidableFlagId,
          causeCode: data.causeCode,
          causeCodeId: data.causeCodeId,
          debtClassification: data.debtClassification,
          debtClassificationId: data.debtClassificationId,
          employeeId: data.employeeId,
          employeeName: data.employeeName,
          errorLogTicketDetails: data.errorLogTicketDetails,
          externalLoginId: data.externalLoginId,
          isAHTicket: data.isAHTicket,
          isDeleted: data.isDeleted,
          isManual: data.isManual,
          isPartiallyAutomated: data.isPartiallyAutomated,
          isTicketDescriptionModified: data.isTicketDescriptionModified,
          mTicketDescription: data.mTicketDescription,
          openDate: data.openDate,
          openDate2: data.openDate2,
          priority: data.priority,
          priorityId: data.priorityId,
          projectId: data.projectId,
          residualDebt: data.residualDebt,
          residualDebtId: data.residualDebtId,
          resolutionCode: data.resolutionCode,
          resolutionId: data.resolutionId,
          severity: data.severity,
          severityId: data.severityId,
          status: data.status,
          statusId: data.statusId,
          ticketDescription: data.ticketDescription,
          ticketId: data.ticketId,
          ticketType: data.ticketType,
          ticketTypeId: data.ticketTypeId,
          tower: data.tower,
          towerId: data.towerId
        });
      }
      })
      const ticketDataString = JSON.stringify(this.errorList);
        const saveData: ErroredTicketDetails = {
          CustomerId: parseInt(this.hdnCustomerId),
          ProjectId: this.projectId, 
          EmployeeId: this.hdnEmployeeId, 
          SupportTypeId: parseInt(supportTypeId),
          TicketDetails: ticketDataString
        };
        this.spinner.show();
        this.erroredTicketsService.saveErrorLogTicketData(saveData).subscribe(result => {
          if(result == 1 && isClose == false) {
            this.rowIdArray=[];
            this.loadErrorLogGrid(supportTypeId);
            this.spinner.hide();
          }
          else if(result == 1 && isClose == true) {
            this.rowIdArray=[];
            this.loadErrorLogGrid(supportTypeId);
            this.spinner.hide();
          }
          else if(result == -2){
            this.rowIdArray=[];
            this.loadErrorLogGrid(supportTypeId);
            this.spinner.hide();
            this.isErrorMsg = true;
            this.isTranslationFailure = true;
          }
          else if(result == -3){
            this.rowIdArray=[];
            this.loadErrorLogGrid(supportTypeId);
            this.spinner.hide();
            this.isErrorMsg = true;
            this.isEncryptionFailure = true;
            isClose = false;
          }
          this.closeValue.emit(isClose);
        });
      }
  }
  onDateChange(dateValue : any, rowID: any) {
    let ProjectID;  
    let ProjectData = [];

    ProjectID = this.erroredTicketsFG.get('projectId').value;
    ProjectData = this.projectList.filter(x => x.value == ProjectID);
    if (ProjectData[0].userTimeZoneName == "" || ProjectData[0].userTimeZoneName == undefined) {
      ProjectData[0].userTimeZoneName = ProjectData[0].projectTimeZoneName;
    }

    let param={
      UserTimeZone: ProjectData[0].userTimeZoneName
    }
    this.analystselfserviceService.getCurrentTimeofTimeZones(param).subscribe(x => {
      let tempDateTime = this.datePipe.transform(new Date(x), Constants.dateTimeZoneFormat);
      let currentTime = new Date(tempDateTime);
      let hours = currentTime.getHours()
      let minutes = currentTime.getMinutes()
      let sec = currentTime.getSeconds();
      if (minutes < 10)
        minutes = 0 + minutes;

        let suffix = "AM";
      if (hours >= 12) {
        suffix = "PM";
        hours = hours - 12;
      }
      if (hours == 0) {
        hours = 12;
      }
      if (sec < 10) {
        sec = 0 + sec;
      }
      let current_time = hours + ":" + minutes + ":" + sec + " " + suffix;
      let Datewithtime = this.datePipe.transform(new Date(dateValue), Constants.dateFormat) + " " + current_time;     
      this.ticketList[rowID-1].openDate = Datewithtime;
      if(!this.rowIdArray.includes(this.ticketList[rowID-1].id)){
        this.rowIdArray.push(this.ticketList[rowID-1].id);
      }
    }); 
  }
  clearErrorLogFilter(modelName: string, fieldName: string, searchOptions: string): void {
    this[modelName] = null;
    this.errorTicketsGrid.filter(null, fieldName, searchOptions);
    this.ClearFilterData();
  }
  toggleErrorLog(event): void {
    if (this.toggleErrorFilter) {
      this.toggleErrorFilter = false;
      this.toggleErroricon = Constants.toggleiconOff;
    } else {
      this.toggleErrorFilter = true;
      this.toggleErroricon = Constants.toggleIconOn;
    }
  }
  getTitle(values: any, id: any): string {
    let data = values.find(x => x.value == id);
    if (values && id && data != undefined) {
      return data.label;
    }
    return '';
  }
}
