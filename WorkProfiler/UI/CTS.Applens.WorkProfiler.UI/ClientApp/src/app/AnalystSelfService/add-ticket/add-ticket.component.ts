// Copyright (c) Applens. All Rights Reserved.
import { Component, OnInit, Attribute } from '@angular/core';
import { AddTicketService } from '../Service/add-ticket.service';
import { BaseInformationModel } from 'src/app/common/models/BaseInformationModel';
import { FormGroup, FormControl, Validators, AbstractControl } from '@angular/forms';
import { DynamicgridComponent } from 'src/app/AnalystSelfService/dynamicgrid/dynamicgrid.component';
import { AddTicketDetailsModel } from '../Models/AddTicketDetailsModel';
import { TimesheetentryComponent } from '../timesheetentry/timesheetentry.component';
import { Constants } from 'src/app/AnalystSelfService/Constants/Constants';
import * as moment from 'moment';
import { DatePipe } from '@angular/common';
import { HeaderService } from 'src/app/Layout/services/header.service';
import { MasterDataModel } from 'src/app/Layout/models/header.models';
import { TranslateService } from '@ngx-translate/core';
import { SpinnerService } from 'src/app/common/services/spinner.service';
import { AnalystselfserviceService } from '../analystselfservice.service';
import { LayoutService } from 'src/app/common/services/layout.service';
import { AppSettingsConfig } from 'src/app/app.settings.config';

@Component({
  selector: 'app-add-ticket',
  templateUrl: './add-ticket.component.html',
  styleUrls: ['./add-ticket.component.scss'],
})
export class AddTicketComponent implements OnInit {
  clickdetected: boolean = false;
  isopendisable: boolean;
  isApplensTicket: boolean;
  projectList: any[];
  statusListAdd: any[];
  priorityListAdd: any[];
  selectedProject: any;
  towerListAdd: any[];
  assignmentGroupAdd: any[];
  ticketTypeList: any[];
  supportTypeList: any[];
  applicationList: any[];
  allAppList: any[];
  AssignTowerMap : any[];
  submitted = false;
  otherValidation = false;
  public virtualItemSize = 30;
  // public addTicketDialog: boolean;
  public addTicketform: FormGroup;
  public cnTicketId: FormControl = new FormControl('', [Validators.required],removeSpaces);
  public cnProject: FormControl = new FormControl('', [Validators.required]);
  public cnSupportType: FormControl = new FormControl('', [
    Validators.required,
  ]);
  public cnApplication: FormControl = new FormControl('', [
    Validators.required,
  ]);
  public cnTower: FormControl = new FormControl('', [Validators.required]);
  public cnAssignmentGroup: FormControl = new FormControl('');
  public cnTicketType: FormControl = new FormControl('', [Validators.required]);
  public cnOpenDate: FormControl = new FormControl(null, [Validators.required]);
  public cnPriority: FormControl = new FormControl('', [Validators.required]);
  public cnStatus: FormControl = new FormControl('', [Validators.required]);
  public cnDescription: FormControl = new FormControl('');
  param: BaseInformationModel = new BaseInformationModel();
  addticketparam: AddTicketDetailsModel = new AddTicketDetailsModel();
  public hiddendata: any;
  public AssignmentGroupHelp = Constants.DropdownAddMsg;
  public ValidationMessage : string  = '';
  public maxdate: Date;
  public dateincrement : number;
  public currentyear: number;
  public startingyear: number;
  displayExemptedMsg=false;
  exemptedMsg: string;
  constructor(
    private translate: TranslateService,
    private timesheetentry: TimesheetentryComponent,
    private dynamicgrid: DynamicgridComponent,
    public addticketservice: AddTicketService,
    private headerService: HeaderService,
    private datepipe: DatePipe,
    private spinnerService: SpinnerService,
    private analystselfService: AnalystselfserviceService,
    private layoutService:LayoutService
  ) {
    this.addTicketform = new FormGroup({
      cnTicketId: this.cnTicketId,
      cnProject: this.cnProject,
      cnSupportType: this.cnSupportType,
      cnApplication: this.cnApplication,
      cnTower: this.cnTower,
      cnAssignmentGroup: this.cnAssignmentGroup,
      cnDescription: this.cnDescription,
      cnTicketType: this.cnTicketType,
      cnOpenDate: this.cnOpenDate,
      cnPriority: this.cnPriority,
      cnStatus: this.cnStatus,
    });
  }

