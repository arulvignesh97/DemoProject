// Copyright (c) Applens. All Rights Reserved.
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { AnalystselfserviceService } from '../../AnalystSelfService/analystselfservice.service';
import { TicketAttributesService } from '../Service/ticket-attributes.service';
import { DynamicgridService } from '../Service/dynamicgrid.service';
import { SelectItem } from 'primeng/api';
import { SpinnerService } from './../../common/services/spinner.service';
import { DatePipe } from '@angular/common';
import { HeaderService } from '../../Layout/services/header.service';
import { MasterDataModel } from '../../Layout/models/header.models';
import { DynamicgridComponent } from '../../AnalystSelfService/dynamicgrid/dynamicgrid.component';
import { Constants } from '../../AnalystSelfService/Constants/Constants';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-ticket-attributes',
  templateUrl: './ticket-attributes.component.html',
  styleUrls: ['./ticket-attributes.component.css'],
  providers: [DatePipe, Constants]
})
export class TicketAttributesComponent implements OnInit {
  @Input() SelectedTicketDetails: any;
  @Input() receivedUpdatedStatus: string;
  expandTickAttr: boolean = false;
  expandDebtAttr: boolean = false;
  expandMandteAttr: boolean = false;
  isSuccess: boolean = false;
  isTicketAttrMandateMsg: boolean = false;
  closeStatus: string;
  isFieldMandate: boolean = false;
  isDebtMandate: boolean = false;
  isMandatryMandate: boolean = false;
  displayDebtAccordion: boolean = false;
  displayMandteAccordion: boolean = false;
  isAppEditable: boolean = false;
  displayCCEdit: boolean = false;
  displayDCEdit: boolean = false;
  disableDebtFields: boolean = false;
  disableCCRC: boolean = false;
  displayAppDropdown: boolean = false;
  displayAppTxtbox: boolean = false;
  disableAppDropDown: boolean = false;
  disableAssignedTo: boolean = false;
  disableDescription: boolean = false;
  disableFlex1: boolean = false;
  disableFlex2: boolean = false;
  disableFlex3: boolean = false;
  disableFlex4: boolean = false;
  disableResolnRemrks: boolean = false;
  isDescriptionErr: boolean = false;
  isAssigneeErr: boolean = false;
  isTickCreatedDteErr: boolean = false;
  isActualDteErr: boolean = false;
  isActualStrtDteErr: boolean = false;
  isActualEndDteErr: boolean = false;
  isActualEndDteClsdErr: boolean = false;
  isActualEndClsDteErr: boolean = false;
  isClosedDteErr: boolean = false;
  isClosedCompleteDteErr: boolean = false;
  isClosedReopnDteErr: boolean = false;
  isClosedOpenDteErr: boolean = false;
  isOpenDteErr: boolean = false;
  isCompletedDteErr: boolean = false;
  isCompletedOpnDteErr: boolean = false;
  isCompletedReopnDteErr: boolean = false;
  isReopenDteErr: boolean = false;
  isPlannedStrtDteErr: boolean = false;
  isPlannedEndDteErr: boolean = false;
  isReleaseDteErr: boolean = false;
  isValidDate: boolean = false;
  validatedAttributes: boolean = false;
  isDebtErr: boolean = false;
  mndteWarningShow: boolean = false;
  mndteWarningBusinessShow: boolean = false;
  isAllfieldsCaptured: boolean = false;
  isPartialMarquee: boolean = false;
  isTickAttrMarquee: boolean = false;
  isTranslationFailure: boolean = false;
  isChangesDone: boolean = false;
  isSaveOrSubmit: string;
  attrList = [];
  lstEnableAttributes = [];
  popupAttributeModels = [];
  ticketdetails: any;
  ticketAttributesList = [];
  debtAttributeList = [];
  debtAttr = [];
  mandateAttributeList = [];
  mandateAttr = [];
  attributesList = [];
  saveAttrList = [];
  attrName: string;
  hdnEmployeeId: string;
  hdnTimeTickerId: string;
  mode: string;
  projectId: string;
  applicationList: SelectItem[] = [];
  priorityList: SelectItem[] = [];
  causeCodeList: SelectItem[] = [];
  resolutionList: SelectItem[] = [];
  debtClassificationList: SelectItem[] = [];
  severityList: SelectItem[] = [];
  ticketSourceList: SelectItem[] = [];
  businessimpactlist: SelectItem[] = [];
  assigneeList = [];
  lstAssignedTo = [];
  avoidableFlgList = [];
  residualDebtList = [];
  releaseTypeList = [];
  kedbUpdatedList = [];
  kedbAvailableList = [];
  radioBtnValList = [];
  autoClassList = [];
  currentTime: string;
  statusName: string;
  hdnCustomerId: string;
  custTimeZoneName: string;
  timeoutValue = 6000;
  statusNameLst = [];
  ticketDesc: string = 'N';
  @Output() getUpdatedStatus = new EventEmitter<any>();
  @Output() freezeServiceStatus = new EventEmitter<any>();
  mandteAssignedto: string = 'Please select valid Assigne.';
  invalidAssignedto: boolean = false;
  gracePeriodTitle: string;
  descriptionTitle: string;
  closedDteTitle: string;
  completedDteTitle: string;
  resolnTitle: string;
  flex1Title: string;
  flex2Title: string;
  flex3Title: string;
  flex4Title: string;
  isPartialTitle: string;
  mainspringEnabled: string;
  actualOldStartDateTime: string;
  actualOldEndDateTime: string;
  closedOldDte: string;
  completedOldDateTime: string;
  reopenOldDateTime: string;
  plannedOldStartDate: string;
  releaseOldDate: string;
  actualSDteTriggerCount: number = 0;
  actualEDteTriggerCount: number = 0;
  closedTriggerCount: number = 0;
  completedTriggerCount: number = 0;
  reopenDateTriggerCount: number = 0;
  plannedSDteTriggerCount: number = 0;
  releaseDteTriggerCount: number = 0;
  actualStartMaxDate: Date;
  actualEndMaxDate: Date;
  closedMaxDate: Date;
  complMaxDate: Date;
  reopenMaxDate: Date;
  pStrtMaxDate: Date;
  releaseMaxDate: Date;
  public currentyear: number;
  public startingyear: number;
  algoKey: string;
  transactionId: number;
  columnList: string[] = [];
  columnControls: ColumnDictionary[];
  showEncryptMessage : boolean = false;
  //single AutoClassification
  isCognizant:any;

