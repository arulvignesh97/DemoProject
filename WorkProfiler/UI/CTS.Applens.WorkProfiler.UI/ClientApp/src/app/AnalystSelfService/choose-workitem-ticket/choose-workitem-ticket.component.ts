// Copyright (c) Applens. All Rights Reserved.
import { Component, OnInit, ViewChild } from '@angular/core';
import { BaseInformationModel, SelectedTicketModel } from 'src/app/common/models/BaseInformationModel';
import { ChooseTicketService } from '../Service/choose-ticket.service';
import { TimesheetentryComponent } from '../timesheetentry/timesheetentry.component';
import { DynamicgridComponent } from '../dynamicgrid/dynamicgrid.component';
import { MasterDataModel } from 'src/app/Layout/models/header.models';
import { HeaderService } from 'src/app/Layout/services/header.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ChooseTicket } from '../Models/ChooseTicket';
import { DatePipe } from '@angular/common';
import { AnalystselfserviceService } from '../analystselfservice.service';
import { AddTicketService } from '../Service/add-ticket.service';
import { Table } from 'primeng/table/table';
import { TranslateService } from '@ngx-translate/core';
import { SpinnerService } from 'src/app/common/services/spinner.service';
import { LazyLoadEvent } from 'primeng/api/primeng-api';
import { AppSettingsConfig } from 'src/app/app.settings.config';
import { LayoutService } from 'src/app/common/services/layout.service';
import { Constants } from '../Constants/Constants';

@Component({
  selector: 'app-choose-workitem-ticket',
  templateUrl: './choose-workitem-ticket.component.html',
  styleUrls: ['./choose-workitem-ticket.component.scss'],
})
export class ChooseWorkitemTicketComponent implements OnInit {
  @ViewChild('dtticketGrid') dtticketGrid: Table;
  @ViewChild('dtWorkitemGrid') dtWorkitemGrid: Table;
  public okenable: boolean;
  dictionaryloading = false;
  loading = false;
  public virtualItemSize = 30;
  projectList: any[];
  assignmentGroupAdd: any[];
  applicationList: any[];
  towerlist: any[];
  ticketStatusList: any[];
  StatusList: any[];
  SelectedProject: any;
  public chooseTicketDialog: boolean;
  public ticket: boolean;
  public tickets: any[];
  public Workitems: any[];
  public workItem: boolean;
  public IsticketgridDisplay: boolean;
  public IsWorkitemDisplay: boolean;
  public CurrentTimeZoneDate: string;
  param: BaseInformationModel = new BaseInformationModel();
  selectedticketparam: SelectedTicketModel = new SelectedTicketModel();
  public hiddendata: any;
  public chooseTicketform: FormGroup;
  public ValidationMessage: string = '';
  public cntktorwrkitm: FormControl = new FormControl();
  public cnSearchapplens: FormControl = new FormControl();
  public cnProject: FormControl = new FormControl('', [Validators.required]);
  public cnTicketId: FormControl = new FormControl();
  public cnAssigneeId: FormControl = new FormControl();
  public cnAssignmentGroup: FormControl = new FormControl();
  public cnApplication: FormControl = new FormControl();
  public cnTower: FormControl = new FormControl();
  public cnOpendatefrom: FormControl = new FormControl();
  public cnOpendateto: FormControl = new FormControl();
  public cnTicketstatus: FormControl = new FormControl();
  public cnClosedatefrom: FormControl = new FormControl();
  public cnClosedateto: FormControl = new FormControl();
  public cnworkitemId: FormControl = new FormControl();
  public cnCreatedDatefrom: FormControl = new FormControl();
  public cnCreatedDateTo: FormControl = new FormControl();
  public cnStatus: FormControl = new FormControl();
  chooseticketparam: ChooseTicket = new ChooseTicket();
  public ticketlist: any[] = [];
  public isDARTTicket: number;
  public searchapplensTicket: boolean;
  heightValue = 25;
  scrollheight = this.heightValue + 'vh';
  public openfrommaxdate: Date;
  public opentomindate: Date;
  public closefrommaxdate: Date;
  public closetomindate: Date;
  public createfrommaxdate: Date;
  public createtomindate: Date;
  public IsADMApplicableforCustomer: boolean;
  public IsExtended: string;
  public IsOnlyTicket: boolean;
  public ChooseDaysCount: string;
  public pageno: number = 1;
  public pagesize: number = 10;
  public totalRecords: number;
  public currentyear: number;
  public startingyear: number;
  public highlight: boolean = false;
  public highlightapp: boolean = false;
  displayExemptedMsg=false;
  exemptedMsg:string;