  ngOnInit(): void {
    this.currentyear = new Date().getFullYear();
    this.startingyear = new Date().getFullYear() -2;
    this.isopendisable = true;
    this.headerService.masterDataEmitter.subscribe(
      (masterData: MasterDataModel) => {
        if (masterData != null) {
          this.hiddendata = masterData.hiddenFields;
          const curr = new Date(); // get current date
          const curr1 = new Date(); // get current date
          const first = curr.getDate() - curr.getDay(); // First day is the day of the month - the day of the week
          const last = first + 6; // last day is the first day + 6

          const firstday = new Date(curr.setDate(first)).toUTCString();
          const lastday = new Date(curr1.setDate(last)).toUTCString();
          this.maxdate = new Date(lastday); // lastday;
          this.isApplensTicket = false;
          this.param.CustomerId = this.hiddendata.customerId;
          this.param.EmployeeId = this.hiddendata.employeeId;
          this.addticketservice
            .GetAddTicketPopup(this.param)
            .subscribe((res: any) => {
              this.allAppList = res.applicationProjectModels;
              this.projectList = res.projectModels;
              if (this.projectList.length === 1) {
                this.cnProject.setValue(this.projectList[0]);
                this.ddlprojectchangeforadd(this.projectList[0].projectId);
              }
            });
        }
      }
    );
  }

  public ddlprojectchangeforadd(projectid): void {  
      this.displayExemptedMsg=this.hiddendata.lstProjectUserID
      .filter(x=>x.projectId==projectid && x.isExempted==true).length>0
      ?true:false;
        if(!this.displayExemptedMsg){
          this.projectChangeForAdd(projectid);
        }
        else{
          this.exemptedMsg=Constants.exemptedMessage;
        }
  }

