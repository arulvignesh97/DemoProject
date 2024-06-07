// Copyright (c) Applens. All Rights Reserved.
import { ChangeDetectorRef, Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';

import { SpinnerService } from '../common/services/spinner.service';
import { MasterDataModel } from '../Layout/models/header.models';
import { HeaderService } from '../Layout/services/header.service';
import { SearchTicketService } from './Service/SearchTicket.service';
import { Constants } from '../common/constants/constants';
import { FileUploadDownload } from '../common/constants/FileUploadDownload';

@Component({
  selector: 'app-searchticket',
  templateUrl: './searchticket.component.html',
  styleUrls: ['./searchticket.component.css']
})
export class SearchticketComponent implements OnInit {
  @ViewChild('dt', { static: false }) SearchTable;
  lstExemptedProject: any;
  exemptedMsg: string;
  NoRecordProject: any[];
  constructor( private fb: FormBuilder,
    private spinnerService: SpinnerService, private searchTicketService: SearchTicketService,
    private headerService: HeaderService, private cdref: ChangeDetectorRef) {
  }
  
  public SearchModalFG: FormGroup = new FormGroup({});
  public ProjectList = [];
  public MasterProjectList = [];
  public TicketDataList = [];
  public TicketSourceList = [];
  public DataEntryList = [];
  public FilterByList = [];
  public LstHiddenFields: MasterDataModel;
  public MasterData = [];
  public mappingCols = [];
  public TicketTypeList = [];
  public TicketStatusList = [];
  public HierarchyMasterList = [];
  public RowHiearchyList = [];
  public MasterHierarchyList = [];
  public SubBusinessClusterMapping = [];
  public SelectedScope: string;
  public IsApp: boolean = true;
  public showsearch = true;
  public IsDataPresent: boolean = false;
  public FromStartRange: number;
  public FromLastRange: number;
  public FromminDate: Date;
  public FrommaxDate: Date;
  public ToStartRange: number;
  public ToLastRange: number;
  public TominDate: Date;
  public TomaxDate: Date;
  public fromDate: string;
  public todayDate: string;
  public IsApporInfra: boolean = true;
  public SelectedProject = [];
  public iserrormsg: boolean = false;
  public MandatoryMsg: string = '';
  public NoRecordMsg: boolean = false;
  public clearSearch: boolean = true;
  public isCognizant: boolean;
  public newTicketType = [];
  public display:boolean = false;
  SelectedProjectTemp: any[];
  isDisplayExempted:boolean=false;

  ngOnInit() {
    this.MandatoryMsg = Constants.MandatoryMsg;
    this.SearchModalFG = this.fb.group({
      SelectedFilter: [""],
      SelectedFromDate: [""],
      SelectedToDate: [""],
    })
    this.headerService.masterDataEmitter.subscribe((masterData: MasterDataModel) => {
      if (masterData != null) {
        this.LstHiddenFields = masterData;
        this.isCognizant = this.LstHiddenFields.hiddenFields.isCognizant;
      }
    });
    this.SelectedScope = "1";

    this.PopulateHeaderColumn();
    this.TicketDataList = [
      { "label": "Tickets with Efforts", "value": 1 },
      { "label": "Tickets without Efforts", "value": 2 },
      { "label": "UnAssigned Tickets", "value": 3 }]

    this.DataEntryList = [
      { "label": "Yes", "value": 1 },
      { "label": "No", "value": 0 }]


    this.TicketSourceList = [
      { "label": "SM Ticket", "value": 0 },
      { "label": "Applens Ticket", "value": 1 }]

    this.FilterByList = [
      { "label": "Open Date", "value": 1 },
      { "label": "Closed Date", "value": 0 }]

    this.RowHiearchyList = [
      { "label": "Application", "formcontrolname": "SelectedApplication", "List": this.ProjectList, "isinfra": true },
      { "label": "Status", "formcontrolname": "SelectedStatus", "List": this.TicketStatusList, "isinfra": true },
      { "label": "TicketSource", "formcontrolname": "SelectedTicketSource", "List": this.TicketSourceList, "isinfra": true },
      { "label": "ApplensTicketType", "formcontrolname": "SelectedApplensTicket", "List": this.TicketTypeList, "isinfra": true },
      { "label": "TicketingData", "formcontrolname": "SelectedTicketData", "List": this.TicketDataList, "isinfra": true },
      { "label": "DataEntryComplete", "formcontrolname": "SelectedDataEntry", "List": this.DataEntryList, "isinfra": true },
    ]
    this.GetHierarchyList(true);
  }
  CreateFormGroup(ConstrolsList) {
    this.cdref.detectChanges();
    this.SearchModalFG = new FormGroup({});
    this.SearchModalFG = this.fb.group({
      SelectedFilter: [""],
      SelectedFromDate: [""],
      SelectedToDate: [""],
      SelectedHierarchy0: [""],
      SelectedHierarchy1: [""],
      SelectedHierarchy2: [""],
      SelectedHierarchy3: [""],
      SelectedHierarchy4: [""],
      SelectedHierarchy5: [""],
      SelectedApplication: [""],
      SelectedStatus: [""],
      SelectedTicketSource: [""],
      SelectedApplensTicket: [""],
      SelectedTicketData: [""],
      SelectedDataEntry: [""]

    })
    this.spinnerService.hide();
  }
  showFilterPopup() {
    this.spinnerService.show();
    // this.modalRef = this.modalService.show(template, {
    //   animated: true,
    //   backdrop: 'static',
    //   class: 'searchticket'
    //  });
    this.display = true;
    this.spinnerService.hide();

  }
  ClosePopUp(){
    this.spinnerService.show();
    this.display = false;
    this.spinnerService.hide();
  }
  GetSearchTickets(onload = false) {
    if (onload) {
      this.SearchModalFG.get("SelectedHierarchy0").setValue(null);
      this.SearchModalFG.get("SelectedHierarchy1").setValue(null);
      this.SearchModalFG.get("SelectedHierarchy2").setValue(null);
      this.SearchModalFG.get("SelectedHierarchy3").setValue(null);
      this.SearchModalFG.get("SelectedHierarchy4").setValue(null);
      this.SearchModalFG.get("SelectedHierarchy5").setValue(null);
      this.SearchModalFG.get("SelectedApplication").setValue(null);
      this.SearchModalFG.get("SelectedStatus").setValue(null);
      this.SearchModalFG.get("SelectedTicketSource").setValue(null);
      this.SearchModalFG.get("SelectedApplensTicket").setValue(null);
      this.SearchModalFG.get("SelectedTicketData").setValue(null);
      this.SearchModalFG.get("SelectedDataEntry").setValue(null);
    }

    this.spinnerService.show();
    this.lstExemptedProject=[];
    this.NoRecordProject=[];
    this.SelectedProjectTemp=this.SelectedProject.slice();
    this.SelectedProjectTemp.forEach((item) => {
      var exemptedProject=this.LstHiddenFields.hiddenFields.lstProjectUserID.filter(x=>x.projectId==item && x.isExempted);
      if(exemptedProject.length>0){
        this.lstExemptedProject.push(exemptedProject[0].projectName);
        this.SelectedProject.splice(this.SelectedProject.indexOf(item),1);
      }
      else{
        this.NoRecordProject.push(this.ProjectList.filter(x=>x.value==item)[0].label);
      }
    });
    if(this.SelectedProject.length>0){
    var SearchTicketParameters = {
      ProjectIDs: this.SelectedProject.toString(),
      StartDate: this.getStartDate(),
      EndDate: this.getEndDate(),
      IsFilterByOpenDate: this.SearchModalFG.get("SelectedFilter").value,
      Hierarchy1ID: this.SearchModalFG.get("SelectedHierarchy0").value === null ? "" : this.SearchModalFG.get("SelectedHierarchy0").value.toString(),
      Hierarchy2ID: this.SearchModalFG.get("SelectedHierarchy1").value === null ? "" : this.SearchModalFG.get("SelectedHierarchy1").value.toString(),
      Hierarchy3ID: this.SearchModalFG.get("SelectedHierarchy2").value === null ? "" : this.SearchModalFG.get("SelectedHierarchy2").value.toString(),
      Hierarchy4ID: this.SearchModalFG.get("SelectedHierarchy3").value === null ? "" : this.SearchModalFG.get("SelectedHierarchy3").value.toString(),
      Hierarchy5ID: this.SearchModalFG.get("SelectedHierarchy4").value === null ? "" : this.SearchModalFG.get("SelectedHierarchy4").value.toString(),
      Hierarchy6ID: this.SearchModalFG.get("SelectedHierarchy5").value === null ? "" : this.SearchModalFG.get("SelectedHierarchy5").value.toString(),
      ApplicationID: this.SearchModalFG.get("SelectedApplication").value === null ? "" : this.SearchModalFG.get("SelectedApplication").value.toString(),
      TicketStatusID: this.SearchModalFG.get("SelectedStatus").value === null ? "" : this.SearchModalFG.get("SelectedStatus").value.toString(),
      TicketSourceID: this.SearchModalFG.get("SelectedTicketSource").value === null ? "" : this.SearchModalFG.get("SelectedTicketSource").value.toString(),
      TicketTypeID: this.SearchModalFG.get("SelectedApplensTicket").value === null ? "" : this.SearchModalFG.get("SelectedApplensTicket").value.toString(),
      TicketingData: this.SearchModalFG.get("SelectedTicketData").value === null ? "" : this.SearchModalFG.get("SelectedTicketData").value.toString(),
      DataEntryCompletion: this.SearchModalFG.get("SelectedDataEntry").value === null ? "" : this.SearchModalFG.get("SelectedDataEntry").value.toString(),
      IsCognizant: this.LstHiddenFields.hiddenFields.isCognizant,
      CustomerID: this.LstHiddenFields.hiddenFields.customerId,
      InfraEnabled: this.IsApp,
      HeiraricyNum: this.SubBusinessClusterMapping.filter(x => x.isInfra === !this.IsApp).length,
      CustomerName: this.LstHiddenFields.selectedCustomer.customerName
    };
    
    this.searchTicketService.GetSearchTickets(SearchTicketParameters).subscribe((response: any) => {

      if (response.length > 0)
      {    
      this.NoRecordMsg=false;
      this.isDisplayExempted=false;
      if(this.lstExemptedProject.length>0){
        this.getExemptedMsg();
      }
      this.MasterData = [];
      this.PopuateMasterData(response);
      this.IsDataPresent = true;
      if (this.IsApp) {
        this.ShowHideColumn("applicationName", true);
        this.ShowHideColumn("towerName", false);
        this.ShowHideColumn("service", true);
        this.ShowHideColumn("serviceGroup", true);
      } else {
        this.ShowHideColumn("applicationName", false);
        this.ShowHideColumn("towerName", true);
        this.ShowHideColumn("service", false);
        this.ShowHideColumn("serviceGroup", false);
      }

      if (this.LstHiddenFields.hiddenFields.isCognizant == 1) {
        //CCAP Fix
      }
      else {
        this.ShowHideColumn("projectName", true);
        this.ShowHideColumn("service", true);
        this.ShowHideColumn("serviceGroup", true);
      }

      var HeiraricyNum = this.SubBusinessClusterMapping.filter(x => x.isInfra === !this.IsApp).length;
      response.filter(x=>x.hierarchy4 !='').length == response.length ? this.ShowHideColumn("hierarchy4", false) : this.ShowHideColumn("hierarchy4", true);
      response.filter(x=>x.hierarchy5 !='').length == response.length ? this.ShowHideColumn("hierarchy5", false) : this.ShowHideColumn("hierarchy5", true);
      response.filter(x=>x.hierarchy6 !='').length == response.length ? this.ShowHideColumn("hierarchy6", false) : this.ShowHideColumn("hierarchy6", true);
      this.spinnerService.hide();
    }
    else{
      this.NoRecordMsg=true;
      this.isDisplayExempted=false;
      if(this.lstExemptedProject.length>0){
        this.getExemptedMsg();
      }
      this.spinnerService.hide();
    }
    });
  }
  else{
    this.NoRecordMsg=false;
    this.spinnerService.hide();
    this.getExemptedMsg();
  }

  }

  getExemptedMsg(){
    this.isDisplayExempted=true;
    this.exemptedMsg="Tickets belonging to <b>" + this.lstExemptedProject.join(', ')+ "</b> are not showcased since it is exempted from using AppLens";
    this.SelectedProject=this.SelectedProjectTemp;
  }

  getStartDate() {
    let sd = this.SearchModalFG.get("SelectedFromDate").value;
    let startDate = `${sd.getMonth()+1}/${sd.getDate()}/${sd.getFullYear()}`;
    return startDate;
  }
  getEndDate() {
    let ed = this.SearchModalFG.get("SelectedToDate").value;
    let endDate = `${ed.getMonth()+1}/${ed.getDate()}/${ed.getFullYear()}`;
    return endDate;
  }
  PopuateMasterData(SearchData) {
    for (let i = 0; i < SearchData.length; i++) {

      var itemJson = {};
      itemJson["applicationName"] = SearchData[i].applicationName;
      itemJson["towerName"] = SearchData[i].towerName;
      itemJson["projectName"] = SearchData[i].projectName;
      itemJson["hierarchy1"] = SearchData[i].hierarchy1;
      itemJson["hierarchy2"] = SearchData[i].hierarchy2;
      itemJson["hierarchy3"] = SearchData[i].hierarchy3;
      itemJson["hierarchy4"] = SearchData[i].hierarchy4;
      itemJson["hierarchy5"] = SearchData[i].hierarchy5;
      itemJson["hierarchy6"] = SearchData[i].hierarchy6;
      itemJson["ticketID"] = SearchData[i].ticketId;
      itemJson["ticketDescription"] = SearchData[i].ticketDescription;
      itemJson["service"] = SearchData[i].service;
      itemJson["serviceGroup"] = SearchData[i].serviceGroup;
      itemJson["ticketType"] = SearchData[i].ticketType;
      itemJson["priority"] = SearchData[i].priority;
      itemJson["status"] = SearchData[i].status;
      itemJson["appLensStatus"] = SearchData[i].appLensStatus;
      itemJson["causeCode"] = SearchData[i].causeCode;
      itemJson["resolutionCode"] = SearchData[i].resolutionCode;
      itemJson["debtClassification"] = SearchData[i].debtClassification;
      itemJson["avoidableFlag"] = SearchData[i].avoidableFlag;
      itemJson["residualDebt"] = SearchData[i].residualDebt;
      itemJson["effortTillDate"] = SearchData[i].effortTillDate;
      itemJson["insertionMode"] = SearchData[i].insertionMode;
      itemJson["openDate"] = SearchData[i].openDate;
      itemJson["closedDate"] = SearchData[i].closedDate;
      itemJson["assignee"] = SearchData[i].assignee;
      itemJson["clientUserID"] = SearchData[i].clientUserId;
      itemJson["reopenDate"] = SearchData[i].reopenDate;
      itemJson["source"] = SearchData[i].source;
      itemJson["severity"] = SearchData[i].severity;
      itemJson["dataEntryComplete"] = SearchData[i].dataEntryComplete;
      itemJson["releaseType"] = SearchData[i].releaseType;
      itemJson["plannedEffort"] = SearchData[i].plannedEffort;
      itemJson["estimatedWorkSize"] = SearchData[i].estimatedWorkSize;
      itemJson["actualWorkSize"] = SearchData[i].actualWorkSize;
      itemJson["plannedStartDate"] = SearchData[i].plannedStartDate;
      itemJson["plannedEndDate"] = SearchData[i].plannedEndDate;
      itemJson["rejectedTimeStamp"] = SearchData[i].rejectedTimeStamp;
      itemJson["kEDBAvailableIndicator"] = SearchData[i].kedbAvailableIndicator;
      itemJson["kEDBUpdated"] = SearchData[i].kedbUpdated;
      itemJson["elevateFlagInternal"] = SearchData[i].elevateFlagInternal;
      itemJson["rCAID"] = SearchData[i].rcaid;
      itemJson["metResponseSLA"] = SearchData[i].metResponseSLA;
      itemJson["metAcknowledgementSLA"] = SearchData[i].metAcknowledgementSLA;
      itemJson["metResolution"] = SearchData[i].metResolution;
      itemJson["actualStartDateTime"] = SearchData[i].actualStartDateTime;
      itemJson["actualEndDateTime"] = SearchData[i].actualEndDateTime;
      itemJson["actualDuration"] = SearchData[i].actualDuration;
      itemJson["natureOfTheTicket"] = SearchData[i].natureOfTheTicket;
      itemJson["comments"] = SearchData[i].comments;
      itemJson["repeatedIncident"] = SearchData[i].repeatedIncident;
      itemJson["relatedTickets"] = SearchData[i].relatedTickets;
      itemJson["kEDBPath"] = SearchData[i].kedbPath;
      itemJson["ticketCreatedBy"] = SearchData[i].ticketCreatedBy;
      itemJson["escalatedFlagCustomer"] = SearchData[i].escalatedFlagCustomer;
      itemJson["approvedBy"] = SearchData[i].approvedBy;
      itemJson["startedDateTime"] = SearchData[i].startedDateTime;
      itemJson["wIPDateTime"] = SearchData[i].wipDateTime;
      itemJson["onHoldDateTime"] = SearchData[i].onHoldDateTime;
      itemJson["completedDateTime"] = SearchData[i].completedDateTime;
      itemJson["cancelledDateTime"] = SearchData[i].cancelledDateTime;
      itemJson["outageDuration"] = SearchData[i].outageDuration;
      itemJson["resolutionRemarks"] = SearchData[i].resolutionRemarks;
      itemJson["flexField1"] = SearchData[i].flexField1;
      itemJson["flexField2"] = SearchData[i].flexField2;
      itemJson["flexField3"] = SearchData[i].flexField3;
      itemJson["flexField4"] = SearchData[i].flexField4;
      itemJson["category"] = SearchData[i].category;
      itemJson["type"] = SearchData[i].type;
      this.MasterData.push(itemJson);
      
    }
  }
  onDateFormat() {
    this.SearchModalFG.get('SelectedFromDate').setValue(null);
    this.SearchModalFG.get('SelectedToDate').setValue(null);
    let date_ob = new Date();
    let date = ("0" + date_ob.getDate()).slice(-2);
    let month = ("0" + (date_ob.getMonth() + 1)).slice(-2);
    let year = date_ob.getFullYear();
    //from date
    this.FromStartRange = year - 2;
    this.FromLastRange = year;
    var fromdate = this.FromStartRange + "/" + month + "/" + date;
    let min = new Date(fromdate)
    this.FromminDate = min;
    let max = new Date()
    this.FrommaxDate = max;
    var selecteddate = year + "/" + month + "/" + 1;
    let selectedmindate = new Date(selecteddate);
    this.SearchModalFG.get('SelectedFromDate').setValue(selectedmindate);
    //to Date
    this.ToStartRange = selectedmindate.getFullYear();
    this.ToLastRange = year;
    var fromdate = selectedmindate.getFullYear() + "/" + ("0" + (selectedmindate.getMonth() + 1)).slice(-2) + "/" + ("0" + (selectedmindate.getDate() + 1)).slice(-2);
    this.TominDate = new Date(fromdate);
    this.TomaxDate = new Date();
    let selectedmaxdate = new Date()
    this.SearchModalFG.get('SelectedToDate').setValue(selectedmaxdate);

  }
  OnFromDateChange(event) {
    let date_ob = new Date();
    let date = ("0" + date_ob.getDate()).slice(-2);
    let month = ("0" + (date_ob.getMonth() + 1)).slice(-2);
    let year = date_ob.getFullYear();

    this.ToStartRange = event.getFullYear();
    this.ToLastRange = year;
    var fromdate = event.getFullYear() + "/" + ("0" + (event.getMonth() + 1)).slice(-2) + "/" + ("0" + (event.getDate() + 1)).slice(-2);
    this.TominDate = new Date(fromdate);
    this.TomaxDate = new Date();

  }
  GetHierarchyList(onload=false) {
    this.spinnerService.show();
    const param = {
      customerID: this.LstHiddenFields.hiddenFields.customerId,
      associateID: this.LstHiddenFields.hiddenFields.employeeId
    }

    this.searchTicketService
      .GetHierarchyList(param)
      .subscribe((response: any) => {
        this.MasterHierarchyList = [];
        this.MasterProjectList = response.projects;
        this.SubBusinessClusterMapping = response.businessCluster;
        if (this.MasterProjectList.length > 0) {


        if ((this.MasterProjectList.filter(x => x.supportTypeId === 3).length > 0) ||(!this.isCognizant &&
          this.MasterProjectList.filter(x => x.supportTypeId === 1).length> 0 
          && this.MasterProjectList.filter(x => x.supportTypeId === 2).length> 0))
         {
            this.IsApporInfra = true;
            this.OnRadioChange(1,onload);
          }
          else if (this.MasterProjectList.filter(x => x.supportTypeId === 1 || x.supportTypeId === 4).length > 0 &&
            this.MasterProjectList.filter(x => x.supportTypeId === 2).length === 0) {
            this.IsApporInfra = false;
            this.OnRadioChange(1,onload);
          }
          else {
            if (this.MasterProjectList.filter(x => x.supportTypeId === 2).length > 0 &&
              this.MasterProjectList.filter(x => x.supportTypeId === 1 || x.supportTypeId === 4).length === 0) {
              this.IsApporInfra = false;
              this.OnRadioChange(2,onload);
            }
            else{
              this.IsApporInfra = true;
              this.OnRadioChange(1,onload);
            }
          }
        }
        else {
          this.OnRadioChange(1,onload);
        }
      });


  }
  OnProjectChange() {
    this.SearchModalFG.reset();
    this.SearchModalFG.get('SelectedFilter').setValue(1);
    this.onDateFormat();
    if (this.SelectedProject.length > 0) {
      this.GetTicketTypes(this.SelectedProject.toString());
      this.GetTicketStatus(this.SelectedProject.toString());
    }
  }
  OnRadioChange(isInfra,onload=false) {
    this.SelectedProject=[];
    if (this.MasterProjectList.length > 0) {
      if (isInfra == 1 && this.IsApporInfra == true) {
        var projectlist = this.MasterProjectList.filter(x => x.supportTypeId == 3 || x.supportTypeId == 1 || x.supportTypeId === 4);
        this.ProjectList = this.getDropDownList(projectlist, 'projectId', 'projectName');
      }
      else if (isInfra == 2 && this.IsApporInfra == true) {
        var projectlist = this.MasterProjectList.filter(x => x.supportTypeId == 3 || x.supportTypeId == 2);
        this.ProjectList = this.getDropDownList(projectlist, 'projectId', 'projectName');
      }
      else if (isInfra == 1 && this.IsApporInfra == false) {
        var projectlist = this.MasterProjectList.filter(x => x.supportTypeId == 1 || x.supportTypeId === 4);
        this.ProjectList = this.getDropDownList(projectlist, 'projectId', 'projectName');
      }
      else {
        if (isInfra == 2 && this.IsApporInfra == false) {
          var projectlist = this.MasterProjectList.filter(x => x.supportTypeId == 2);
          this.ProjectList = this.getDropDownList(projectlist, 'projectId', 'projectName');
        }
      }
      if (this.ProjectList.length === 1) {
        this.SelectedProject = [this.ProjectList[0].value];
        this.OnProjectChange();
      }

      this.IsApp = isInfra == 1 ? false : true;
      this.SearchModalFG.reset();
      this.MasterHierarchyList = [];
      var SubClusterList = [];
      this.cdref.detectChanges();
      var AppHierarchyList = this.SubBusinessClusterMapping.filter(x => x.isInfra === (isInfra === 1) ? false : true);
      var finalList = [];
      for (var i = 0; i < AppHierarchyList.length; i++) {
        SubClusterList = [
          {
            "label": AppHierarchyList[i].businessClusterName,
            "formcontrolname": "SelectedHierarchy" + i,
            "List": i == 0 ? this.getDropDownList(AppHierarchyList[i].subBusinessClusterList, 'businessClusterMapId', 'businessClusterBaseName') : [],
            "isinfra": true
          },
        ]
        finalList.push(SubClusterList[0]);

      }
      this.RowHiearchyList[0].label = (isInfra === 1) ? "Application" : "Tower";
      for (var i = 0; i < this.RowHiearchyList.length; i++) {
        finalList.push(this.RowHiearchyList[i]);
      }
    }
    else {
      this.IsApporInfra = false;
      var AppLength = this.SubBusinessClusterMapping.filter(x => x.isInfra === false);
      if (AppLength.length > 0) {
        var AppHierarchyList = this.SubBusinessClusterMapping.filter(x => x.isInfra === false);
      }
      else {
        var AppHierarchyList = this.SubBusinessClusterMapping.filter(x => x.isInfra === true);
      }
      var finalList = [];
      for (var i = 0; i < AppHierarchyList.length; i++) {
        SubClusterList = [
          {
            "label": AppHierarchyList[i].businessClusterName,
            "formcontrolname": "SelectedHierarchy" + i,
            "List": i == 0 ? this.getDropDownList(AppHierarchyList[i].subBusinessClusterList, 'businessClusterMapId', 'businessClusterBaseName') : [],
            "isinfra": true
          },
        ]
        finalList.push(SubClusterList[0]);

      }
      this.RowHiearchyList[0].label = (AppLength.length > 0) ? "Application" : "Tower";
      for (var i = 0; i < this.RowHiearchyList.length; i++) {
        finalList.push(this.RowHiearchyList[i]);
      }
    }
    this.MasterHierarchyList = finalList;
    this.CreateFormGroup(this.MasterHierarchyList);
    this.SearchModalFG.get('SelectedFilter').setValue(1);
    this.onDateFormat();
    if (this.ProjectList.length === 1 && onload) {
      this.GetSearchTickets(true);
    }
  }
  GetTicketTypes(ProjectIDs) {
    const param = {
      ProjectId: ProjectIDs,
      IsCognizant: this.LstHiddenFields.hiddenFields.isCognizant
    }
    this.searchTicketService
      .GetTicketTypes(param)
      .subscribe((response: any) => {
        this.TicketTypeList = [];
       
        var index = this.MasterHierarchyList.findIndex(x => x.label === 'ApplensTicketType');
        if (this.IsApp == false && this.IsApporInfra == true) {
          var tickettypelist = response.filter(x => x.supportTypeId == 3 || x.supportTypeId == 1 || x.supportTypeId == 0);
          this.TicketTypeList = this.getDropDownList(tickettypelist, 'ticketTypeId', 'ticketTypeName');
        }
        else if (this.IsApp == true && this.IsApporInfra == true) {
          var tickettypelist = response.filter(x => x.supportTypeId == 3 || x.supportTypeId == 2 || x.supportTypeId == 0);
          this.TicketTypeList = this.getDropDownList(tickettypelist, 'ticketTypeId', 'ticketTypeName');
        }
        else if (this.IsApp == false && this.IsApporInfra == false) {
          var tickettypelist = response.filter(x => x.supportTypeId == 1 || x.supportTypeId == 0);
          this.TicketTypeList = this.getDropDownList(tickettypelist, 'ticketTypeId', 'ticketTypeName');
        }
        else {
          if (this.IsApp == true && this.IsApporInfra == false) {
            var tickettypelist = response.filter(x => x.supportTypeId == 2 || x.supportTypeId == 0);
            this.TicketTypeList = this.getDropDownList(tickettypelist, 'ticketTypeId', 'ticketTypeName');
          }
        }
        this.newTicketType = []
        this.TicketTypeList.forEach((item, index) => {
        if (this.newTicketType.findIndex(i => i.label   == item.label  ) === -1) 
        {
          this.newTicketType.push(item)
        }
        });
        this.MasterHierarchyList[index].List = this.newTicketType;
      });

  }
  GetTicketStatus(ProjectIDs) {
    const param = {
      ProjectId: ProjectIDs
    }
    this.searchTicketService
      .GetTicketStatus(param)
      .subscribe((response: any) => {
        this.TicketStatusList = [];
        var index = this.MasterHierarchyList.findIndex(x => x.label === 'Status')
        this.TicketStatusList = this.getDropDownList(response, 'statusId', 'statusName');
        this.MasterHierarchyList[index].List = this.TicketStatusList;
      });

  }
  LoadHierarchy(index, name) {
    var AppList = this.SubBusinessClusterMapping.filter(x => x.isInfra === (this.IsApp === false) ? false : true);

    var formcontrolnumber = parseInt(name.split(/([0-9]+)/)[1]);
    for (var x = formcontrolnumber + 1; x <= AppList.length; x++) {
      this.SearchModalFG.get(this.MasterHierarchyList[x].formcontrolname).reset();
    }   
    if (index < (AppList.length - 1)) {
      var FilteredAppList = AppList[index + 1].subBusinessClusterList;
      var SubHierarchyLength = this.SearchModalFG.get(name).value;
      var HierarchyListFinal = [];

      for (var i = 0; i < SubHierarchyLength.length; i++) {
        var SubHierarchyList = FilteredAppList.filter(x => x.parentBusinessClusterMapId ===
          SubHierarchyLength[i]
        )
        if (SubHierarchyList.length > 0) {
          for (var y = 0; y < SubHierarchyList.length; y++) {
            HierarchyListFinal.push(SubHierarchyList[y]);
          }
        }
      }
      this.MasterHierarchyList[index + 1].List = this.getDropDownList(HierarchyListFinal, 'businessClusterMapId', 'businessClusterBaseName')
    }
    else {
      if (this.SelectedProject.length > 0) {
        if ((AppList.length - 1) === index) {
          var FilteredAppList = AppList[index].subBusinessClusterList;
          var appList = FilteredAppList[0].applicationList;
          if (appList.length > 0) {
            var SubHierarchyLength = this.SearchModalFG.get(name).value;
            var AppListFinal = [];
            var AppFinalList = [];

            for (var i = 0; i < SubHierarchyLength.length; i++) {
              var SubAppList = appList.filter(x => x.parentBusinessClusterId ===
                SubHierarchyLength[i]
              )
              for (var z = 0; z < this.SelectedProject.length; z++) {
                var finallist = SubAppList.filter(x => x.projectId === this.SelectedProject[z])
                if (finallist.length > 0) {
                  for (var y = 0; y < finallist.length; y++) {
                    AppListFinal.push(finallist[y]);
                  }
                }
              }
            }
            this.MasterHierarchyList[index + 1].List = this.getDropDownList(AppListFinal, 'applicationId', 'applicationName')
          }
        }
      }
    }
  }
  getDropDownList(projectDropList, id, name): any {
    if (name !== "projectName" && name !== "ticketTypeName" && name !== "statusName" && name !== "applicationName") {
      projectDropList = this.RemoveDuplicates(projectDropList, '');
    }
    if (name === "applicationName") {
      projectDropList = this.RemoveDuplicates(projectDropList, 'applicationName');
    }
    if (name === "ticketTypeName") {
      projectDropList = this.RemoveDuplicates(projectDropList, 'ticketTypeName');
    }
    if (name === "statusName") {
      projectDropList = this.RemoveDuplicates(projectDropList, 'statusName');
    }
    var lstlength = projectDropList.length;
    var masterDropDown = [];
    for (var i = 0; i < lstlength; i++) {
      var labelList = projectDropList[i];
      var exempted=this.LstHiddenFields.hiddenFields.lstProjectUserID.filter(x=>x.projectName==labelList[name])
      var list = { "label": labelList[name], "value": labelList[id] 
      ,"isExempted":exempted.length>0?exempted[0].isExempted:false}
      masterDropDown.push(list);
    }
    return masterDropDown.sort((a, b) => {
      if (a.label > b.label) {
        return 1;
      } else if (a.label < b.label) {
        return -1;
      } else {
        return 0;
      }
    });
  }
  PopulateHeaderColumn() {
    this.mappingCols = [];
    this.mappingCols = [
      { field: 'applicationName', header: 'ApplicationName', hide: false, showfilter: true },
      { field: 'towerName', header: 'TowerName', hide: false, showfilter: true },
      { field: 'projectName', header: 'ProjectName', hide: false, showfilter: true },
      { field: 'hierarchy1', header: 'Hierarchy1', hide: false, showfilter: true },
      { field: 'hierarchy2', header: 'Hierarchy2', hide: false, showfilter: true },
      { field: 'hierarchy3', header: 'Hierarchy3', hide: false, showfilter: true },
      { field: 'hierarchy4', header: 'Hierarchy4', hide: false, showfilter: true },
      { field: 'hierarchy5', header: 'Hierarchy5', hide: false, showfilter: true },
      { field: 'hierarchy6', header: 'Hierarchy6', hide: false, showfilter: true },
      { field: 'ticketID', header: 'TicketID', hide: false, showfilter: true },
      { field: 'ticketDescription', header: 'TicketDescription', hide: false, showfilter: true },
      { field: 'service', header: 'Service', hide: false, showfilter: true },
      { field: 'serviceGroup', header: 'ServiceGroup', hide: false, showfilter: true },
      { field: 'ticketType', header: 'TicketType', hide: false, showfilter: true },
      { field: 'priority', header: 'Priority', hide: false, showfilter: true },
      { field: 'status', header: 'Status', hide: false, showfilter: true },
      { field: 'appLensStatus', header: 'AppLensStatus', hide: false, showfilter: true },
      { field: 'causeCode', header: 'CauseCode', hide: false, showfilter: true },
      { field: 'resolutionCode', header: 'ResolutionCode', hide: false, showfilter: true },
      { field: 'debtClassification', header: 'DebtClassification', hide: false, showfilter: true },
      { field: 'avoidableFlag', header: 'AvoidableFlag', hide: false, showfilter: true },
      { field: 'residualDebt', header: 'ResidualDebt', hide: false, showfilter: true },
      { field: 'effortTillDate', header: 'EffortTillDate', hide: false, showfilter: true },
      { field: 'insertionMode', header: 'InsertionMode', hide: false, showfilter: true },
      { field: 'openDate', header: 'OpenDate', hide: false, showfilter: true },
      { field: 'closedDate', header: 'ClosedDate', hide: false, showfilter: true },
      { field: 'assignee', header: 'Assignee', hide: false, showfilter: true },
      { field: 'clientUserID', header: 'External Login ID', hide: false, showfilter: true },
      { field: 'reopenDate', header: 'ReopenDate', hide: false, showfilter: true },
      { field: 'source', header: 'Source', hide: false, showfilter: true },
      { field: 'severity', header: 'Severity', hide: false, showfilter: true },
      { field: 'dataEntryComplete', header: 'DataEntryComplete', hide: false, showfilter: true },
      { field: 'releaseType', header: 'ReleaseType', hide: true, showfilter: true },
      { field: 'plannedEffort', header: 'PlannedEffort', hide: true, showfilter: true },
      { field: 'estimatedWorkSize', header: 'Estimated WorkSize', hide: true, showfilter: true },
      { field: 'actualWorkSize', header: 'Actual WorkSize', hide: true, showfilter: true },
      { field: 'plannedStartDate', header: 'PlannedStartDate', hide: false, showfilter: true },
      { field: 'plannedEndDate', header: 'PlannedEndDate', hide: true, showfilter: true },
      { field: 'rejectedTimeStamp', header: 'RejectedTimeStamp', hide: true, showfilter: true },
      { field: 'kEDBAvailableIndicator', header: 'KEDBAvailableIndicator', hide: true, showfilter: true },
      { field: 'kEDBUpdated', header: 'KEDBUpdated', hide: true, showfilter: true },
      { field: 'elevateFlagInternal', header: 'ElevateFlagInternal', hide: true, showfilter: true },
      { field: 'rCAID', header: 'RCAID', hide: true, showfilter: true },
      { field: 'metResponseSLA', header: 'MetResponseSLA', hide: true, showfilter: true },
      { field: 'metAcknowledgementSLA', header: 'MetAcknowledgementSLA', hide: true, showfilter: true },
      { field: 'metResolution', header: 'MetResolution', hide: true, showfilter: true },
      { field: 'actualStartDateTime', header: 'ActualStartDateTime', hide: true, showfilter: true },
      { field: 'actualEndDateTime', header: 'ActualEndDateTime', hide: true, showfilter: true },
      { field: 'actualDuration', header: 'ActualDuration', hide: true, showfilter: true },
      { field: 'natureOfTheTicket', header: 'NatureOfTheTicket', hide: true, showfilter: true },
      { field: 'comments', header: 'Comments', hide: true, showfilter: true },
      { field: 'repeatedIncident', header: 'RepeatedIncident', hide: true, showfilter: true },
      { field: 'relatedTickets', header: 'RelatedTickets', hide: true, showfilter: true },
      { field: 'kEDBPath', header: 'KEDBPath', hide: true, showfilter: true },
      { field: 'ticketCreatedBy', header: 'Ticket CreatedBy', hide: true, showfilter: true },
      { field: 'escalatedFlagCustomer', header: 'EscalatedFlagCustomer', hide: true, showfilter: true },
      { field: 'approvedBy', header: 'ApprovedBy', hide: true, showfilter: true },
      { field: 'startedDateTime', header: 'Started DateTime', hide: true, showfilter: true },
      { field: 'wIPDateTime', header: 'WIP DateTime', hide: true, showfilter: true },
      { field: 'onHoldDateTime', header: 'OnHold DateTime', hide: true, showfilter: true },
      { field: 'completedDateTime', header: 'Completed DateTime', hide: false, showfilter: true },
      { field: 'cancelledDateTime', header: 'Cancelled DateTime', hide: false, showfilter: true },
      { field: 'outageDuration', header: 'OutageDuration', hide: true, showfilter: true },
      { field: 'resolutionRemarks', header: 'ResolutionRemarks', hide: true, showfilter: true },
      { field: 'flexField1', header: 'FlexField1', hide: true, showfilter: true },
      { field: 'flexField2', header: 'FlexField2', hide: true, showfilter: true },
      { field: 'flexField3', header: 'FlexField3', hide: true, showfilter: true },
      { field: 'flexField4', header: 'FlexField4', hide: true, showfilter: true },
      { field: 'category', header: 'Category', hide: true, showfilter: true },
      { field: 'type', header: 'Type', hide: true, showfilter: true }
    ];
  }
  Resetpopup() {
    this.SearchModalFG.reset();
    this.SelectedProject = [];
    this.SearchModalFG.get('SelectedFilter').setValue(1);
    this.onDateFormat();
  }
  Search() {
    if (this.SelectedProject.length === 0) {
      this.iserrormsg = true;
      setTimeout(() => { this.iserrormsg = false; }, 5000);
    }
    else {
      this.iserrormsg = false;
      this.display = false;
      this.IsDataPresent = false;
      this.GetSearchTickets();
    }
  }
  exportToExcel() {
    this.spinnerService.show();
    this.SelectedProjectTemp=[];
    this.SelectedProjectTemp=this.SelectedProject.slice();
    this.SelectedProjectTemp.forEach((item) => {
      var exemptedProject=this.LstHiddenFields.hiddenFields.lstProjectUserID.filter(x=>x.projectId==item && x.isExempted);
      if(exemptedProject.length>0){
        this.lstExemptedProject.push(exemptedProject[0].projectName);
        this.SelectedProject.splice(this.SelectedProject.indexOf(item),1);
      }});
    var SearchTicketParameters = {
      ProjectIDs: this.SelectedProject.toString(),
      StartDate: this.getStartDate(),
      EndDate: this.getEndDate(),
      IsFilterByOpenDate: this.SearchModalFG.get("SelectedFilter").value,
      Hierarchy1ID: this.SearchModalFG.get("SelectedHierarchy0").value === null ? "" : this.SearchModalFG.get("SelectedHierarchy0").value.toString(),
      Hierarchy2ID: this.SearchModalFG.get("SelectedHierarchy1").value === null ? "" : this.SearchModalFG.get("SelectedHierarchy1").value.toString(),
      Hierarchy3ID: this.SearchModalFG.get("SelectedHierarchy2").value === null ? "" : this.SearchModalFG.get("SelectedHierarchy2").value.toString(),
      Hierarchy4ID: this.SearchModalFG.get("SelectedHierarchy3").value === null ? "" : this.SearchModalFG.get("SelectedHierarchy3").value.toString(),
      Hierarchy5ID: this.SearchModalFG.get("SelectedHierarchy4").value === null ? "" : this.SearchModalFG.get("SelectedHierarchy4").value.toString(),
      Hierarchy6ID: this.SearchModalFG.get("SelectedHierarchy5").value === null ? "" : this.SearchModalFG.get("SelectedHierarchy5").value.toString(),
      ApplicationID: this.SearchModalFG.get("SelectedApplication").value === null ? "" : this.SearchModalFG.get("SelectedApplication").value.toString(),
      TicketStatusID: this.SearchModalFG.get("SelectedStatus").value === null ? "" : this.SearchModalFG.get("SelectedStatus").value.toString(),
      TicketSourceID: this.SearchModalFG.get("SelectedTicketSource").value === null ? "" : this.SearchModalFG.get("SelectedTicketSource").value.toString(),
      TicketTypeID: this.SearchModalFG.get("SelectedApplensTicket").value === null ? "" : this.SearchModalFG.get("SelectedApplensTicket").value.toString(),
      TicketingData: this.SearchModalFG.get("SelectedTicketData").value === null ? "" : this.SearchModalFG.get("SelectedTicketData").value.toString(),
      DataEntryCompletion: this.SearchModalFG.get("SelectedDataEntry").value === null ? "" : this.SearchModalFG.get("SelectedDataEntry").value.toString(),
      IsCognizant: this.LstHiddenFields.hiddenFields.isCognizant,
      CustomerID: this.LstHiddenFields.hiddenFields.customerId,
      InfraEnabled: this.IsApp,
      HeiraricyNum: this.SubBusinessClusterMapping.filter(x => x.isInfra === !this.IsApp).length,
      CustomerName: this.LstHiddenFields.selectedCustomer.customerName
    };
    this.searchTicketService.DownloadSearchTicket(SearchTicketParameters).subscribe(x => {
      this.spinnerService.hide();
      var TodayDate = new Date();
      var CustomerName = this.LstHiddenFields.selectedCustomer.customerName;
      var DateString = (TodayDate.getMonth() + 1) + '/' + TodayDate.getDate() + '/' + TodayDate.getFullYear();
      const fileName = Constants.SearchTicket + "-" + CustomerName + "-" + DateString + Constants.ExcelExtension;
      FileUploadDownload.downloadFileFromBlob(x, fileName, Constants.ExcelMacroType);
    });
    this.SelectedProject=this.SelectedProjectTemp;
  }
  ShowAllColumns(event) {
    if (event.checked) {
      this.ShowHideColumn("releaseType", false);
      this.ShowHideColumn("plannedEffort", false);
      this.ShowHideColumn("estimatedWorkSize", false);
      this.ShowHideColumn("actualWorkSize", false);
      this.ShowHideColumn("plannedEndDate", false);
      this.ShowHideColumn("rejectedTimeStamp", false);
      this.ShowHideColumn("kEDBAvailableIndicator", false);
      this.ShowHideColumn("kEDBUpdated", false);
      this.ShowHideColumn("elevateFlagInternal", false);
      this.ShowHideColumn("rCAID", false);
      this.ShowHideColumn("metResponseSLA", false);
      this.ShowHideColumn("metAcknowledgementSLA", false);
      this.ShowHideColumn("metResolution", false);
      this.ShowHideColumn("actualStartDateTime", false);
      this.ShowHideColumn("actualEndDateTime", false);
      this.ShowHideColumn("actualDuration", false);
      this.ShowHideColumn("natureOfTheTicket", false);
      this.ShowHideColumn("comments", false);
      this.ShowHideColumn("repeatedIncident", false);
      this.ShowHideColumn("relatedTickets", false);
      this.ShowHideColumn("kEDBPath", false);
      this.ShowHideColumn("ticketCreatedBy", false);
      this.ShowHideColumn("escalatedFlagCustomer", false);
      this.ShowHideColumn("approvedBy", false);
      this.ShowHideColumn("startedDateTime", false);
      this.ShowHideColumn("wIPDateTime", false);
      this.ShowHideColumn("onHoldDateTime", false);
      this.ShowHideColumn("outageDuration", false);
      this.ShowHideColumn("resolutionRemarks", false);
      this.ShowHideColumn("flexField1", false);
      this.ShowHideColumn("flexField2", false);
      this.ShowHideColumn("flexField3", false);
      this.ShowHideColumn("flexField4", false);
      this.ShowHideColumn("category", false);
      this.ShowHideColumn("type", false);
    }
    else {
      this.ShowHideColumn("releaseType", true);
      this.ShowHideColumn("plannedEffort", true);
      this.ShowHideColumn("estimatedWorkSize", true);
      this.ShowHideColumn("actualWorkSize", true);
      this.ShowHideColumn("plannedEndDate", true);
      this.ShowHideColumn("rejectedTimeStamp", true);
      this.ShowHideColumn("kEDBAvailableIndicator", true);
      this.ShowHideColumn("kEDBUpdated", true);
      this.ShowHideColumn("elevateFlagInternal", true);
      this.ShowHideColumn("rCAID", true);
      this.ShowHideColumn("metResponseSLA", true);
      this.ShowHideColumn("metAcknowledgementSLA", true);
      this.ShowHideColumn("metResolution", true);
      this.ShowHideColumn("actualStartDateTime", true);
      this.ShowHideColumn("actualEndDateTime", true);
      this.ShowHideColumn("actualDuration", true);
      this.ShowHideColumn("natureOfTheTicket", true);
      this.ShowHideColumn("comments", true);
      this.ShowHideColumn("repeatedIncident", true);
      this.ShowHideColumn("relatedTickets", true);
      this.ShowHideColumn("kEDBPath", true);
      this.ShowHideColumn("ticketCreatedBy", true);
      this.ShowHideColumn("escalatedFlagCustomer", true);
      this.ShowHideColumn("approvedBy", true);
      this.ShowHideColumn("startedDateTime", true);
      this.ShowHideColumn("wIPDateTime", true);
      this.ShowHideColumn("onHoldDateTime", true);
      this.ShowHideColumn("outageDuration", true);
      this.ShowHideColumn("resolutionRemarks", true);
      this.ShowHideColumn("flexField1", true);
      this.ShowHideColumn("flexField2", true);
      this.ShowHideColumn("flexField3", true);
      this.ShowHideColumn("flexField4", true);
      this.ShowHideColumn("category", true);
      this.ShowHideColumn("type", true);
    }

  }
  ShowHideColumn(ColumnName, IsHide) {
    for (let i = 0; i < this.mappingCols.length; i++) {
      var obj = this.mappingCols[i]
      if (obj.field === ColumnName) {
        obj.hide = IsHide;
        break;
      }
    }
  }
  ClearFilter(GridValue,InputValue ) {
    InputValue.value='';
    this.cdref.detectChanges();
  }
  toggle() {
    this.showsearch = !this.showsearch;
  }
  clearAllCustomize() {
    this.clearSearch = !this.clearSearch;
    this.cdref.detectChanges();
    this.clearSearch = !this.clearSearch;
    this.SearchTable.reset();
    this.cdref.detectChanges();
  }
  RemoveDuplicates(dropdownList, name): any {
    var filterArray = dropdownList.reduce((list, current) => {
      if (name !== "projectName" && name !== "ticketTypeName" && name !== "statusName" && name !== "applicationName") {
        if (!list.some(item => item.businessClusterBaseName === current.businessClusterBaseName
          && item.businessClusterMapId === current.businessClusterMapId
          && item.parentBusinessClusterMapId === current.parentBusinessClusterMapId)) {
          list.push(current);
        }
      }
      if (name === "applicationName") {
        if (!list.some(item => item.parentBusinessClusterId === current.parentBusinessClusterId
          && item.applicationId === current.applicationId
          && item.applicationName === current.applicationName)) {
          list.push(current);
        }
      }
      if (name === "ticketTypeName") {
        if (!list.some(item => item.ticketTypeId === current.ticketTypeId
          && item.ticketTypeName === current.ticketTypeName)) {
          list.push(current);
        }
      }
      if (name === "statusName") {
        if (!list.some(item => item.statusId === current.statusId
          && item.statusName === current.statusName
        )) {
          list.push(current);
        }
      }
      return list;
    }, []);
    return filterArray;

  }
}