  constructor(
    private translate: TranslateService,
    private timesheetentry: TimesheetentryComponent,
    public addticketservice: AddTicketService,
    public chooseTicketService: ChooseTicketService,
    private dynamicgrid: DynamicgridComponent,
    private headerService: HeaderService,
    private datepipe: DatePipe,
    private analystselfService: AnalystselfserviceService,
    private spinnerService: SpinnerService,
    private layoutService:LayoutService
  ) {
    this.chooseTicketform = new FormGroup({
      cntktorwrkitm: this.cntktorwrkitm,
      cnSearchapplens: this.cnSearchapplens,
      cnTicketId: this.cnTicketId,
      cnProject: this.cnProject,
      cnAssigneeId: this.cnAssigneeId,
      cnApplication: this.cnApplication,
      cnTower: this.cnTower,
      cnAssignmentGroup: this.cnAssignmentGroup,
      cnOpendatefrom: this.cnOpendatefrom,
      cnOpendateto: this.cnOpendateto,
      cnTicketstatus: this.cnTicketstatus,
      cnClosedatefrom: this.cnClosedatefrom,
      cnClosedateto: this.cnClosedateto,
      cnworkitemId: this.cnworkitemId,
      cnCreatedDatefrom: this.cnCreatedDatefrom,
      cnCreatedDateTo: this.cnCreatedDateTo,
      cnStatus: this.cnStatus,
    });
  }