  public projectChangeForAdd(projectid):void{
    const scope = [];
    this.hiddendata.lstScope.forEach((element) => {
      if (element.projectId === projectid) {
        scope.push(element.scope);
      }
    });
    if (
      scope.filter((x) => x === 1 || x === 4).length > 0 &&
      scope.filter((x) => x === 2 || x === 3).length === 0
    ) {
      this.setValidationMessage('CompleteITSM');

      return;
    } else {
      this.ValidationMessage = '';
    }
    this.cnSupportType.reset();
    this.isopendisable = false;
    this.cnSupportType.reset();
    this.addticketservice
      .GetDetailsByProjectID(projectid)
      .subscribe((response: any) => {
        this.statusListAdd = response.lstStatusListForAdd;
        let filtr = this.statusListAdd.find(x => x.isDefaultTicketStatus === 'Y'
          || x.isDefaultTicketStatus === 'y')
        if (filtr) {
          this.cnStatus.setValue(filtr);
        }
        this.priorityListAdd = response.lstPriorityListForAdd;
        let priorityfltr = this.priorityListAdd.find(x => x.isDefaultPrority === 'Y'
          || x.isDefaultPrority === 'y')
        if (priorityfltr) {
          this.cnPriority.setValue(priorityfltr);
        }
      });
    this.param.ProjectId = JSON.stringify(projectid);
    this.GetAddTicketDetails(this.param.ProjectId, 0, true);
    let UserTimeZoneName = this.cnProject.value.userTimeZoneName;
    let ProjectTimeZoneName = this.cnProject.value.projectTimeZoneName;
    if (UserTimeZoneName == '' || UserTimeZoneName == undefined) {
      UserTimeZoneName = ProjectTimeZoneName;
    }
    let params = {
      UserTimeZone: UserTimeZoneName
    };
    this.analystselfService.getCurrentTimeofTimeZones(params).subscribe(x => {
      const curr = new Date(x); // get current date
      const curr1 = new Date(x); // get current date
      const first = curr.getDate() - curr.getDay(); // First day is the day of the month - the day of the week
      const last = first + 6; // last day is the first day + 6

      const firstday = new Date(curr.setDate(first)).toUTCString();
      const lastday = new Date(curr1.setDate(last)).toUTCString();
      this.maxdate = new Date(lastday); // lastday;
      this.dateincrement =0;
    });
  }
  public GetAddTicketDetails(projectid, supportTypeID, supporttypecheck): void {
    this.param.ProjectId = projectid.toString();
    this.param.UserId = this.hiddendata.employeeId;
    this.param.SupportTypeId = supportTypeID;
    this.addticketservice
      .GetDetailsAddTicket(this.param)
      .subscribe((res3: any) => {
        this.AssignTowerMap=res3.lstAssignmentTowerMapModel;
        if (
          (res3.tmSupporttype.supportTypeId === 3 && supportTypeID === 2) ||
          res3.tmSupporttype.supportTypeId === 2
        ) {
          this.towerListAdd = res3.lstTower;
        }
        if (
          (res3.tmSupporttype.supportTypeId === 3 && supportTypeID !== 0) ||
          res3.tmSupporttype.supportTypeId !== 3
        ) {
          this.assignmentGroupAdd = res3.lstAssignmentGroup;
        }
        if (
          (res3.tmSupporttype.supportTypeId === 3 && supportTypeID === 1) ||
          res3.tmSupporttype.supportTypeId === 1
        ) {
          this.ddlaccountchangeadd(projectid);
        }
        this.ticketTypeList = [];
        res3.lstTicketTypeBysupportType.forEach((element) => {
          if (
            element.isDefaultTicketType === 'Y' ||
            element.isDefaultTicketType === 'y'
          ) {
            this.ticketTypeList.push(element);
            this.cnTicketType.setValue(element);
          } else {
            this.ticketTypeList.push(element);
          }
        });

        if (supporttypecheck) {
          if (res3.tmSupporttype.supportTypeId !== 3) {
            this.supportTypeList = [
              { value: 1, label: 'App' },
              { value: 2, label: 'Infra' },
            ];
            let filtr = this.supportTypeList.find(x => x.value === res3.tmSupporttype.supportTypeId);
            if (filtr) {
              this.cnSupportType.setValue(filtr);
              this.cnSupportType.disable();
            } else {
              this.cnSupportType.enable();
            }
            if (res3.tmSupporttype.supportTypeId === 1) {
              this.cnTower.reset();
              this.cnTower.disable();
              this.cnApplication.enable();
            } else if (res3.tmSupporttype.supportTypeId === 2) {
              this.cnTower.enable();
              this.cnApplication.reset();
              this.cnApplication.disable();
            }
          } else if (res3.tmSupporttype.supportTypeId === 3) {
            this.supportTypeList = [
              { value: 1, label: 'App' },
              { value: 2, label: 'Infra' },
            ];
            this.cnSupportType.enable();
          }
        }
        if ((res3.lstAssignmentGroup.length === 0 || res3.lstTower.length === 0)
          && this.cnSupportType.value?.value === 2) {
          if (res3.lstAssignmentGroup.length === 0 && res3.lstTower.length === 0) {
            this.setValidationMessage('AppTowerConfigure');
          } else if (res3.lstAssignmentGroup.length == 0) {
            this.setValidationMessage('AppConfigure');
          } else if (res3.lstTower.length === 0) {
            this.setValidationMessage('TowerConfigure');
          }
        }
        else {
          this.ValidationMessage = '';
        }
      });
  }
  public onTicketChange(isapplens: number): void {
    if (isapplens === 1) {
      this.cnTicketId.reset();
      this.isApplensTicket = true;
      this.cnTicketId.disable();
    } else {
      this.isApplensTicket = false;
      this.cnTicketId.enable();
    }
  }
  public ddlaccountchangeadd(projectid: string): void {
    this.applicationList = [];
    this.allAppList.forEach((element) => {
      if (element.projectId === parseInt(projectid)) {
        const obj = {
          applicationId: element.applicationId,
          applicationName: element.applicationName,
        };
        this.applicationList.push(obj);
      }
    });
  }
  public supportTypeChange(supportvalue): void {
    this.cnTower.reset();
    this.cnApplication.reset();
    
    const supportTypeID = supportvalue;
    const projectid = this.cnProject.value.projectId;
    if (projectid > 0) {
      this.GetAddTicketDetails(projectid, supportTypeID, false);
      if (supportTypeID === 1) {
        this.cnTower.setValue('');
        this.cnTower.disable();
        this.cnApplication.enable();
      } else if (supportTypeID === 2) {
        this.cnApplication.setValue('');
        this.cnApplication.disable();
        this.cnTower.enable();
        if(this.cnAssignmentGroup.value != null||undefined){
          this.AssignmentTowerMapping();
        }
      }
    }
  }
  public cancelAddTicket(): boolean {
    this.displayExemptedMsg=false;
    this.addTicketform.reset();    
    this.statusListAdd = [];
    this.priorityListAdd = [];
    this.towerListAdd = [];
    this.assignmentGroupAdd = [];
    this.ticketTypeList = [];
    this.supportTypeList = [];
    this.applicationList = [];
    if (this.projectList.length === 1) {
      this.cnProject.setValue(this.projectList[0]);
      this.ddlprojectchangeforadd(this.projectList[0].projectId);
    }
    return false;
  }
  getVirtualScroll(list: any): boolean {
    if (list && list.length > this.virtualItemSize) {
      return true;
    } else {
      return false
    }
  }
  setValidationMessage(pattern: string): void {
    this.translate.get(pattern)
      .subscribe(message => {
        this.ValidationMessage = message;
        if (message !== 'Ticket(s) can be created only for the project which has completed ITSM configuration') {
          setTimeout(() => {
            this.ValidationMessage = '';
          }, 4000);
        }
      });
  }
public AssignmentTowerMapping(){
  
  if(this.AssignTowerMap.find(x=>x.assignmentGroupMapId==this.cnAssignmentGroup.value.assignmentGroupId)==undefined||null){
      this.cnTower.setValue('');
    }
    else{
      this.cnTower.setValue(this.towerListAdd.find(y=>y.towerId==
        this.AssignTowerMap.find(x=>x.assignmentGroupMapId==this.cnAssignmentGroup.value.assignmentGroupId).towerId));
    }
}

