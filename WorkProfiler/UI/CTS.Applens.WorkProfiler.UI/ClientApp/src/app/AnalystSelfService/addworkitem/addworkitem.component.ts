// Copyright (c) Applens. All Rights Reserved.
import { Component, OnInit} from '@angular/core';
import { DynamicgridComponent } from '../dynamicgrid/dynamicgrid.component';
import { AddworkitemService } from 'src/app/AnalystSelfService/Service/addworkitem.service';
import { Constants } from 'src/app/AnalystSelfService/Constants/Constants';
import { DatePipe } from '@angular/common';
import { HeaderService} from 'src/app/Layout/services/header.service';
import { MasterDataModel } from 'src/app/Layout/models/header.models';
import { FormGroup, FormControl, Validators,FormGroupDirective, AbstractControl } from '@angular/forms';
import { SpinnerService} from 'src/app/common/services/spinner.service';
import { LayoutService } from 'src/app/common/services/layout.service';
import { AppSettingsConfig } from 'src/app/app.settings.config';

@Component({
  selector: 'app-addworkitem',
  templateUrl: './addworkitem.component.html',
  styleUrls: ['./addworkitem.component.scss'],
  providers: [DatePipe]
})
export class AddworkitemComponent implements OnInit {
  opensprint:boolean;
  projectDetails = [];
  checkWorkItemIdlist = [];
  UOMApplicable: boolean = false;
  UOMtitle: string ='';
  AdmInputlist = [];
  isSaveSprint: boolean = false;
  wiName: boolean = false;
  wiDes: boolean = false;
  isSaveWorkItem: boolean = false;
  mandateCount: number = 0;
  CustomerId: string;
  bugPhaselist = [];
  public currentyear: number;
  public startingyear: number;
  bugPhaseList = [];
  bugTypeList = [];
  addSprint: boolean = false;
  EmployeeId: string;
  attributeList = [];
  IsSprintChecked: boolean = false;
  IsWorkItemChecked: boolean = false;
  duplicatesprint: boolean = false;
  IsKanban:boolean = false;
  masterDetails = [];
  applicationList = [];
  epicList = [];
  epiclist = [];
  podList = [];
  podlist = [];
  priorityList = [];
  isApplensAsALM: boolean;
  sprintList = [];
  statusList = [];
  userstoryList = [];
  userstorylist = [];
  worktypeList = [];
  mandatoryList = [];
  assignee: string = "";
  checkSprintlist = [];
  bugPhase: boolean = false;
  bugType: boolean = false;
  ValidationMessage: string = '';
  MandateMessage: string = '';
  BreakMessage: string = ' <br> '
  SuccessMessage: string = '';
  scopeList = [];
  projectscope = [];
  wiValidationMessage: string = '';
  projectlstDetails = [];
  keyup: boolean = false;
  ExecutionMethod: number;
  sprintDateError: boolean = false;
  mileStone: string;
  duplicateworkItemId: boolean = false;
  workTypeList = [];
  workTypelist = [];
  public fdate: Date;
  public tdate: Date;
  markasmilestone: boolean = false;
  public wiISMName: FormControl = new FormControl('', [Validators.required]);
  public wiPODName: FormControl = new FormControl('', [Validators.required]);
  public wiStartDate: FormControl = new FormControl('', [Validators.required]);
  public wiEndDate: FormControl = new FormControl('', [Validators.required]);
  public wiISMDescription: FormControl = new FormControl('', [Validators.required]);
  public ProjectId: FormControl =  new FormControl({value: '', disabled: false})
  public Application: FormControl = new FormControl({value: '', disabled: false})
  public WorkItemType: FormControl = new FormControl({value: '', disabled: false})
  public WorkItemId: FormControl = new FormControl({value: '', disabled: false})
  public EpicWBS: FormControl = new FormControl({value: '', disabled: false})
  public IterationSprintMilestone: FormControl = new FormControl({value: '', disabled: false})
  public UserStory: FormControl = new FormControl({value: '', disabled: false})
  public Assignee: FormControl = new FormControl({value: '', disabled: false})
  public IterationSprintMilestoneStartDate: FormControl = new FormControl({value: '', disabled: false})
  public IterationSprintMilestoneEndDate: FormControl = new FormControl({value: '', disabled: false})
  public PlannedStartDate: FormControl = new FormControl({value: '', disabled: false})
  public PlannedEndDate: FormControl = new FormControl({value: '', disabled: false})
  public Priority: FormControl = new FormControl({value: '', disabled: false})
  public Status: FormControl = new FormControl({value: '', disabled: false})
  public PlannedEffort: FormControl = new FormControl({value: '', disabled: false})
  public EstimationPoints: FormControl = new FormControl({value: '', disabled: false})
  public WorkItemTitle: FormControl = new FormControl({value: '', disabled: false})
  public Description: FormControl = new FormControl({value: '', disabled: false})
  public MarkasMilestone: FormControl = new FormControl({value: '', disabled: false})
  public BugPhase: FormControl = new FormControl({value: '', disabled: false})
  public BugType: FormControl = new FormControl({value: '', disabled: false})
  public addWorkItemform: FormGroup;
  public addSprintform: FormGroup;
  PODMessage: string = '';
  DateMessage: string = '';
  startDate: string;
  endDate: string;
  displayExemptedMsg=false;
  exemptedMsg: string;
  hiddendata: any;