  ngOnInit(): void {
    this.currentyear = new Date().getFullYear();
    this.startingyear = new Date().getFullYear() -2;
    this.okenable = false;
    this.ValidationMessage = '';
    this.headerService.masterDataEmitter.subscribe(
      (masterData: MasterDataModel) => {
        if (masterData != null) {
          this.hiddendata = masterData.hiddenFields;
          this.cnAssigneeId.setValue(this.hiddendata.employeeId);
          this.param.CustomerId = this.hiddendata.customerId;
          this.param.EmployeeId = this.hiddendata.employeeId;
          this.ChooseDaysCount = this.hiddendata.chooseDaysCount;
          this.IsADMApplicableforCustomer = AppSettingsConfig.settings.ADMApplicableforCustomer;
          this.IsExtended = this.hiddendata.isExtended;
          const isCognizant = this.hiddendata.isCognizant;
          if (isCognizant === 0 && !this.IsADMApplicableforCustomer) {
            this.IsOnlyTicket = true;
          } else {
            this.IsOnlyTicket = false;
          }
          this.chooseTicketService
            .ChooseUnAssignedTicket(this.param)
            .subscribe((res: any) => {
              this.CurrentTimeZoneDate = res.currentTime;
              this.projectList = res.lstProjectsModel;
              this.assignmentGroupAdd = res.lstAssignmentGroupDetails;
              if (this.assignmentGroupAdd.length <= 0) {
                this.cnAssignmentGroup.disable();
              } else {
                this.cnAssignmentGroup.enable();
              }
              this.applicationList = res.lstApplicationDetails;
              this.towerlist = res.lstTowerDetails;

              if (this.towerlist.length <= 0) {
                this.cnTower.disable();
              } else {
                this.cnTower.enable();
              }
              if (this.applicationList.length <= 0) {
                this.cnApplication.disable();
              } else {
                this.cnApplication.enable();
              }
              this.ticketStatusList = res.lsTicketStatusDetails.filter(
                (x) => x.type === 'T'
              );
              this.StatusList = res.lsTicketStatusDetails.filter(
                (x) => x.type === 'W'
              );

              if (this.StatusList.length <= 0) {
                this.cnStatus.disable();
              } else {
                this.cnStatus.enable();
              }
              if (this.ticketStatusList.length <= 0) {
                this.cnTicketstatus.disable();
              } else {
                this.cnTicketstatus.enable();
              }
              if (this.projectList.length === 1) {
                this.cnProject.setValue(this.projectList[0]);
                this.Decidescope(this.projectList[0].projectId);
                let opendatefrom = new Date(this.CurrentTimeZoneDate);
                opendatefrom = new Date(opendatefrom.setMonth(opendatefrom.getMonth(), opendatefrom.getDate() - 5));
                let opendateto = new Date(this.CurrentTimeZoneDate);
                let createdDatefrom = new Date(this.CurrentTimeZoneDate);
                createdDatefrom = new Date(createdDatefrom.setMonth(createdDatefrom.getMonth(), createdDatefrom.getDate() - 7));

                this.cnOpendateto.setValue(opendateto);
                this.cnOpendatefrom.setValue(opendatefrom);
                let createddateto = new Date(this.CurrentTimeZoneDate)
                this.cnCreatedDateTo.setValue(createddateto);
                this.cnCreatedDatefrom.setValue(createdDatefrom);
                this.onSelectProject(this.projectList[0].projectId);
              } else {
                let opendatefrom = new Date();
                let opendateto = new Date();
                this.cnOpendateto.setValue(opendateto);
                this.cnOpendatefrom.setValue(opendatefrom);

                this.cnCreatedDateTo.setValue(opendateto);
                this.cnCreatedDatefrom.setValue(opendatefrom);
              }
            });
          this.chooseTicketDialog = true;
          this.ticket = true;
          this.cntktorwrkitm.setValue('T');
          this.workItem = false;
          this.IsWorkitemDisplay = false;
          this.IsticketgridDisplay = false
        }
      }
    );
  }
  public Decidescope(ProjectID): void {
    const scope = [];
    this.hiddendata.lstScope.forEach((element) => {
      if (element.projectId === ProjectID) {
        scope.push(element.scope);
      }
    });
    if (
      scope.filter((x) => x === 2 || x === 3).length > 0 &&
      scope.filter((x) => x === 1 || x === 4).length === 0
    ) {
      this.ticket = true;
      this.cntktorwrkitm.setValue('T');
      this.workItem = false;
    } else if (
      scope.filter((x) => x === 1 || x === 4).length > 0 &&
      scope.filter((x) => x === 2 || x === 3).length === 0
    ) {
      this.ticket = false;
      this.workItem = true;
      this.cntktorwrkitm.setValue('W');
    }
  }
  setValidationMessage(pattern: string): void {
    this.translate.get(pattern)
      .subscribe(message => {
        this.ValidationMessage = message;
        setTimeout(() => {
          this.ValidationMessage = '';
        }, 4000);
      });
  }
  public searchClick(): void {
    this.ticketlist = [];
    const projectid = this.cnProject.value.projectId;
    if (this.cnProject.value === null ||
      this.cnProject.value === undefined ||
      this.cnProject.value === '') {
      this.highlight = true;
      this.setValidationMessage('Capture all the mandatory fields and try again');
    }
    else if(this.workItem && this.cnProject.value != null && (this.cnApplication.value === null ||
      this.cnApplication.value === undefined || this.cnApplication.value === '' || this.cnApplication.value.length === 0)){
      this.highlight = false;
      this.highlightapp = true;
      this.setValidationMessage('Capture all the mandatory fields and try again');
    }
    else {
      this.ValidationMessage = '';
      this.highlight = false;
      this.highlightapp = false;
      this.getTicket();
    }
  }
  public getTicket() {
    this.spinnerService.show();
    const TicketIDDesc = this.cnTicketId.value;
    const AssigneeID = this.cnAssigneeId.value;
    const ProjectID = this.cnProject.value.projectId;
    const ProjectTimeZoneName = this.cnProject.value.projectTimeZoneName;
    const UserTimeZoneName = this.cnProject.value.userTimeZoneName;
    let ApplicationID = '';
    if (this.cnApplication.value !== null || undefined) {
      ApplicationID = this.cnApplication.value.map(x => x.applicationId).join();
    }
    let TowerID = '';

    if (this.cnTower.value !== null || undefined) {
      TowerID = this.cnTower.value.map(x => x.towerId).join();
    }
    let AssignmentgroupIds = '';
    if (this.cnAssignmentGroup.value !== null || undefined) {
      AssignmentgroupIds = this.cnAssignmentGroup.value.map(x => x.assignmentGroupMapId).join();
    }
    let OpenDateFrom = this.cnOpendatefrom.value;
    let OpenDateTo = this.cnOpendateto.value;

    if ((OpenDateFrom !== '' && OpenDateFrom !== null)
      && (OpenDateTo === '' && OpenDateTo === null)) {
      OpenDateTo = OpenDateFrom;
      this.cnOpendateto.setValue(new Date(OpenDateFrom));
    }
    if ((OpenDateFrom === '' && OpenDateFrom === null)
      && (OpenDateTo !== '' && OpenDateTo !== null)) {
      OpenDateFrom = OpenDateTo;
      this.cnOpendatefrom.setValue(new Date(OpenDateTo));
    }
    let StatusID = '';
    if (this.ticket) {
      if (this.cnTicketstatus.value !== null || undefined) {
        StatusID = this.cnTicketstatus.value.map(x => x.ticketStatusId).join();
      }
    } else {
      if (this.cnStatus.value !== null || undefined) {
        StatusID = this.cnStatus.value.map(x => x.ticketStatusId).join();
      }
    }
    let CloseDateFrom = this.cnClosedatefrom.value;
    let CloseDateTo = this.cnClosedateto.value;

    if ((CloseDateFrom !== '' && CloseDateFrom !== null)
      && (CloseDateTo === '' && CloseDateTo === null)) {
      CloseDateTo = CloseDateFrom;
      this.cnClosedateto.setValue(new Date(CloseDateFrom));
    }

    if ((CloseDateFrom === '' && CloseDateFrom === null)
      && (CloseDateTo !== '' && CloseDateTo !== null)) {
      CloseDateFrom = CloseDateTo;
      this.cnClosedatefrom.setValue(new Date(CloseDateFrom));
    }

    const WorkItemFlag = this.ticket ? 'T' : 'W';
    const WorkItemID = this.cnworkitemId.value;
    let CreatedDateFrom = this.cnCreatedDatefrom.value;
    let CreatedDateTo = this.cnCreatedDateTo.value;

    if (CreatedDateFrom !== '' && CreatedDateTo === '') {
      CreatedDateTo = CreatedDateFrom;
      this.cnCreatedDateTo.setValue(new Date(CreatedDateFrom));
    }

    if ((CreatedDateFrom === '' && CreatedDateFrom === null)
      && (CreatedDateTo !== '' && CreatedDateTo !== null)) {
      CreatedDateFrom = CreatedDateTo;
      this.cnCreatedDatefrom.setValue(new Date(CreatedDateTo));
    }

    (this.chooseticketparam.WorkItemFlag = WorkItemFlag),
      (this.chooseticketparam.AssignedTo = AssigneeID),
      (this.chooseticketparam.ProjectId = ProjectID),
      (this.chooseticketparam.CreateDateBegin = this.datepipe.transform(this.cnOpendatefrom.value, 'MM/dd/yyyy')),
      (this.chooseticketparam.CreateDateEnd = this.datepipe.transform(this.cnOpendateto.value, 'MM/dd/yyyy')),
      (this.chooseticketparam.TicketIdDesc = TicketIDDesc),
      (this.chooseticketparam.ApplicationId = ApplicationID),
      (this.chooseticketparam.CloseDateBegin = this.datepipe.transform(this.cnClosedatefrom.value, 'MM/dd/yyyy')),
      (this.chooseticketparam.CloseDateEnd = this.datepipe.transform(this.cnClosedateto.value, 'MM/dd/yyyy')),
      (this.chooseticketparam.StatusId = StatusID),
      (this.chooseticketparam.IsDARTTicket = this.isDARTTicket),
      (this.chooseticketparam.AssigneeId = AssigneeID),
      (this.chooseticketparam.ProjectTimeZoneName = ProjectTimeZoneName),
      (this.chooseticketparam.UserTimeZoneName = UserTimeZoneName),
      (this.chooseticketparam.AssignmentgroupIds = AssignmentgroupIds),
      (this.chooseticketparam.TowerId = TowerID),
      (this.chooseticketparam.ApplicationId = ApplicationID),
      (this.chooseticketparam.WorkItemId = WorkItemID),
      (this.chooseticketparam.CreatedDateFrom = this.datepipe.transform(this.cnCreatedDatefrom.value, 'MM/dd/yyyy')),
      (this.chooseticketparam.CreatedDateTo = this.datepipe.transform(this.cnCreatedDateTo.value, 'MM/dd/yyyy')),
      (this.chooseticketparam.PageNo = this.ticket ? 1 : this.pageno),
      (this.chooseticketparam.PageSize = this.ticket ? 10 : this.pagesize),
      this.chooseTicketService
        .ChooseTicketDetails(this.chooseticketparam)
        .subscribe((res: any) => {
          if (
            (this.ticket && (res !== undefined || res !== null)
              && (res.timeSheetList !== null
                || res.timeSheetList !== undefined)
            ) ||
            (this.workItem &&
              res !== undefined &&
              (res.chooseWorkItemDetail.lstWorkItemDetails !== undefined ||
                res.chooseWorkItemDetail.lstWorkItemDetails !== null)
            )
          ) {
            if (this.ticket) {
              this.IsticketgridDisplay = true;
              this.IsWorkitemDisplay = false;
              this.tickets = res.timeSheetList;
              if (this.tickets && this.tickets.length>0) {
                this.okenable = true;
              } else {
                this.okenable = false;
              }
              let ClosedDateLimit = this.dynamicgrid.selectdate.split("-");
              let closedDateWeekStart = ClosedDateLimit[0].trim();
              this.tickets.forEach(element => {
                element.openDateTime = this.datepipe.transform(element.openDateTime, 'MM/dd/yyyy hh:mm:ss a');
                if (element.closeddate != undefined &&
                  element.closeddate !== '' &&
                  element.closeddate != null && element.dartStatusId === 8) {
                  let validDateClosed = element.closeddate.split(" ")[0];
                  let one;
                  let two;
                  one = new Date(closedDateWeekStart.split("/").reverse().join("-"));
                  two = new Date(validDateClosed);

                  let diff = one - two;

                  let difftocheck = diff / 1000 / 60 / 60 / 24;

                  if (difftocheck >= 15) {
                    element['disableflag'] = true
                    element['toolTip'] = 'Efforts cannot be tracked for the tickets which was closed beyond 15 days';
                    return true;
                  }
                  else {
                    element['disableflag'] = false
                    element['toolTip'] = '';
                  }
                } else {
                  element['toolTip'] = '';
                }
              });
            } else {
              this.IsWorkitemDisplay = true;
              this.IsticketgridDisplay = false;
              this.Workitems = res.chooseWorkItemDetail.lstWorkItemDetails;
              if (this.Workitems) {
                this.okenable = true;
              } else {
                this.okenable = false;
              }
              this.totalRecords = res.chooseWorkItemDetail.totalRowCount;
            }
          } else {
            this.IsWorkitemDisplay = false;
            this.IsticketgridDisplay = false;
          }
          this.spinnerService.hide();
        });
  }
  loadWorkitems(event: LazyLoadEvent) {
    let page = (event.first / 10);
    page = page + 1;
    this.pagesize = event.rows;
    this.pageno = page;
    this.getTicket();
  }
  public onSelectProject(projid): void {
      this.displayExemptedMsg=this.hiddendata.lstProjectUserID
      .filter(x=>x.projectId==projid && x.isExempted==true).length>0
      ?true:false;
        if(!this.displayExemptedMsg){
          this.onProjectSelection(projid);
        }
        else{
          this.exemptedMsg=Constants.exemptedMessage;
          this.IsticketgridDisplay=false;
        }
  }