  public ticketAttributesFG: FormGroup = new FormGroup({});
  constructor(private fb: FormBuilder, private analystselfserviceService: AnalystselfserviceService,
    private ticketAttributesService: TicketAttributesService, private spinner: SpinnerService,
    private datePipe: DatePipe, private headerService: HeaderService, private dynamicgridComponent: DynamicgridComponent,
    private dynamicgridService: DynamicgridService, private translate: TranslateService) { }
  ngOnInit() {
    this.currentyear = new Date().getFullYear();
    this.startingyear = new Date().getFullYear() - 2;
    this.dynamicgridService.checkGraceperiodEmitter.subscribe((data) => {
      this.checkIsGracePeriodMet(data);
    });
    this.attrList = [
      { attrName: 'Project Name', isVisible: false, attrType: 'T', formControlName: 'projectName', fieldType: 'T' },
      { attrName: 'Application', isVisible: false, attrType: 'T', formControlName: 'appId', fieldType: 'T' },
      { attrName: 'Tower', isVisible: false, attrType: 'T', formControlName: 'towerName', fieldType: 'T' },
      { attrName: 'Ticket / Task (Line item) #', isVisible: false, attrType: 'T', formControlName: 'ticketTask', fieldType: 'T' },
      { attrName: 'Ticket Open Date', isVisible: false, attrType: 'T', formControlName: 'ticketOpenDte', fieldType: 'T' },
      { attrName: 'Ticket Description', isVisible: false, attrType: 'T', formControlName: 'description', fieldType: 'T' },
      { attrName: 'Priority', isVisible: false, attrType: 'T', formControlName: 'priorityId', fieldType: 'T' },
      { attrName: 'Ticket Type', isVisible: false, attrType: 'T', formControlName: 'ticketType', fieldType: 'T' },
      { attrName: 'Cause Code', isVisible: false, attrType: 'D', formControlName: 'causeCodeId', fieldType: 'D' },
      { attrName: 'Resolution Code', isVisible: false, attrType: 'D', formControlName: 'resolutionCodeId', fieldType: 'D' },
      { attrName: 'Debt Classification', isVisible: false, attrType: 'D', formControlName: 'debtClassificationId', fieldType: 'D' },
      { attrName: 'Avoidable Flag', isVisible: false, attrType: 'D', formControlName: 'avoidableFlgId', fieldType: 'D' },
      { attrName: 'Residual Debt', isVisible: false, attrType: 'D', formControlName: 'residualFlgId', fieldType: 'D' },
      { attrName: 'Severity', isVisible: false, attrType: 'M', formControlName: 'severityId', fieldType: 'M' },
      { attrName: 'Assigned To', isVisible: false, attrType: 'M', formControlName: 'assignedTo', fieldType: 'M' },
      { attrName: 'Ticket Source', isVisible: false, attrType: 'M', formControlName: 'ticketSourceId', fieldType: 'M' },
      { attrName: 'KEDB – Path', isVisible: false, attrType: 'M', formControlName: 'kEDBPath', fieldType: 'M' },
      { attrName: 'Flex Field (1)', isVisible: false, attrType: 'M', formControlName: 'flexField1', fieldType: 'M' },
      { attrName: 'Flex Field (2)', isVisible: false, attrType: 'M', formControlName: 'flexField2', fieldType: 'M' },
      { attrName: 'Flex Field (3)', isVisible: false, attrType: 'M', formControlName: 'flexField3', fieldType: 'M' },
      { attrName: 'Flex Field (4)', isVisible: false, attrType: 'M', formControlName: 'flexField4', fieldType: 'M' },
      { attrName: 'Release Type', isVisible: false, attrType: 'M', formControlName: 'releaseTypeFlgId', fieldType: 'M' },
      { attrName: 'Estimated Work Size', isVisible: false, attrType: 'M', formControlName: 'estimatedWorkSize', fieldType: 'M' },
      { attrName: 'Actual Effort', isVisible: false, attrType: 'M', formControlName: 'actualEffort', fieldType: 'M' },
      { attrName: 'Ticket Create Date', isVisible: false, attrType: 'M', formControlName: 'ticketCreatedDte', fieldType: 'M' },
      { attrName: 'Actual Start date Time', isVisible: false, attrType: 'M', formControlName: 'actualStartDte', fieldType: 'M' },
      { attrName: 'Actual End date Time', isVisible: false, attrType: 'M', formControlName: 'actualEndDte', fieldType: 'M' },
      { attrName: 'Closed date', isVisible: false, attrType: 'M', formControlName: 'closedDte', fieldType: 'M' },
      { attrName: 'KEDB Updated / Added', isVisible: false, attrType: 'M', formControlName: 'kedbUpFlgId', fieldType: 'M' },
      { attrName: 'Resolution Method', isVisible: false, attrType: 'M', formControlName: 'resolutionMethodName', fieldType: 'M' },
      { attrName: 'KEDB Available Indicator', isVisible: false, attrType: 'M', formControlName: 'kedbAvailFlgId', fieldType: 'M' },
      { attrName: 'RCA ID', isVisible: false, attrType: 'M', formControlName: 'rcaId', fieldType: 'M' },
      { attrName: 'Met Response SLA (Y/N)', isVisible: false, attrType: 'M', formControlName: 'metResFlgId', fieldType: 'M' },
      { attrName: 'Met Resolution (Y/N)', isVisible: false, attrType: 'M', formControlName: 'metResolnFlgId', fieldType: 'M' },
      { attrName: 'Open Date Time', isVisible: false, attrType: 'M', formControlName: 'openDteTime', fieldType: 'M' },
      { attrName: 'Completed Date Time', isVisible: false, attrType: 'M', formControlName: 'completedDte', fieldType: 'M' },
      { attrName: 'Reopen Date Time', isVisible: false, attrType: 'M', formControlName: 'reopenDte', fieldType: 'M' },
      { attrName: 'Planned Start Date', isVisible: false, attrType: 'M', formControlName: 'plannedStrtDte', fieldType: 'M' },
      { attrName: 'Comments', isVisible: false, attrType: 'M', formControlName: 'comments', fieldType: 'M' },
      { attrName: 'Planned Effort', isVisible: false, attrType: 'M', formControlName: 'plannedEffort', fieldType: 'M' },
      { attrName: 'Planned End Date', isVisible: false, attrType: 'M', formControlName: 'plannedEndDte', fieldType: 'M' },
      { attrName: 'Release Date', isVisible: false, attrType: 'M', formControlName: 'releaseDte', fieldType: 'M' },
      { attrName: 'Ticket Summary', isVisible: false, attrType: 'M', formControlName: 'ticketSummary', fieldType: 'M' },
      { attrName: 'Is Partially Automated', isVisible: false, attrType: 'M', formControlName: 'isPartial', fieldType: 'M' },
      { attrName: 'Ticket Status', isVisible: false, attrType: 'O', formControlName: 'isPartial', fieldType: 'NA' },
      { attrName: 'Business Impact', isVisible: false, attrType: 'M', formControlName: 'AHBusinessImpact', fieldType: 'NA' },
      { attrName: 'Impact Comments', isVisible: false, attrType: 'M', formControlName: 'AHImpactComments', fieldType: 'NA' },
    ];

    this.ticketAttributesFG = this.fb.group({
      projectName: new FormControl(),
      appId: new FormControl(),
      appName: new FormControl(),
      towerId: new FormControl(),
      towerName: new FormControl(),
      ticketTask: new FormControl(),
      description: new FormControl(),
      ticketOpenDte: new FormControl(),
      openDteTime: new FormControl(),
      ticketCreatedDte: new FormControl(),
      actualStartDte: new FormControl(),
      actualEndDte: new FormControl(),
      closedDte: new FormControl(),
      openDte: new FormControl(),
      completedDte: new FormControl(),
      reopenDte: new FormControl(),
      plannedStrtDte: new FormControl(),
      plannedEndDte: new FormControl(),
      releaseDte: new FormControl(),
      priorityId: new FormControl(),
      ticketType: new FormControl(),
      causeCodeId: new FormControl(),
      resolutionCodeId: new FormControl(),
      debtClassificationId: new FormControl(),
      severityId: new FormControl(),
      ticketSourceId: new FormControl(),
      AHBusinessImpact: new FormControl(),
      AHImpactComments: new FormControl(),
      avoidableFlgId: new FormControl(),
      residualFlgId: new FormControl(),
      releaseTypeFlgId: new FormControl(),
      kedbUpFlgId: new FormControl(),
      kedbAvailFlgId: new FormControl(),
      metResFlgId: new FormControl(),
      metResolnFlgId: new FormControl(),
      kEDBPath: new FormControl(),
      isPartial: new FormControl(),
      assignedTo: new FormControl(),
      flexField1: new FormControl(),
      flexField2: new FormControl(),
      flexField3: new FormControl(),
      flexField4: new FormControl(),
      estimatedWorkSize: new FormControl(),
      actualEffort: new FormControl(),
      resolutionMethodName: new FormControl(),
      rcaId: new FormControl(),
      comments: new FormControl(),
      plannedEffort: new FormControl(),
      ticketSummary: new FormControl(),
      hdnIsAppEditable: new FormControl(),
      hdnDebtCLMode: new FormControl(),
      hdnTicketTypeId: new FormControl(),
      hdnServiceId: new FormControl(),
      hdnStatusName: new FormControl(),
      hdnDartStatusId: new FormControl(),
      hdnDartStatusName: new FormControl(),
      hdnTicketStatusId: new FormControl(),
      hdnTicketProjectId: new FormControl(),
      hdnSupportTypeId: new FormControl(),
      hdnRowNumber: new FormControl(),
      hdnIsAttributeUpdated: new FormControl(),
      hdnCurrentDate: new FormControl(),
      hdnIsDebtEnabled: new FormControl(),
      hdnProjectTimeZoneName: new FormControl(),
      hdnUserTimeZoneName: new FormControl(),
      hdnIsAHTagged: new FormControl(),
      hdnGracePeriod: new FormControl(),
      hdnIsGracePeriodMet: new FormControl(),
      hdnOptionalAttributeType: new FormControl(),
      hdnIsFlexField1Configured: new FormControl(),
      hdnIsFlexField2Configured: new FormControl(),
      hdnIsFlexField3Configured: new FormControl(),
      hdnIsFlexField4Configured: new FormControl(),
      hdnClosedDateProject: new FormControl(),
      hdnCompletedDateProject: new FormControl(),
      hdnIsResolutionReConfigured: new FormControl(),
      hdnAutoClassificationType: new FormControl(),
      hdnAssigneUser: new FormControl(),
      hdnIsTicketDescriptionOpted: new FormControl(),
      debtClassificationMode: new FormControl()
    });
    const curr = new Date();
    const last = curr.getDate();
    const lastday = new Date(curr.setDate(last)).toUTCString();
    this.actualStartMaxDate = new Date(lastday);
    this.actualEndMaxDate = new Date(lastday);
    this.closedMaxDate = new Date(lastday);
    this.complMaxDate = new Date(lastday);
    this.reopenMaxDate = new Date(lastday);
    this.pStrtMaxDate = new Date(lastday);
    this.releaseMaxDate = new Date(lastday);
    this.ticketdetails = this.SelectedTicketDetails;
    this.hdnTimeTickerId = this.ticketdetails.timeTickerId;
    this.closeStatus = this.dynamicgridComponent.closeStatus;
    this.statusNameLst = this.ticketdetails.lstStatusDetails.filter(x => x.ticketStatusId == this.ticketdetails.ticketStatusMapId);
    if (this.statusNameLst.length != 0 && this.statusNameLst != undefined) {
      this.statusName = this.statusNameLst[0].dartStatusName;
    }
    else {
      this.statusName = '';
    }
    this.headerService.masterDataEmitter.subscribe((masterData: MasterDataModel) => {
      if (masterData != null) {
        this.hdnCustomerId = masterData.hiddenFields.customerId;
        this.hdnEmployeeId = masterData.hiddenFields.employeeId;
        this.custTimeZoneName = masterData.hiddenFields.customerTimeZoneName;
        this.isCognizant=masterData.hiddenFields.isCognizant;
      }
    });
    this.ticketAttributesFG.controls['hdnTicketProjectId'].setValue(this.ticketdetails.projectId.toString());
    this.projectId = this.ticketAttributesFG.get('hdnTicketProjectId').value;
    this.spinner.show();
    this.bindPopUpAttributes(this.ticketdetails);
    this.columnControls =
      [{ controlName: 'description', columnName: 'TicketDescription' }, { controlName: 'causeCodeId', columnName: 'CauseCodeMapID' },
      { controlName: 'comments', columnName: 'Comments' }, { controlName: 'flexField1', columnName: 'FlexField1' },
      { controlName: 'flexField2', columnName: 'FlexField2' }, { controlName: 'flexField3', columnName: 'FlexField3' },
      { controlName: 'flexField4', columnName: 'FlexField4' }, { controlName: 'kedbAvailFlgId', columnName: 'KEDBAvailableIndicatorMapID' },
      { controlName: 'releaseTypeFlgId', columnName: 'ReleaseTypeMapID' }, { controlName: 'resolutionCodeId', columnName: 'ResolutionCodeMapID' },
      { controlName: 'resolutionMethodName', columnName: 'ResolutionRemarks' }, { controlName: 'ticketSourceId', columnName: 'TicketSourceMapID' },
      { controlName: 'ticketSummary', columnName: 'TicketSummary' }, { controlName: 'hdnTicketTypeId', columnName: 'TicketTypeMapID' },
      { controlName: '', columnName: 'RelatedTickets' }, { controlName: '', columnName: 'Category' }];
  }
  bindPopUpAttributes(ticketdetails) {
    var serviceId;
    var ticketTypeId;
    var debtEnabled;
    var isAttributeUpdated;
    if (ticketdetails.serviceId != null) {
      serviceId = ticketdetails.serviceId.toString();
    }
    else {
      serviceId = null;
    }
    if (ticketdetails.ticketTypeMapId != null) {
      ticketTypeId = ticketdetails.ticketTypeMapId.toString();
    }
    else {
      ticketTypeId = null;
    }
    if (ticketdetails.isDebtEnabled != null) {
      debtEnabled = ticketdetails.isDebtEnabled.toString();
    }
    else {
      debtEnabled = null;
    }
    if (ticketdetails.isMainspringConfigured != null) {
      this.mainspringEnabled = ticketdetails.isMainspringConfigured.toString();
    }
    else {
      this.mainspringEnabled = null;
    }
    if (ticketdetails.isAttributeUpdated != null) {
      isAttributeUpdated = ticketdetails.isAttributeUpdated.toString();
    }
    else {
      isAttributeUpdated = null;
    }
    var attributes = {
      ProjectId: Number(ticketdetails.projectId),
      TicketId: ticketdetails.ticketId,
      StatusId: Number(ticketdetails.ticketStatusMapId),
      ServiceId: serviceId,
      UserId: ticketdetails.assignedTo,
      CustomerTimeZone: this.custTimeZoneName,
      TicketTypeId: ticketTypeId,
      IsDebtEnabled: debtEnabled,
      StatusName: this.statusName,
      IsMainSpring: this.mainspringEnabled,
      IsAttributeUpdated: isAttributeUpdated == 'N' ? '0' : isAttributeUpdated,
      ProjectTimeZoneName: ticketdetails.projectTimeZoneName,
      UserTimeZoneName: ticketdetails.userTimeZoneName,
      IsAHTagged: ticketdetails.isAHTagged,
      GracePeriod: ticketdetails.gracePeriod,
      IsGracePeriodMet: ticketdetails.isGracePeriodMet,
      ClosedDate: ticketdetails.closedDate == '' ? null : ticketdetails.closedDate,
      CompletedDate: ticketdetails.completedDate == '' ? null : ticketdetails.completedDate,
      supportTypeID: ticketdetails.supportTypeId,
      CustomerId: parseInt(this.hdnCustomerId),
      EmployeeId: this.hdnEmployeeId
    };
    if (this.closeStatus == 'N') {
      this.ticketAttributesService.popupAttribute(attributes).subscribe(x => {
        this.attributesList.push(JSON.parse(x));
        this.popupAttributeModels = this.attributesList[0].PopupAttributeModels;
        if (this.attributesList.length != 0 && this.attributesList != undefined) {
          this.ticketAttributesFG.controls['hdnIsAppEditable'].setValue(this.attributesList[0].IsAppEditable);
          this.ticketAttributesFG.controls['hdnIsAttributeUpdated'].setValue(this.attributesList[0].IsAttributeUpdated);
          this.ticketAttributesFG.controls['hdnProjectTimeZoneName'].setValue(this.attributesList[0].ProjectTimeZoneName);
          this.ticketAttributesFG.controls['hdnUserTimeZoneName'].setValue(this.attributesList[0].UserTimeZoneName);
          this.ticketAttributesFG.controls['hdnIsTicketDescriptionOpted'].setValue(this.attributesList[0].IsTicketDescriptionOpted);
        }
        if (this.popupAttributeModels != undefined && this.popupAttributeModels.length != 0) {
          this.ticketAttributesFG.controls['hdnIsDebtEnabled'].setValue(this.popupAttributeModels[0].IsDebtEnabled);
          this.ticketAttributesFG.controls['hdnSupportTypeId'].setValue(this.popupAttributeModels[0].SupportTypeId);
          this.ticketAttributesFG.controls['hdnDebtCLMode'].setValue(this.popupAttributeModels[0].DebtClassificationMode.toString());
          this.ticketAttributesFG.controls['hdnAutoClassificationType'].setValue(this.popupAttributeModels[0].AutoClassificationType.toString());
          this.ticketAttributesFG.controls['hdnTicketTypeId'].setValue(this.popupAttributeModels[0].TicketTypeId);
          this.ticketAttributesFG.controls['hdnServiceId'].setValue(this.popupAttributeModels[0].ServiceId);
          this.ticketAttributesFG.controls['hdnStatusName'].setValue(this.popupAttributeModels[0].StatusName);
          this.ticketAttributesFG.controls['hdnDartStatusId'].setValue(this.popupAttributeModels[0].DARTStatusId);
          this.ticketAttributesFG.controls['hdnDartStatusName'].setValue(this.popupAttributeModels[0].DARTStatusName);
          this.ticketAttributesFG.controls['hdnTicketStatusId'].setValue(this.popupAttributeModels[0].TicketStatusId);
          this.ticketAttributesFG.controls['hdnTicketProjectId'].setValue(this.popupAttributeModels[0].ProjectId);
          this.ticketAttributesFG.controls['hdnIsAHTagged'].setValue(this.popupAttributeModels[0].IsAHTagged);
          this.ticketAttributesFG.controls['hdnGracePeriod'].setValue(this.popupAttributeModels[0].GracePeriod);
          this.ticketAttributesFG.controls['hdnIsGracePeriodMet'].setValue(this.popupAttributeModels[0].IsGracePeriodMet);
          this.ticketAttributesFG.controls['hdnOptionalAttributeType'].setValue(this.popupAttributeModels[0].OptionalAttributeType);
          this.ticketAttributesFG.controls['hdnIsFlexField1Configured'].setValue(this.popupAttributeModels[0].IsFlexField1Configured);
          this.ticketAttributesFG.controls['hdnIsFlexField2Configured'].setValue(this.popupAttributeModels[0].IsFlexField2Configured);
          this.ticketAttributesFG.controls['hdnIsFlexField3Configured'].setValue(this.popupAttributeModels[0].IsFlexField3Configured);
          this.ticketAttributesFG.controls['hdnIsFlexField4Configured'].setValue(this.popupAttributeModels[0].IsFlexField4Configured);
          this.ticketAttributesFG.controls['hdnClosedDateProject'].setValue(this.popupAttributeModels[0].ClosedDateProject);
          this.ticketAttributesFG.controls['hdnCompletedDateProject'].setValue(this.popupAttributeModels[0].CompletedDateProject);
          this.ticketAttributesFG.controls['hdnIsResolutionReConfigured'].setValue(this.popupAttributeModels[0].IsResolutionReConfigured);
          this.ticketAttributesFG.controls['hdnCurrentDate'].setValue(this.popupAttributeModels[0].CurrentDate);
          this.applicationList = this.getDropDownList(this.popupAttributeModels[0].ApplicationList, 'ApplicationId', 'ApplicationName', '');
          this.priorityList = this.getDropDownList(this.popupAttributeModels[0].LstPriority, 'PriorityId', 'PriorityName', '');
          this.causeCodeList = this.getDropDownList(this.popupAttributeModels[0].LstCause, 'CauseId', 'CauseName', 'CauseIsMapped');
          this.resolutionList = this.getDropDownList(this.popupAttributeModels[0].LstResolution, 'ResolutionId', 'ResolutionName', 'IsMapped');
          this.debtClassificationList = this.getDropDownList(this.popupAttributeModels[0].LstDebtClassification, 'DebtClassificationId', 'DebtClassificationName', '');
          this.severityList = this.getDropDownList(this.popupAttributeModels[0].LstSeverity, 'SeverityId', 'SeverityName', '');
          this.ticketSourceList = this.getDropDownList(this.popupAttributeModels[0].LstTicketSource, 'TicketSourceId', 'TicketSourceName', '');
          this.businessimpactlist = this.getDropDownList(this.popupAttributeModels[0].LstBusinessImpact, 'BusinessImpactId', 'BusinessImpactName', '');
          this.avoidableFlgList = this.popupAttributeModels[0].LstAvoidableFlagModel;
          this.residualDebtList = this.popupAttributeModels[0].LstResidualDebtModel;
          this.releaseTypeList = this.popupAttributeModels[0].LstReleaseType;
          this.kedbUpdatedList = this.popupAttributeModels[0].LstKEDBUpdated;
          this.kedbAvailableList = this.popupAttributeModels[0].LstKEDBAvailable;
          this.radioBtnValList = this.popupAttributeModels[0].LstMetSLA;
          this.ticketAttributesFG.controls['ticketTask'].setValue(this.popupAttributeModels[0].TicketId);
          this.ticketAttributesFG.controls['towerName'].setValue(this.popupAttributeModels[0].TowerName);
          this.ticketAttributesFG.controls['towerId'].setValue(this.popupAttributeModels[0].TowerId);
          this.ticketAttributesFG.controls['appName'].setValue(this.popupAttributeModels[0].ApplicationName);
          this.ticketAttributesFG.controls['appId'].setValue(this.getValue(parseInt(this.popupAttributeModels[0].ApplicationId)));
          this.ticketAttributesFG.controls['description'].setValue(this.popupAttributeModels[0].TicketDescription);
          this.ticketAttributesFG.controls['priorityId'].setValue(this.getValue(this.popupAttributeModels[0].PriotityId));
          this.ticketAttributesFG.controls['AHBusinessImpact'].setValue(this.getValue(this.popupAttributeModels[0].BusinessImpactId));
          this.ticketAttributesFG.controls['AHImpactComments'].setValue(this.getValue(this.popupAttributeModels[0].ImpactComments));
          this.ticketAttributesFG.controls['ticketType'].setValue(this.popupAttributeModels[0].TicketType);
          this.ticketAttributesFG.controls['kEDBPath'].setValue(this.popupAttributeModels[0].KEDBPath);
          this.ticketAttributesFG.controls['assignedTo'].setValue(this.popupAttributeModels[0].AssignedTo);
          this.ticketAttributesFG.controls['flexField1'].setValue(this.popupAttributeModels[0].FlexField1);
          this.ticketAttributesFG.controls['flexField2'].setValue(this.popupAttributeModels[0].FlexField2);
          this.ticketAttributesFG.controls['flexField3'].setValue(this.popupAttributeModels[0].FlexField3);
          this.ticketAttributesFG.controls['flexField4'].setValue(this.popupAttributeModels[0].FlexField4);
          this.ticketAttributesFG.controls['estimatedWorkSize'].setValue(this.popupAttributeModels[0].EstimatedWorkSize);
          this.ticketAttributesFG.controls['actualEffort'].setValue(parseFloat(this.popupAttributeModels[0].ActualEffort).toFixed(2));
          this.ticketAttributesFG.controls['plannedEffort'].setValue(parseFloat(this.popupAttributeModels[0].PlannedEffort).toFixed(2));
          this.ticketAttributesFG.controls['resolutionMethodName'].setValue(this.popupAttributeModels[0].ResolutionMethodName);
          this.ticketAttributesFG.controls['comments'].setValue(this.popupAttributeModels[0].Comments);
          this.ticketAttributesFG.controls['ticketSummary'].setValue(this.popupAttributeModels[0].TicketSummary);
          this.ticketAttributesFG.controls['causeCodeId'].setValue(this.getValue(this.popupAttributeModels[0].CauseCodeId));
          this.ticketAttributesFG.controls['resolutionCodeId'].setValue(this.getValue(this.popupAttributeModels[0].ResolutionCodeId));
          this.ticketAttributesFG.controls['debtClassificationId'].setValue(this.getValue(this.popupAttributeModels[0].DebtClassificationId));
          this.ticketAttributesFG.controls['severityId'].setValue(this.getValue(this.popupAttributeModels[0].SeverityId));
          this.ticketAttributesFG.controls['ticketSourceId'].setValue(this.getValue(this.popupAttributeModels[0].TicketSourceId));
          this.ticketAttributesFG.controls['avoidableFlgId'].setValue(this.getValue(this.popupAttributeModels[0].AvoidableFlag));
          this.ticketAttributesFG.controls['residualFlgId'].setValue(this.getValue(this.popupAttributeModels[0].ResidualDebtId));
          this.ticketAttributesFG.controls['releaseTypeFlgId'].setValue(this.popupAttributeModels[0].ReleaseTypeId);
          this.ticketAttributesFG.controls['kedbUpFlgId'].setValue(this.popupAttributeModels[0].KedbUpdateId);
          this.ticketAttributesFG.controls['kedbAvailFlgId'].setValue(this.popupAttributeModels[0].KedbAvailableId);
          this.ticketAttributesFG.controls['metResFlgId'].setValue(this.getValue(this.popupAttributeModels[0].MetResponseSLAId));
          this.ticketAttributesFG.controls['metResolnFlgId'].setValue(this.getValue(this.popupAttributeModels[0].MetResolutionId));
          this.ticketAttributesFG.controls['isPartial'].setValue(this.getValue(this.popupAttributeModels[0].IsPartiallyAutomated.toString()));
          this.ticketAttributesFG.controls['rcaId'].setValue(this.popupAttributeModels[0].RCAId);
          this.ticketAttributesFG.controls['ticketOpenDte'].setValue(this.setDateText(this.popupAttributeModels[0].TicketOpenDate));
          this.ticketAttributesFG.controls['openDteTime'].setValue(this.setDateText(this.popupAttributeModels[0].TicketOpenDate));
          this.ticketAttributesFG.controls['ticketCreatedDte'].setValue(this.setDateText(this.popupAttributeModels[0].TicketCreatedDate));
          this.ticketAttributesFG.controls['actualStartDte'].setValue(this.setDateText(this.popupAttributeModels[0].ActualStartDateTime));
          this.ticketAttributesFG.controls['actualEndDte'].setValue(this.setDateText(this.popupAttributeModels[0].ActualEndtDateTime));
          this.ticketAttributesFG.controls['openDte'].setValue(this.setDateText(this.popupAttributeModels[0].OpenDateTime));
          this.ticketAttributesFG.controls['closedDte'].setValue(this.setDateText(this.popupAttributeModels[0].ClosedDate));
          this.ticketAttributesFG.controls['completedDte'].setValue(this.setDateText(this.popupAttributeModels[0].CompletedDateTime));
          this.ticketAttributesFG.controls['reopenDte'].setValue(this.setDateText(this.popupAttributeModels[0].ReopenDateTime));
          this.ticketAttributesFG.controls['plannedStrtDte'].setValue(this.setDateText(this.popupAttributeModels[0].PlannedStartDate));
          this.ticketAttributesFG.controls['plannedEndDte'].setValue(this.setDateText(this.popupAttributeModels[0].PlannedEndDate));
          this.ticketAttributesFG.controls['releaseDte'].setValue(this.setDateText(this.popupAttributeModels[0].ReleaseDate));
        }
        this.actualOldStartDateTime = this.ticketAttributesFG.get('actualStartDte').value;
        this.actualOldStartDateTime = this.ticketAttributesFG.get('actualEndDte').value;
        this.closedOldDte = this.ticketAttributesFG.get('closedDte').value;
        this.completedOldDateTime = this.ticketAttributesFG.get('completedDte').value;
        this.plannedOldStartDate = this.ticketAttributesFG.get('plannedStrtDte').value;
        this.reopenOldDateTime = this.ticketAttributesFG.get('reopenDte').value;
        this.releaseOldDate = this.ticketAttributesFG.get('releaseDte').value;

        if (isNaN(this.ticketAttributesFG.get('plannedEffort').value) == true) {
          this.ticketAttributesFG.controls['plannedEffort'].setValue('');
        }
        else if (isNaN(this.ticketAttributesFG.get('estimatedWorkSize').value) == true) {
          this.ticketAttributesFG.controls['estimatedWorkSize'].setValue('');
        }
        if (this.ticketAttributesFG.get('assignedTo').value != null && this.ticketAttributesFG.get('assignedTo').value != '') {
          this.disableAssignedTo = true;
        }
        else {
          this.disableAssignedTo = false;
        }
        if (this.applicationList.length == 1) {
          this.ticketAttributesFG.controls['appId'].setValue(this.applicationList[0].value);
        }
        if (this.priorityList.length == 1) {
          this.ticketAttributesFG.controls['priorityId'].setValue(this.priorityList[0].value);
        }
        if (this.causeCodeList.length == 1) {
          this.ticketAttributesFG.controls['causeCodeId'].setValue(this.causeCodeList[0].value);
        }
        if (this.resolutionList.length == 1) {
          this.ticketAttributesFG.controls['resolutionCodeId'].setValue(this.resolutionList[0].value);
        }
        if (this.debtClassificationList.length == 1) {
          this.ticketAttributesFG.controls['debtClassificationId'].setValue(this.debtClassificationList[0].value);
        }
        if (this.severityList.length == 1) {
          this.ticketAttributesFG.controls['severityId'].setValue(this.severityList[0].value);
        }
        if (this.ticketSourceList.length == 1) {
          this.ticketAttributesFG.controls['ticketSourceId'].setValue(this.ticketSourceList[0].value);
        }
        if (this.ticketAttributesFG.get('hdnIsAppEditable').value == false) {
          this.displayAppTxtbox = true;
          this.displayAppDropdown = false;
        }
        else if (this.ticketAttributesFG.get('hdnIsGracePeriodMet').value == false) {
          this.displayAppDropdown = true;
          this.disableAppDropDown = false;
          this.ticketAttributesFG.controls['appId'].enable();
          this.displayAppTxtbox = false;
        }
        else {
          this.displayAppTxtbox = false;
          this.displayAppDropdown = true;
          this.disableAppDropDown = true;
          this.ticketAttributesFG.controls['appId'].disable();
        }
        this.getAlgoKeyAndColumn(Number(ticketdetails.projectId), Number(this.ticketAttributesFG.controls['hdnSupportTypeId'].value));
      });
    }
  }
  getAlgoKeyAndColumn(projectId: number, supportTypeID: number) {
    this.ticketAttributesService.getAlgoKey(projectId, supportTypeID).subscribe(x => {
      this.algoKey = x.algoKey;
      this.getProjectName();
      if (this.algoKey == 'AL002') {
        if(this.popupAttributeModels[0].DebtClassificationMode.toString() == null || this.popupAttributeModels[0].DebtClassificationMode.toString() == "0"
        || this.popupAttributeModels[0].DebtClassificationMode.toString() == "1" || this.popupAttributeModels[0].DebtClassificationMode.toString() == "3")
        {
           this.transactionId = x.transactionId;
           this.columnList = x.columnList;
           this.paramForNewAlgo();
        }
      }
    });
  }
  newAlgoClassification(newParam) {
    let isAHTagged = this.ticketAttributesFG.get('hdnIsAHTagged').value;
    let isGracePeriodMet = this.ticketAttributesFG.get('hdnIsGracePeriodMet').value;
    if (isAHTagged != true && isGracePeriodMet != true) {
      const supportTypeId = Number(this.ticketAttributesFG.controls['hdnSupportTypeId'].value);
      const ticketId = this.ticketAttributesFG.get('ticketTask').value;
      let towerappId: string;
      let assignGrp = '';
      let jsonParams: string;
      let appName: string;
      const isDebtEnabled = this.ticketAttributesFG.get('hdnIsDebtEnabled').value;
      const dartStatusId = this.ticketAttributesFG.get('hdnDartStatusId').value;
      if (isDebtEnabled == 'Y' || isDebtEnabled == '1') {
        if (dartStatusId == '9' || dartStatusId == '8') {
          if (this.columnList.includes('AssignmentGroupID')) {
            assignGrp = `,"AssignmentGroupID":${this.SelectedTicketDetails.assignmentGroupId}`;
          }
          if (supportTypeId == 1) {
            appName = this.ticketAttributesFG.get('appName').value;
            towerappId = this.ticketAttributesFG.get('appId').value;
            jsonParams = `{"TicketId":"${ticketId}","ProjectId":${this.projectId},"ApplicationId":${towerappId},"TransactionId":${this.transactionId}${assignGrp}${newParam}}`;
          }
          else if (supportTypeId == 2) {
            appName = null;
            towerappId = this.ticketAttributesFG.get('towerId').value;
            jsonParams = `{"TicketId":"${ticketId}","ProjectId":${this.projectId},"TowerId":${towerappId},"TransactionId":${this.transactionId}${assignGrp}${newParam}}`;
          }
          const causeCode = Number(this.ticketAttributesFG.get('causeCodeId').value);
          const resolutionCode = Number(this.ticketAttributesFG.get('resolutionCodeId').value);
          const resolutionMethodName = this.ticketAttributesFG.get('resolutionMethodName').value;
          const comments = this.ticketAttributesFG.get('comments').value;
          const ticketSummary = this.ticketAttributesFG.get('ticketSummary').value;
          const ticketDescription = this.ticketAttributesFG.get('description').value;
          const serviceId = this.ticketAttributesFG.get('hdnServiceId').value;
          const ticketTypeId = this.ticketAttributesFG.get('hdnTicketTypeId').value;
          const userId = this.hdnEmployeeId;
          const autoClassificationType = this.ticketAttributesFG.get('hdnAutoClassificationType').value;

          const paramData = {
            CauseCode: causeCode,
            TicketDescription: ticketDescription,
            ResolutionCode: resolutionCode,
            ProjectId: this.projectId,
            Application: appName,
            ServiceId: serviceId,
            TicketTypeId: ticketTypeId,
            TimeTickerId: this.hdnTimeTickerId.toString(),
            UserId: userId,
            ApplicationId: supportTypeId == 1 ? Number(towerappId) : 0,
            ResolutionMethodName: resolutionMethodName,
            Comments: comments,
            TicketSummary: ticketSummary,
            SupportTypeId: supportTypeId,
            TowerId: supportTypeId == 2 ? Number(towerappId) : 0,
            AutoClassificationType: Number(autoClassificationType),
            classificationType: (causeCode != 0 || resolutionCode != 0) ? 1 : 2,
            jsonParam: jsonParams
          };
          if (Number(causeCode) != 0) {
            this.causeCodeMappingChange(Number(causeCode), this.projectId, Number(resolutionCode));
          }
          if(this.isCognizant!='0'){
          this.ticketAttributesService.newAlgoClassification(paramData).subscribe(result => {
            if (result != null) {
              let responseObject = JSON.parse(result);
              if (responseObject.DebtClassificationId != null || responseObject.DebtClassificationId != undefined) {
                this.ticketAttributesFG.controls['debtClassificationId'].setValue(responseObject.DebtClassificationId);
                this.displayDCEdit = responseObject.DebtClassificationId != '0';
                this.debtClassificationEnableDisable(responseObject.DebtClassificationId != '0', false);
              }
              if (responseObject.AvoidableFlag != null || responseObject.AvoidableFlag != undefined) {
                this.ticketAttributesFG.controls['avoidableFlgId'].setValue(responseObject.AvoidableFlag);
              }
              if (responseObject.ResidualDebt != null || responseObject.ResidualDebt != undefined) {
                this.ticketAttributesFG.controls['residualFlgId'].setValue(responseObject.ResidualDebt);
              }
            }
          });
        }
        }
      }
    }
  }
  newAlgoCheck(attributeName) {
    if (this.algoKey.includes('AL002') && (this.columnList.includes(attributeName) || attributeName.includes('Cause') || attributeName.includes('Resolution'))) {
      this.paramForNewAlgo();
    }
  }