  constructor( private dynamicGridComponent : DynamicgridComponent,
    private addWorkItemService : AddworkitemService,private  readonly datePipe: DatePipe,
    private headerService : HeaderService, private spinneservice: SpinnerService,
    private layoutService:LayoutService){
      this.addSprintform = new FormGroup({
        wiISMName: this.wiISMName,
        wiPODName: this.wiPODName,
        wiStartDate: this.wiStartDate,
        wiEndDate: this.wiEndDate,
        wiISMDescription: this.wiISMDescription,
      });
      this.addWorkItemform = new FormGroup({
        ProjectId: this.ProjectId,
        Application: this.Application,
        WorkItemType: this.WorkItemType,
        WorkItemId: this.WorkItemId,
        EpicWBS: this.EpicWBS,
        IterationSprintMilestone: this.IterationSprintMilestone,
        UserStory: this.UserStory,
        Assignee: this.Assignee,
        IterationSprintMilestoneStartDate: this.IterationSprintMilestoneStartDate,
        IterationSprintMilestoneEndDate: this.IterationSprintMilestoneEndDate,
        PlannedStartDate: this.PlannedStartDate,
        PlannedEndDate: this.PlannedEndDate,
        Priority: this.Priority,
        Status: this.Status,
        PlannedEffort: this.PlannedEffort,
        EstimationPoints:this.EstimationPoints,
        WorkItemTitle: this.WorkItemTitle,
        Description: this.Description,
        MarkasMilestone: this.MarkasMilestone,
        BugPhase: this.BugPhase,
        BugType: this.BugType
      });
  }

  ngOnInit(): void {
    this.currentyear = new Date().getFullYear();
    this.startingyear = new Date().getFullYear() -2;
    this.UOMApplicable = false;
    this.addWorkItemform.controls["Assignee"].disable();
    this.addWorkItemform.controls["IterationSprintMilestoneStartDate"].disable();
    this.addWorkItemform.controls["IterationSprintMilestoneEndDate"].disable();
    this.bugPhase = false;
    this.bugType = false;
    this.attributeList = 
   [{attributeName: "ProjectId"},
    {attributeName: "Application"},
    {attributeName: "WorkItemType"},
    {attributeName: "WorkItemTitle"},
    {attributeName: "WorkItemId"},
    {attributeName: "Status"},
    {attributeName: "IterationSprintMilestone"},
    {attributeName: "PlannedStartDate"},
    {attributeName: "PlannedEndDate"},
    {attributeName: "PlannedEffort"},
    {attributeName: "EstimationPoints"}
  ]
    this.addWorkItemform.addControl("HdnProjectId",new FormControl({IsInvalid:false}));
    this.addWorkItemform.addControl("HdnWorkItemId",new FormControl({IsInvalid:false}));
    this.addWorkItemform.addControl("HdnWorkItemType",new FormControl({IsInvalid:false}));
    this.addWorkItemform.addControl("HdnWorkItemTitle",new FormControl({IsInvalid:false}));
    this.addWorkItemform.addControl("HdnApplication",new FormControl({IsInvalid:false}));
    this.addWorkItemform.addControl("HdnStatus",new FormControl({IsInvalid:false}));
    this.addWorkItemform.addControl("HdnIterationSprintMilestone",new FormControl({IsInvalid:false}));
    this.addWorkItemform.addControl("HdnIterationSprintMilestoneStartDate",new FormControl({IsInvalid:false}));
    this.addWorkItemform.addControl("HdnIterationSprintMilestoneEndDate",new FormControl({IsInvalid:false}));
    this.addWorkItemform.addControl("HdnPlannedStartDate",new FormControl({IsInvalid:false}));
    this.addWorkItemform.addControl("HdnPlannedEndDate",new FormControl({IsInvalid:false}));
    this.addWorkItemform.addControl("HdnPlannedEffort",new FormControl({IsInvalid:false}));
    this.addWorkItemform.addControl("HdnEstimationPoints",new FormControl({IsInvalid:false}));
    this.addWorkItemform.addControl("HdnBugType",new FormControl({IsInvalid:false}));
    this.addWorkItemform.addControl("HdnBugPhase",new FormControl({IsInvalid:false}));
    
    this.startDate = Constants.IMStartDate;
    this.endDate = Constants.IMEndDate;
    this.mileStone = Constants.MileStone;
    this.headerService.masterDataEmitter.subscribe((masterData: MasterDataModel) => {
      if(masterData != null){
      this.hiddendata = masterData.hiddenFields;
      this.CustomerId = masterData.hiddenFields.customerId;
      this.EmployeeId = masterData.hiddenFields.employeeId;
      this.scopeList = masterData.hiddenFields.lstScope;
      this.projectlstDetails = masterData.hiddenFields.lstProjectUserID;
      this.assignee = masterData.hiddenFields.employeeId + '-' + masterData.hiddenFields.employeeName
      this.Assignee.setValue(this.assignee);
      this.fdate = new Date(localStorage.getItem("Firstdayofweek"));
      this.tdate = new Date(localStorage.getItem("Lastdayofweek"));
    this.opensprint = false;
    this.spinneservice.show();
    this.AdmInputlist = [{
      EmployeeId : this.EmployeeId,
      CustomerId :  this.CustomerId
    }];
    this.addWorkItemService.GetWorkItempopup(this.AdmInputlist).subscribe(x =>{
        this.projectDetails = x;
        this.spinneservice.hide();
       if (this.projectDetails.length == 1) {
        this.ProjectId.setValue(this.projectDetails[0]);
        this.LoadMasterDetails(this.projectDetails[0]);
       }

    });
  }
});
  }
  LoadMasterDetails(projId){
    this.wiValidationMessage = "";
      this.displayExemptedMsg=this.hiddendata.lstProjectUserID
      .filter(x=>x.projectId==projId.projectID && x.isExempted==true).length>0
      ?true:false;
        if(!this.displayExemptedMsg){
          this.LoadProjectMasterDetails(projId);
        }
        else{
          this.exemptedMsg=Constants.exemptedMessage;
        }
  }