  onProjectSelection(projectid):void{
    this.refreshCheckBoxes();
    this.Decidescope(projectid);
    let ProjectTimeZoneName;
    let UserTimeZoneName;
    if (this.cnProject.value.userTimeZoneName != null
      && this.cnProject.value.userTimeZoneName != "") {
      UserTimeZoneName = this.cnProject.value.userTimeZoneName;
    }
    else {
      UserTimeZoneName = this.cnProject.value.projectTimeZoneName;
    }
    let params = {
      UserTimeZone: UserTimeZoneName
    };
    if (projectid > 0) {
      this.analystselfService.getCurrentTimeofTimeZones(params).subscribe(x => {
        this.CurrentTimeZoneDate = x;
        let opendatefrom = new Date(this.CurrentTimeZoneDate);
        opendatefrom = new Date(opendatefrom.setMonth(opendatefrom.getMonth(), opendatefrom.getDate() - 5));
        let opendateto = new Date(this.CurrentTimeZoneDate);
        let createdDatefrom = new Date(this.CurrentTimeZoneDate);
        createdDatefrom = new Date(createdDatefrom.setMonth(createdDatefrom.getMonth(), createdDatefrom.getDate() - 7));
        let createddateto = new Date(this.CurrentTimeZoneDate);

        this.cnOpendateto.setValue(opendateto);
        this.cnOpendatefrom.setValue(opendatefrom);

        this.cnCreatedDateTo.setValue(createddateto);
        this.cnCreatedDatefrom.setValue(createdDatefrom);
      });
    }
    let lstselected;
    lstselected = this.projectList.filter(function (value) {
      return value.projectId === projectid;
    });
    if (lstselected[0].supportTypeId === 1) {
      this.getApplicationDetailsByProject(projectid);
      this.cnTower.reset();
      this.cnTower.disable();
    }
    else {
      this.getTowerDetailsByProjectID(projectid.toString(), this.hiddendata.customerId);
      this.getApplicationDetailsByProject(projectid);
    }
    this.getAssignmentGroupByProject(projectid.toString());
    this.getTicketStatusByProject(projectid);
  }
  public getTicketStatusByProject(ProjectID) {
    this.param.ProjectId = ProjectID;
    this.chooseTicketService.GetTicketStatusByProject(ProjectID).subscribe((res: any) => {
      this.ticketStatusList = res.filter(
        (x) => x.type === 'T'
      );
      if (this.ticketStatusList.length > 0) {
        this.cnTicketstatus.reset();
        this.cnTicketstatus.enable();
      } else {
        this.cnTicketstatus.reset();
        this.cnTicketstatus.disable();
      }
      this.StatusList = res.filter(
        (x) => x.type === 'W'
      );
      if (this.StatusList.length > 0) {
        this.cnStatus.enable();
      } else { this.cnStatus.disable(); }
    });
  }
  public getTowerDetailsByProjectID(projectid: string, customerid: string) {
    this.param.CustomerId = customerid;
    this.param.ProjectId = projectid;
    this.chooseTicketService.GetTowerDetailsByProjectID(this.param).subscribe((res: any) => {
      if (res.length > 0) {
        this.towerlist = res;
        this.cnTower.reset();
        this.cnTower.enable();
      } else {
        this.cnTower.reset();
        this.cnTower.disable();
      }
    });
  }
  public getAssignmentGroupByProject(ProjectID: string) {
    this.param.EmployeeId = this.hiddendata.employeeId;
    this.param.ProjectId = ProjectID;
    this.chooseTicketService.GetAssignmentGroupByProject(this.param).subscribe((res: any) => {
      if (res.length > 0) {
        this.assignmentGroupAdd = res;
        this.cnAssignmentGroup.reset();
        this.cnAssignmentGroup.enable();
      }
      else {
        this.cnAssignmentGroup.reset();
        this.cnAssignmentGroup.disable();
      }
    });
  }
  public getApplicationDetailsByProject(ProjectID) {
    this.chooseTicketService.GetApplicationDetailsByProject(ProjectID).subscribe((res: any) => {
      let strResultset = res;
      if (strResultset.length > 0) {
        this.applicationList = strResultset;
        this.cnApplication.reset();
        this.cnApplication.enable();
      }
      else {
        this.cnApplication.reset();
        this.cnApplication.disable();
      }
    });
  }
  public clearpopupdata(): void {
    this.displayExemptedMsg=false;
    this.cnSearchapplens.reset();
    this.isDARTTicket = 0;
    this.cnTicketId.reset();
    this.cnAssigneeId.reset();
    this.cnAssignmentGroup.reset();
    this.cnApplication.reset();
    this.cnTower.reset();
    this.cnOpendatefrom.reset();
    this.cnOpendateto.reset();
    this.cnTicketstatus.reset();
    this.cnClosedatefrom.reset();
    this.cnClosedateto.reset();
    this.cnworkitemId.reset();
    this.cnCreatedDatefrom.reset();
    this.cnCreatedDateTo.reset();
    this.cnStatus.reset();
    this.dynamicgrid.chooseTicketDialog = true;
    this.IsWorkitemDisplay = false;
    this.IsticketgridDisplay = false;
    this.highlight = false;
    this.highlightapp = false;
  }
  public btnchange(value: string): void {
    if(!this.displayExemptedMsg){
    this.clearpopupdata();
    }
    if (value === 'T') {
      this.ticket = true;
      this.okenable = false;
      this.cntktorwrkitm.setValue('T');
      this.workItem = false;
      this.IsWorkitemDisplay = false;
      this.cnAssigneeId.setValue(this.hiddendata.employeeId);
      if (this.projectList.length === 1 || this.cnProject.value !== '') {
        if (this.projectList.length === 1) {
          this.cnProject.setValue(this.projectList[0]);
        }
        let opendatefrom = new Date(this.CurrentTimeZoneDate);
        opendatefrom = new Date(opendatefrom.setMonth(opendatefrom.getMonth(), opendatefrom.getDate() - 5));
        let opendateto = new Date(this.CurrentTimeZoneDate);
        let createdDatefrom = new Date(this.CurrentTimeZoneDate);
        createdDatefrom = new Date(createdDatefrom.setMonth(createdDatefrom.getMonth(), createdDatefrom.getDate() - 7));

        this.cnOpendateto.setValue(opendateto);
        this.cnOpendatefrom.setValue(opendatefrom);
        let createddateto = new Date(this.CurrentTimeZoneDate)
        this.cnCreatedDateTo.setValue(createddateto);
        this.cnCreatedDatefrom.setValue(createdDatefrom);

      } else {
        let opendatefrom = new Date();
        let opendateto = new Date();
        this.cnOpendateto.setValue(opendateto);
        this.cnOpendatefrom.setValue(opendatefrom);

        this.cnCreatedDateTo.setValue(opendateto);
        this.cnCreatedDatefrom.setValue(opendatefrom);
      }
    } else if (value === 'W') {
      this.okenable = false;
      this.workItem = true;
      this.cntktorwrkitm.setValue('W');
      this.ticket = false;
      this.IsticketgridDisplay = false;
      this.isDARTTicket = 0;
      this.cnAssigneeId.setValue(this.hiddendata.employeeId);
      if (this.projectList.length === 1 || this.cnProject.value !== '') {
        if (this.projectList.length === 1) {
          this.cnProject.setValue(this.projectList[0]);
        }
        let opendatefrom = new Date(this.CurrentTimeZoneDate);
        opendatefrom = new Date(opendatefrom.setMonth(opendatefrom.getMonth(), opendatefrom.getDate() - 5));
        let opendateto = new Date(this.CurrentTimeZoneDate);
        let createdDatefrom = new Date(this.CurrentTimeZoneDate);
        createdDatefrom = new Date(createdDatefrom.setMonth(createdDatefrom.getMonth(), createdDatefrom.getDate() - 7));

        this.cnOpendateto.setValue(opendateto);
        this.cnOpendatefrom.setValue(opendatefrom);
        let createddateto = new Date(this.CurrentTimeZoneDate)
        this.cnCreatedDateTo.setValue(createddateto);
        this.cnCreatedDatefrom.setValue(createdDatefrom);

      } else {
        let opendatefrom = new Date();
        let opendateto = new Date();
        this.cnOpendateto.setValue(opendateto);
        this.cnOpendatefrom.setValue(opendatefrom);
        this.cnCreatedDateTo.setValue(opendateto);
        this.cnCreatedDatefrom.setValue(opendatefrom);
      }
    }
  }
  public chkChoose_change(data, event) {
    let obj;
    if (event) {
      if (this.ticket) {
        let isticketexist = this.ticketlist.find(x => x.TicketID == data.ticketNumber);
        let isgridexist = this.dynamicgrid.bkpticketdetails.find(x =>
          x.ticketId === data.ticketNumber && x.projectId ===this.cnProject.value.projectId);
        if (typeof isticketexist !== 'object' && typeof isgridexist !=='object') {
          obj = { TicketID: data.ticketNumber, SupportTypeID: data.supportTypeId, Type: 'T', WorkItemID: '' }
          this.ticketlist.push(obj);
        }
      }
      else if (this.workItem) {
        let isworkitemexist = this.ticketlist.find(x => x.WorkItemID == data.workItemId)
        let isgridexist = this.dynamicgrid.bkpticketdetails.find(x =>
          x.ticketId === data.workItemId && x.projectId ===this.cnProject.value.projectId);
        if (typeof isworkitemexist !== 'object'&& typeof isgridexist !=='object') {
          obj = { TicketID: '', SupportTypeID: 0, Type: 'W', WorkItemID: data.workItemId }
          this.ticketlist.push(obj);
        }
      }
    } else {
      if (this.ticket) {
        let isticketexist = this.ticketlist.findIndex(
          x => x.TicketID == data.ticketNumber && x.SupportTypeID == data.supportTypeId);
        if (isticketexist !== -1) {
          this.ticketlist.splice(isticketexist, 1);
        }
      }
      else if (this.workItem) {
        let isworkitemexist = this.ticketlist.findIndex(x => x.WorkItemID == data.workItemId)
        if (isworkitemexist !== -1) {
          this.ticketlist.splice(isworkitemexist, 1);
        }
      }
    }
  }
  public bindSelectedTickets() {
    this.spinnerService.show();
    let CustomerID = this.hiddendata.customerId;
    let EmployeeID = this.hiddendata.employeeId
    let ProjectID = this.cnProject.value.projectId;

    let Dates = this.dynamicgrid.selectdate.split('-');
    let StartDate = Dates[0].trim();
    let EndDate = Dates[1].trim();
    let FirstDateOfWeek = StartDate;
    let LastDateOfWeek = EndDate;
    let IsCognizant = this.hiddendata.isCognizant
    if (this.ticketlist.length > 0) {
      this.selectedticketparam.CustomerID = CustomerID;
      this.selectedticketparam.TicketID_Desc = this.ticketlist;
      this.selectedticketparam.EmployeeID = EmployeeID;
      this.selectedticketparam.ProjectID = ProjectID.toString();
      this.selectedticketparam.FirstDateOfWeek = FirstDateOfWeek;
      this.selectedticketparam.LastDateOfWeek = LastDateOfWeek;
      this.selectedticketparam.Mode = "ChooseTicket";
      this.selectedticketparam.isCognizant = IsCognizant;
    }
    this.addticketservice.GetSelectedTicketDetails(this.selectedticketparam)
      .subscribe((res: any) => {
        if (res.lstOverallTicketDetails !== null) {
          this.dynamicgrid.BindSelectedTicketsorWorkItems(
            res.lstOverallTicketDetails
          );
        }
        this.dynamicgrid.chooseTicketDialog = false;
        this.spinnerService.hide();
      })
    return false;
  }
  public chkboxchange(event: boolean) {
    if (event) {
      this.isDARTTicket = 1;
      this.refreshCheckBoxes();
    }
    else {
      this.isDARTTicket = 0;
      this.refreshCheckBoxes();
    }
  };