  paramForNewAlgo() {
    let addParam = '';
    this.columnList.forEach(x => {
      let key = this.columnControls.find(y => y.columnName === x)?.controlName;
      if (key === '') {
        addParam = `${addParam},"${x}":""`;
      }
      else if (key !== null && key !== undefined) {
        let attrValue = this.ticketAttributesFG.controls[key].value;
        attrValue = attrValue == null ? '' : attrValue;
        addParam = `${addParam},"${x}":"${attrValue}"`;
      }
      else {
        //Mandatory else block
      }
    });
    this.newAlgoClassification(addParam);
  }

  setDateText(date) {
    if (new Date(date).getFullYear() == 1900 || date == "") {
      return "";
    }
    else {
      let attrDate = new Date(date);
      return attrDate;
    }
  }
  getDropDownList(projectDropList, id, name, attrNme): any {
    var lstlength = projectDropList.length;
    var masterDropDown = [];
    var FinalLst = [];
    for (var i = 0; i < lstlength; i++) {
      var labelList = projectDropList[i];
      var list = {
        "label": labelList[name], "value": labelList[id],
        "attrValue": labelList[attrNme], "disabled": false
      };
      masterDropDown.push(list);
    }
    if (id == 'ResolutionId' || id == 'resolutionId' || id == 'DebtClassificationId'
      || id == 'SeverityId') {
      masterDropDown.splice(0, 1);
    }
    if (id == 'CauseId' || id == 'ResolutionId' || id == 'resolutionId') {
      var trueLst = masterDropDown.filter(x => x.attrValue == 'True');
      var falseLst = masterDropDown.filter(x => x.attrValue != 'True');
      if (trueLst.length > 0) {
        var separator = { "label": '---------------------', "value": '', "attrValue": 'True', "disabled": true };
        FinalLst.push(...trueLst);
        FinalLst.push(separator);
      }
      FinalLst.push(...falseLst);
      return FinalLst;
    }
    else {
      return masterDropDown;
    }
  }
  getProjectName() {
    var params = {
      ProjectID: this.projectId
    };
    this.analystselfserviceService.getProjectName(params).subscribe(x => {
      this.ticketAttributesFG.controls['projectName'].setValue(x);
      if (this.ticketAttributesFG.get('causeCodeId').value != '' && this.ticketAttributesFG.get('causeCodeId').value != null) {
        this.causeCodeMappingChange(this.ticketAttributesFG.get('causeCodeId').value, this.projectId, this.ticketAttributesFG.get('causeCodeId').value);
      }
      this.bindTicketAttributesByProject(this.ticketdetails);
    });
  }
  bindTicketAttributesByProject(ticketdetails) {
    var dartStatusId = this.ticketAttributesFG.get('hdnDartStatusId').value;
    var ticketTypeId;
    if (this.ticketAttributesFG.get('hdnDebtCLMode').value == '1') {
      this.disableDebtFields = true;
      this.ticketAttributesFG.controls['debtClassificationId'].disable();
      this.ticketAttributesFG.controls['avoidableFlgId'].disable();
      this.ticketAttributesFG.controls['residualFlgId'].disable();
      if (this.ticketAttributesFG.get('hdnAutoClassificationType').value == '1') {
        this.displayDCEdit = true;
      }
      else if (this.ticketAttributesFG.get('hdnAutoClassificationType').value == '2') {
        if (this.algoKey === 'AL001') {
          this.disableCCRC = true;
          this.ticketAttributesFG.controls['causeCodeId'].disable();
          this.ticketAttributesFG.controls['resolutionCodeId'].disable();
          this.displayCCEdit = true;
        }
        else {
          this.displayDCEdit = true;
        }
      }
      else if (this.algoKey === 'AL002') {
        this.disableCCRC = true;
        this.ticketAttributesFG.controls['causeCodeId'].disable();
        this.ticketAttributesFG.controls['resolutionCodeId'].disable();
        this.displayDCEdit = true;
      }
    }
    else if (this.ticketAttributesFG.get('hdnDebtCLMode').value == '3') {
      this.disableDebtFields = false;
      this.ticketAttributesFG.controls['debtClassificationId'].disable();
      this.ticketAttributesFG.controls['avoidableFlgId'].disable();
      this.ticketAttributesFG.controls['residualFlgId'].disable();
      this.displayDCEdit = true;
    }
    else {
      this.disableCCRC = false;
      this.disableDebtFields = false;
      this.ticketAttributesFG.controls['causeCodeId'].enable();
      this.ticketAttributesFG.controls['resolutionCodeId'].enable();
      this.ticketAttributesFG.controls['debtClassificationId'].enable();
      this.ticketAttributesFG.controls['avoidableFlgId'].enable();
      this.ticketAttributesFG.controls['residualFlgId'].enable();
    }
    if (this.ticketdetails.isCognizant == '1') {
      ticketTypeId = 0;
    }
    else {
      ticketTypeId = Number(this.ticketdetails.ticketTypeMapId);
    }
    var inputParam = {
      ProjectId: Number(this.projectId),
      ServiceId: Number(this.ticketdetails.serviceId),
      DARTStatusId: Number(this.ticketdetails.dartStatusId),
      TicketStatusID: Number(this.ticketdetails.ticketStatusMapId),
      IsAttributeUpdated: this.ticketdetails.isAttributeUpdated.toString(),
      IsCognizant: this.ticketdetails.isCognizant,
      TicketTypeId: ticketTypeId,
      SupportTypeId: Number(this.ticketdetails.supportTypeId)
    };
    this.ticketAttributesService.getAttributeDetails(inputParam).subscribe(x => {
      this.ticketAttributesList = x;
      if (this.ticketAttributesList.length != 0 && this.ticketAttributesList.length != 0) {
        for (var i = 0; i < this.ticketAttributesList.length; i++) {
          this.lstEnableAttributes[i] = this.attrList.filter(a => a.attrName === this.ticketAttributesList[i].attributeName)[0];
          this.lstEnableAttributes[i].isVisible = true;
          this.lstEnableAttributes[i].attrType = this.ticketAttributesList[i].attributeType;
        }
        if (this.lstEnableAttributes.filter(a => a.attrName == 'Ticket Status').length == 1) {
          this.lstEnableAttributes.filter(a => a.attrName == 'Ticket Status')[0].attrType = 'O';
        }
        if (this.SelectedTicketDetails.isAHTicket == '1' && (dartStatusId == 8 || dartStatusId == 9)) {
          this.attrList[45].isVisible = true;
          this.attrList[46].isVisible = true;
        }

      }
      if (this.lstEnableAttributes.filter(b => b.attrName == 'Ticket Description').length == 2) {
        this.ticketDesc = 'M';
        this.lstEnableAttributes.filter(b => b.attrName == 'Ticket Description').splice(1, 1);
      }
      else if (this.lstEnableAttributes.filter(b => b.attrName == 'Ticket Description').length > 0) {
        this.ticketDesc = 'M';
      }
      else {
        this.ticketDesc = 'N';
      }
      this.ticketAttributesFG.controls['ticketOpenDte'].disable();
      if (this.lstEnableAttributes.filter(b => b.attrName == 'Open Date Time').length == 1) {
        this.ticketAttributesFG.controls['openDteTime'].disable();
      }
      if (this.lstEnableAttributes.filter(b => b.attrName == 'Ticket Create Date').length == 1) {
        this.ticketAttributesFG.controls['ticketCreatedDte'].disable();
      }
      this.debtAttr = this.lstEnableAttributes.filter(a => a.fieldType == 'D');
      for (var j = 0; j < this.debtAttr.length; j++) {
        this.ticketAttributesFG.get(this.debtAttr[j].formControlName).setValidators([Validators.required]);
        this.ticketAttributesFG.get(this.debtAttr[j].formControlName).updateValueAndValidity();
      }
      this.mandateAttr = this.lstEnableAttributes.filter(a => a.fieldType == 'M');
      for (var k = 0; k < this.mandateAttr.length; k++) {
        this.ticketAttributesFG.get(this.mandateAttr[k].formControlName).setValidators([Validators.required]);
        this.ticketAttributesFG.get(this.mandateAttr[k].formControlName).updateValueAndValidity();
      }
      this.mandateAttributeList = this.ticketAttributesList.filter(b => b.attributeType == 'M');
      this.debtAttributeList = this.ticketAttributesList.filter(b => b.attributeType == 'D');
      if (this.debtAttributeList.length == 0 && this.mandateAttributeList.length == 0) {
        this.displayDebtAccordion = false;
        this.displayMandteAccordion = false;
      }
      else if (this.debtAttributeList.length != 0 && this.mandateAttributeList.length == 0) {
        this.displayDebtAccordion = true;
        this.displayMandteAccordion = false;
      }
      else if (this.debtAttributeList.length == 0 && this.mandateAttributeList.length != 0) {
        this.displayDebtAccordion = false;
        this.displayMandteAccordion = true;
      }
      else {
        this.displayDebtAccordion = true;
        this.displayMandteAccordion = true;
      }
      if (this.lstEnableAttributes.filter(a => a.attrName == 'Is Partially Automated').length > 0) {
        this.isPartialMarquee = true;
        this.isTickAttrMarquee = false;
      }
      else {
        this.isPartialMarquee = false;
        this.isTickAttrMarquee = true;
      }
    });

    this.freezeDebtFieldsByGracePeriod(ticketdetails);
    var TicketTypeId = this.ticketAttributesFG.get('ticketType').value;
    if (TicketTypeId == 'A' || TicketTypeId == 'H') {
      this.disableDescription = false;
    }
    this.spinner.hide();
  }
  causeCodeMappingChange(causeCode, projectId, resolutionCode) {
    var args = {
      ProjectId: projectId,
      CauseCode: causeCode.toString()
    };
    this.ticketAttributesService.getResolutionPriority(args).subscribe(x => {
      if (x != null && x.length != 0 && x != undefined) {
        this.resolutionList = this.getDropDownList(x, 'resolutionId', 'resolutionName', 'isMapped');
      }
    });
  }
  causeCodeOnChange(classificationType) {
    if (this.algoKey.includes('AL001')) {
      var isAHTagged = this.ticketAttributesFG.get('hdnIsAHTagged').value;
      var isGracePeriodMet = this.ticketAttributesFG.get('hdnIsGracePeriodMet').value;
      if (isAHTagged != true && isGracePeriodMet != true) {
        var supportTypeId = this.ticketAttributesFG.get('hdnSupportTypeId').value;
        var causeCode = this.ticketAttributesFG.get('causeCodeId').value;
        var resolutionCode = this.ticketAttributesFG.get('resolutionCodeId').value;
        var projectId = this.projectId;
        this.autoClassList = [];
        if (supportTypeId != 0) {
          var isDebtEnabled = this.ticketAttributesFG.get('hdnIsDebtEnabled').value;
          if (isDebtEnabled == 'Y' || isDebtEnabled == '1') {
            var appNme;
            var applicationId;
            var projTowerId;
            if (supportTypeId == 1) {
              appNme = this.ticketAttributesFG.get('appName').value;
              applicationId = this.ticketAttributesFG.get('appId').value;
              projTowerId = 0;
            }
            else if (supportTypeId == 2) {
              appNme = null;
              applicationId = 0;
              projTowerId = this.ticketAttributesFG.get('towerId').value;
            }
            var resolutionMethodName = this.ticketAttributesFG.get('resolutionMethodName').value;
            var comments = this.ticketAttributesFG.get('comments').value;
            var ticketSummary = this.ticketAttributesFG.get('ticketSummary').value;
            var ticketDescription = this.ticketAttributesFG.get('description').value;
            var dartStatusId = this.ticketAttributesFG.get('hdnDartStatusId').value;
            var serviceId = this.ticketAttributesFG.get('hdnServiceId').value;
            var ticketTypeId = this.ticketAttributesFG.get('hdnTicketTypeId').value;
            var userId = this.hdnEmployeeId;
            var autoClassificationType = this.ticketAttributesFG.get('hdnAutoClassificationType').value;
            var timeTickerId = this.hdnTimeTickerId;
            if ((Number(causeCode) != 0 && Number(resolutionCode) != 0 && classificationType == 1 && !(supportTypeId == 2 && Number(autoClassificationType) == 2))
              || (Number(autoClassificationType) == 2 && ticketDescription != '' && classificationType == 2)) {
              if (dartStatusId == '9' || dartStatusId == '8') {
                this.displayCCEdit = false;
                this.displayDCEdit = false;
                var paramData = {
                  CauseCode: Number(causeCode),
                  TicketDescription: ticketDescription,
                  ResolutionCode: Number(resolutionCode),
                  ProjectId: projectId,
                  Application: appNme,
                  ServiceId: serviceId,
                  TicketTypeId: ticketTypeId,
                  TimeTickerId: timeTickerId.toString(),
                  UserId: userId,
                  ApplicationId: Number(applicationId),
                  ResolutionMethodName: resolutionMethodName,
                  Comments: comments,
                  TicketSummary: ticketSummary,
                  SupportTypeId: supportTypeId,
                  TowerId: Number(projTowerId),
                  AutoClassificationType: Number(autoClassificationType),
                  classificationType: classificationType
                };
                this.spinner.show();
                this.ticketAttributesService.getCauseCodeResolutionCode(paramData).subscribe(x => {
                  this.autoClassList.push(JSON.parse(x));
                  if (this.autoClassList[0].DebtClassificationId != null || this.autoClassList[0].DebtClassificationId) {
                    if (this.autoClassList[0].DebtClassificationId != '0') {
                      if (classificationType == 1) {
                        this.displayDCEdit = true;
                      }
                      else if (this.ticketAttributesFG.get('hdnAutoClassificationType').value == '2' && classificationType == 2) {
                        this.displayCCEdit = true;
                      }
                    }
                    this.debtClassificationEnableDisable(this.autoClassList[0].DebtClassificationId != '0', classificationType == 2);
                    this.ticketAttributesFG.controls['debtClassificationId'].setValue(this.autoClassList[0].DebtClassificationId);
                  }
                  if (this.autoClassList[0].AvoidableFlag != null || this.autoClassList[0].AvoidableFlag != undefined) {
                    this.ticketAttributesFG.controls['avoidableFlgId'].setValue(this.autoClassList[0].AvoidableFlag);
                  }
                  if (this.autoClassList[0].ResidualDebt != null || this.autoClassList[0].ResidualDebt != undefined) {
                    this.ticketAttributesFG.controls['residualFlgId'].setValue(this.autoClassList[0].ResidualDebt);
                  }
                  if ((this.autoClassList[0].CauseCodeId != null || this.autoClassList[0].CauseCodeId != undefined) && this.autoClassList[0].CauseCodeId != '0') {
                    this.ticketAttributesFG.controls['causeCodeId'].setValue(this.autoClassList[0].CauseCodeId);
                  }
                  if ((this.autoClassList[0].ResolutionCodeId != null || this.autoClassList[0].ResolutionCodeId != undefined) && this.autoClassList[0].ResolutionCodeId != '0') {
                    this.ticketAttributesFG.controls['resolutionCodeId'].setValue(this.autoClassList[0].ResolutionCodeId);
                    this.causeCodeMappingChange(this.autoClassList[0].CauseCodeId, projectId, this.autoClassList[0].ResolutionCodeId);
                  }
                  this.spinner.hide();
                });
              }
            }
            else {
              this.spinner.show();
              if (((Number(autoClassificationType) == 1 || Number(autoClassificationType) == 0) &&
                classificationType == 1 && !(supportTypeId == 2 && Number(autoClassificationType) == 2))
                || (Number(autoClassificationType) == 2 && classificationType == 2)) {
                this.debtClassificationEnableDisable(false, true);
              }
              if (Number(causeCode) != 0) {
                this.causeCodeMappingChange(Number(causeCode), projectId, Number(resolutionCode));
              }
              this.spinner.hide();
            }
          }
        }
      }
    }
    else {
      this.paramForNewAlgo();
    }
  }
  changeDebtClassificationMode() {
    this.debtClassificationEnableDisable(false, true);
  }
  debtClassificationEnableDisable(isEnable, isClassify) {
    if (isEnable == true) {
      this.disableDebtFields = true;
      this.ticketAttributesFG.controls['debtClassificationId'].disable();
      this.ticketAttributesFG.controls['avoidableFlgId'].disable();
      this.ticketAttributesFG.controls['residualFlgId'].disable();
    }
    else {
      this.disableDebtFields = false;
      this.ticketAttributesFG.controls['debtClassificationId'].enable();
      this.ticketAttributesFG.controls['avoidableFlgId'].enable();
      this.ticketAttributesFG.controls['residualFlgId'].enable();
    }
    if (this.ticketAttributesFG.get('hdnAutoClassificationType').value == '2' && isClassify) {
      if (isEnable == true) {
        this.disableCCRC = true;
        this.ticketAttributesFG.controls['causeCodeId'].disable();
        this.ticketAttributesFG.controls['resolutionCodeId'].disable();
      }
      else {
        this.disableCCRC = false;
        this.ticketAttributesFG.controls['causeCodeId'].enable();
        this.ticketAttributesFG.controls['resolutionCodeId'].enable();
      }
    }
  }
  freezeDebtFieldsByGracePeriod(ticketdetails) {
    this.gracePeriodTitle = '';
    this.descriptionTitle = '';
    this.closedDteTitle = '';
    this.completedDteTitle = '';
    this.resolnTitle = '';
    this.flex1Title = '';
    this.flex2Title = '';
    this.flex3Title = '';
    this.flex4Title = '';
    this.isPartialTitle = '';
    var isAHTagged = this.ticketAttributesFG.get('hdnIsAHTagged').value;
    var isGracePeriodMet = this.ticketAttributesFG.get('hdnIsGracePeriodMet').value;
    var optionalAttributeType = this.ticketAttributesFG.get('hdnOptionalAttributeType').value;
    var isFlexField1Configured = this.ticketAttributesFG.get('hdnIsFlexField1Configured').value;
    var isFlexField2Configured = this.ticketAttributesFG.get('hdnIsFlexField2Configured').value;
    var isFlexField3Configured = this.ticketAttributesFG.get('hdnIsFlexField3Configured').value;
    var isFlexField4Configured = this.ticketAttributesFG.get('hdnIsFlexField4Configured').value;
    var closedDateProject = this.ticketAttributesFG.get('hdnClosedDateProject').value;
    var completedDateProject = this.ticketAttributesFG.get('hdnCompletedDateProject').value;
    if (isGracePeriodMet == true || isAHTagged == true) {
      this.disableCCRC = true;
      this.disableDebtFields = true;
        this.ticketAttributesFG.controls['causeCodeId'].disable();
        this.ticketAttributesFG.controls['resolutionCodeId'].disable();
      this.ticketAttributesFG.controls['debtClassificationId'].disable();
      this.ticketAttributesFG.controls['avoidableFlgId'].disable();
      this.ticketAttributesFG.controls['residualFlgId'].disable();
      this.gracePeriodTitle = Constants.GracePeriodMessage;
      if (this.ticketAttributesFG.get('hdnDartStatusId').value == 8) {
        this.ticketAttributesFG.controls['closedDte'].disable();
        this.ticketAttributesFG.controls['completedDte'].enable();
        this.closedDteTitle = Constants.GracePeriodMessage;
      }
      else {
        this.ticketAttributesFG.controls['closedDte'].enable();
        this.ticketAttributesFG.controls['completedDte'].disable();
        this.completedDteTitle = Constants.GracePeriodMessage;
      }
      if (optionalAttributeType == 2 || optionalAttributeType == 3) {
        this.disableDescription = true;
        this.descriptionTitle = Constants.GracePeriodMessage;
        if (this.ticketAttributesFG.get('hdnIsResolutionReConfigured').value == true) {
          this.disableResolnRemrks = true;
          this.resolnTitle = Constants.GracePeriodMessage;
        }
      }
      else if (optionalAttributeType == 1 || optionalAttributeType == 3) {
        if (isFlexField1Configured == true) {
          this.disableFlex1 = true;
          this.flex1Title = Constants.GracePeriodMessage;
        }
        if (isFlexField2Configured == true) {
          this.disableFlex2 = true;
          this.flex2Title = Constants.GracePeriodMessage;
        }
        if (isFlexField3Configured == true) {
          this.disableFlex3 = true;
          this.flex3Title = Constants.GracePeriodMessage;
        }
        if (isFlexField4Configured == true) {
          this.disableFlex4 = true;
          this.flex4Title = Constants.GracePeriodMessage;
        }
      }
      var freezeData = {
        ticketDetails: ticketdetails,
        closedDateProject: closedDateProject,
        completedDateProject: completedDateProject
      };
      if (isGracePeriodMet == true || isGracePeriodMet == 1) {
        this.freezeServiceStatus.emit(freezeData);
        this.ticketAttributesFG.controls['isPartial'].disable();
        this.isPartialTitle = Constants.GracePeriodMessage;
      }
      else {
        this.ticketAttributesFG.controls['isPartial'].enable();
      }
    }
    else {
      this.ticketAttributesFG.controls['isPartial'].enable();
    }
    if (this.closeStatus == 'Y') {
      this.dynamicgridComponent.closeAttributesModal(ticketdetails);
      this.spinner.hide();
    }
    this.expandTickAttr = true;
    this.expandDebtAttr = false;
    this.expandMandteAttr = false;
  }
  onDropDownChange($event, attr) {
    this.ticketAttributesFG.get(attr).setValidators([Validators.required]);
    this.ticketAttributesFG.get(attr).updateValueAndValidity();
    if (this.algoKey.includes('AL002')) {
      if (attr.includes('cause')) {
        this.newAlgoCheck('CauseCodeMapID');
      }
      else if (attr.includes('resolution')) {
        this.newAlgoCheck('ResolutionCodeMapID');
      }
      else if (attr.includes('ticket')) {
        this.newAlgoCheck('TicketSourceMapID');
      }
      else {
        //Mandatory else block
      }
    }
    else {
      if (attr == 'causeCodeId' || attr == 'resolutionCodeId') {
       if(this.isCognizant!='0'){ //Single AC
        this.causeCodeOnChange(1);
       }
      }
    }
  }
  btnAttrSave(isToClose) {
    this.clearWarningMessages();
    this.closeStatus = isToClose;
    var supportTypeId = this.ticketAttributesFG.get('hdnSupportTypeId').value;
    var isTicketDescription = this.ticketAttributesFG.get('description').value;
    if (supportTypeId == 2 && isTicketDescription == '') {
      this.ticketAttributesFG.get('description').setValidators([Validators.required]);
      this.ticketAttributesFG.get('description').updateValueAndValidity();
      this.isDescriptionErr = true;
      this.expandTickAttr = true;
      this.expandDebtAttr = false;
      this.expandMandteAttr = false;
      this.isTicketAttrMandateMsg = true;
      setTimeout(() => { this.isTicketAttrMandateMsg = false; }, this.timeoutValue);
    }
    else {
      this.validateDateAttributes('save');
    }
  }
  btnAttrSubmitClick() {
    this.validateDateAttributes('submit');
  }
  validateDateAttributes(mode) {

    for (var j = 0; j < this.mandateAttr.length; j++) {
      this.ticketAttributesFG.get(this.mandateAttr[j].formControlName).clearValidators();
      this.ticketAttributesFG.get(this.mandateAttr[j].formControlName).updateValueAndValidity();
    }
    this.clearWarningMessages();
    this.isValidDate = true;
    this.isSaveOrSubmit = mode;
    var userTimeZoneName;
    var now = new Date(this.ticketAttributesFG.get('hdnCurrentDate').value);
    userTimeZoneName = this.ticketAttributesFG.get('hdnUserTimeZoneName').value;
    var projectTimeZoneName = this.ticketAttributesFG.get('hdnProjectTimeZoneName').value;
    if (userTimeZoneName == '') {
      userTimeZoneName = projectTimeZoneName;
    }
    var params = {
      UserTimeZone: userTimeZoneName
    };
    this.analystselfserviceService.getCurrentTimeofTimeZones(params).subscribe(x => {
      this.currentTime = x;
      now = new Date(this.currentTime);
      if (this.ticketAttributesFG.get('openDteTime').value != null && this.ticketAttributesFG.get('openDteTime').value != ''
        && this.ticketAttributesFG.get('completedDte').value != null && this.ticketAttributesFG.get('completedDte').value != ''
        && this.ticketAttributesFG.get('completedDte').value.getFullYear().toString() != '0001'
        && (this.ticketAttributesFG.get('completedDte').value < this.ticketAttributesFG.get('openDteTime').value)) {
        this.ticketAttributesFG.get('completedDte').setValidators([Validators.required]);
        this.ticketAttributesFG.get('completedDte').updateValueAndValidity();
        this.ticketAttributesFG.get('completedDte').setErrors({ 'invalid': true });
        this.isCompletedOpnDteErr = true;
        this.isCompletedDteErr = true;
        this.isValidDate = false;
      }
      if (((this.ticketAttributesFG.get('completedDte').value != null && this.ticketAttributesFG.get('completedDte').value != ''
        && this.ticketAttributesFG.get('reopenDte').value != null && this.ticketAttributesFG.get('reopenDte').value != ''
        && (this.ticketAttributesFG.get('completedDte').value < this.ticketAttributesFG.get('reopenDte').value))
        || this.ticketAttributesFG.get('completedDte').value > now)
        && this.ticketAttributesFG.get('completedDte').value.getFullYear().toString() != '0001'
        && this.ticketAttributesFG.get('completedDte').value != null && this.ticketAttributesFG.get('completedDte').value != ''
        && this.ticketAttributesFG.get('reopenDte').value.getFullYear().toString() != '0001'
        && this.ticketAttributesFG.get('reopenDte').value != null && this.ticketAttributesFG.get('reopenDte').value != '') {
        this.ticketAttributesFG.get('completedDte').setValidators([Validators.required]);
        this.ticketAttributesFG.get('completedDte').updateValueAndValidity();
        this.ticketAttributesFG.get('completedDte').setErrors({ 'invalid': true });
        this.isCompletedReopnDteErr = true;
        this.isCompletedDteErr = true;
        this.isValidDate = false;
      }
      if (this.ticketAttributesFG.get('completedDte').value != null && this.ticketAttributesFG.get('completedDte').value != ''
        && this.ticketAttributesFG.get('closedDte').value != null && this.ticketAttributesFG.get('closedDte').value != ''
        && this.ticketAttributesFG.get('completedDte').value.getFullYear().toString() != '0001'
        && this.ticketAttributesFG.get('closedDte').value.getFullYear().toString() != '0001'
        && (this.ticketAttributesFG.get('closedDte').value < this.ticketAttributesFG.get('completedDte').value)) {
        this.ticketAttributesFG.get('closedDte').setValidators([Validators.required]);
        this.ticketAttributesFG.get('closedDte').updateValueAndValidity();
        this.ticketAttributesFG.get('closedDte').setErrors({ 'invalid': true });
        this.isClosedCompleteDteErr = true;
        this.isClosedDteErr = true;
        this.isValidDate = false;
      }
      if (((this.ticketAttributesFG.get('closedDte').value != null && this.ticketAttributesFG.get('closedDte').value != ''
        && this.ticketAttributesFG.get('reopenDte').value != null && this.ticketAttributesFG.get('reopenDte').value != ''
        && (this.ticketAttributesFG.get('closedDte').value < this.ticketAttributesFG.get('reopenDte').value))
        || this.ticketAttributesFG.get('completedDte').value > now)
        && this.ticketAttributesFG.get('closedDte').value.getFullYear().toString() != '0001'
        && this.ticketAttributesFG.get('closedDte').value != null && this.ticketAttributesFG.get('closedDte').value != ''
        && this.ticketAttributesFG.get('reopenDte').value.getFullYear().toString() != '0001'
        && this.ticketAttributesFG.get('reopenDte').value != null && this.ticketAttributesFG.get('reopenDte').value != '') {
        this.ticketAttributesFG.get('closedDte').setValidators([Validators.required]);
        this.ticketAttributesFG.get('closedDte').updateValueAndValidity();
        this.ticketAttributesFG.get('closedDte').setErrors({ 'invalid': true });
        this.isClosedReopnDteErr = true;
        this.isClosedDteErr = true;
        this.isValidDate = false;
      }
      if (((this.ticketAttributesFG.get('openDteTime').value != null && this.ticketAttributesFG.get('openDteTime').value != ''
        && this.ticketAttributesFG.get('closedDte').value != '' && this.ticketAttributesFG.get('closedDte').value != null
        && (this.ticketAttributesFG.get('closedDte').value < this.ticketAttributesFG.get('openDteTime').value))
        || this.ticketAttributesFG.get('closedDte').value > now)
        && this.ticketAttributesFG.get('closedDte').value.getFullYear().toString() != '0001'
        && this.ticketAttributesFG.get('closedDte').value != null && this.ticketAttributesFG.get('closedDte').value != '') {
        this.ticketAttributesFG.get('closedDte').setValidators([Validators.required]);
        this.ticketAttributesFG.get('closedDte').updateValueAndValidity();
        this.ticketAttributesFG.get('closedDte').setErrors({ 'invalid': true });
        this.isClosedOpenDteErr = true;
        this.isClosedDteErr = true;
        this.isValidDate = false;
      }
      if (this.ticketAttributesFG.get('openDteTime').value != null && this.ticketAttributesFG.get('openDteTime').value != ''
        && this.ticketAttributesFG.get('actualEndDte').value != null && this.ticketAttributesFG.get('actualEndDte').value != ''
        && this.ticketAttributesFG.get('actualEndDte').value.getFullYear().toString() != '0001'
        && (this.ticketAttributesFG.get('actualEndDte').value < this.ticketAttributesFG.get('openDteTime').value)) {
        this.ticketAttributesFG.get('actualEndDte').setValidators([Validators.required]);
        this.ticketAttributesFG.get('actualEndDte').updateValueAndValidity();
        this.ticketAttributesFG.get('actualEndDte').setErrors({ 'invalid': true });
        this.isActualEndDteErr = true;
        this.isActualDteErr = true;
        this.isValidDate = false;
      }
      if (this.ticketAttributesFG.get('actualStartDte').value != null && this.ticketAttributesFG.get('actualStartDte').value != ''
        && this.ticketAttributesFG.get('actualEndDte').value != null && this.ticketAttributesFG.get('actualEndDte').value != ''
        && this.ticketAttributesFG.get('actualStartDte').value.getFullYear().toString() != '0001'
        && this.ticketAttributesFG.get('actualEndDte').value.getFullYear().toString() != '0001'
        && (this.ticketAttributesFG.get('actualEndDte').value < this.ticketAttributesFG.get('actualStartDte').value)) {
        this.ticketAttributesFG.get('actualEndDte').setValidators([Validators.required]);
        this.ticketAttributesFG.get('actualEndDte').updateValueAndValidity();
        this.ticketAttributesFG.get('actualEndDte').setErrors({ 'invalid': true });
        this.isActualEndDteClsdErr = true;
        this.isActualDteErr = true;
        this.isValidDate = false;
      }
      if (this.ticketAttributesFG.get('actualEndDte').value != null && this.ticketAttributesFG.get('actualEndDte').value != ''
        && this.ticketAttributesFG.get('closedDte').value != null && this.ticketAttributesFG.get('closedDte').value != ''
        && this.ticketAttributesFG.get('actualEndDte').value.getFullYear().toString() != '0001'
        && this.ticketAttributesFG.get('closedDte').value.getFullYear().toString() != '0001'
        && (this.ticketAttributesFG.get('actualEndDte').value > this.ticketAttributesFG.get('closedDte').value)) {
        this.ticketAttributesFG.get('actualEndDte').setValidators([Validators.required]);
        this.ticketAttributesFG.get('actualEndDte').updateValueAndValidity();
        this.ticketAttributesFG.get('actualEndDte').setErrors({ 'invalid': true });
        this.isActualEndClsDteErr = true;
        this.isActualDteErr = true;
        this.isValidDate = false;
      }
      if (this.ticketAttributesFG.get('openDteTime').value != null && this.ticketAttributesFG.get('openDteTime').value != ''
        && this.ticketAttributesFG.get('reopenDte').value != null && this.ticketAttributesFG.get('reopenDte').value != ''
        && this.ticketAttributesFG.get('reopenDte').value.getFullYear().toString() != '0001'
        && (this.ticketAttributesFG.get('reopenDte').value < this.ticketAttributesFG.get('openDteTime').value)) {
        this.ticketAttributesFG.get('reopenDte').setValidators([Validators.required]);
        this.ticketAttributesFG.get('reopenDte').updateValueAndValidity();
        this.ticketAttributesFG.get('reopenDte').setErrors({ 'invalid': true });
        this.isReopenDteErr = true;
        this.isValidDate = false;
      }
      if (this.ticketAttributesFG.get('plannedStrtDte').value != null && this.ticketAttributesFG.get('plannedStrtDte').value != ''
        && this.ticketAttributesFG.get('plannedEndDte').value != null && this.ticketAttributesFG.get('plannedEndDte').value != ''
        && this.ticketAttributesFG.get('plannedStrtDte').value.getFullYear().toString() != '0001'
        && this.ticketAttributesFG.get('plannedEndDte').value.getFullYear().toString() != '0001'
        && (this.ticketAttributesFG.get('plannedEndDte').value < this.ticketAttributesFG.get('plannedStrtDte').value)) {
        this.ticketAttributesFG.get('plannedEndDte').setValidators([Validators.required]);
        this.ticketAttributesFG.get('plannedEndDte').updateValueAndValidity();
        this.ticketAttributesFG.get('plannedEndDte').setErrors({ 'invalid': true });
        this.isPlannedEndDteErr = true;
        this.isValidDate = false;
      }
      if (this.isValidDate == false) {
        this.isFieldMandate = true;
        this.isTicketAttrMandateMsg = true;
        setTimeout(() => { this.isTicketAttrMandateMsg = false; }, this.timeoutValue);
      }
      else {
        this.validateAllFields(mode);
      }
      return false;
    });
  }
  validateAllFields(mode) {
    var isBusinessImpact = this.ticketAttributesFG.get('AHBusinessImpact').value;
    var isImpactComments = this.ticketAttributesFG.get('AHImpactComments').value != null ? this.ticketAttributesFG.get('AHImpactComments').value.trim() : '';
    var dartStatusId = this.ticketAttributesFG.get('hdnDartStatusId').value;
    this.isSaveOrSubmit = mode;
    var isMandateValidated = true;
    var isDebtValidated = true;
    if (this.isSaveOrSubmit == 'save') {
      for (var i = 0; i < this.debtAttr.length; i++) {
        this.ticketAttributesFG.get(this.debtAttr[i].formControlName).clearValidators();
        this.ticketAttributesFG.get(this.debtAttr[i].formControlName).updateValueAndValidity();
        if (this.ticketAttributesFG.get(this.debtAttr[i].formControlName).value == null ||
          this.ticketAttributesFG.get(this.debtAttr[i].formControlName).value == '') {
          isDebtValidated = false;
        }
      }
      for (var j = 0; j < this.mandateAttr.length; j++) {
        this.ticketAttributesFG.get(this.mandateAttr[j].formControlName).clearValidators();
        this.ticketAttributesFG.get(this.mandateAttr[j].formControlName).updateValueAndValidity();
        if (this.mandateAttr[j].formControlName != 'actualEffort'
          && (this.ticketAttributesFG.get(this.mandateAttr[j].formControlName).value == null ||
            this.ticketAttributesFG.get(this.mandateAttr[j].formControlName).value == '' ||
            this.ticketAttributesFG.get(this.mandateAttr[j].formControlName).value <= 0 ||
            this.ticketAttributesFG.get(this.mandateAttr[j].formControlName).value == '0.0' ||
            this.ticketAttributesFG.get(this.mandateAttr[j].formControlName).value == '0.00' ||
            this.ticketAttributesFG.get(this.mandateAttr[j].formControlName).value == '0')
        ) {
          isMandateValidated = false;
        }
      }
      if ((this.SelectedTicketDetails.isAHTicket == "1" && (dartStatusId == 8 ||
        dartStatusId == 9)) && (isBusinessImpact == null || isBusinessImpact == 0)) {
        isMandateValidated = false;
      }
      if ((this.SelectedTicketDetails.isAHTicket == "1" && (dartStatusId == 8 ||
        dartStatusId == 9)) && (isImpactComments == null || isImpactComments == "")) {
        isMandateValidated = false;
      }
      var assignee = true;
      if (this.disableAssignedTo == false && (this.ticketAttributesFG.get('assignedTo').value == ""
        || this.ticketAttributesFG.get('assignedTo').value == null)) {
        this.ticketAttributesFG.get('assignedTo').setValidators([Validators.required]);
        this.ticketAttributesFG.get('assignedTo').updateValueAndValidity();
        this.invalidAssignedto = true;
        assignee = false;
      }
      if (isMandateValidated == true && isDebtValidated == true && this.invalidAssignedto == false) {
        this.saveData();
      }
      else {
        this.ticketAttributesFG.controls["hdnIsAttributeUpdated"].setValue('N');
        if (assignee) {
          this.validatedAttributes = true;
          this.saveData();
        }
        else {
          this.validatedAttributes = false;
          this.isTicketAttrMandateMsg = true;
        }
      }
    }
    else if (this.isSaveOrSubmit == "submit") {
      for (var k = 0; k < this.debtAttr.length; k++) {
        if (this.ticketAttributesFG.get(this.debtAttr[k].formControlName).value == "0") {
          this.ticketAttributesFG.get(this.debtAttr[k].formControlName).setValue(null);
        }
        this.ticketAttributesFG.get(this.debtAttr[k].formControlName).setValidators([Validators.required]);
        this.ticketAttributesFG.get(this.debtAttr[k].formControlName).updateValueAndValidity();
        if (this.ticketAttributesFG.get(this.debtAttr[k].formControlName).value == null) {
          isDebtValidated = false;
        }
      }
      if (isDebtValidated == false && (this.disableCCRC != true || this.disableDebtFields != true)) {
        this.isTicketAttrMandateMsg = true;
        this.isDebtErr = true;
        this.isDebtMandate = true;
        this.expandTickAttr = false;
        this.expandDebtAttr = true;
        this.expandMandteAttr = false;
        setTimeout(() => { this.isTicketAttrMandateMsg = false; }, this.timeoutValue);
      }
      else {
        for (var l = 0; l < this.mandateAttr.length; l++) {
          this.ticketAttributesFG.get(this.mandateAttr[l].formControlName).setValidators([Validators.required]);
          this.ticketAttributesFG.get(this.mandateAttr[l].formControlName).updateValueAndValidity();
          if ((this.mandateAttr[l].formControlName != "actualEffort" && this.mandateAttr[l].formControlName != "plannedEffort"
            && this.mandateAttr[l].formControlName != "estimatedWorkSize" && this.mandateAttr[l].formControlName != "releaseTypeFlgId"
            && this.mandateAttr[l].formControlName != "kedbAvailFlgId"
            && this.mandateAttr[l].formControlName != "kedbUpFlgId") &&
            (this.ticketAttributesFG.get(this.mandateAttr[l].formControlName).value == null ||
              this.ticketAttributesFG.get(this.mandateAttr[l].formControlName).value == "0" ||
              this.ticketAttributesFG.get(this.mandateAttr[l].formControlName).value == "")) {
            isMandateValidated = false;
          }
          else if ((this.mandateAttr[l].formControlName == "plannedEffort" || this.mandateAttr[l].formControlName == "estimatedWorkSize"
            || this.mandateAttr[l].formControlName == "releaseTypeFlgId" || this.mandateAttr[l].formControlName == "kedbAvailFlgId"
            || this.mandateAttr[l].formControlName == "kedbUpFlgId")
            && (this.ticketAttributesFG.get(this.mandateAttr[l].formControlName).value <= 0 ||
              this.ticketAttributesFG.get(this.mandateAttr[l].formControlName).value == "0.0" ||
              this.ticketAttributesFG.get(this.mandateAttr[l].formControlName).value == "0.00" ||
              this.ticketAttributesFG.get(this.mandateAttr[l].formControlName).value == "0")) {
            this.ticketAttributesFG.get(this.mandateAttr[l].formControlName).setValidators([Validators.required, Validators.min(0.02)]);
            this.ticketAttributesFG.get(this.mandateAttr[l].formControlName).updateValueAndValidity();
            isMandateValidated = false;
          }
        }
        var SupportTypeId = this.ticketAttributesFG.get('hdnSupportTypeId').value;
        var ticketDescription = this.ticketAttributesFG.get('description').value;
        var mlDescription = this.ticketAttributesFG.get('hdnIsTicketDescriptionOpted').value;
        var isBusinessImpact = this.ticketAttributesFG.get('AHBusinessImpact').value;
        var isImpactComments = this.ticketAttributesFG.get('AHImpactComments').value != null ? this.ticketAttributesFG.get('AHImpactComments').value.trim() : "";
        var dartStatusId = this.ticketAttributesFG.get('hdnDartStatusId').value;
        if (SupportTypeId == 2 && ticketDescription == "") {
          this.isFieldMandate = true;
          this.ticketAttributesFG.get('description').setValidators([Validators.required]);
          this.ticketAttributesFG.get('description').updateValueAndValidity();
          this.isDescriptionErr = true;
          this.isTicketAttrMandateMsg = true;
          setTimeout(() => { this.isTicketAttrMandateMsg = false; }, this.timeoutValue);
        }
        else if ((this.SelectedTicketDetails.isAHTicket == "1" && (dartStatusId == 8 ||
          dartStatusId == 9)) && (isBusinessImpact == null || isBusinessImpact == 0)) {
          this.ticketAttributesFG.get('AHBusinessImpact').setValidators([Validators.required]);
          this.ticketAttributesFG.get('AHBusinessImpact').updateValueAndValidity();
          this.mndteWarningBusinessShow = true;
          this.isFieldMandate = true;
          this.isAllfieldsCaptured = true;
          this.isTicketAttrMandateMsg = true;
          this.expandTickAttr = false;
          this.expandDebtAttr = false;
          this.expandMandteAttr = true;
          if ((isImpactComments == null || isImpactComments == "")) {
            this.ticketAttributesFG.get('AHImpactComments').setValidators([Validators.required]);
            this.ticketAttributesFG.get('AHImpactComments').updateValueAndValidity();
            this.ticketAttributesFG.controls["AHImpactComments"].setValue('');
            this.mndteWarningShow = true;
          }
          setTimeout(() => { this.isTicketAttrMandateMsg = false; }, this.timeoutValue);
        }
        else if ((this.SelectedTicketDetails.isAHTicket == "1" && (dartStatusId == 8 ||
          dartStatusId == 9)) && (isImpactComments == null || isImpactComments == "")) {
          this.ticketAttributesFG.controls["AHImpactComments"].setValue('');
          this.ticketAttributesFG.get('AHImpactComments').setValidators([Validators.required]);
          this.ticketAttributesFG.get('AHImpactComments').updateValueAndValidity();
          this.mndteWarningShow = true;
          this.isFieldMandate = true;
          this.isAllfieldsCaptured = true;
          this.isTicketAttrMandateMsg = true;
          this.expandTickAttr = false;
          this.expandDebtAttr = false;
          this.expandMandteAttr = true;

          setTimeout(() => { this.isTicketAttrMandateMsg = false; }, this.timeoutValue);
        }
        else if (isMandateValidated == true) {
          if (this.ticketDesc == "M" && mlDescription == true
            && (this.ticketAttributesFG.get('description').value == "" || this.ticketAttributesFG.get('description').value == null)) {
            this.ticketAttributesFG.get('description').setValidators([Validators.required]);
            this.ticketAttributesFG.get('description').updateValueAndValidity();
            this.isDescriptionErr = true;
            this.expandTickAttr = true;
            this.expandDebtAttr = false;
            this.expandMandteAttr = false;
            this.isTicketAttrMandateMsg = true;
            setTimeout(() => { this.isTicketAttrMandateMsg = false; }, this.timeoutValue);
          }
          else {
            this.saveData();
          }
        }
        else {
          this.isFieldMandate = true;
          this.isAllfieldsCaptured = true;
          this.isTicketAttrMandateMsg = true;
          this.expandTickAttr = false;
          this.expandDebtAttr = false;
          this.expandMandteAttr = true;
          setTimeout(() => { this.isTicketAttrMandateMsg = false; }, this.timeoutValue);
        }
      }
    }
  }
  saveData() {
    if (this.isTicketAttrMandateMsg != true) {
      var projectId = this.ticketAttributesFG.get('hdnTicketProjectId').value;
      var ticketId = this.ticketAttributesFG.get('ticketTask').value;
      var priorityId = this.ticketAttributesFG.get('priorityId').value;
      var causeCodeId = this.ticketAttributesFG.get('causeCodeId').value;
      var appId = this.ticketAttributesFG.get('appId').value;
      var resolutionCodeId = this.ticketAttributesFG.get('resolutionCodeId').value;
      var debtClassificationId = this.ticketAttributesFG.get('debtClassificationId').value;
      var avoidableFlgId = this.ticketAttributesFG.get('avoidableFlgId').value;
      var residualDebtId = this.ticketAttributesFG.get('residualFlgId').value;
      var severityId = this.ticketAttributesFG.get('severityId').value;
      var ticketSourceId = this.ticketAttributesFG.get('ticketSourceId').value;
      var kedbPath = this.ticketAttributesFG.get('kEDBPath').value;
      var flexField1 = this.ticketAttributesFG.get('flexField1').value;
      var flexField2 = this.ticketAttributesFG.get('flexField2').value;
      var flexField3 = this.ticketAttributesFG.get('flexField3').value;
      var flexField4 = this.ticketAttributesFG.get('flexField4').value;
      var releaseTypeFlgId = this.ticketAttributesFG.get('releaseTypeFlgId').value;
      var kedbUpFlgId = this.ticketAttributesFG.get('kedbUpFlgId').value;
      var resolutionMethodName = this.ticketAttributesFG.get('resolutionMethodName').value;
      var kedbAvailFlgId = this.ticketAttributesFG.get('kedbAvailFlgId').value;
      var rcaId = this.ticketAttributesFG.get('rcaId').value;
      var metResFlgId = this.ticketAttributesFG.get('metResFlgId').value;
      var metResolnFlgId = this.ticketAttributesFG.get('metResolnFlgId').value;
      var comments = this.ticketAttributesFG.get('comments').value;
      var ticketSummary = this.ticketAttributesFG.get('ticketSummary').value;
      var isPartial = this.ticketAttributesFG.get('isPartial').value;
      var ticketCreatedDte = this.datePipe.transform(this.ticketAttributesFG.get('ticketCreatedDte').value, 'yyyy-MM-ddTHH:mm:ss');
      var actualStartDte = this.datePipe.transform(this.ticketAttributesFG.get('actualStartDte').value, 'yyyy-MM-ddTHH:mm:ss');
      var actualEndDte = this.datePipe.transform(this.ticketAttributesFG.get('actualEndDte').value, 'yyyy-MM-ddTHH:mm:ss');
      var closedDte = this.datePipe.transform(this.ticketAttributesFG.get('closedDte').value, 'yyyy-MM-ddTHH:mm:ss');
      var ticketOpenDte = this.datePipe.transform(this.ticketAttributesFG.get('openDteTime').value, 'yyyy-MM-ddTHH:mm:ss');
      var completedDte = this.datePipe.transform(this.ticketAttributesFG.get('completedDte').value, 'yyyy-MM-ddTHH:mm:ss');
      var reopenDte = this.datePipe.transform(this.ticketAttributesFG.get('reopenDte').value, 'yyyy-MM-ddTHH:mm:ss');
      var plannedStrtDte = this.datePipe.transform(this.ticketAttributesFG.get('plannedStrtDte').value, 'yyyy-MM-ddTHH:mm:ss');
      var plannedEndDte = this.datePipe.transform(this.ticketAttributesFG.get('plannedEndDte').value, 'yyyy-MM-ddTHH:mm:ss');
      var releaseDte = this.datePipe.transform(this.ticketAttributesFG.get('releaseDte').value, 'yyyy-MM-ddTHH:mm:ss');
      var estimatedWorkSize = this.ticketAttributesFG.get('estimatedWorkSize').value;
      var actualEffort = this.ticketAttributesFG.get('actualEffort').value;
      var plannedEffort = this.ticketAttributesFG.get('plannedEffort').value;
      var isAttributeUpdated = this.ticketAttributesFG.get('hdnIsAttributeUpdated').value;
      var ticketStatusId = this.ticketAttributesFG.get('hdnTicketStatusId').value;
      var userTimeZone = this.ticketAttributesFG.get('hdnUserTimeZoneName').value;
      var projectTimeZone = this.ticketAttributesFG.get('hdnProjectTimeZoneName').value;
      var supportTypeId = this.ticketAttributesFG.get('hdnSupportTypeId').value;
      var ticketTypeId = this.ticketAttributesFG.get('hdnTicketTypeId').value;
      this.mode = "Ticket";
      var AHBusinessImpact = this.ticketAttributesFG.get('AHBusinessImpact').value;
      var AHImpactComments = this.ticketAttributesFG.get('AHImpactComments').value != null ? this.ticketAttributesFG.get('AHImpactComments').value.trim() : "";
      var userId;
      if (this.disableAssignedTo == true) {
        userId = this.popupAttributeModels[0].AssignedUserID;
      }
      else {
        userId = this.ticketAttributesFG.get('assignedTo').value.employeeId;
      }
      if (this.ticketAttributesFG.get('hdnSupportTypeId').value == 2) {
        appId = 0;
      }
      var insertAttrParams = {
        ProjectId: Number(projectId),
        UserId: userId,
        TicketId: ticketId,
        Application: Number(appId),
        Priority: Number(priorityId),
        TicketDescription: this.ticketAttributesFG.get('description').value,
        CauseCode: Number(causeCodeId),
        ResolutionCode: Number(resolutionCodeId),
        DebtClassificationId: Number(debtClassificationId),
        AvoidalFlagId: Number(avoidableFlgId),
        ResidualDebtId: Number(residualDebtId),
        Severity: Number(severityId),
        Source: Number(ticketSourceId),
        KEDBPath: kedbPath,
        ReleaseType: Number(releaseTypeFlgId),
        KEDBUpdated: Number(kedbUpFlgId),
        TicketSummary: ticketSummary,
        ResolutionMethod: resolutionMethodName,
        KEDBAvailableIndicator: Number(kedbAvailFlgId),
        RCAID: rcaId,
        MetResponseSLA: metResFlgId,
        MetResolution: metResolnFlgId,
        TicketOpenDate: ticketOpenDte,
        TicketCreateDate: ticketCreatedDte,
        ActualStartDateTime: actualStartDte,
        ActualEndDateTime: actualEndDte,
        CloseDate: closedDte,
        CompletedDateTime: completedDte,
        ReopenDate: reopenDte,
        PlannedStartDateAndTime: plannedStrtDte,
        PlannedEndDate: plannedEndDte,
        ReleaseDate: releaseDte,
        Comments: comments,
        IsPartiallyAutomated: Number(isPartial),
        AHBusinessImpact: Number(AHBusinessImpact),
        AHImpactComments: AHImpactComments,
        FlexField1: flexField1,
        FlexField2: flexField2,
        FlexField3: flexField3,
        FlexField4: flexField4,
        EstimatedWorkSize: estimatedWorkSize == "" ? 0.00 : Number(estimatedWorkSize),
        ActualEffort: actualEffort == "" ? 0.00 : Number(actualEffort),
        PlannedEffort: plannedEffort == "" ? 0.00 : Number(plannedEffort),
        TicketTypeId: ticketTypeId,
        IsAttributeUpdated: isAttributeUpdated,
        TicketStatusID: ticketStatusId,
        UserTimeZone: userTimeZone,
        ProjectTimeZone: projectTimeZone,
        SupportTypeID: supportTypeId,
      };
      this.saveAttrList.push(insertAttrParams);
      var params = {
        InsertAttributeList: this.saveAttrList,
        UserID: this.hdnEmployeeId,
        AvoidalFlagId: avoidableFlgId,
        ResidualDebtId: Number(residualDebtId),
        IsAttributeUpdated: isAttributeUpdated,
        TicketStatusID: ticketStatusId,
        UserTimeZone: userTimeZone,
        ProjectTimeZone: projectTimeZone,
        SupportTypeID: supportTypeId,
      };
      this.spinner.show();
      this.ticketAttributesService.insertAttributeDetails(params).subscribe(x => {
        if(x == "EncryptionFailed")
        {
          this.isTicketAttrMandateMsg = true;
          this.showEncryptMessage = true;
          this.spinner.hide();
        } 
        else
        {
          if (x != undefined && x != "") {
            this.actualSDteTriggerCount = 0;
            this.actualEDteTriggerCount = 0;
            this.closedTriggerCount = 0;
            this.completedTriggerCount = 0;
            this.reopenDateTriggerCount = 0;
            this.plannedSDteTriggerCount = 0;
            this.releaseDteTriggerCount = 0;
            if (x == false) {
              this.isTranslationFailure = true;
              setTimeout(() => { this.isSuccess = false; }, this.timeoutValue);
            }
            else {
              if (this.ticketAttributesFG.get('hdnIsAttributeUpdated').value == "N") {
                this.ticketAttributesFG.controls["hdnIsAttributeUpdated"].setValue('0');
              }
              this.isSuccess = true;
              setTimeout(() => { this.isSuccess = false; }, this.timeoutValue);
            }
            this.expandTickAttr = true;
            this.expandDebtAttr = false;
            this.expandMandteAttr = false;
            this.updateTd();
            var data = {
              ticketDetails: this.ticketdetails,
              serviceId: this.ticketdetails.serviceId,
              ticketStatusMapId: this.ticketdetails.ticketStatusMapId,
              closeDte: this.ticketAttributesFG.get('closedDte').value,
              completeDte: this.ticketAttributesFG.get('completedDte').value,
              mode: this.mode,
              opnAttr: true,
              closeStatus: this.closeStatus
            };
            this.getUpdatedStatus.emit(data);
            this.spinner.hide();
          }
          else {
            this.isChangesDone = true;
          }
        }
      });
    }
  }