  LoadProjectMasterDetails(projectId){
    this.ClearLists();
    let isApplensasALMlist = [];
    this.projectscope = this.scopeList.filter(x => x.projectId == projectId.projectID);
    isApplensasALMlist  = this.projectlstDetails.filter(z => z.projectId == projectId.projectID)
    this.isApplensAsALM = isApplensasALMlist[0].isApplensAsALM
    if((this.projectscope.filter(y => y.scope == 1 || y.scope == 4).length == 0 && 
    this.projectscope.filter(y => y.scope == 2 || y.scope == 3).length > 0) || this.isApplensAsALM == true){
      this.wiValidationMessage = Constants.WorkItemInfoMessage;
      this.addSprint = false;
      this.UOMApplicable = false;
    }
    else{
    this.wiValidationMessage = "";
    this.addSprint = true;
    this.UOMApplicable = true;
    this.UOMtitle = projectId.workItemMeasurement;
    let FromDate = this.datePipe.transform(this.fdate,Constants.DateFormat);
    let ToDate =  this.datePipe.transform(this.tdate,Constants.DateFormat);
    this.spinneservice.show();
    let Params = {
      projectId : projectId.projectID,
      startDate : FromDate,
      endDate : ToDate
    }
    this.addWorkItemService.GetDropDownValuesForWorkItem(Params).subscribe(x =>{
      this.masterDetails = x;
      this.mandatoryList = x.listMandateAttributes;
      this.applicationList = x.listApplicationData;
      this.workTypelist = x.listWorkItemTypeData;
      if(this.applicationList.length == 1){
        this.Application.setValue(this.applicationList[0]);
        this.GetMandateAttributes(this.applicationList[0]);
      }
      this.statusList = x.listStatusData;
      let  newStatus = [];
      if(this.statusList.filter(x => x.statusId == 1).length == 1){
         newStatus = this.statusList.filter(x => x.statusId == 1)
        this.Status.setValue(newStatus[0]);
      }
      if(this.statusList.length == 1){
        this.Status.setValue(this.statusList[0]);
      }
      this.priorityList = x.listPriorityData;
      if(this.priorityList.length == 1){
        this.Priority.setValue(this.priorityList[0]);
      }
      this.BindSprint(x.listSprint);
      this.epiclist = x.listEpic;
      this.podlist = x.listPOD;
      this.userstorylist = x.listUserStory;
      this.bugPhaselist = x.listBugPhase;
      this.spinneservice.hide();
    });
    }
  }
  ClearLists(){ 
    this.markasmilestone = false;
    this.opensprint = false;
    this.applicationList = [];
    this.Application.setValue('');
    this.podList = [];
    this.wiPODName.setValue('');
    this.worktypeList = [];
    this.WorkItemType.setValue('');
    this.WorkItemId.setValue("");
    this.epicList = [];
    this.EpicWBS.setValue('');
    this.sprintList = [];
    this.IterationSprintMilestone.setValue('');
    this.userstoryList = [];
    this.UserStory.setValue('');
    this.IterationSprintMilestoneEndDate.setValue("");
    this.IterationSprintMilestoneStartDate.setValue("");
    this.PlannedStartDate.setValue("");
    this.PlannedEndDate.setValue("");
    this.priorityList = [];
    this.Priority.setValue('');
    this.statusList = [];
    this.Status.setValue('');
    this.bugTypeList = [];
    this.BugType.setValue('');
    this.bugPhaseList = [];
    this.BugPhase.setValue('');
    this.PlannedEffort.setValue("");
    this.EstimationPoints.setValue("");
    this.MarkasMilestone.setValue("");
    this.WorkItemTitle.setValue("");
    this.Description.setValue("");
   
  }
  BindSprint(sprintlist){
  this.sprintList = sprintlist;
  if(sprintlist != null){
  if(this.sprintList.length == 1){
    this.IterationSprintMilestone.setValue(this.sprintList[0])
    this.BindSprintStartEndDates(this.sprintList[0]);
  }
  }
}
  GetDropDownValuesSprint(){
    let FromDate = this.datePipe.transform(this.fdate,Constants.DateFormat);
    let ToDate =  this.datePipe.transform(this.tdate,Constants.DateFormat);
    let  Params = {
      projectId : this.ProjectId.value.projectID,
      startDate : FromDate,
      endDate : ToDate
    }
    this.spinneservice.show();
    this.addWorkItemService.GetDropDownValuesSprint(Params).subscribe(x =>{
      this.addWorkItemform.controls['IterationSprintMilestone'].setValue('');
      this.addWorkItemform.controls['IterationSprintMilestoneStartDate'].setValue('');
      this.addWorkItemform.controls['IterationSprintMilestoneEndDate'].setValue('');
      this.BindSprint(x.listSprint);
      this.spinneservice.hide();
    });
   
}

plannedEffortKeyPress(evt) {
  let  charCode = (evt.which) ? evt.which : evt.keyCode;
  let  number = this.PlannedEffort.value.split('.');
  if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
  }
  if (number.length > 1 && charCode == 46) {
      return false;
  }
  let  arr = [];
  let  dotPos = this.PlannedEffort.value.indexOf(".");
  if(dotPos != -1){
  arr = this.PlannedEffort.value.split(".");
  let  carotpos = arr[1].length;
  
  if(carotpos >1){
    return false;
  }
}
if(this.keyup){
  this.EstimationPoints.setValue(this.PlannedEffort.value);
}
return true;
}
bindeppe(){
  if(this.keyup){
    this.EstimationPoints.setValue(this.PlannedEffort.value);
  }
}

  BindSprintStartEndDates(sprlist){
    let sDate = this.datePipe.transform(sprlist.sprintStartDate,Constants.DateFormat);
    let eDate =  this.datePipe.transform(sprlist.sprintEndDate,Constants.DateFormat);
    this.IterationSprintMilestoneStartDate.setValue(sDate);
    this.IterationSprintMilestoneEndDate.setValue(eDate);
  }
  GetMandateAttributes(applist){
    this.bugPhase = false;
    this.bugType = false;
    this.IsKanban = false;
    this.WorkItemType.setValue('');
    this.EstimationPoints.setValue('');
    this.PlannedEffort.setValue('');
    if(applist.executionMethod == 0){
      let  projectexecutionDetails;
      projectexecutionDetails = this.projectDetails.filter(x => x.projectID == this.ProjectId.value.projectID);
      if(!projectexecutionDetails[0].isMultiple){
      if(projectexecutionDetails[0].isAgile){
        this.ExecutionMethod = Constants.Agile;
        this.mileStone = Constants.MileStone;
      }
      else if(projectexecutionDetails[0].isKanban){
        this.IsKanban = true;
        this.ExecutionMethod = Constants.Agile;
        this.mileStone = Constants.Expedite;
      }
      else if(projectexecutionDetails[0].isIterative){
        this.ExecutionMethod = Constants.Iterative;
      }
      else if(projectexecutionDetails[0].isWBS){
        this.ExecutionMethod = Constants.Waterfall;
      }
      else{
        this.ExecutionMethod = Constants.Others;
      }
      }
      else{
        this.ExecutionMethod = Constants.Others;
      }
    }
    else{
      if(applist.executionMethod <= 14 && applist.executionMethod >= Constants.Agile){
        if(applist.executionMethod == Constants.Kanban){
          this.IsKanban = true;
          this.ExecutionMethod = Constants.Agile;
          this.mileStone = Constants.Expedite;
        }
        else{
        this.ExecutionMethod = Constants.Agile;
        this.mileStone = Constants.MileStone;
        }
      }
      else if(applist.executionMethod == Constants.Waterfall || applist.executionMethod == Constants.Iterative){
        this.ExecutionMethod = applist.executionMethod;
      }
      else{
        this.ExecutionMethod = Constants.Others;
      }
    }
    if(this.ExecutionMethod == Constants.Agile){
      this.startDate = Constants.SStartDate;
      this.endDate = Constants.SEndDate;
    }
    else{
      this.startDate = Constants.IMStartDate;
      this.endDate = Constants.IMEndDate;
      this.mileStone = Constants.MileStone;
    }
    if(this.ExecutionMethod != Constants.Agile){
    this.worktypeList = this.workTypelist.filter(x => x.workTypeId !=2);
    }
    else{
      this.worktypeList = this.workTypelist;
    }
    let  lstFilteredattributes = [];
    lstFilteredattributes = this.mandatoryList.filter(x => x.executionMethodId == this.ExecutionMethod);
    this.MandateAttributesBasedOnExecution(lstFilteredattributes);
    if(this.worktypeList.length == 1){
      this.WorkItemType.setValue(this.worktypeList[0]);
      this.WorkTypeValidation(this.worktypeList[0]);
    }
    
  }
  MandateAttributesBasedOnExecution(lstFilteredItems) {
    let  lstMandate = [];
    lstMandate = lstFilteredItems.filter(x => x.mandateId == 1);
    let  lstNonMandate = [];
    lstNonMandate = lstFilteredItems.filter(x => x.mandateId == 2);
    let  lstNotApplicable = [];
    lstNotApplicable = lstFilteredItems.filter(x => x.mandateId == 3);
    if(lstMandate != null){
      for(  let  r = 0; r < lstMandate.length; r++){
        this.addWorkItemform.controls[lstMandate[r].attributeName].enable();
        document.getElementById(lstMandate[r].attributeName).className = "mandatory";
      }
    }
      if(lstNonMandate != null){
        for(  let  r = 0; r < lstNonMandate.length; r++){
          this.addWorkItemform.controls[lstNonMandate[r].attributeName].enable();
         
        }
      }
        if(lstNotApplicable != null){
          for(  let  r = 0; r < lstNotApplicable.length; r++){
            if(lstNotApplicable[r].attributeName == "MarkasMilestone"){
              this.markasmilestone = false;
            }
            this.addWorkItemform.controls[lstNotApplicable[r].attributeName].disable();
            
          }
        }
        this.addWorkItemform.controls["Assignee"].disable();
        this.addWorkItemform.controls["IterationSprintMilestoneStartDate"].disable();
        this.addWorkItemform.controls["IterationSprintMilestoneEndDate"].disable();
           
    if(this.priorityList.length == 0 && this.ExecutionMethod != Constants.Others){
      this.addWorkItemform.controls['Priority'].disable();
    }

}
  WorkTypeValidation(workType){
  this.BugPhase.setValue('');
  this.BugType.setValue('');
  this.BugType.enable();
  this.EstimationPoints.setValue('');
  this.PlannedEffort.setValue('');
  if(this.ExecutionMethod == Constants.Agile){
    if(workType.workTypeId == 2){
      this.addWorkItemform.controls['UserStory'].disable();
      this.addWorkItemform.controls['UserStory'].setValue('');
      this.BindEpic();
      this.addWorkItemform.controls['EpicWBS'].enable();
    }
  else{
        this.BindUserStory();
        this.addWorkItemform.controls['UserStory'].enable();
        this.addWorkItemform.controls['EpicWBS'].setValue('');
        this.addWorkItemform.controls['EpicWBS'].disable();
    }
  }
  else{
      this.addWorkItemform.controls['UserStory'].disable();
      this.addWorkItemform.controls['UserStory'].setValue('');
      this.BindEpic();
      this.addWorkItemform.controls['EpicWBS'].enable();
      }

  if (workType.workTypeId == 4) {
    this.addWorkItemform.controls['EstimationPoints'].disable();
    document.getElementById("EstimationPoints").className = "";
  }
  else {
    this.addWorkItemform.controls['EstimationPoints'].enable();
    document.getElementById("EstimationPoints").className = "mandatory";
  }
  if (this.ExecutionMethod == Constants.Waterfall || this.IsKanban) {
    if (workType.workTypeId == "3") {
        this.markasmilestone = true;
        this.addWorkItemform.controls['MarkasMilestone'].enable();
        this.addWorkItemform.controls['MarkasMilestone'].setValue("false");
    }
    else {
      this.addWorkItemform.controls['MarkasMilestone'].disable();
      this.markasmilestone = false;
      this.addWorkItemform.controls['MarkasMilestone'].setValue("");
    }
  }
  if (workType.workTypeId == 4) {
    this.bugType = true;
    this.bugPhase = true;
    this.BindBugPhase();
  }
  else{
    this.bugType = false;
    this.bugPhase = false;
  }
  let  lstEffortBased = [];
  lstEffortBased = this.projectDetails.filter(x => x.projectID == this.ProjectId.value.projectID);
  if (lstEffortBased[0].isEffortBased == true) {
      if (workType.workTypeId == 2 || workType.workTypeId == 3) {
          this.addWorkItemform.controls['EstimationPoints'].disable();
          this.addWorkItemform.controls['EstimationPoints'].setValue(this.addWorkItemform.controls['PlannedEffort'].value)
          this.keyup = true;
      }
      else {
         this.keyup = false;
        this.addWorkItemform.controls['EstimationPoints'].disable();
        document.getElementById("EstimationPoints").className = "";
      }
  }

  
  }
  BindBugPhase(){
    this.bugPhaseList = this.bugPhaselist;
    if(this.bugPhaseList.length == 1){
      this.BugPhase.setValue(this.bugPhaseList[0]);
      this.BindBugType(this.bugPhaseList[0]);
    }
  }
  BindBugType(bugphaselist){
    let  selectedbugphaselist = this.bugPhaseList.find(x => x.bugPhaseID == bugphaselist.bugPhaseID);
    this.bugTypeList = selectedbugphaselist.bugPhaseType;
    if(this.bugTypeList.length == 1){
      this.BugType.setValue(this.bugTypeList[0]);
      this.addWorkItemform.controls['BugType'].disable();
    }
    else{
      this.addWorkItemform.controls['BugType'].enable();
    }
  }

  SaveWorkItem() {
    this.isSaveWorkItem = true;
    let  reqCount = this.ValidateMandatoryWorkItem();
    let  Error = this.ValidateSprintDates();
    let  ZeroError = this.PlannedEstimateEffortValidation();

    if(reqCount == 0 && Error == '' && ZeroError == '' && this.IsWorkItemChecked){
      this.spinneservice.show();
      let  errorMessage = '';
      let  workitemdetail = [];
      workitemdetail.push
          ({
              ProjectID: this.ProjectId.value.projectID,
              WorkItemTypeID: this.WorkItemType.value.workTypeMapId,
              WorkItemID: this.WorkItemId.value.trim(),
              ApplicationID: this.Application.value.applicationId,
              EpicID: this.EpicWBS.value == "" ? "0" :this.EpicWBS.value.workItemDetailsId.toString(),
              SprintID: this.IterationSprintMilestone.value.sprintDetailsId,
              UserStoryID: this.UserStory.value == "" ? "0" :this.UserStory.value.workItemDetailsId.toString(),
              Assignee: this.EmployeeId,
              SprintStartDate: new Date(this.IterationSprintMilestoneStartDate.value),
              SprintEndDate: new Date(this.IterationSprintMilestoneEndDate.value),
              PlannedStartDate: new Date(this.datePipe.transform(this.PlannedStartDate.value, Constants.DateFormat)),
              PlannedEndDate: new Date(this.datePipe.transform(this.PlannedEndDate.value, Constants.DateFormat)),
              StatusID: this.Status.value.statusMapId,
              PriorityID: this.Priority.value ==  null ?  null : this.Priority.value.priorityId,
              WorkItemTitle: this.WorkItemTitle.value.trim(),
              Description: this.Description.value.trim(),
              IsMilestonemet: this.MarkasMilestone.value == "false" ? false : this.MarkasMilestone.value == "true" ? true : null,
              PlannedEstimate:parseFloat(this.PlannedEffort.value),
              EstimationPoints: this.EstimationPoints.value ==  "" ? null :this.EstimationPoints.value.trim(),
              BugPhaseTypeMapId: this.BugType.value ==  "" ?  null : this.BugType.value.bugPhaseTypeMapId,
            });
          this.addWorkItemService.AddWorkItem(workitemdetail).subscribe(x => {
             if(x){
               this.BindSelectedWorkItems();
               this.CloseWorkItem();
            this.spinneservice.hide();
             } 
          });

    }
    else{
      let  errorMessage = '';
      if(reqCount > 0){
        errorMessage = "Please fill the highlighted fields";
      }
      if(Error != ''){
        if (errorMessage != '') {
          errorMessage = errorMessage + this.BreakMessage + Error;
        }
        else {
          errorMessage = Error;
        }
      }
      if (ZeroError != '') {
        if (errorMessage != '') {
          errorMessage = errorMessage + this.BreakMessage + ZeroError;
        }
        else {
          errorMessage = ZeroError;
        }
      }
      this.wiValidationMessage = errorMessage;
      setTimeout(() => {this.wiValidationMessage = ''}, 4000);
    }
  }
  BindSelectedWorkItems(){
    let FromDate = this.datePipe.transform(this.fdate,'dd/MM/yyyy');
    let ToDate =  this.datePipe.transform(this.tdate,'dd/MM/yyyy');
    let  TicketList = [];
    let  obj = { TicketID: '', SupportTypeID: 0, Type: 'W', WorkItemID: this.WorkItemId.value.trim() }
      TicketList.push(obj);
      let  CustomerID = this.CustomerId;
      let  EmployeeID = this.EmployeeId;
      let  ProjectID = (this.ProjectId.value.projectID).toString();
      let  TicketID = TicketList;
      let  FirstDateOfWeek = FromDate;
      let  LastDateOfWeek = ToDate;
      let  IsCognizant = 1;
      let  Params = {
            CustomerID: CustomerID,
            TicketID_Desc: TicketID,
            EmployeeID: EmployeeID,
            ProjectID: ProjectID,
            FirstDateOfWeek: FirstDateOfWeek,
            LastDateOfWeek: LastDateOfWeek,
            Mode: "ChooseTicket",
            isCognizant :IsCognizant
        }
        this.addWorkItemService.GetSelectedTicketDetails(Params).subscribe(x =>{
          this.dynamicGridComponent.BindSelectedTicketsorWorkItems(x.lstOverallTicketDetails);
        })
  }
  ValidateSprintDates(){
    let  ItrStartDate = this.IterationSprintMilestoneStartDate.value;
    let  ItrEndDate = this.IterationSprintMilestoneEndDate.value;
    let  PlannedStartDate = this.datePipe.transform(this.PlannedStartDate.value, Constants.DateFormat);
    let  PlannedEndDate = this.datePipe.transform(this.PlannedEndDate.value, Constants.DateFormat);
    let  PlannedError;
    let  ItrPlannedError;
    let  Error = '';

    if (PlannedEndDate != null && PlannedStartDate != null && (new Date(PlannedStartDate) > new Date(PlannedEndDate))) {
      this.addWorkItemform.controls['HdnPlannedStartDate'].value.IsInvalid = true;
      this.addWorkItemform.controls['HdnPlannedEndDate'].value.IsInvalid = true;
      PlannedError = 1;
    }
    if(PlannedEndDate != null && PlannedStartDate != null && ItrStartDate != "" && ItrEndDate != "" &&
      (new Date(PlannedStartDate) < new Date(ItrStartDate) || new Date(PlannedStartDate) > new Date(ItrEndDate) ||
        new Date(PlannedEndDate) < new Date(ItrStartDate) || new Date(PlannedEndDate) > new Date(ItrEndDate))) {
      this.addWorkItemform.controls['HdnPlannedStartDate'].value.IsInvalid = true;
      this.addWorkItemform.controls['HdnPlannedEndDate'].value.IsInvalid = true;
      ItrPlannedError = 1;
    }

      if (PlannedError == 1 || ItrPlannedError == 1) {
          if (PlannedError == 1) {
              Error = "Planned Start Date should be less than Planned End Date";
          }
          if (ItrPlannedError == 1) {
              if (Error == '') {
                  Error = "Planned Start & End date should fall between Sprint Start & End date";
              }
              else {
                  Error = Error + '<br>' + "Planned Start & End date should fall between Sprint Start & End date";
              }
          }
      }
      return Error;
  }
  PlannedEstimateEffortValidation() {    
    let  PlnEffort = this.addWorkItemform.controls['PlannedEffort'].value;
    let  PlannedEffortError;
    let  Error = '';

    if (PlnEffort != '') {
        if (PlnEffort <= 0 || PlnEffort == ".") {
            this.addWorkItemform.controls['HdnPlannedEffort'].value.IsInvalid = true;
            PlannedEffortError = 1;
        }
        else {
            this.addWorkItemform.controls['HdnPlannedEffort'].value.IsInvalid = false;
            PlannedEffortError = 0;
        }
    }
        if (PlannedEffortError == 1) {
            Error = "Planned Effort cannot be Zero";
        }

    return Error;
}

  ValidateMandatoryWorkItem(){
    this.mandateCount = 0;
    for( let  j = 0; j< this.attributeList.length; j++){
      if((document.getElementById(this.attributeList[j].attributeName).className == "mandatory")
       && (((this.attributeList[j].attributeName == 'WorkItemId' 
       || this.attributeList[j].attributeName == 'EstimationPoints'
       ||this.attributeList[j].attributeName == 'WorkItemTitle')
       &&(this.addWorkItemform.controls[this.attributeList[j].attributeName].value.trim() == ''))
        || (this.addWorkItemform.controls[this.attributeList[j].attributeName].value == '' ))){
        this.addWorkItemform.controls['Hdn'+[this.attributeList[j].attributeName]].value.IsInvalid = true;
        this.mandateCount++;
      }
      else{
        this.addWorkItemform.controls['Hdn'+[this.attributeList[j].attributeName]].value.IsInvalid = false;
      }
      if(this.WorkItemType.value.workTypeId == 4){
        if(this.addWorkItemform.controls['BugType'].value == ''){
          this.addWorkItemform.controls['HdnBugType'].value.IsInvalid = true;
          this.mandateCount++;
        }
        else{
          this.addWorkItemform.controls['HdnBugType'].value.IsInvalid = false;
        }
        if(this.addWorkItemform.controls['BugPhase'].value == ''){
          this.addWorkItemform.controls['HdnBugPhase'].value.IsInvalid = true;
          this.mandateCount++;
        }
        else{
          this.addWorkItemform.controls['HdnBugPhase'].value.IsInvalid = false;
        }
      }
    }   
    return this.mandateCount;
  }
  BindEpic(){
    this.epicList = this.epiclist;
    if(this.epicList.length == 1){
      this.EpicWBS.setValue(this.epicList[0]);
    }
  }
  BindUserStory(){
    this.userstoryList = this.userstorylist;
    if(this.userstoryList.length == 1){
      this.EpicWBS.setValue(this.userstoryList[0]);
    }
  }
  CreateISM(){
    this.opensprint = true;
    this.isSaveSprint = false;
    this.ClearSprintValues();
    this.podList = this.podlist;
    if(this.podList.length == 1){
      this.wiPODName.setValue(this.podList[0]);
    }
  }
  CancelSprint(){
    this.opensprint = false;
  }
  ClearSprintValues(){
    this.isSaveSprint = false;
    this.addSprintform.reset();
    this.addSprintform.clearValidators();
  }
  CloseWorkItem(){
    this.dynamicGridComponent.AddWorkItempopup = false;
  }
  CheckSprintName(){
    if(this.wiISMName.value.trim() != ''){
      this.spinneservice.show();
    this.checkSprintlist = [{
      Name : this.wiISMName.value.trim(),
      ProjectId :  this.ProjectId.value.projectID
    }];
    this.addWorkItemService.CheckSprintName(this.checkSprintlist).subscribe(x =>{
      if(x[0].validationMessage != "valid"){
        this.duplicatesprint = true;
        this.IsSprintChecked = false;
        this.ValidationMessage = x[0].validationMessage;
        setTimeout(()=>{this.ValidationMessage = ''},4000);
      } 
      else{
        this.ValidationMessage = '';
        this.IsSprintChecked = true;
        this.duplicatesprint = false;
      }
      this.spinneservice.hide();
    });
  }

  }
  CheckWorkItemId(){
    if(this.WorkItemId.value.trim() != '' && this.ProjectId.value.projectID != undefined){
      this.spinneservice.show();
    this.checkWorkItemIdlist = [{
      Name : this.WorkItemId.value.trim(),
      ProjectId :  this.ProjectId.value.projectID
    }];
    this.addWorkItemService.CheckWorkItemId(this.checkWorkItemIdlist).subscribe(x =>{
      if(x[0].validationMessage != "valid"){
        this.duplicateworkItemId = true;
        this.IsWorkItemChecked = false;
        this.wiValidationMessage = x[0].validationMessage;
        setTimeout(()=>{this.wiValidationMessage = ''},4000);
      } 
      else{
        this.wiValidationMessage = '';
        this.duplicateworkItemId = false;
        this.IsWorkItemChecked = true;
      }
      this.spinneservice.hide();
    });
  }
  }
  ClearWorkItemValues(){
    this.displayExemptedMsg=false;
    this.isSaveWorkItem = false;
    this.addSprint = false;
    this.UOMApplicable = false;
    for( let  j = 0; j< this.attributeList.length; j++){
      this.addWorkItemform.controls[this.attributeList[j].attributeName].setValue('');
    }
    this.addWorkItemform.controls['IterationSprintMilestoneStartDate'].setValue('');
    this.addWorkItemform.controls['IterationSprintMilestoneEndDate'].setValue('');
    this.EpicWBS.setValue('');
    this.UserStory.setValue('');
    this.Description.setValue('');
    this.Priority.setValue('');
    this.BugPhase.setValue('');
    this.BugType.setValue('');
    this.MarkasMilestone.setValue('');
    this.wiValidationMessage = '';
  }
  SaveSprintDetails() {
    if(this.CheckEligibleforSaveSprint() && this.IsSprintChecked){
      this.spinneservice.show();
      let  sprintDetails = [];
      sprintDetails.push({
          ProjectID: this.ProjectId.value.projectID,
          SprintName: this.wiISMName.value.trim(),
          SprintDescription: this.wiISMDescription.value.trim(),
        StartDate: this.datePipe.transform(this.wiStartDate.value, Constants.DateFormat),
        EndDate: this.datePipe.transform(this.wiEndDate.value, Constants.DateFormat),
          UserID: this.EmployeeId,
          StatusMapId: Constants.NewStatus,
          PODDetailID: this.wiPODName.value.podDetailID,
      })
      this.addWorkItemService.SaveSprintDetails(sprintDetails).subscribe(x => {
      if(x){
        this.spinneservice.hide();
        this.ClearSprintValues();
        this.isSaveSprint = false;
        this.opensprint = false;
        this.GetDropDownValuesSprint();
       this.SuccessMessage = Constants.SprintAdded;
        setTimeout(()=>{this.SuccessMessage = ''},4000);
      }
      });
    }
  }
    CheckEligibleforSaveSprint(){
    this.MandateMessage = '';
    this.PODMessage = '';
    this.DateMessage = '';
    this.isSaveSprint = true;
    this.sprintDateError = false;
    if (this.addSprintform.invalid) {
      this.MandateMessage = Constants.MandateMessage
      if(this.podList.length == 0 ){
        if(this.wiEndDate.value != '' && this.wiStartDate.value != '' &&
        (this.wiISMName.value != null && this.wiISMName.value.trim() !='')
        && (this.wiISMDescription.value != null && this.wiISMDescription.value.trim() != '')){
          this.MandateMessage = '';
        }
        
        this.PODMessage  = Constants.PODMessage;
      }
    }
    if(this.wiISMName.value != null){
      if(this.wiISMName.value.trim() == ''){
      this.wiName = true;
      this.MandateMessage = Constants.MandateMessage;
         }
         else{
          this.wiName = false;
         }
        }
  if(this.wiISMDescription.value != null){
  if(this.wiISMDescription.value.trim() == ''){
    this.wiDes = true;
   this.MandateMessage = Constants.MandateMessage;
     }
     else{
      this.wiDes = false;
     }
}
    if(this.wiStartDate.value > this.wiEndDate.value){
      this.DateMessage  = Constants.DateMessage;
      this.sprintDateError = true;
    }

    if(this.MandateMessage.trim() == '' && this.PODMessage.trim() == '' && this.DateMessage.trim() == ''){
      return true;
    }
    else{
      let validmessage = '';
      if (this.MandateMessage != '') {
        validmessage = this.MandateMessage;
      }
      if (this.PODMessage != '') {
        if (validmessage != '') {
          validmessage = validmessage + this.BreakMessage + this.PODMessage;
        }
        else {
          validmessage = this.PODMessage;
        }
      }
      if (this.DateMessage != '') {
        if (validmessage != '') {
          validmessage = validmessage + this.BreakMessage + this.DateMessage;
        }
        else {
          validmessage = this.DateMessage;
        }
      }
      this.ValidationMessage = validmessage;
      setTimeout(()=>{this.ValidationMessage = ''},4000);
      return false;
    }
  }
  //SAST fix
  WorkitemValidate(item : string){
  return this.addWorkItemform.get(item).value.IsInvalid;
  }
}