  public refreshCheckBoxes(): void {
    this.cnTicketstatus.reset();
    this.cnStatus.reset();
    this.cnApplication.reset();
    this.cnAssignmentGroup.reset();
    this.cnTower.reset();
  }
  public setdate_startend(startDate, endDate) {
    if (startDate == '' && endDate != '') {
      let SelectedDate = new Date(endDate);
      this.openfrommaxdate = SelectedDate; 
    }
    else if (endDate == '' && startDate != '') {
      let SelectedDate = new Date(startDate);
      this.opentomindate = SelectedDate;
    }
  }

  public setdate_closestartend(startDate, endDate) {
    if (startDate == '' && endDate != '') {
      let SelectedDate = new Date(endDate);
      this.closefrommaxdate = SelectedDate;
    }
    else if (endDate == '' && startDate != '') {
      let SelectedDate = new Date(startDate);
      this.closetomindate = SelectedDate;
    }
  }

  public setdate_createdstarteend(startDate, endDate) {
    if (startDate == '' && endDate != '') {
      let SelectedDate = new Date(endDate);
      this.createfrommaxdate = SelectedDate;
    }
    else if (endDate == '' && startDate != '') {
      let SelectedDate = new Date(startDate);
      this.createtomindate = SelectedDate;
    }
  }
  public openfromchange(): void {
    let startDate = this.cnOpendatefrom.value;
    let endDate = '';
    if (startDate !== null) {
      this.setdate_startend(startDate, endDate);
    }
  }
  public opentochange(): void {
    let startDate = '';
    let endDate = this.cnOpendateto.value;
    if (endDate !== null) {
      this.setdate_startend(startDate, endDate);
    }
  }
  public closefromchange(): void {
    let startDate = this.cnClosedatefrom.value;
    let endDate = '';
    if (startDate !== null) {
      this.setdate_closestartend(startDate, endDate);
    }
  }
  public closetochange(): void {
    let startDate = '';
    let endDate = this.cnClosedateto.value;
    if (endDate !== null) {
      this.setdate_closestartend(startDate, endDate);
    }
  }
  public createfromchange(): void {
    let startDate = this.cnCreatedDatefrom.value;
    let endDate = '';
    if (startDate !== null) {
      this.setdate_createdstarteend(startDate, endDate);
    }
  }
  public createtochange(): void {
    let startDate = '';
    let endDate = this.cnCreatedDateTo.value;
    if (endDate !== null) {
      this.setdate_createdstarteend(startDate, endDate);
    }
  }
}