  public saveAddTicket(): void {
    this.clickdetected = true;
    this.submitted = true;
    if (this.cnProject.value === '' || this.cnProject.value === null ||
      this.cnSupportType.value === '' || this.cnSupportType.value === null) {
        this.clickdetected = false;
      this.setValidationMessage('ProjectSupport');
      return;
    }
    if (this.cnSupportType.value.value === 2) {
      this.cnDescription.setValidators([Validators.required]);
      this.cnAssignmentGroup.setValidators([Validators.required]);
      this.cnAssignmentGroup.updateValueAndValidity();
      this.cnDescription.updateValueAndValidity();
    } else {
      this.cnDescription.clearValidators();
      this.cnAssignmentGroup.clearValidators();
      this.cnAssignmentGroup.updateValueAndValidity();
      this.cnDescription.updateValueAndValidity();
    }
    if (this.addTicketform.invalid) {
      this.otherValidation = true;
      this.setValidationMessage('PleaseFillTheHighlightedFields');
      this.clickdetected = false;
      return;
    } else {
      if(this.cnTicketId.value ===''|| this.cnTicketType.value ===''
        ||this.cnOpenDate.value === null|| 
        this.cnPriority.value ===''|| this.cnStatus.value ===''||
        (this.cnSupportType.value.value ===1 && this.cnApplication.value ==='') ||
        (this.cnSupportType.value.value === 2 && (this.cnDescription.value === '' 
        || this.cnAssignmentGroup.value === '' || this.cnTower.value ===''))) {
          this.clickdetected = false;
          this.otherValidation = true;
          this.setValidationMessage('PleaseFillTheHighlightedFields');
        } else {
          this.ValidationMessage = '';
          this.CheckTicketIDForDuplicate();
        }
    }
  }
  public CheckTicketIDForDuplicate(): void {
    const tktid = this.cnTicketId.value;
    const prjid = this.cnProject.value.projectId;
    if (this.isApplensTicket) {
      this.GetAppLensTicketID();
    } else {
      this.param.TicketId = tktid;
      this.param.SupportTypeId = this.cnSupportType.value.value;
      this.param.ProjectId = JSON.stringify(prjid);
      this.ajaxTicketID(this.param);
    }
  }
  public onModelChange(newval) {
    if (newval === null || newval === undefined || newval === '') {
      //CCAP FIX
    }
    else {
      if (this.dateincrement === 0) {
        this.dateincrement = this.dateincrement+1;
        let value = new Date(newval.getMonth()+1+'/'+newval.getDate() + '/' + 
        newval.getFullYear() + ' ' + this.maxdate.toTimeString());
        this.cnOpenDate.setValue(value);
      }
    }
  }
  public ajaxTicketID(args: BaseInformationModel): void {
    this.addticketservice
      .GetTicketInfoDetails(args)
      .subscribe((res: string) => {
        if (res === '1') {
          this.setValidationMessage('TicketIDalreadyExists');
          this.clickdetected = false;
        } else {
          this.clickdetected = true;
          this.ValidationMessage = '';
          this.SaveTicketDetails();
        }
      });
  }