  updateTd() {
    this.ticketdetails.isAttributeUpdated = this.ticketAttributesFG.get('hdnIsAttributeUpdated').value;
    this.ticketdetails.ticketStatusMapId = this.ticketAttributesFG.get('hdnTicketStatusId').value;
    this.ticketdetails.userTimeZoneName = this.ticketAttributesFG.get('hdnUserTimeZoneName').value;
    this.ticketdetails.projectTimeZoneName = this.ticketAttributesFG.get('hdnProjectTimeZoneName').value;
    this.ticketdetails.supportTypeId = this.ticketAttributesFG.get('hdnSupportTypeId').value;
    this.ticketdetails.isAHTagged = this.ticketAttributesFG.get('hdnIsAHTagged').value;
    this.ticketdetails.gracePeriod = this.ticketAttributesFG.get('hdnGracePeriod').value;
    this.ticketdetails.isGracePeriodMet = this.ticketAttributesFG.get('hdnIsGracePeriodMet').value;
    this.ticketdetails.serviceId = this.ticketAttributesFG.get('hdnServiceId').value;
    this.ticketdetails.ticketTypeMapId = this.ticketAttributesFG.get('hdnTicketTypeId').value;
    this.ticketdetails.dartStatusId = this.ticketAttributesFG.get('hdnDartStatusId').value;
    this.ticketdetails.isDebtEnabled = this.ticketAttributesFG.get('hdnIsDebtEnabled').value;
    this.ticketdetails.ticketId = this.ticketAttributesFG.get('ticketTask').value;
    this.ticketdetails.applicationId = this.ticketAttributesFG.get('appId').value;
    this.ticketdetails.towerId = this.ticketAttributesFG.get('towerId').value;
    this.ticketdetails.ticketDescription = this.ticketAttributesFG.get('description').value;
    this.ticketdetails.closedDate = this.ticketAttributesFG.get('closedDte').value;
    this.ticketdetails.completedDate = this.ticketAttributesFG.get('completedDte').value;
    return false;
  }
  checkIsGracePeriodMet(tickDetails) {
    var clseDte;
    var completedDte;
    if (tickDetails.closedDate == "") {
      clseDte = null;
    }
    else {
      clseDte = tickDetails.closedDate;
    }
    if (tickDetails.completedDate == "") {
      completedDte = null;
    }
    else {
      completedDte = tickDetails.completedDate;
    }
    var argsGracePeriod = {
      ClosedDate: clseDte,
      CompletedDate: completedDte,
      GracePeriod: tickDetails.gracePeriod,
      ProjectTimeZoneName: tickDetails.projectTimeZoneName,
      UserTimeZoneName: tickDetails.userTimeZoneName,
      IsGracePeriodMet: tickDetails.isGracePeriodMet,
      DARTStatusID: Number(tickDetails.dartStatusId)
    };
    if (tickDetails.isGracePeriodMet != true && tickDetails.isAHTagged != true && (clseDte != null || completedDte != null)) {
      this.ticketAttributesService.checkIsGracePeriodMet(argsGracePeriod).subscribe(y => {
        if (y != 0 && this.mode == "Main") {
          tickDetails.isGracePeriodMet = (y.isGracePeriodMet);
          if (y.closedDate != null) {
            tickDetails.closedDate = y.closedDate;
          }
          if (y.completedDate != null) {
            tickDetails.completedDate = y.completedDate;
          }
          if (y.isGracePeriodMet == true) {
            var freezeData = {
              ticketDetails: tickDetails,
              closedDateProject: tickDetails.closedDate,
              completedDateProject: tickDetails.completedDate
            };
            this.freezeServiceStatus.emit(freezeData);
          }
        }
        else if (y != 0 && this.mode == "Ticket") {
          this.ticketAttributesFG.controls["hdnIsGracePeriodMet"].setValue(y.isGracePeriodMet);
          if (y.closedDate != null) {
            this.ticketAttributesFG.controls["hdnClosedDateProject"].setValue(y.closedDate);
          }
          if (y.completedDate != null) {
            this.ticketAttributesFG.controls["hdnCompletedDateProject"].setValue(y.completedDate);
          }
          this.freezeDebtFieldsByGracePeriod(tickDetails);
        }
      });
    }
    else if (this.closeStatus == "Y") {
      this.dynamicgridComponent.closeAttributesModal(tickDetails);
      this.spinner.hide();
      return false;
    }
  }
  searchAssignee(assignee) {
    var projectId = this.ticketAttributesFG.get('hdnTicketProjectId').value;
    var params = {
      ProjectID: Number(projectId),
      AssigneName: assignee.query
    };
    this.analystselfserviceService.getAssigneNameByProjectId(params).subscribe(x => {
      if (x.length > 0 || x != undefined) {
        this.assigneeList = x;
      }
    });
  }
  getValue(value): string {
    return value !== "" && value !== "0" ? value : null;
  }
  clearWarningMessages() {
    this.mndteWarningBusinessShow = false;
    this.mndteWarningShow = false;
    this.isFieldMandate = false;
    this.isDebtMandate = false;
    this.isDebtErr = false;
    this.isAllfieldsCaptured = false;
    this.isDescriptionErr = false;
    this.isClosedOpenDteErr = false;
    this.isCompletedDteErr = false;
    this.isClosedCompleteDteErr = false;
    this.isActualEndDteErr = false;
    this.isReopenDteErr = false;
    this.isActualEndDteClsdErr = false;
    this.isActualEndClsDteErr = false;
    this.isCompletedReopnDteErr = false;
    this.isClosedReopnDteErr = false;
    this.isPlannedEndDteErr = false;
    this.isTicketAttrMandateMsg = false;
    this.isTranslationFailure = false;
    this.isChangesDone = false;
    this.showEncryptMessage = false;
  }
  onKeyPress(event, name) {
    var invalid;
    var attrVal = this.ticketAttributesFG.get(name).value;
    if (attrVal != null) {
      var str = attrVal.toString();
      if (event.shiftKey == true || event.key == "e" || event.keyCode == 101 || (str.indexOf('.') != -1 && event.keyCode == 46)) {
        event.preventDefault();
      }
      else if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 100)
        || (event.keyCode >= 102 && event.keyCode <= 105) || event.keyCode == 8 || event.keyCode == 9 ||
        event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 46 || event.keyCode == 190) {
        invalid = false;
      }
      else {
        event.preventDefault();
      }
    }
  }
  onKeyUp(event) {
    var k;
    k = event.charCode;
    if (k != 60 && k != 62 && k != 91 && k != 93) {
      return k;
    }
    else {
      event.preventDefault();
    }
  }
  onDateChange(oldVal, dateValue, attr) {
    var onChangeDte = this.datePipe.transform(dateValue, 'yyyy-MM-dd');
    var onChangeTime = this.datePipe.transform(dateValue, 'hh:mm:ss a');
    var userTimeZoneName;
    if (attr == 'actualStartDte') {
      this.actualSDteTriggerCount = this.actualSDteTriggerCount + 1;
    }
    else if (attr == 'actualEndDte') {
      this.actualEDteTriggerCount = this.actualEDteTriggerCount + 1;
    }
    else if (attr == 'closedDte') {
      this.closedTriggerCount = this.closedTriggerCount + 1;
    }
    else if (attr == 'completedDte') {
      this.completedTriggerCount = this.completedTriggerCount + 1;
    }
    else if (attr == 'plannedStrtDte') {
      this.plannedSDteTriggerCount = this.plannedSDteTriggerCount + 1;
    }
    else if (attr == 'releaseDte') {
      this.releaseDteTriggerCount = this.releaseDteTriggerCount + 1;
    }
    else if (attr == 'reopenDte') {
      this.reopenDateTriggerCount = this.reopenDateTriggerCount + 1;
    }
    userTimeZoneName = this.ticketAttributesFG.get('hdnUserTimeZoneName').value;
    var projectTimeZoneName = this.ticketAttributesFG.get('hdnProjectTimeZoneName').value;
    if (userTimeZoneName == "") {
      userTimeZoneName = projectTimeZoneName;
    }
    var params = {
      UserTimeZone: userTimeZoneName
    };
    this.analystselfserviceService.getCurrentTimeofTimeZones(params).subscribe(x => {
      var dateTimeNow = this.datePipe.transform(x, 'MM/dd/yyyy hh:mm:ss a');
      var utcDate = this.datePipe.transform(x, 'HH:mm:ss a');
      var userUtcDate = this.datePipe.transform(dateValue, 'HH:mm:ss a');
      var currentTime = new Date(dateTimeNow);
      var hours = currentTime.getHours();
      var minutes = currentTime.getMinutes();
      var sec = currentTime.getSeconds();
      if (minutes < 10)
        minutes = 0 + minutes;
      var suffix = "AM";
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
      var current_time = hours + ":" + minutes + ":" + sec + " " + suffix;
      var dateWithTime = onChangeDte + " " + current_time;
      var prjDate = onChangeDte + " " + onChangeTime;
      var tdyDateTime = new Date();
      var tdyDate = this.datePipe.transform(tdyDateTime, 'MM/dd/yyyy');
      var setMaxDate = tdyDate + " " + current_time;
      if (attr == "actualStartDte") {
        if (this.actualSDteTriggerCount == 1 && this.actualOldStartDateTime != "") {
          this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
          this.actualStartMaxDate = new Date(setMaxDate);
          this.actualOldStartDateTime = this.ticketAttributesFG.get(attr).value;
        }
        else {
          if ((oldVal == undefined || oldVal == null || oldVal == "") && attr == "actualStartDte") {
            this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
            this.actualStartMaxDate = new Date(setMaxDate);
            this.actualOldStartDateTime = this.ticketAttributesFG.get(attr).value;
          }
          else {
            if (userUtcDate < utcDate) {
              this.ticketAttributesFG.controls[attr].setValue(new Date(prjDate));
              this.actualOldStartDateTime = this.ticketAttributesFG.get(attr).value;
            }
            else {
              this.actualStartMaxDate = new Date(setMaxDate);
              this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
              this.actualOldStartDateTime = this.ticketAttributesFG.get(attr).value;
            }
          }
        }
      }
      if (attr == "actualEndDte") {
        if (this.actualEDteTriggerCount == 1 && this.actualOldEndDateTime != "") {
          this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
          this.actualEndMaxDate = new Date(setMaxDate);
          this.actualOldEndDateTime = this.ticketAttributesFG.get(attr).value;
        }
        else {
          if ((oldVal == undefined || oldVal == null || oldVal == "") && attr == "actualEndDte") {
            this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
            this.actualEndMaxDate = new Date(setMaxDate);
            this.actualOldEndDateTime = this.ticketAttributesFG.get(attr).value;
          }
          else {
            if (userUtcDate < utcDate) {
              this.ticketAttributesFG.controls[attr].setValue(new Date(prjDate));
              this.actualOldEndDateTime = this.ticketAttributesFG.get(attr).value;
            }
            else {
              this.actualEndMaxDate = new Date(setMaxDate);
              this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
              this.actualOldEndDateTime = this.ticketAttributesFG.get(attr).value;
            }
          }
        }
      }
      if (attr == "closedDte") {
        if (this.closedTriggerCount == 1 && this.closedOldDte != "") {
          this.ticketAttributesFG.controls[attr].setValue(new Date(prjDate));
          this.closedMaxDate = new Date(setMaxDate);
          this.closedOldDte = this.ticketAttributesFG.get(attr).value;
        }
        else {
          if ((oldVal == undefined || oldVal == null || oldVal == "") && attr == "closedDte") {
            this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
            this.closedMaxDate = new Date(setMaxDate);
            this.closedOldDte = this.ticketAttributesFG.get(attr).value;
          }
          else {
            if (userUtcDate < utcDate) {
              this.ticketAttributesFG.controls[attr].setValue(new Date(prjDate));
              this.closedOldDte = this.ticketAttributesFG.get(attr).value;
            }
            else {
              this.closedMaxDate = new Date(setMaxDate);
              this.ticketAttributesFG.controls[attr].setValue(new Date(prjDate));
              this.closedOldDte = this.ticketAttributesFG.get(attr).value;
            }
          }
        }
      }
      if (attr == "completedDte") {
        if (this.completedTriggerCount == 1 && this.completedOldDateTime != "") {
          this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
          this.complMaxDate = new Date(setMaxDate);
          this.completedOldDateTime = this.ticketAttributesFG.get(attr).value;
        }
        else {
          if ((oldVal == undefined || oldVal == null || oldVal == "") && attr == "completedDte") {
            this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
            this.complMaxDate = new Date(setMaxDate);
            this.completedOldDateTime = this.ticketAttributesFG.get(attr).value;
          }
          else {
            if (userUtcDate < utcDate) {
              this.ticketAttributesFG.controls[attr].setValue(new Date(prjDate));
              this.completedOldDateTime = this.ticketAttributesFG.get(attr).value;
            }
            else {
              this.complMaxDate = new Date(setMaxDate);
              this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
              this.completedOldDateTime = this.ticketAttributesFG.get(attr).value;
            }
          }
        }
      }
      if (attr == "reopenDte") {
        if (this.reopenDateTriggerCount == 1 && this.reopenOldDateTime != "") {
          this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
          this.reopenMaxDate = new Date(setMaxDate);
          this.reopenOldDateTime = this.ticketAttributesFG.get(attr).value;
        }
        else {
          if ((oldVal == undefined || oldVal == null || oldVal == "") && attr == "reopenDte") {
            this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
            this.reopenMaxDate = new Date(setMaxDate);
            this.reopenOldDateTime = this.ticketAttributesFG.get(attr).value;
          }
          else {
            if (userUtcDate < utcDate) {
              this.ticketAttributesFG.controls[attr].setValue(new Date(prjDate));
              this.reopenOldDateTime = this.ticketAttributesFG.get(attr).value;
            }
            else {
              this.reopenMaxDate = new Date(setMaxDate);
              this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
              this.reopenOldDateTime = this.ticketAttributesFG.get(attr).value;
            }
          }
        }
      }
      if (attr == "plannedStrtDte") {
        if (this.plannedSDteTriggerCount == 1 && this.plannedOldStartDate != "") {
          this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
          this.pStrtMaxDate = new Date(setMaxDate);
          this.plannedOldStartDate = this.ticketAttributesFG.get(attr).value;
        }
        else {
          if ((oldVal == undefined || oldVal == null || oldVal == "") && attr == "plannedStrtDte") {
            this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
            this.pStrtMaxDate = new Date(setMaxDate);
            this.plannedOldStartDate = this.ticketAttributesFG.get(attr).value;
          }
          else {
            if (userUtcDate < utcDate) {
              this.ticketAttributesFG.controls[attr].setValue(new Date(prjDate));
              this.plannedOldStartDate = this.ticketAttributesFG.get(attr).value;
            }
            else {
              this.pStrtMaxDate = new Date(setMaxDate);
              this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
              this.plannedOldStartDate = this.ticketAttributesFG.get(attr).value;
            }
          }
        }
      }
      if (attr == "releaseDte") {
        if (this.releaseDteTriggerCount == 1 && this.releaseOldDate != "") {
          this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
          this.releaseMaxDate = new Date(setMaxDate);
          this.releaseOldDate = this.ticketAttributesFG.get(attr).value;
        }
        else {
          if ((oldVal == undefined || oldVal == null || oldVal == "") && attr == "releaseDte") {
            this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
            this.releaseMaxDate = new Date(setMaxDate);
            this.releaseOldDate = this.ticketAttributesFG.get(attr).value;
          }
          else {
            if (userUtcDate < utcDate) {
              this.ticketAttributesFG.controls[attr].setValue(new Date(prjDate));
              this.releaseOldDate = this.ticketAttributesFG.get(attr).value;
            }
            else {
              this.releaseMaxDate = new Date(setMaxDate);
              this.ticketAttributesFG.controls[attr].setValue(new Date(dateWithTime));
              this.releaseOldDate = this.ticketAttributesFG.get(attr).value;
            }
          }
        }
      }
    });
  }
}

interface ColumnDictionary {
  controlName: string;
  columnName: string;
}