  public SaveTicketDetails(): void {
    this.spinnerService.show();
    const openDate = this.cnOpenDate.value;
    const TicketID = this.param.TicketId.trim();
    const IsCognizant = this.hiddendata.isCognizant;
    let UserID;
    this.hiddendata.lstProjectUserID.forEach((element) => {
      const ProjectID = element.projectId;
      if (ProjectID === this.cnProject.value.projectId) {
        UserID = element.userId;
      }
    });
    const EmployeeID = this.hiddendata.employeeId;
    const PriorityMapID = this.cnPriority.value.priorityId;
    const StatusMapID = this.cnStatus.value.ticketStatusId;
    const StatusID = this.cnStatus.value.statusId;
    const TicketTypeMapID = this.cnTicketType.value.ticketTypeId;
    const SupportTypeID = this.cnSupportType.value.value;
    let TowerID;
    if (SupportTypeID === 2) {
      TowerID = this.cnTower.value.towerId;
    }
    const AssignmentGroupID = this.cnAssignmentGroup.value?.assignmentGroupId;
    const AssignmentGroup = this.cnAssignmentGroup.value?.assignmentGroupName;
    let IsSDTicket;
    if (this.isApplensTicket) {
      IsSDTicket = 1;
    } else {
      IsSDTicket = 0;
    }

    const CustomerID = this.hiddendata.customerId;
    const Dates = this.dynamicgrid.selectdate.split('-');
    const StartDate = Dates[0].trim();
    const EndDate = Dates[1].trim();
    const FirstDayOfWeek = StartDate;
    const LastDayOfWeek = EndDate;
    this.addticketparam.TicketId = this.isApplensTicket ? TicketID : this.cnTicketId.value;
    if (SupportTypeID === 1) {
      this.addticketparam.ApplicationId = this.cnApplication.value.applicationId;
    }
    this.addticketparam.ProjectId = this.cnProject.value.projectId;
    this.addticketparam.CustomerId = parseInt(CustomerID);
    this.addticketparam.TicketDescription = this.cnDescription.value;
    this.addticketparam.StatusId = StatusID;
    this.addticketparam.DartStatusId = StatusMapID;
    this.addticketparam.PriorityMapId = PriorityMapID;
    this.addticketparam.TicketTypeMapId = TicketTypeMapID;
    this.addticketparam.IsSDTicket = IsSDTicket;
    this.addticketparam.UserId = UserID;
    this.addticketparam.EmployeeId = EmployeeID;
    this.addticketparam.IsCognizant = IsCognizant;
    this.addticketparam.FirstDayofWeek = FirstDayOfWeek;
    this.addticketparam.LastDayofWeek = LastDayOfWeek;
    this.addticketparam.OpenDateUI = this.datepipe.transform(openDate, 'MM/dd/yyyy HH:mm:ss a');
    this.addticketparam.UserTimeZone = this.cnProject.value.userTimeZoneName;
    this.addticketparam.ProjectTimeZone = this.cnProject.value.projectTimeZoneName;
    this.addticketparam.SupportTypeId = SupportTypeID;
    if (SupportTypeID === 2) {
      this.addticketparam.TowerID = TowerID;
    }
    this.addticketparam.AssignmentGroupId = AssignmentGroupID;
    this.addticketparam.AssignmentGroup = AssignmentGroup;
    this.addticketservice
      .AddTicketDetails(this.addticketparam)
      .subscribe((res: any) => {
        if (res !== null) {
          this.dynamicgrid.BindSelectedTicketsorWorkItems(
            res.lstOverallTicketDetails
          );
          this.dynamicgrid.addTicketDialog = false;
          this.clickdetected = true;
          this.ValidationMessage = '';
        }
        else
        {
          this.clickdetected = false;
          this.otherValidation = true;
          this.setValidationMessage(Constants.EncryptFailMessage);
        }
        this.spinnerService.hide();
      });
  }

  public GetAppLensTicketID(): void {
    const CustomerID = this.hiddendata.customerId;
    this.addticketservice
      .GetTicketIdByCustomerID(CustomerID)
      .subscribe((res: string) => {
        if (res !== null) {
         
          this.param.TicketId = res;
          this.param.ProjectId = JSON.stringify(this.cnProject.value.projectId);
          this.param.SupportTypeId = this.cnSupportType.value.value;
          this.ajaxTicketID(this.param);
        }
      });
  }
}
export function removeSpaces(control: AbstractControl) {
  if (control && control.value && !control.value.replace(/\s/g, '').length) {
    control.setValue('');
  }
  return null;
}