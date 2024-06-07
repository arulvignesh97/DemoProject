// Copyright (c) Applens. All Rights Reserved.
import { Component, OnInit, TemplateRef} from '@angular/core';
import { Weekdays, deleteticket } from 'src/app/AnalystSelfService/Models/GridDetails';
import { Savedata } from 'src/app/AnalystSelfService/Models/GridDetails';
import { TranslateService } from '@ngx-translate/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { DatePipe } from '@angular/common';
import { DynamicgridService } from 'src/app/AnalystSelfService/Service/dynamicgrid.service'
import { parseDate } from 'ngx-bootstrap/chronos';
import { TimesheetentryComponent } from 'src/app/AnalystSelfService/timesheetentry/timesheetentry.component';
import { Constants } from 'src/app/AnalystSelfService/Constants/Constants';
import { HeaderService} from 'src/app/Layout/services/header.service';
import { MasterDataModel, LanguageModel } from 'src/app/Layout/models/header.models';
import { DatepickerDateCustomClasses } from 'ngx-bootstrap/datepicker';
import { AppSettingsConfig } from 'src/app/app.settings.config';
import { IfStmt } from '@angular/compiler';
import { SpinnerService} from 'src/app/common/services/spinner.service';


@Component({
  selector: 'app-dynamicgrid',
  templateUrl: './dynamicgrid.component.html',
  styleUrls: ['./dynamicgrid.component.css'],
  providers: [DatePipe, TimesheetentryComponent, Constants]
})
export class DynamicgridComponent implements OnInit {
  public FromdatePickerConfig: Partial<BsDatepickerModule>;
  MandatoryHours: any;
  ServiceList = [];
  lang :string ="en";
  selectdate: string;
  refreshlist = [];
  selectedDate : Date;
  values: Date[];
  ValidationMessage : string = '';
  SuccessMessage : string = ''
  BUBenchMarkMet : string = '';
  BUBenchMarkNotMet : string = '';
  ticketdetails = [];
  bkpticketdetails = [];
  hidenondelivery: boolean;
  hideworkItem : boolean;
  copyrestrict : boolean = false;
  addTicketIcon: boolean = false;
  addWorkItemIcon: boolean =  false;
  chooseTicketicon: boolean =  false;
  addNondeliveryicon: boolean =  false;
  savebtn: boolean =  false;
  submitbtn: boolean =  false;
  infopopup : boolean = false;
  IsCognizant: boolean = true;
  Deletepopup: boolean = false;
  dataSource: any;
  IsDaily: boolean;
  weekdays: Weekdays;
  servicedetails = [];
  statusdetails = [];
  bkpservicedetails = [];
  lstweekdays: Weekdays[];
  activitydetails = [];
  displayweek = [];
  isSuccess  = false;
  displayAttributes: boolean = false;
  firstday: string;
  lastday: string;
  supportTypeId: any;
  ServiceId: any;
  ticketTypeId: any;
  selectedTicketDetails: any;
  attrServiceId: number;
  serviceTickettypelist = [];
  serviceTitle: string;
  userLevelServiceList = [];
  copylist = [];
  copylistprocessed =[];
  duplicatelist = [];
  actvitylist = [];
  duplicateservicelist =[];
  servicelist = [];
  Today_date: any = this.datePipe.transform(new Date(), Constants.DateFormateMonthfirst);
  userLeveltaskList = [];
  ServiceActivityDetails = [];
  lstApplicableServices = [];
  LstOrgBenchMarkDetailsByService = [];
  LstBUBenchMarkDetailsByService = [];
  validateservicelist = [];
  validatedlist = [];
  validateadddedlist = [];
  AddNonDeliverypopup : boolean = false;
  AddWorkItempopup: boolean = false;
  public addTicketDialog: boolean;
  public chooseTicketDialog: boolean;
  public erroredTicketDialog = false;
  lstsavedata: Savedata;
  ticketdata = [];
  timesheetdata = [];
  dateformate: string;
  Todaydate: Date;
  Freezedatecountlogic: number = AppSettingsConfig.settings.DailyFreezeDay;
  futuredisable: boolean = false;
  effort1: number = 0.00;
  effort2: number = 0.00;
  effort3: number = 0.00;
  effort4: number = 0.00;
  effort5: number = 0.00;
  effort6: number = 0.00;
  effort7: number = 0.00;
  Totaleffort1: string;
  Totaleffort2: string;
  Totaleffort3: string;
  Totaleffort4: string;
  Totaleffort5: string;
  Totaleffort6: string;
  Totaleffort7: string;
  IsEffortTrack: boolean;
  NorecordMessage: boolean = false;
  CustomerId : string;
  EmployeeId : string;
  userDetails = [];
  hoursvalidation: boolean;
  customerTimeZoneName : string;
  openAttr: boolean = false;
  closeStatus: string = 'N';
  public mode: string = 'Main'
  dailyhourmandatoryhour: boolean = false;
  ischeckboxchecked: boolean = false;
  mandatoryhourMon: boolean = false;
  mandatoryhourTue: boolean = false;
  mandatoryhourWed: boolean = false;
  mandatoryhourThu: boolean = false;
  mandatoryhourFri: boolean = false;
  displaydate: Date;
  ddd:string;
  currentstartweekdate: string;
  currentendweekdate: string;
  dateCustomClasses: DatepickerDateCustomClasses[];
  colourGeen: boolean;
  colourRed: boolean;
  Deleteticket: deleteticket;
  deletetotaleffort: number;
  glowErrorCorrection: boolean = false;
  Deleteindex: number;
  mandatoryattribute: boolean = false;
  weeklyvalidation: boolean = false;
  public dateWeek: Date;
  previousStartDate: string;
  previousendDate: string;
  currDay: number;
  header : string = "Ticket/ WorkItem ID";
  chooseticketheader : string = "Choose Ticket/Work Item";
   constructor(private translate: TranslateService, private datePipe: DatePipe, 
    private dynamicgridservice: DynamicgridService, 
    private timesheetentry: TimesheetentryComponent,
    private headerService: HeaderService,
    private spinneservice: SpinnerService) {
     
  }
  
  ngOnInit(): void {   
    const now = new Date();   
    this.hidenondelivery = true;
    this.hideworkItem = true;
    this.dateCustomClasses = [
      { date: now, classes: ['backgblue'] }
    ];
    this.headerService.masterDataEmitter.subscribe((masterData: MasterDataModel) => {
      if (masterData != null) {
     this.selectedDate = new Date();
     this.userDetails = masterData.hiddenFields.lstProjectUserID;
     this.CustomerId = masterData.hiddenFields.customerId;
     this.EmployeeId = masterData.hiddenFields.employeeId;
     this.customerTimeZoneName = masterData.hiddenFields.customerTimeZoneName;
     this.IsCognizant = masterData.hiddenFields.isCognizant == 1 ? true : false;
     this.IsDaily =  masterData.hiddenFields.isDaily == 1 ? true : false;
     this.IsEffortTrack =  masterData.hiddenFields.isEffortConfigured == 1 ? true : false;
     this.hidenondelivery = !this.IsCognizant && !this.IsEffortTrack ? false : true ;
        this.hideworkItem = (this.IsCognizant || ((!this.IsCognizant) && AppSettingsConfig.settings.ADMApplicableforCustomer)) == true ? true : false;
        if(!this.IsCognizant && !AppSettingsConfig.settings.ADMApplicableforCustomer){
          this.header = "Ticket ID";
          this.chooseticketheader="Choose Ticket"
        }
        else{
          this.header = "Ticket/ WorkItem ID";
          this.chooseticketheader="Choose Ticket/Work Item"
        }
     if(this.CustomerId != null)
     this.checkErrorTickCount();
     this.cleartotalhours();
       this.Todaydate = new Date();
       this.displaydate = new Date();
        this.FromdatePickerConfig = Object.assign({},
        {
          showWeekNumbers: false,
          dateInputFormat: 'dd/MM/YYYY',
          todayHighlight: true
          });              
          let Params = {
          CustomerId: parseInt(this.CustomerId),
          EmployeeID: this.EmployeeId
        }
        if(this.CustomerId != null){
        this.dynamicgridservice.MandatoryHours(Params).subscribe(x => {         
          if(this.IsDaily){           
          this.MandatoryHours = x.toFixed(2);
          }
          else{
            this.MandatoryHours = (x*5).toFixed(2);
          }
    
        });
      }
        this.dateselection(new Date())
      }
    });
    
  }

  AddWorkItem() {
    this.AddWorkItempopup = true;
  }
  openinfopopup(){
    this.infopopup = true;
  }
  public addTicketPopup(): void {
      this.addTicketDialog = true;
    }
  public searchPopup(): void{
      this.chooseTicketDialog = true;
    }
  erroredTicketsPopup(param: boolean): void {
        this.erroredTicketDialog = param;
  }
  AddNonDeliveryActivity() {
    this.AddNonDeliverypopup = true;
  }
  BindSelectedTicketsorWorkItems(addedlist) {
    if (addedlist.length > 1) {
      for (let i = 0; i < addedlist.length; i++) {
        this.bkpticketdetails.push(addedlist[i]);
      }     
    }
    else {
      this.bkpticketdetails.push(addedlist[0]);
    }
    this.validatedlist = this.GetSelectedServices(addedlist);
    this.validateadddedlist = this.validatedlist.map(c => ({
      assignmentGroupId: c.assignmentGroupId,
      openDateNTime : c.openDateNTime,
      activityId: c.activityId,
      activityTitle: c.activityTitle,
      applicationId: c.applicationId,
      assignedTo: c.assignedTo,
      benchMarkColor: c.benchMarkColor,
      benchMarkTitle: c.benchMarkTitle,
      closedDate: c.closedDate,
      completedDate: c.completedDate,
      dartStatusId: c.dartStatusId,
      effort1: c.effort1,
      effort2: c.effort2,
      effort3: c.effort3,
      effort4: c.effort4,
      effort5: c.effort5,
      effort6: c.effort6,
      effort7: c.effort7,
      effortTillDate: c.effortTillDate,
      employeeId: c.employeeId,
      freezeStatus1: c.freezeStatus1,
      freezeStatus2: c.freezeStatus2,
      freezeStatus3: c.freezeStatus3,
      freezeStatus4: c.freezeStatus4,
      freezeStatus5: c.freezeStatus5,
      freezeStatus6: c.freezeStatus6,
      freezeStatus7: c.freezeStatus7,
      gracePeriod: c.gracePeriod,
      gridValidationservice: c.gridValidationservice,
      infraTitle:c.infraTitle,
      isAHTagged: c.isAHTagged,
      isAHTicket: c.isAHTicket,
      isALMToolConfigured: c.isALMToolConfigured,
      isActivityTracked: c.isActivityTracked,
      isAttributeUpdated: c.isAttributeUpdated,
      isCognizant: c.isCognizant,
      isDebtEnabled: c.isDebtEnabled,
      isDeleted: c.isDeleted,
      isEffortTracked: c.isEffortTracked,
      isFreezed: c.isFreezed,
      isGracePeriodMet: c.isGracePeriodMet,
      isMainspringConfigured: c.isMainspringConfigured,
      isNonTicket: c.isNonTicket,
      isSDTicket: c.isSDTicket,
      isTicket: c.isTicket,
      itsmEffort: c.itsmEffort,
      lstActivityModel: c.lstActivityModel,
      lstServiceModel: c.lstServiceModel,
      lstStatusDetails: c.lstStatusDetails,
      lstTaskModel: c.lstTaskModel,
      lstTicketTypeModel: c.lstTicketTypeModel,
      lstTicketTypeServiceDetails: c.lstTicketTypeServiceDetails,
      lstUserLevelDetails: c.lstUserLevelDetails,
      projectId: c.projectId,
      projectTimeZoneName: c.projectTimeZoneName,
      serviceId: c.serviceId,
      ServiceTitle: c.ServiceTitle,
      StatusTitle: c.StatusTitle,
      suggestedActivityName: c.suggestedActivityName,
      supportTypeId: c.supportTypeId,
      ticketDescription: c.ticketDescription,     
      ticketIDTitle: c.ticketIDTitle,
      ticketId: c.ticketId,
      ticketStatusMapId: c.ticketStatusMapId,
      ticketTypeMapId: c.ticketTypeMapId,
      timeSheetDate1: c.timeSheetDate1,
      timeSheetDate2: c.timeSheetDate2,
      timeSheetDate3: c.timeSheetDate3,
      timeSheetDate4: c.timeSheetDate4,
      timeSheetDate5: c.timeSheetDate5,
      timeSheetDate6: c.timeSheetDate6,
      timeSheetDate7: c.timeSheetDate7,
      timeSheetDetailId1: c.timeSheetDetailId1,
      timeSheetDetailId2: c.timeSheetDetailId2,
      timeSheetDetailId3: c.timeSheetDetailId3,
      timeSheetDetailId4: c.timeSheetDetailId4,
      timeSheetDetailId5: c.timeSheetDetailId5,
      timeSheetDetailId6: c.timeSheetDetailId6,
      timeSheetDetailId7: c.timeSheetDetailId7,
      timeSheetID4: c.timeSheetID4,
      timeSheetId1: c.timeSheetId1,
      timeSheetId2: c.timeSheetId2,
      timeSheetId3: c.timeSheetId3,
      timeSheetId5: c.timeSheetId5,
      timeSheetId6: c.timeSheetId6,
      timeSheetId7: c.timeSheetId7,
      timeTickerId: c.timeTickerId,
      towerId: c.towerId,
      type: c.type,
      userTimeZoneName: c.userTimeZoneName
      
    })
    );
    for (let j = 0; j < this.validateadddedlist.length; j++){
      if (this.validateadddedlist[j].type == 'ND') {
        this.validateadddedlist[j].activityId = 0;
      }
      this.ticketdetails.push(this.validateadddedlist[j]);
    }   
    if(this.ticketdetails.length > 0){
      this.NorecordMessage = false;
    }
  }
  GridLoad(finallist){
    this.ticketdetails = finallist;
  }
  Loadgriddata(){
    this.copyrestrict = false;
    this.lang = this.translate.currentLang;
    this.weeklyvalidation = false;
    if (this.currentstartweekdate == undefined && this.currentendweekdate == undefined) {
      const curr = new Date();
      const first = curr.getDate() - curr.getDay();
      const last = first + 6;
      this.currentstartweekdate = this.dateformatechange(this.dateformatecomversion(this.datePipe.transform(new Date(curr.setDate(first)).toDateString(), 'MM/dd/yyyy'), 0));
      this.currentendweekdate = this.dateformatechange(this.dateformatecomversion(this.datePipe.transform(new Date(curr.setDate(last)).toDateString(),
       'MM/dd/yyyy'), first <= 0 ? 1 : 0));
    }
    let Iscog;
    if(this.IsCognizant){
      Iscog = "1";
    }
    else{
      Iscog = "0";
    }
    this.NorecordMessage = false;
    this.cleartotalhours();
    this.lstweekdays = [];
    this.EnableAllButtons();
    let lstGetticketdetail = {
      CustomerId: this.CustomerId,     
      FirstDateOfWeek: this.firstday,
      LastDateOfWeek: this.lastday,
      Mode: "",
      isCognizant: Iscog
    }
    this.spinneservice.show();
    this.dynamicgridservice.Getticketdetail(lstGetticketdetail).subscribe(x => {
      this.ServiceActivityDetails = x.lstServiceActivityDetails
      this.LstBUBenchMarkDetailsByService = x.lstBUBenchMarkDetails
      this.LstOrgBenchMarkDetailsByService = x.lstOrgBenchMarkDetails
      this.lstApplicableServices = x.lstApplicableServices
      this.spinneservice.hide();
      if( x.lstOverallTicketDetails != null && x.lstOverallTicketDetails.length == 0){
        this.NorecordMessage = true;
      }
      if( x.lstOverallTicketDetails != null){
      this.bkpticketdetails = x.lstOverallTicketDetails.map(c => ({       
        openDateNTime : c.openDateNTime,
        activityId: c.activityId,
        activityTitle: c.activityTitle,
        applicationId: c.applicationId,
        assignedTo: c.assignedTo,
        benchMarkColor: c.benchMarkColor,
        benchMarkTitle: c.benchMarkTitle,
        closedDate: c.closedDate,
        completedDate: c.completedDate,
        dartStatusId: c.dartStatusId,
        effort1: c.effort1,
        effort2: c.effort2,
        effort3: c.effort3,
        effort4: c.effort4,
        effort5: c.effort5,
        effort6: c.effort6,
        effort7: c.effort7,
        effortTillDate: c.effortTillDate,
        employeeId: c.employeeId,
        freezeStatus1: c.freezeStatus1,
        freezeStatus2: c.freezeStatus2,
        freezeStatus3: c.freezeStatus3,
        freezeStatus4: c.freezeStatus4,
        freezeStatus5: c.freezeStatus5,
        freezeStatus6: c.freezeStatus6,
        freezeStatus7: c.freezeStatus7,
        gracePeriod: c.gracePeriod,
        gridValidationservice: c.gridValidationservice,
        infraTitle:c.infraTitle,
        isAHTagged: c.isAHTagged,
        isAHTicket: c.isAHTicket,
        isALMToolConfigured: c.isALMToolConfigured,
        isActivityTracked: c.isActivityTracked,
        isAttributeUpdated: c.isAttributeUpdated,
        isCognizant: c.isCognizant,
        isDebtEnabled: c.isDebtEnabled,
        isDeleted: c.isDeleted,
        isEffortTracked: c.isEffortTracked,
        isFreezed: c.isFreezed,
        isGracePeriodMet: c.isGracePeriodMet,
        isMainspringConfigured: c.isMainspringConfigured,
        isNonTicket: c.isNonTicket,
        isSDTicket: c.isSDTicket,
        isTicket: c.isTicket,
        itsmEffort: c.itsmEffort,
        lstActivityModel: c.lstActivityModel,
        lstServiceModel: c.lstServiceModel,
        lstStatusDetails: c.lstStatusDetails,
        lstTaskModel: c.lstTaskModel,
        lstTicketTypeModel: c.lstTicketTypeModel,
        lstTicketTypeServiceDetails: c.lstTicketTypeServiceDetails,
        lstUserLevelDetails: c.lstUserLevelDetails,
        projectId: c.projectId,
        projectTimeZoneName: c.projectTimeZoneName,
        serviceId: c.serviceId,
        ServiceTitle: c.ServiceTitle,
        StatusTitle: c.StatusTitle,
        suggestedActivityName: c.suggestedActivityName,
        supportTypeId: c.supportTypeId,
        ticketDescription: c.ticketDescription,       
        ticketId: c.ticketId,
        ticketIDTitle: c.ticketIDTitle,
        ticketStatusMapId: c.ticketStatusMapId,
        ticketTypeMapId: c.ticketTypeMapId,
        timeSheetDate1: c.timeSheetDate1,
        timeSheetDate2: c.timeSheetDate2,
        timeSheetDate3: c.timeSheetDate3,
        timeSheetDate4: c.timeSheetDate4,
        timeSheetDate5: c.timeSheetDate5,
        timeSheetDate6: c.timeSheetDate6,
        timeSheetDate7: c.timeSheetDate7,
        timeSheetDetailId1: c.timeSheetDetailId1,
        timeSheetDetailId2: c.timeSheetDetailId2,
        timeSheetDetailId3: c.timeSheetDetailId3,
        timeSheetDetailId4: c.timeSheetDetailId4,
        timeSheetDetailId5: c.timeSheetDetailId5,
        timeSheetDetailId6: c.timeSheetDetailId6,
        timeSheetDetailId7: c.timeSheetDetailId7,
        timeSheetID4: c.timeSheetID4,
        timeSheetId1: c.timeSheetId1,
        timeSheetId2: c.timeSheetId2,
        timeSheetId3: c.timeSheetId3,
        timeSheetId5: c.timeSheetId5,
        timeSheetId6: c.timeSheetId6,
        timeSheetId7: c.timeSheetId7,
        timeTickerId: c.timeTickerId,
        towerId: c.towerId,
        type: c.type,
        userTimeZoneName: c.userTimeZoneName
      })
      );
    }
    if( x.lstWeekDays != null){
      this.lstweekdays = x.lstWeekDays.map(data => ({
        Date: data.date,
        Day: data.day,
        DisplayDate: data.displayDate,
        FreezeStatus: data.freezeStatus,
        StatusID: data.statusId,
        griddispalydate: "",
        gridmonth: "",
        gridday: "",
        griddate: "",
        EnabeheaderND: "",
        IsChanged: Constants.false

      }))
      for (let date of this.lstweekdays){
       
        date.griddispalydate = date.DisplayDate.replace("<br/>", " ");
        date.gridmonth = date.griddispalydate.split(" ", 3)[0];
        date.gridday = date.griddispalydate.split(" ", 4)[3];
        date.griddate = date.griddispalydate.split(" ", 3)[2];
        if (!this.IsCognizant && !this.IsEffortTrack) {
          date.FreezeStatus = Constants.true;
        }
        else {
          if (this.IsDaily) {
            if (date.StatusID != "2" && date.StatusID != "3" && date.StatusID != "6" && date.StatusID != "4") {
              date.FreezeStatus = this.freezestatusDaily(date);
            }
          }
          else {
            if (this.lstweekdays.filter(F => F.StatusID == "2" || F.StatusID == "3" || F.StatusID == "6" || F.StatusID == "4").length <= 0) {
              if (!this.weeklyvalidation) {
                this.freezestatusweekly(date);
              }
            }
          }
        }
      }
    }
    if(x.lstOverallTicketDetails != null){
      x.lstOverallTicketDetails =  this.GetSelectedServices(x.lstOverallTicketDetails)
      this.ticketdetails = x.lstOverallTicketDetails;     
      for(let t = 0; t<this.ticketdetails.length; t++){
       if(this.ticketdetails[t].type == 'ND'){
           this.ticketdetails[t].activityId = 0;
        }
      }
    }
    if(this.lstweekdays !=null){
      this.FreezeButtons(this.lstweekdays);
    }
    });

  }
  FreezeButtons(lstweek){
    if(new Date(this.currentstartweekdate) > new Date(this.datePipe.transform(localStorage.getItem("Lastdayofweek"), 'MM/dd/yyyy'))){
      if(lstweek.filter(x => x.FreezeStatus == "true").length == 7){
        this.DisableAllButtons();
      }
      else{
        this.EnableAllButtons();
      }
    }
    else if((this.datePipe.transform(localStorage.getItem("Firstdayofweek"), 'MM/dd/yyyy') == this.currentstartweekdate)){
     if(this.IsDaily && (this.IsCognizant ||(!this.IsCognizant && this.IsEffortTrack))){
      let afterfreezecount = 0;
      for(let j=0;j<lstweek.length;j++){
          if(new Date(this.datePipe.transform(lstweek[j].Date,'MM/dd/yyyy')) > new Date(this.datePipe.transform(this.Today_date,'MM/dd/yyyy'))){
             if(lstweek[j].StatusID == 0){
              afterfreezecount++;
             }
           }
          }
      if(lstweek.filter(x=>x.FreezeStatus == "true").length == 7) {  
      if(afterfreezecount > 0 ){
           this.EnableAllButtons();
      }
      else{
        this.DisableAllButtons();
      }
       }
       else{
         this.EnableAllButtons();
       }
      }
      else if(!this.IsDaily && (this.IsCognizant ||(!this.IsCognizant && this.IsEffortTrack))){
        if(lstweek.filter(x => x.FreezeStatus == "true").length == 7){
          this.DisableAllButtons();
        }
        else{
          this.EnableAllButtons();
        }
      }
      else{
        this.EnableAllButtons();
      }
    }
    
    else if(new Date(this.currentendweekdate) < new Date(this.datePipe.transform(localStorage.getItem("Firstdayofweek"), 'MM/dd/yyyy'))){
      if(!this.IsDaily &&(lstweek.filter(x=> x.StatusID == 2).length > 0) && (lstweek.filter(x => x.FreezeStatus == "true").length == 7)){
         this.DisableAllButtons();
      }
      else if(this.IsDaily &&(lstweek.filter(x=> x.StatusID == 2).length == 7)){
      this.DisableAllButtons();
      }
      else{
      this.addTicketIcon = true;
      this.addWorkItemIcon = false;
      this.chooseTicketicon = true;
      this.addNondeliveryicon = false;
      this.savebtn = (!this.IsCognizant && !this.IsEffortTrack) ? true : false;
      this.submitbtn = (!this.IsCognizant && !this.IsEffortTrack) ? true : false;
      }
    }
    else{
      this.EnableAllButtons();
    }
  }
  DisableAllButtons(){
    this.addTicketIcon = true;
    this.addWorkItemIcon = true;
    this.chooseTicketicon = true;
    this.addNondeliveryicon = true;
    this.savebtn = true;
    this.submitbtn = true;
    this.copyrestrict = true;
  }
  EnableAllButtons(){
    this.addTicketIcon = false;
    this.addWorkItemIcon = false;
    this.chooseTicketicon = false;
    this.addNondeliveryicon = false;
    this.savebtn = false;
    this.submitbtn = false;
    this.copyrestrict = false;
  }
  sumoftotaleffort(lstOverallTicketDetails : any){
    this.hoursvalidation = true;
    this.effort1 = 0.00;
    this.effort2 = 0.00;
    this.effort3 = 0.00;
    this.effort4 = 0.00;
    this.effort5 = 0.00;
    this.effort6 = 0.00;
    this.effort7 = 0.00;

    for (let i = 0; i < lstOverallTicketDetails.length; i++) {

      if (lstOverallTicketDetails[i].effort1 != "") {
        this.effort1 = parseFloat(this.effort1.toFixed(2)) + parseFloat(lstOverallTicketDetails[i].effort1);
      }
      if (lstOverallTicketDetails[i].effort2 != "") {
        this.effort2 = parseFloat(this.effort2.toFixed(2)) + parseFloat(lstOverallTicketDetails[i].effort2);
      }
      if (lstOverallTicketDetails[i].effort3 != "") {
        this.effort3 = parseFloat(this.effort3.toFixed(2)) + parseFloat(lstOverallTicketDetails[i].effort3);
      }
      if (lstOverallTicketDetails[i].effort4 != "") {
        this.effort4 = parseFloat(this.effort4.toFixed(2)) + parseFloat(lstOverallTicketDetails[i].effort4);
      }
      if (lstOverallTicketDetails[i].effort5 != "") {
        this.effort5 = parseFloat(this.effort5.toFixed(2)) + parseFloat(lstOverallTicketDetails[i].effort5);
      }
      if (lstOverallTicketDetails[i].effort6 != "") {
        this.effort6 = parseFloat(this.effort6.toFixed(2)) + parseFloat(lstOverallTicketDetails[i].effort6);
      }
      if (lstOverallTicketDetails[i].effort7 != "") {
        this.effort7 = parseFloat(this.effort7.toFixed(2)) + parseFloat(lstOverallTicketDetails[i].effort7);
      }
    }
    this.hoursvalidation = this.effort1 > 24 || this.effort2 > 24 || this.effort3 > 24 || this.effort4 > 24 || this.effort5 > 24 || this.effort6 > 24
      || this.effort7 > 24
      ? false : true
      
  }
  mandatoryhourvalidation(){
    this.dailyhourmandatoryhour = true;
    this.mandatoryattribute = false;
    if (this.IsDaily) {
      if (this.lstweekdays[0].IsChanged == Constants.true) {
        this.dailyhourmandatoryhour = true;
        this.mandatoryattribute = this.ticketdetails.filter(x => x.isAttributeUpdated == 0 && x.type == "T" && (x.effort1 != "")).length > 0 ? true : this.mandatoryattribute
      }
      if (this.lstweekdays[1].IsChanged == Constants.true) {
        this.dailyhourmandatoryhour = this.dailyhourmandatoryhour === true ? this.effort2 >= this.MandatoryHours ? true : false : false;
        this.mandatoryhourMon = this.effort2 >= this.MandatoryHours ? false : true;
        this.mandatoryattribute = this.ticketdetails.filter(x => x.isAttributeUpdated == 0 && x.type == "T" && (x.effort2 != "")).length > 0 ? true : this.mandatoryattribute
      }
      if (this.lstweekdays[2].IsChanged == Constants.true) {
        this.dailyhourmandatoryhour = this.dailyhourmandatoryhour === true ? this.effort3 >= this.MandatoryHours ? true : false : false;
        this.mandatoryhourTue = this.effort3 >= this.MandatoryHours ? false : true;
        this.mandatoryattribute = this.ticketdetails.filter(x => x.isAttributeUpdated == 0 && x.type == "T" && (x.effort3 != "")).length > 0 ? true : this.mandatoryattribute
      }
      if (this.lstweekdays[3].IsChanged == Constants.true) {
        this.dailyhourmandatoryhour = this.dailyhourmandatoryhour === true ? this.effort4 >= this.MandatoryHours ? true : false : false;
        this.mandatoryhourWed = this.effort4 >= this.MandatoryHours ? false : true;
        this.mandatoryattribute = this.ticketdetails.filter(x => x.isAttributeUpdated == 0 && x.type == "T" && (x.effort4 != "")).length > 0 ? true : this.mandatoryattribute
      }
      if (this.lstweekdays[4].IsChanged == Constants.true) {
        this.dailyhourmandatoryhour = this.dailyhourmandatoryhour === true ? this.effort5 >= this.MandatoryHours ? true : false : false;
        this.mandatoryhourThu = this.effort5 >= this.MandatoryHours ? false : true;
        this.mandatoryattribute = this.ticketdetails.filter(x => x.isAttributeUpdated == 0 && x.type == "T" && (x.effort5 != "")).length > 0 ? true : this.mandatoryattribute
      }
      if (this.lstweekdays[5].IsChanged == Constants.true) {
        this.dailyhourmandatoryhour = this.dailyhourmandatoryhour === true ? this.effort6 >= this.MandatoryHours ? true : false : false;
        this.mandatoryhourFri = this.effort6 >= this.MandatoryHours ? false : true;
        this.mandatoryattribute = this.ticketdetails.filter(x => x.isAttributeUpdated == 0 && x.type == "T" && (x.effort6 != "")).length > 0 ? true : this.mandatoryattribute
      }
      if (this.lstweekdays[6].IsChanged == Constants.true) {
        this.dailyhourmandatoryhour = true;
        this.mandatoryattribute = this.ticketdetails.filter(x => x.isAttributeUpdated == 0 && x.type == "T" && (x.effort7 != "")).length > 0 ? true : this.mandatoryattribute
      } 
    }
    else{
      this.dailyhourmandatoryhour = (parseFloat(this.effort1.toFixed(2)) + parseFloat(this.effort2.toFixed(2)) + parseFloat(this.effort3.toFixed(2)) + 
      parseFloat(this.effort4.toFixed(2)) + 
      parseFloat(this.effort5.toFixed(2)) + parseFloat(this.effort6.toFixed(2)) + parseFloat(this.effort7.toFixed(2))) >= this.MandatoryHours ? true : false;
      this.mandatoryattribute = this.ticketdetails.filter(x => x.isAttributeUpdated == 0 && x.type == "T" && (x.effort1 != "" || x.effort2 != "" || x.effort3 != ""
        || x.effort4 != "" || x.effort5 != "" || x.effort6 != "" || x.effort7 != "")).length > 0 ? true : this.mandatoryattribute;
    }
  }
  BenchMarkCalculation(TicketDetail): string {   
    this.colourRed = false;
    this.colourGeen = false;
    if (AppSettingsConfig.settings.IsBenchMarkApplicable && TicketDetail.serviceId != 0) {
      if (this.lstApplicableServices.filter(a => a.serviceId == TicketDetail.serviceId).length != -1 && parseFloat(TicketDetail.effortTillDate) > 0.00) {
        let BUBenchMark = 0;
        let OrgBenchMark = 0;
        var BenchMarkString = "";
        let LegendClass = "";
        let BenchMarkLevel = 0;
        let lstBUBenchMark = this.LstBUBenchMarkDetailsByService.filter(b => b.serviceId == TicketDetail.serviceId)
        let lstOrgBenchMark = this.LstOrgBenchMarkDetailsByService.filter(o => o.serviceId == TicketDetail.serviceId)
        this.setBUBenchMarkMet('BUBenchMarkMet');
        this.setBUBenchMarkNotMet('BUBenchMarkNotMet');
        if (lstBUBenchMark == null || lstBUBenchMark.length == 0){
          BenchMarkString = Constants.BenchMarkNotAvailable;
        }
        else{
          if (lstBUBenchMark == null || lstBUBenchMark.length == 1) {
              BUBenchMark = lstOrgBenchMark[0].benchMarkValue;
              BenchMarkLevel = lstOrgBenchMark[0].benchMarkLevel;
              lstOrgBenchMark = lstOrgBenchMark.filter(obj => obj.benchMarkLevel == BenchMarkLevel);
              OrgBenchMark = lstOrgBenchMark != null && lstOrgBenchMark.length > 0 ?
                lstOrgBenchMark[0].benchMarkValue : 0;

              if (parseFloat(TicketDetail.effortTillDate) > BUBenchMark) {
                //BorderRed
                TicketDetail.benchMarkColor = "RED";
                BenchMarkString = this.BUBenchMarkNotMet + Constants.org + OrgBenchMark + Constants.BU + BUBenchMark;
              }
              else {
                //orderGreen"
                TicketDetail.benchMarkColor = "GREEN";
                BenchMarkString = this.BUBenchMarkMet + Constants.org + OrgBenchMark + Constants.BU + BUBenchMark;
              }
          }
          else if (lstBUBenchMark != null && lstBUBenchMark.length > 0 && lstOrgBenchMark != null && lstOrgBenchMark.length > 0) {
            lstBUBenchMark = lstBUBenchMark.filter(b => b.benchMarkLevel != 0);
            lstOrgBenchMark = lstOrgBenchMark.filter(o => o.benchMarkLevel != 0);

            if (lstBUBenchMark != null && lstBUBenchMark.length > 0 && lstOrgBenchMark != null && lstOrgBenchMark.length > 0){
              let BUhighest = Math.max.apply(Math, lstBUBenchMark.map(function (o) { return o.benchMarkValue; }))
              let Orghighest = Math.max.apply(Math, lstOrgBenchMark.map(function (o) { return o.benchMarkValue; })) 
              let FromValue = 0;
              if (parseFloat(TicketDetail.effortTillDate) > BUhighest) {
                BUBenchMark = BUhighest;
                OrgBenchMark = Orghighest;
                //BorderRed
                TicketDetail.benchMarkColor = "RED";
                BenchMarkString = this.BUBenchMarkNotMet + Constants.org + OrgBenchMark + Constants.BU + BUBenchMark;
              }
              else{
                for (let i = 0; i < lstBUBenchMark.length; i++) {
                  let ToValue = lstBUBenchMark[i].benchMarkValue;
                  if (parseFloat(TicketDetail.effortTillDate) > FromValue && parseFloat(TicketDetail.effortTillDate) <= ToValue) {
                    BUBenchMark = lstBUBenchMark[i].benchMarkValue;
                    //BorderGreen
                    TicketDetail.benchMarkColor = "GREEN";
                    BenchMarkString = this.BUBenchMarkMet;
                    BenchMarkLevel = lstBUBenchMark[i].benchMarkLevel;
                    break;
                  }
                  FromValue = lstBUBenchMark[i].benchMarkValue;
                }
                lstOrgBenchMark = lstOrgBenchMark.filter(obj =>
                  obj.serviceId == TicketDetail.serviceId &&
                  obj.benchMarkLevel == BenchMarkLevel);
                if (lstOrgBenchMark != null && lstOrgBenchMark.length > 0){
                  OrgBenchMark = lstOrgBenchMark[0].benchMarkValue;
                }

                BenchMarkString = BenchMarkString + Constants.org + OrgBenchMark + Constants.BU + BUBenchMark;
              }
            }

          }
          
        }
      }
    }
    else{
      BenchMarkString = "";
    }
    return BenchMarkString;
  }
  GetSelectedServices(lstOverallTicketDetails) {  
    this.futuredisable = false;  
    this.sumoftotaleffort(lstOverallTicketDetails);

    for (let i = 0; i < lstOverallTicketDetails.length; i++) {
     if(this.IsCognizant){
      lstOverallTicketDetails[i].benchMarkTitle = this.BenchMarkCalculation(lstOverallTicketDetails[i]);
      }
      this.Totaleffort1 = this.effort1 > 0 ? parseFloat(this.effort1.toString()).toFixed(2) : "";
      this.Totaleffort2 = this.effort2 > 0 ? parseFloat(this.effort2.toString()).toFixed(2) : "";
      this.Totaleffort3 = this.effort3 > 0 ? parseFloat(this.effort3.toString()).toFixed(2) : "";
      this.Totaleffort4 = this.effort4 > 0 ? parseFloat(this.effort4.toString()).toFixed(2) : "";
      this.Totaleffort5 = this.effort5 > 0 ? parseFloat(this.effort5.toString()).toFixed(2) : "";
      this.Totaleffort6 = this.effort6 > 0 ? parseFloat(this.effort6.toString()).toFixed(2) : "";
      this.Totaleffort7 = this.effort7 > 0 ? parseFloat(this.effort7.toString()).toFixed(2) : "";

      
      if (lstOverallTicketDetails[i].timeSheetDate1 == null || this.datePipe.transform(lstOverallTicketDetails[i].timeSheetDate1, Constants.DateFormateMonthfirst)
          == this.datePipe.transform(this.lstweekdays[0].Date, Constants.DateFormateMonthfirst)) {
          lstOverallTicketDetails[i].freezeStatus1 = this.lstweekdays[0].FreezeStatus;
        }
      if (lstOverallTicketDetails[i].timeSheetDate2 == null || this.datePipe.transform(lstOverallTicketDetails[i].timeSheetDate2, Constants.DateFormateMonthfirst)
          == this.datePipe.transform(this.lstweekdays[1].Date, Constants.DateFormateMonthfirst)) {
          lstOverallTicketDetails[i].freezeStatus2 = this.lstweekdays[1].FreezeStatus;
        }
      if (lstOverallTicketDetails[i].timeSheetDate3 == null || this.datePipe.transform(lstOverallTicketDetails[i].timeSheetDate3, Constants.DateFormateMonthfirst)
          == this.datePipe.transform(this.lstweekdays[2].Date, Constants.DateFormateMonthfirst)) {
          lstOverallTicketDetails[i].freezeStatus3 = this.lstweekdays[2].FreezeStatus;
        }
      if (lstOverallTicketDetails[i].timeSheetDate4 == null || this.datePipe.transform(lstOverallTicketDetails[i].timeSheetDate4, Constants.DateFormateMonthfirst)
          == this.datePipe.transform(this.lstweekdays[3].Date, Constants.DateFormateMonthfirst)) {
          lstOverallTicketDetails[i].freezeStatus4 = this.lstweekdays[3].FreezeStatus;
        }
      if (lstOverallTicketDetails[i].timeSheetDate5 == null || this.datePipe.transform(lstOverallTicketDetails[i].timeSheetDate5, Constants.DateFormateMonthfirst)
          == this.datePipe.transform(this.lstweekdays[4].Date, Constants.DateFormateMonthfirst)) {
          lstOverallTicketDetails[i].freezeStatus5 = this.lstweekdays[4].FreezeStatus;
        }
      if (lstOverallTicketDetails[i].timeSheetDate6 == null || this.datePipe.transform(lstOverallTicketDetails[i].timeSheetDate6, Constants.DateFormateMonthfirst)
          == this.datePipe.transform(this.lstweekdays[5].Date, Constants.DateFormateMonthfirst)) {
          lstOverallTicketDetails[i].freezeStatus6 = this.lstweekdays[5].FreezeStatus;
        }
      if (lstOverallTicketDetails[i].timeSheetDate7 == null || this.datePipe.transform(lstOverallTicketDetails[i].timeSheetDate7, Constants.DateFormateMonthfirst)
          == this.datePipe.transform(this.lstweekdays[6].Date, Constants.DateFormateMonthfirst)) {
          lstOverallTicketDetails[i].freezeStatus7 = this.lstweekdays[6].FreezeStatus;
        }
      let Flag = 0;

      if (lstOverallTicketDetails[i].isAHTicket == '1' ) {
        if(lstOverallTicketDetails[i].lstServiceModel.filter(z=>(z.serviceId == 3 || z.serviceId == 11)).length > 0){
          Flag = 1;
          lstOverallTicketDetails[i].lstServiceModel =
            lstOverallTicketDetails[i].lstServiceModel.filter(z => (z.serviceId == 3 || z.serviceId == 11));
        }
        lstOverallTicketDetails[i].lstStatusDetails = 
        lstOverallTicketDetails[i].lstStatusDetails.filter(h => (h.dartStatusId != 7 && h.dartStatusId != 5 && h.dartStatusId != 13))
      }
      if (Flag == 0 &&  lstOverallTicketDetails[i].type== "T" && lstOverallTicketDetails[i].supportTypeId != 2 ) {
        if (lstOverallTicketDetails[i].lstUserLevelDetails != null && lstOverallTicketDetails[i].lstUserLevelDetails.length > 0) {
          if (lstOverallTicketDetails[i].lstTicketTypeServiceDetails != null && lstOverallTicketDetails[i].lstTicketTypeServiceDetails.length > 0) {
          //userlevel logic
          this.ServiceList = [];
          this.userLevelServiceList = [];
          this.serviceTickettypelist = [];
          let serttttypelist = [];
          let ticketTypeMapId = lstOverallTicketDetails[i].ticketTypeMapId
          this.ServiceList  = lstOverallTicketDetails[i].lstTicketTypeServiceDetails.filter(z=> z.ticketTypeMappingId == ticketTypeMapId);
         for( let y = 0; y < this.ServiceList.length; y++) {
          if(lstOverallTicketDetails[i].lstServiceModel.filter(z=>z.serviceId ==this.ServiceList[y].serviceId) != null 
          && lstOverallTicketDetails[i].lstServiceModel.filter(z=>z.serviceId ==this.ServiceList[y].serviceId).length > 0){
              serttttypelist.push(lstOverallTicketDetails[i].lstServiceModel.filter(z=>z.serviceId ==this.ServiceList[y].serviceId));
          }
          }
         if(serttttypelist.length > 0){
         for(let s = 0;s< serttttypelist.length;s++){
          this.serviceTickettypelist.push(serttttypelist[s][0]);
         }
        }
        let uselevellist = [];
         if(this.serviceTickettypelist.length > 0) {
            for( let g = 0; g < lstOverallTicketDetails[i].lstUserLevelDetails.length; g++){
              if(this.serviceTickettypelist.filter(h =>h.serviceLevelId == lstOverallTicketDetails[i].lstUserLevelDetails[g].serviceLevelId).length>0)
              uselevellist.push(this.serviceTickettypelist.filter(h =>h.serviceLevelId == lstOverallTicketDetails[i].lstUserLevelDetails[g].serviceLevelId));
            }
           if(uselevellist.length > 0){
             for(let t = 0;t < uselevellist.length; t++){
               for (let uk = 0; uk < uselevellist[t].length; uk++){
                 this.userLevelServiceList.push(uselevellist[t][uk]);
               }
             }
           }
            if (this.userLevelServiceList.length > 0){
            lstOverallTicketDetails[i].lstServiceModel = this.userLevelServiceList;
            }
            else {
              lstOverallTicketDetails[i].lstServiceModel = [];
              for(let v = 0; v < this.serviceTickettypelist.length; v++){
                lstOverallTicketDetails[i].lstServiceModel.push(this.serviceTickettypelist[v]);
               }
            }
          }
          else{
            for( let p = 0; p < lstOverallTicketDetails[i].lstUserLevelDetails.length; p++){
              this.userLevelServiceList = lstOverallTicketDetails[i].lstServiceModel.
              filter(h =>h.serviceLevelId == lstOverallTicketDetails[i].lstUserLevelDetails[p].serviceLevelId)
            }
            lstOverallTicketDetails[i].lstServiceModel = this.userLevelServiceList;
          }
        }
      }
        else {
          if(lstOverallTicketDetails[i].lstTicketTypeServiceDetails != null){
            this.ServiceList = [];
            this.serviceTickettypelist = [];
            let serttttypelist = [];
            this.ServiceList  = lstOverallTicketDetails[i].lstTicketTypeServiceDetails.filter(z=> z.ticketTypeMappingId == lstOverallTicketDetails[i].ticketTypeMapId);
            for (let q = 0; q < this.ServiceList.length; q++) {
              if (lstOverallTicketDetails[i].lstServiceModel.filter(z => z.serviceId == this.ServiceList[q].serviceId) != null
                && lstOverallTicketDetails[i].lstServiceModel.filter(z => z.serviceId == this.ServiceList[q].serviceId).length > 0) {
                    serttttypelist.push(lstOverallTicketDetails[i].lstServiceModel.filter(z => z.serviceId == this.ServiceList[q].serviceId));
              }
            }
           if(serttttypelist.length > 0){
            for(let s = 0;s< serttttypelist.length;s++){
             this.serviceTickettypelist.push(serttttypelist[s][0]);
            }
           }
           if(this.serviceTickettypelist.length > 0){
            lstOverallTicketDetails[i].lstServiceModel = [];
           for(let w = 0; w < this.serviceTickettypelist.length; w++){
            lstOverallTicketDetails[i].lstServiceModel.push(this.serviceTickettypelist[w]);
           }
          }
      }
    }
      }
      if(lstOverallTicketDetails[i].lstServiceModel != null && lstOverallTicketDetails[i].lstServiceModel.length == 0){
        lstOverallTicketDetails[i].ServiceTitle = Constants.ServiceMappingUnAvailable;
      }
      if ((lstOverallTicketDetails[i].supportTypeId == 1 && lstOverallTicketDetails[i].type == "T")  || (lstOverallTicketDetails[i].type == "W")){
        if(lstOverallTicketDetails[i].serviceId != 0){
          if(lstOverallTicketDetails[i].lstServiceModel.filter(r => r.serviceId == lstOverallTicketDetails[i].serviceId).length == 0){
            lstOverallTicketDetails[i].ServiceTitle = Constants.ServiceInactivated;
            lstOverallTicketDetails[i].serviceId = 0;
            lstOverallTicketDetails[i].activityId = 0;
          }
        }
      }
      if(lstOverallTicketDetails[i].isGracePeriodMet && lstOverallTicketDetails[i].type == 'T'){
        lstOverallTicketDetails[i].ServiceTitle = Constants.GracePeriodMessage;
        lstOverallTicketDetails[i].StatusTitle = Constants.GracePeriodMessage;
      }
       //Activity
     if((lstOverallTicketDetails[i].type == 'T' && lstOverallTicketDetails[i].supportTypeId == 1) || lstOverallTicketDetails[i].type == 'W'){
      if(lstOverallTicketDetails[i].lstServiceModel.length == 1 && lstOverallTicketDetails[i].serviceId == 0){
        lstOverallTicketDetails[i].serviceId = lstOverallTicketDetails[i].lstServiceModel[0].serviceId;
      }
     let bkpindex = this.bkpticketdetails.findIndex(e => e.ticketId == lstOverallTicketDetails[i].ticketId && e.projectId == lstOverallTicketDetails[i].projectId)
      lstOverallTicketDetails[i].lstActivityModel = 
         this.bkpticketdetails[bkpindex].lstActivityModel.filter(r => r.serviceId == lstOverallTicketDetails[i].serviceId)
       lstOverallTicketDetails[i].ticketIDTitle = lstOverallTicketDetails[i].ticketId; 
       if(lstOverallTicketDetails[i].lstActivityModel.length == 1){
        lstOverallTicketDetails[i].activityId = lstOverallTicketDetails[i].lstActivityModel[0].activityId;
       }
      }
      if((new Date(this.currentendweekdate) < new Date(this.datePipe.transform(localStorage.getItem("Firstdayofweek"), 'MM/dd/yyyy'))) && 
       (this.datePipe.transform(localStorage.getItem("Firstdayofweek"), 'MM/dd/yyyy') != this.currentstartweekdate)){
        this.futuredisable = true;
      }
      if(lstOverallTicketDetails[i].type == "ND"){
        lstOverallTicketDetails[i].ticketIDTitle = Constants.ProjectName + this.userDetails.find(u => u.projectId == lstOverallTicketDetails[i].projectId).projectName 
        + Constants.ProjectID + this.userDetails.find(u => u.projectId == lstOverallTicketDetails[i].projectId).esaProjectId;
        if (lstOverallTicketDetails[i].activityId == 1){
          let validate = this.IsDaily == true ? true : this.lstweekdays.filter(x => x.StatusID == "2" || x.StatusID == "3" || x.StatusID == "6").length > 0 ? false : true;
          if (validate) {
            if (new Date(this.datePipe.transform(this.lstweekdays[0].Date, Constants.DateFormateMonthfirst)) > new Date(this.Today_date) && 
            (this.lstweekdays[0].StatusID == "0" || 
            this.lstweekdays[0].StatusID == "1" || this.lstweekdays[0].Date == "4" || this.lstweekdays[0].Date == "5" )) {
              lstOverallTicketDetails[i].freezeStatus1 = "false";
              this.lstweekdays[0].EnabeheaderND = "false";
            }
            if (new Date(this.datePipe.transform(this.lstweekdays[1].Date, Constants.DateFormateMonthfirst)) > new Date(this.Today_date) && 
            (this.lstweekdays[1].StatusID == "0" || this.lstweekdays[1].StatusID == "1" || this.lstweekdays[1].Date == "4" || this.lstweekdays[1].Date == "5")) {
              lstOverallTicketDetails[i].freezeStatus2 = "false";
              this.lstweekdays[1].EnabeheaderND = "false";
            }
            if (new Date(this.datePipe.transform(this.lstweekdays[2].Date, Constants.DateFormateMonthfirst)) > new Date(this.Today_date) && 
            (this.lstweekdays[2].StatusID == "0" || this.lstweekdays[2].StatusID == "1" || this.lstweekdays[2].Date == "4" || this.lstweekdays[2].Date == "5")) {
              lstOverallTicketDetails[i].freezeStatus3 = "false";
              this.lstweekdays[2].EnabeheaderND = "false";
            }
            if (new Date(this.datePipe.transform(this.lstweekdays[3].Date, Constants.DateFormateMonthfirst)) > new Date(this.Today_date) && 
            (this.lstweekdays[3].StatusID == "0" || this.lstweekdays[3].StatusID == "1" || this.lstweekdays[3].Date == "4" || this.lstweekdays[3].Date == "5")) {
              lstOverallTicketDetails[i].freezeStatus4 = "false";
              this.lstweekdays[3].EnabeheaderND = "false";
            }
            if (new Date(this.datePipe.transform(this.lstweekdays[4].Date, Constants.DateFormateMonthfirst)) > new Date(this.Today_date) && 
            (this.lstweekdays[4].StatusID == "0" || this.lstweekdays[4].StatusID == "1" || this.lstweekdays[4].Date == "4" || this.lstweekdays[4].Date == "5")) {
              lstOverallTicketDetails[i].freezeStatus5 = "false";
              this.lstweekdays[4].EnabeheaderND = "false";
            }
            if (new Date(this.datePipe.transform(this.lstweekdays[5].Date, Constants.DateFormateMonthfirst)) > new Date(this.Today_date) && 
            (this.lstweekdays[5].StatusID == "0" || this.lstweekdays[5].StatusID == "1" || this.lstweekdays[5].Date == "4" || this.lstweekdays[5].Date == "5")) {
              lstOverallTicketDetails[i].freezeStatus6 = "false";
              this.lstweekdays[5].EnabeheaderND = "false";
            }
            if (new Date(this.datePipe.transform(this.lstweekdays[6].Date, Constants.DateFormateMonthfirst)) > new Date(this.Today_date) && 
            (this.lstweekdays[6].StatusID == "0" || this.lstweekdays[6].StatusID == "1" || this.lstweekdays[6].Date == "4" || this.lstweekdays[6].Date == "5")) {
              lstOverallTicketDetails[i].freezeStatus7 = "false";
              this.lstweekdays[6].EnabeheaderND = "false";
            }
          }
        }
        lstOverallTicketDetails[i].lstServiceModel = [];
        lstOverallTicketDetails[i].lstActivityModel = [];
        lstOverallTicketDetails[i].lstStatusDetails = [];
       
      }
      if (lstOverallTicketDetails[i].supportTypeId == 2 && lstOverallTicketDetails[i].type == 'T') {
        lstOverallTicketDetails[i].ticketIDTitle = lstOverallTicketDetails[i].ticketId; 
        lstOverallTicketDetails[i].lstServiceModel = [];
        this.userLeveltaskList = [];
        if(lstOverallTicketDetails[i].lstUserLevelDetails != null && lstOverallTicketDetails[i].lstUserLevelDetails.length > 0){
          for( let j = 0; j < lstOverallTicketDetails[i].lstUserLevelDetails.length; j++){
            this.userLeveltaskList = lstOverallTicketDetails[i].lstTaskModel.filter(h =>h.serviceLevelId == lstOverallTicketDetails[i].lstUserLevelDetails[j].serviceLevelId)
          }
          if(this.userLeveltaskList.length > 0){
            lstOverallTicketDetails[i].lstTaskModel = this.userLeveltaskList;
          }

        }
        if(lstOverallTicketDetails[i].activityId != 0){
          if(lstOverallTicketDetails[i].lstTaskModel.filter(r => r.infraTransactionTaskId == lstOverallTicketDetails[i].activityId).length == 0){
            lstOverallTicketDetails[i].InfraTitle = Constants.TaskInactivated;
            lstOverallTicketDetails[i].activityId = 0;
          }
        }
        if(lstOverallTicketDetails[i].lstTaskModel.length == 1 && lstOverallTicketDetails[i].activityId == 0){
           lstOverallTicketDetails[i].activityId = lstOverallTicketDetails[i].lstTaskModel[0].infraTransactionTaskId;
        }
      }
      if(!this.IsCognizant && lstOverallTicketDetails[i].type == "W"){
        lstOverallTicketDetails[i].ticketTypeMapId = 0;
      }

        lstOverallTicketDetails[i].lstServiceModel.sort((a, b) => a.serviceName.localeCompare(b.serviceName))
      }      
  return lstOverallTicketDetails;
  }
  freezestatusDaily(dateparam: any): string {
    this.dateWeek = new Date();
    this.dateWeek.setDate(this.dateWeek.getDate() - (this.dateWeek.toString().split(" ")[0] == "Mon" ? this.Freezedatecountlogic + 2 : 
    this.dateWeek.toString().split(" ")[0] == "Tue" ? this.Freezedatecountlogic + 2 : this.Freezedatecountlogic));
    let T_date = this.datePipe.transform(this.dateWeek, Constants.DateFormateMonthfirst);
    let C_date = this.datePipe.transform(dateparam.Date, Constants.DateFormateMonthfirst);
    if (new Date(C_date) < new Date(T_date)) {
      return Constants.true;
    }
    else if (new Date(C_date) > new Date(this.Today_date)) {
      return Constants.true;
    }
    else {
      return Constants.false;
    }
    
  }
  freezestatusweekly(dateparam: any){
    this.weeklyvalidation = true;
    let date = new Date();
    this.previousStartDate = this.datePipe.transform(this.getPreviousSunday(this.getPreviousSaturday(date)), 'MM/dd/yyyy');
    this.previousendDate = this.datePipe.transform(this.getPreviousSaturday(date), 'MM/dd/yyyy');
    this.currDay = this.Todaydate.getDay();
    if ((this.currDay <= 3 && this.previousStartDate == this.datePipe.transform(new Date(this.lstweekdays[0].Date), 'MM/dd/yyyy')
      && this.previousendDate == this.datePipe.transform(new Date(this.lstweekdays[6].Date), 'MM/dd/yyyy'))
      || (this.currentstartweekdate == this.datePipe.transform(new Date(this.lstweekdays[0].Date), 'MM/dd/yyyy')
        && this.currentendweekdate == this.datePipe.transform(new Date(this.lstweekdays[6].Date), 'MM/dd/yyyy'))) {
      this.lstweekdays.forEach(x => {
        x.FreezeStatus = Constants.false;
      });
    }
    else {
      this.lstweekdays.forEach(x => {
        x.FreezeStatus = Constants.true;
      });
    }
  }

  getPreviousSaturday(d) {
    let t = new Date(d);
    t.setDate(t.getDate() + (6 - 7 - t.getDay()) % 7);
    return t;
  }

  getPreviousSunday(d) {
    let t = new Date(d);
    t.setDate(t.getDate() + (0 - 7 - t.getDay()) % 7);
    return t;
  }

  BindActivityDetails(e,td,index){
    this.servicelist = [];
    this.validateservicelist = this.ticketdetails.map(c => ({
      activityId: c.activityId,
      activityTitle: c.activityTitle,
      applicationId: c.applicationId,
      assignedTo: c.assignedTo,
      benchMarkColor: c.benchMarkColor,
      benchMarkTitle: c.benchMarkTitle,
      closedDate: c.closedDate,
      completedDate: c.completedDate,
      dartStatusId: c.dartStatusId,
      effort1: c.effort1,
      effort2: c.effort2,
      effort3: c.effort3,
      effort4: c.effort4,
      effort5: c.effort5,
      effort6: c.effort6,
      effort7: c.effort7,
      effortTillDate: c.effortTillDate,
      employeeId: c.employeeId,
      freezeStatus1: c.freezeStatus1,
      freezeStatus2: c.freezeStatus2,
      freezeStatus3: c.freezeStatus3,
      freezeStatus4: c.freezeStatus4,
      freezeStatus5: c.freezeStatus5,
      freezeStatus6: c.freezeStatus6,
      freezeStatus7: c.freezeStatus7,
      gracePeriod: c.gracePeriod,
      gridValidationservice: c.gridValidationservice,
      infraTitle:c.infraTitle,
      isAHTagged: c.isAHTagged,
      isAHTicket: c.isAHTicket,
      isALMToolConfigured: c.isALMToolConfigured,
      isActivityTracked: c.isActivityTracked,
      isAttributeUpdated: c.isAttributeUpdated,
      isCognizant: c.isCognizant,
      isDebtEnabled: c.isDebtEnabled,
      isDeleted: c.isDeleted,
      isEffortTracked: c.isEffortTracked,
      isFreezed: c.isFreezed,
      isGracePeriodMet: c.isGracePeriodMet,
      isMainspringConfigured: c.isMainspringConfigured,
      isNonTicket: c.isNonTicket,
      isSDTicket: c.isSDTicket,
      isTicket: c.isTicket,
      itsmEffort: c.itsmEffort,
      lstActivityModel: c.lstActivityModel,
      lstServiceModel: c.lstServiceModel,
      lstStatusDetails: c.lstStatusDetails,
      lstTaskModel: c.lstTaskModel,
      lstTicketTypeModel: c.lstTicketTypeModel,
      lstTicketTypeServiceDetails: c.lstTicketTypeServiceDetails,
      lstUserLevelDetails: c.lstUserLevelDetails,
      projectId: c.projectId,
      projectTimeZoneName: c.projectTimeZoneName,
      serviceId: c.serviceId,
      ServiceTitle: c.ServiceTitle,
      StatusTitle: c.StatusTitle,
      suggestedActivityName: c.suggestedActivityName,
      supportTypeId: c.supportTypeId,
      ticketDescription: c.ticketDescription,     
      ticketId: c.ticketId,
      ticketIDTitle: c.ticketIDTitle,
      ticketStatusMapId: c.ticketStatusMapId,
      ticketTypeMapId: c.ticketTypeMapId,
      timeSheetDate1: c.timeSheetDate1,
      timeSheetDate2: c.timeSheetDate2,
      timeSheetDate3: c.timeSheetDate3,
      timeSheetDate4: c.timeSheetDate4,
      timeSheetDate5: c.timeSheetDate5,
      timeSheetDate6: c.timeSheetDate6,
      timeSheetDate7: c.timeSheetDate7,
      timeSheetDetailId1: c.timeSheetDetailId1,
      timeSheetDetailId2: c.timeSheetDetailId2,
      timeSheetDetailId3: c.timeSheetDetailId3,
      timeSheetDetailId4: c.timeSheetDetailId4,
      timeSheetDetailId5: c.timeSheetDetailId5,
      timeSheetDetailId6: c.timeSheetDetailId6,
      timeSheetDetailId7: c.timeSheetDetailId7,
      timeSheetID4: c.timeSheetID4,
      timeSheetId1: c.timeSheetId1,
      timeSheetId2: c.timeSheetId2,
      timeSheetId3: c.timeSheetId3,
      timeSheetId5: c.timeSheetId5,
      timeSheetId6: c.timeSheetId6,
      timeSheetId7: c.timeSheetId7,
      timeTickerId: c.timeTickerId,
      towerId: c.towerId,
      type: c.type,
      userTimeZoneName: c.userTimeZoneName
    })
    );
    
    this.duplicateservicelist = this.validateservicelist.filter(x => x.projectId = td.projectId && x.type == td.type && x.ticketId == td.ticketId && x.serviceId != 0)
   if(this.duplicateservicelist.length > 1){
    for (let g = 0; g < this.duplicateservicelist.length; g++) {
      if(this.duplicateservicelist[g].serviceId != 0){
      this.servicelist.push(parseInt(this.duplicateservicelist[g].serviceId));
    }
    }
    if(this.servicelist.filter((item,iex) => this.servicelist.indexOf(item) === iex).length != 1){
      this.ticketdetails[index].serviceId = 0;
      e.target.value = 0;
      this.ticketdetails[index].lstActivityModel = [];
      if(td.type == 'W'){
        this.setValidationMessage('WorkItemsWithSameIdSameService');
        setTimeout(() => { this.ValidationMessage = '' }, 3000);
      }
      else {
        this.setValidationMessage('TicketsWithSameIdSameService');
        setTimeout(() => { this.ValidationMessage = '' }, 3000);
        }
    }
    else{
     this.UpdateService(e,index,td);
    }
  }
  else{
    this.UpdateService(e,index,td);
  }
  this.ticketdetails[index].activityId = 0;
  }
  UpdateService(e,index,td){
    this.spinneservice.show();
    if(td.type == 'W' && parseInt(e.target.value) != 0){
    this.updateWorkItemServiceandStatus(td.projectId, td.ticketId, td.timeTickerId, this.EmployeeId, parseInt(e.target.value), td.ticketStatusMapId);
    }
    else{
      this.updateIsAttributeByTicketId(td,parseInt(e.target.value),td.ticketStatusMapId,td.closedDate,td.completedDate,this.mode,false,this.closeStatus);
      }
    this.ticketdetails[index].lstActivityModel = 
    this.ServiceActivityDetails.filter(n=> n.projectId == td.projectId && n.serviceId == td.serviceId);

  }
  UpdateStatus(e,td){
    this.spinneservice.show();
    if(td.type == 'W' && parseInt(e.target.value) != 0){
    this.updateWorkItemServiceandStatus(td.projectId, td.ticketId, td.timeTickerId, this.EmployeeId, td.serviceId, parseInt(e.target.value));
    }
    else{
      this.updateIsAttributeByTicketId(td,td.serviceId,parseInt(e.target.value),td.closedDate,td.completedDate,this.mode,false,this.closeStatus);
      }
  }
  copygriddata(list){
    if (list.type != "ND" && this.copyrestrict != true) {
      this.copylist = [];
      this.copylist.push(list);
      this.copylistprocessed = this.copylist.map(c => ({
        activityId: c.activityId,
        activityTitle: c.activityTitle,
        applicationId: c.applicationId,
        assignedTo: c.assignedTo,
        benchMarkColor: c.benchMarkColor,
        benchMarkTitle: c.benchMarkTitle,
        closedDate: c.closedDate,
        completedDate: c.completedDate,
        dartStatusId: c.dartStatusId,
        effort1: c.effort1,
        effort2: c.effort2,
        effort3: c.effort3,
        effort4: c.effort4,
        effort5: c.effort5,
        effort6: c.effort6,
        effort7: c.effort7,
        effortTillDate: c.effortTillDate,
        employeeId: c.employeeId,
        freezeStatus1: c.freezeStatus1,
        freezeStatus2: c.freezeStatus2,
        freezeStatus3: c.freezeStatus3,
        freezeStatus4: c.freezeStatus4,
        freezeStatus5: c.freezeStatus5,
        freezeStatus6: c.freezeStatus6,
        freezeStatus7: c.freezeStatus7,
        gracePeriod: c.gracePeriod,
        gridValidationservice: c.gridValidationservice,
        infraTitle:c.infraTitle,
        isAHTagged: c.isAHTagged,
        isAHTicket: c.isAHTicket,
        isALMToolConfigured: c.isALMToolConfigured,
        isActivityTracked: c.isActivityTracked,
        isAttributeUpdated: c.isAttributeUpdated,
        isCognizant: c.isCognizant,
        isDebtEnabled: c.isDebtEnabled,
        isDeleted: c.isDeleted,
        isEffortTracked: c.isEffortTracked,
        isFreezed: c.isFreezed,
        isGracePeriodMet: c.isGracePeriodMet,
        isMainspringConfigured: c.isMainspringConfigured,
        isNonTicket: c.isNonTicket,
        isSDTicket: c.isSDTicket,
        isTicket: c.isTicket,
        itsmEffort: c.itsmEffort,
        lstActivityModel: c.lstActivityModel,
        lstServiceModel: c.lstServiceModel,
        lstStatusDetails: c.lstStatusDetails,
        lstTaskModel: c.lstTaskModel,
        lstTicketTypeModel: c.lstTicketTypeModel,
        lstTicketTypeServiceDetails: c.lstTicketTypeServiceDetails,
        lstUserLevelDetails: c.lstUserLevelDetails,
        projectId: c.projectId,
        projectTimeZoneName: c.projectTimeZoneName,
        serviceId: c.serviceId,
        ServiceTitle: c.ServiceTitle,
        StatusTitle: c.StatusTitle,
        suggestedActivityName: c.suggestedActivityName,
        supportTypeId: c.supportTypeId,
        ticketDescription: c.ticketDescription,     
        ticketId: c.ticketId,
        ticketIDTitle: c.ticketIDTitle,
        ticketStatusMapId: c.ticketStatusMapId,
        ticketTypeMapId: c.ticketTypeMapId,
        timeSheetDate1: c.timeSheetDate1,
        timeSheetDate2: c.timeSheetDate2,
        timeSheetDate3: c.timeSheetDate3,
        timeSheetDate4: c.timeSheetDate4,
        timeSheetDate5: c.timeSheetDate5,
        timeSheetDate6: c.timeSheetDate6,
        timeSheetDate7: c.timeSheetDate7,
        timeSheetDetailId1: c.timeSheetDetailId1,
        timeSheetDetailId2: c.timeSheetDetailId2,
        timeSheetDetailId3: c.timeSheetDetailId3,
        timeSheetDetailId4: c.timeSheetDetailId4,
        timeSheetDetailId5: c.timeSheetDetailId5,
        timeSheetDetailId6: c.timeSheetDetailId6,
        timeSheetDetailId7: c.timeSheetDetailId7,
        timeSheetID4: c.timeSheetID4,
        timeSheetId1: c.timeSheetId1,
        timeSheetId2: c.timeSheetId2,
        timeSheetId3: c.timeSheetId3,
        timeSheetId5: c.timeSheetId5,
        timeSheetId6: c.timeSheetId6,
        timeSheetId7: c.timeSheetId7,
        timeTickerId: c.timeTickerId,
        towerId: c.towerId,
        type: c.type,
        userTimeZoneName: c.userTimeZoneName
      })
      );

      this.copylistprocessed[0].activityId = 0;
      this.copylistprocessed[0].effort1 = "";
      this.copylistprocessed[0].effort2 = "";
      this.copylistprocessed[0].effort3 = "";
      this.copylistprocessed[0].effort4 = "";
      this.copylistprocessed[0].effort5 = "";
      this.copylistprocessed[0].effort6 = "";
      this.copylistprocessed[0].effort7 = "";
      this.copylistprocessed[0].timeSheetDetailId1 = 0;
      this.copylistprocessed[0].timeSheetDetailId2 = 0;
      this.copylistprocessed[0].timeSheetDetailId3 = 0;
      this.copylistprocessed[0].timeSheetDetailId4 = 0;
      this.copylistprocessed[0].timeSheetDetailId5 = 0;
      this.copylistprocessed[0].timeSheetDetailId6 = 0;
      this.copylistprocessed[0].timeSheetDetailId7 = 0;
      this.bkpticketdetails.push(this.copylistprocessed[0]);
      this.ticketdetails.push(this.copylistprocessed[0]);
    }
  }
  servicActivityValidation(ticketdata) {
    if (this.IsCognizant) {
        this.ticketdetails.forEach(x => {
          ((x.serviceId == "0" && x.supportTypeId == 1) || x.activityId == "0" || x.ticketStatusMapId == "0") && x.type != "ND" ?
            (x.effort1 == "" && x.effort2 == "" && x.effort3 == ""
              && x.effort4 == "" && x.effort5 == "" && x.effort6 == "" && x.effort7 == "") ?
              x.gridValidationservice = false : x.gridValidationservice = true : x.gridValidationservice = false;
        });

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
   
  setBUBenchMarkNotMet(pattern: string): void { 
    this.translate.get(pattern)
          .subscribe(message => {           
            this.BUBenchMarkNotMet = message;          
    });    
  }
  setBUBenchMarkMet(pattern: string): void { 
    this.translate.get(pattern)
          .subscribe(message => {           
            this.BUBenchMarkMet = message;          
    });    
  }  
  setSuccessMessage(pattern: string): void { 
    this.translate.get(pattern)
          .subscribe(message => {           
            this.SuccessMessage = message;
            setTimeout(() => {
              this.SuccessMessage = '';
            }, 4000);
    });    
  }
  savegriddata(savedatainput: any, flag: number){
    if(savedatainput != null && savedatainput.length > 0){
    for (let i = 0; i < savedatainput.length; i++) {
      if (savedatainput[i].type == "ND") {
        savedatainput[i].activityId = this.bkpticketdetails[i].activityId;
      }
    }
    this.sumoftotaleffort(savedatainput);
    this.ischeckboxselected();
    this.mandatoryhourvalidation();
    this.servicActivityValidation(savedatainput);
    let isDaily = this.IsDaily ? "1" : "0";
    let Params = {
      lstticketData: this.SerializeData(savedatainput),
      EmployeeID: this.EmployeeId,
      IsDaily: isDaily,
      Flag: flag
    }
    if (this.ticketdetails.filter(t => t.gridValidationservice).length <= 0) {
      if (this.hoursvalidation) {
        if (flag == 1) {
          this.spinneservice.show();
          for(let i = 0;i<Params.lstticketData.length;i++){
            if (Params.lstticketData[i].Type == "ND"){
              Params.lstticketData[i].ActivityID = this.bkpticketdetails[i].activityId;
              for(let j=0; j<7; j++){
                Params.lstticketData[i].TimesheetDetails[j].ActivityID = this.bkpticketdetails[i].activityId;
              }
            }
          }
          this.dynamicgridservice.SaveGridData(Params).subscribe(x => {
            if (x && flag == 1) {
              this.headerService.languageDataEmitter.next(this.Todaydate);
              this.Loadgriddata();
              this.spinneservice.hide();
              this.setSuccessMessage('TimeSheetSavedSuccessfully');
              setTimeout(() => { this.SuccessMessage = '' }, 3000);
            }
          })
        }
        if (flag == 2) {
          if (this.ischeckboxchecked) {
            if (this.dailyhourmandatoryhour) {
              if (!this.mandatoryattribute) {
                this.spinneservice.show();
                for(let i = 0;i<Params.lstticketData.length;i++){
                  if (Params.lstticketData[i].Type == "ND"){
                    Params.lstticketData[i].ActivityID = this.bkpticketdetails[i].activityId;
                    for(let j=0; j<7; j++){
                      Params.lstticketData[i].TimesheetDetails[j].ActivityID = this.bkpticketdetails[i].activityId;
                    }
                  }
                }
                this.dynamicgridservice.SaveGridData(Params).subscribe(x => {
                  if (x && flag == 2) {
                    this.headerService.languageDataEmitter.next(this.Todaydate);
                    this.Loadgriddata();
                    this.spinneservice.hide();
                    this.setSuccessMessage('TimeSheetSubmittedSuccessfully');
                    setTimeout(() => { this.SuccessMessage = '' }, 3000);
                  }
                })
              }
              else {
                this.setValidationMessage('Mandate attributes should be filled');
                setTimeout(() => { this.ValidationMessage = '' }, 3000);
              }
            }
            else {
              this.setValidationMessage('PleaseCaptureMandatoryHours');
              setTimeout(() => { this.ValidationMessage = '' }, 3000);
            }
          }
          else {
            this.setValidationMessage('SelectOneDayToSubmitTimesheet');
            setTimeout(() => { this.ValidationMessage = '' }, 3000);
          }
        }
      }
      else {
        this.setValidationMessage('TheTotalEffortCannotExceed24hrs');
        setTimeout(() => { this.ValidationMessage = '' }, 3000);
      }
    }
    else{
      this.setValidationMessage('PleaseSelectHighlightedRowsToEnterTimeSheet');
      setTimeout(() => { this.ValidationMessage = '' }, 3000);
    }
  }
  }
  oncheckboxclick(data : any){
    data.IsChanged = data.IsChanged == Constants.false ? Constants.true : Constants.false
  }
  ischeckboxselected(){
    if (this.IsDaily) {
      this.ischeckboxchecked = this.lstweekdays.filter(t => t.IsChanged == Constants.true).length > 0 ? true : false
    }
    else{
      this.ischeckboxchecked = true;
    }
  }
  SerializeData(lstdata: any):any {
    let TimesheetId;
    let TimesheetDate;
    let Effort;
    let TimeSheetDetailId;
    this.ticketdata = [];
    let IsFreezed;
    let IsChanged;
   
    for(let d = 0; d < lstdata.length; d++){
      this.timesheetdata = [];
      let userdetails = [];
      userdetails = this.userDetails.filter(x=>x.projectId == lstdata[d].projectId);
      let userId = parseInt(lstdata[d].assignedTo);      
      if(lstdata[d].type == "W"){
        lstdata[d].ticketTypeMapId = 0;
        }
      if(lstdata[d].itsmEffort == ""){
        lstdata[d].itsmEffort = 0
      }
      if(lstdata[d].assignedTo == ""){
        lstdata[d].assignedTo = 0
      }
      if(lstdata[d].effortTillDate == ""){
        lstdata[d].effortTillDate = 0
      }
      for (let i = 0; i < 7; i++) {
        if(i == 0){
          TimesheetId = lstdata[d].timeSheetId1;
          TimesheetDate = this.lstweekdays[0].Date;
          Effort =lstdata[d].effort1;
          TimeSheetDetailId = lstdata[d].timeSheetDetailId1;
          IsChanged = this.lstweekdays[0].IsChanged;
          IsFreezed = lstdata[d].isNonTicket == "1" && lstdata[d].activityId == 1 ?
            (this.lstweekdays[0].StatusID == "2" || this.lstweekdays[0].StatusID == "3" || 
            this.lstweekdays[0].StatusID == "6") ? this.lstweekdays[0].FreezeStatus : Constants.false : this.lstweekdays[0].FreezeStatus;
        }
        else if(i == 1){
          TimesheetId = lstdata[d].timeSheetId2;
          TimesheetDate = this.lstweekdays[1].Date;
          Effort =lstdata[d].effort2;
          TimeSheetDetailId = lstdata[d].timeSheetDetailId2;
          IsChanged = this.lstweekdays[1].IsChanged;
          IsFreezed = lstdata[d].isNonTicket == "1" && lstdata[d].activityId == 1 ?
            (this.lstweekdays[1].StatusID == "2" || this.lstweekdays[1].StatusID == "3" || 
            this.lstweekdays[1].StatusID == "6") ? this.lstweekdays[1].FreezeStatus : Constants.false : this.lstweekdays[1].FreezeStatus;
        }
        else if(i == 2){
          TimesheetId = lstdata[d].timeSheetId3;
          TimesheetDate = this.lstweekdays[2].Date;
          Effort =lstdata[d].effort3;
          TimeSheetDetailId = lstdata[d].timeSheetDetailId3;
          IsChanged = this.lstweekdays[2].IsChanged;
          IsFreezed = lstdata[d].isNonTicket == "1" && lstdata[d].activityId == 1 ?
            (this.lstweekdays[2].StatusID == "2" || this.lstweekdays[2].StatusID == "3" || 
            this.lstweekdays[2].StatusID == "6") ? this.lstweekdays[2].FreezeStatus : Constants.false : this.lstweekdays[2].FreezeStatus;
        }
        else if(i == 3){
          TimesheetId = lstdata[d].timeSheetID4;
          TimesheetDate = this.lstweekdays[3].Date;
          Effort =lstdata[d].effort4;
          TimeSheetDetailId = lstdata[d].timeSheetDetailId4;
          IsChanged = this.lstweekdays[3].IsChanged;
          IsFreezed = this.lstweekdays[3].FreezeStatus;
          IsFreezed = lstdata[d].isNonTicket == "1" && lstdata[d].activityId == 1 ?
            (this.lstweekdays[3].StatusID == "2" || this.lstweekdays[3].StatusID == "3" || 
            this.lstweekdays[3].StatusID == "6") ? this.lstweekdays[3].FreezeStatus : Constants.false : this.lstweekdays[3].FreezeStatus;
        }
        else if(i == 4){
          TimesheetId = lstdata[d].timeSheetId5;
          TimesheetDate = this.lstweekdays[4].Date;
          Effort =lstdata[d].effort5;
          TimeSheetDetailId = lstdata[d].timeSheetDetailId5;
          IsChanged = this.lstweekdays[4].IsChanged;
          IsFreezed = lstdata[d].isNonTicket == "1" && lstdata[d].activityId == 1 ?
            (this.lstweekdays[4].StatusID == "2" || this.lstweekdays[4].StatusID == "3" || 
            this.lstweekdays[4].StatusID == "6") ? this.lstweekdays[4].FreezeStatus : Constants.false : this.lstweekdays[4].FreezeStatus;
        }
        else if(i == 5){
          TimesheetId = lstdata[d].timeSheetId6;
          TimesheetDate = this.lstweekdays[5].Date;
          Effort =lstdata[d].effort6;
          TimeSheetDetailId = lstdata[d].timeSheetDetailId6;
          IsChanged = this.lstweekdays[5].IsChanged;
          IsFreezed = lstdata[d].isNonTicket == "1" && lstdata[d].activityId == 1 ?
            (this.lstweekdays[5].StatusID == "2" || this.lstweekdays[5].StatusID == "3" || 
            this.lstweekdays[5].StatusID == "6") ? this.lstweekdays[5].FreezeStatus : Constants.false : this.lstweekdays[5].FreezeStatus;
        }
        else{
          TimesheetId = lstdata[d].timeSheetId7;
          TimesheetDate = this.lstweekdays[6].Date;
          Effort =lstdata[d].effort7;
          TimeSheetDetailId = lstdata[d].timeSheetDetailId7;
          IsChanged = this.lstweekdays[6].IsChanged;
          IsFreezed = lstdata[d].isNonTicket == "1" && lstdata[d].activityId == 1 ?
            (this.lstweekdays[6].StatusID == "2" || this.lstweekdays[6].StatusID == "3" || 
            this.lstweekdays[6].StatusID == "6") ? this.lstweekdays[6].FreezeStatus : Constants.false : this.lstweekdays[6].FreezeStatus;
        }
        if(Effort == ""){
          Effort = 0;
        }
        if(TimesheetId == null){
          TimesheetId = 0;
        }
        if(lstdata[d].isNonTicket == "0"){
          lstdata[d].isNonTicket = false;
        }
        else{
          lstdata[d].isNonTicket = true;
        }
        let TimesheetParams = {
          ActivityID: parseInt(lstdata[d].activityId),
          ApplicationID: lstdata[d].applicationId,
          CustomerID: parseInt(this.CustomerId),
          Hours: parseFloat(Effort),
          IsChanged: IsChanged,
          IsFreezed: IsFreezed,
          IsNonTicket: lstdata[d].isNonTicket,
          ProjectID: lstdata[d].projectId,
          ServiceID: parseInt(lstdata[d].serviceId),
          StatusID: null,
          SuggestedActivityName: lstdata[d].suggestedActivityName,
          SupportTypeID: lstdata[d].supportTypeId,
          TicketDescription: lstdata[d].ticketDescription,
          TicketID: lstdata[d].ticketId,
          TicketStatus: parseInt(lstdata[d].ticketStatusMapId),
          TicketType: parseInt(lstdata[d].ticketTypeMapId),
          TimeSheetID: parseInt(TimesheetId),
          TimeTickerID: lstdata[d].timeTickerId,
          TimesheetDate: TimesheetDate,
          TimesheetDetailID: TimeSheetDetailId,
          TowerID: lstdata[d].towerId,
          Type: lstdata[d].type,
          UserID: parseInt(lstdata[d].assignedTo)
    
        }
        this.timesheetdata.push(TimesheetParams);
       
      }
    

      let TicketParams = {
    ActivityID: parseInt(lstdata[d].activityId),
    ApplicationID: lstdata[d].applicationId,
    DARTStatusID: parseInt(lstdata[d].dartStatusId),
    ITSMEffort: parseFloat(lstdata[d].itsmEffort),
    ProjectID: lstdata[d].projectId,
    ServiceID: parseInt(lstdata[d].serviceId),
    SupportTypeID: lstdata[d].supportTypeId,
    TicketDescription: lstdata[d].ticketDescription,
    TicketID: lstdata[d].ticketId,
    TicketStatus: parseInt(lstdata[d].ticketStatusMapId),
    TicketType: parseInt(lstdata[d].ticketTypeMapId),
    TimeTickerID: lstdata[d].timeTickerId,
    TimesheetDetails: this.timesheetdata,
    TotalEffort: parseFloat(lstdata[d].effortTillDate),
    Type: lstdata[d].type,
    UserID: parseInt(lstdata[d].assignedTo)
    }
    this.timesheetdata = [];
    this.ticketdata.push(TicketParams);
  }
  return this.ticketdata;
  }
  CheckforDuplicateActivity(td,indx){
    this.duplicatelist = [];    
    this.duplicatelist = this.ticketdetails.filter(x => x.projectId == td.projectId && x.type == td.type && x.ticketId == td.ticketId && x.activityId != 0)

    this.actvitylist = [];
    for (let g = 0; g < this.duplicatelist.length; g++) {
      this.actvitylist.push(parseInt(this.duplicatelist[g].activityId));
    }
    let actvitylength = this.actvitylist.length;
    if(this.actvitylist.filter((item,index) => this.actvitylist.indexOf(item) === index).length != actvitylength){
      this.ticketdetails[indx].activityId = 0;
      if(td.type == 'T' && td.supportTypeId != 2){
      this.setValidationMessage('TicketsWithSameIdSameActivity');
      setTimeout(() => { this.ValidationMessage = '' }, 3000);
      }
      else if(td.type == 'W')
      {
        this.setValidationMessage('WorkItemWithSameIdSameActivity');
      setTimeout(() => { this.ValidationMessage = '' }, 3000);
      }
      else{
        this.setValidationMessage('TicketsWithSameTask');
        setTimeout(() => { this.ValidationMessage = '' }, 3000);
      }
    }
   
  
  }
  ConformationNo(){
    this.Deleteticket[0] = [];
    this.Deletepopup = false;
  }
  ConformationYes(){
    if ((this.IsCognizant ? this.Deleteticket.activityId : 1) > 0 && (this.ticketdetails[this.Deleteindex].effort1 != "" || 
    this.ticketdetails[this.Deleteindex].effort2 != "" || this.ticketdetails[this.Deleteindex].effort3 != ""
      || this.ticketdetails[this.Deleteindex].effort4 != "" || this.ticketdetails[this.Deleteindex].effort5 || 
      this.ticketdetails[this.Deleteindex].effort6 || this.ticketdetails[this.Deleteindex].effort7)) {
        let Params = {
        ProjectID: this.Deleteticket.projectId.toString(),
        CustomerID: parseInt(this.CustomerId),
        EmployeeID: this.EmployeeId.toString(),
        TimeTickerID: this.Deleteticket.timeTickerId.toString(),
        TicketID: this.Deleteticket.ticketId,
        ServiceID: this.Deleteticket.serviceId.toString(),
        ActivityID: this.Deleteticket.activityId,
        StartDate: this.datePipe.transform(localStorage.getItem("Firstdayofweek"), 'MM/dd/yyyy'),
        EndDate: this.datePipe.transform(localStorage.getItem("Lastdayofweek"), 'MM/dd/yyyy'),
        SubmitterID: "",
        TxtHours: this.deletetotaleffort.toFixed(2),
        TickSupportTypeID: this.Deleteticket.supportTypeId,
        Type: this.Deleteticket.type
      }
      this.spinneservice.show();
      this.dynamicgridservice.DeleteGridData(Params).subscribe(x => {
        this.ticketdetails.splice(this.Deleteindex, 1);
        this.bkpticketdetails.splice(this.Deleteindex, 1);
        this.spinneservice.hide();
        this.Deletepopup = false;
      })
    }
    else {
      this.ticketdetails.splice(this.Deleteindex,1);
      this.bkpticketdetails.splice(this.Deleteindex,1);
      this.Deletepopup = false;
    }
  }
  deletegriddata(ticketdata: any, index: any): void{    
    let deleteflag = true;
    this.Deleteindex = index;
    this.deletetotaleffort = 0;
    if (this.lstweekdays.filter(w => w.StatusID == "2" || w.StatusID == "3" || w.StatusID == "6").length <= 0) {
      this.Deleteticket = ticketdata;
      deleteflag = true;
    }
    else {
      let datamode = this.lstweekdays.filter(w => w.StatusID == "2" || w.StatusID == "3" || w.StatusID == "6")
      for (let r = 0; r < datamode.length; r++) {
        if (datamode[r].Day == Constants.Day1 && parseInt(ticketdata.effort1) > 0) {
          deleteflag = false;
        }
        else if (datamode[r].Day == Constants.Day2 && parseInt(ticketdata.effort2) > 0) {
          deleteflag = false;
        }
        else if (datamode[r].Day == Constants.Day3 && parseInt(ticketdata.effort3) > 0) {
          deleteflag = false;
        }
        else if (datamode[r].Day == Constants.Day4 && parseInt(ticketdata.effort4) > 0) {
          deleteflag = false;
        }
        else if (datamode[r].Day == Constants.Day5 && parseInt(ticketdata.effort5) > 0) {
          deleteflag = false;
        }
        else if (datamode[r].Day == Constants.Day6 && parseInt(ticketdata.effort6) > 0) {
          deleteflag = false;
        }
        else if (datamode[r].Day == Constants.Day7 && parseInt(ticketdata.effort7) > 0) {
          deleteflag = false;
        }
      }
    }
   
    if (deleteflag) {
      this.Deletepopup = true;

      ticketdata.effort1 != "" ? this.deletetotaleffort += parseFloat(ticketdata.effort1) : this.deletetotaleffort = this.deletetotaleffort;
      ticketdata.effort2 != "" ? this.deletetotaleffort += parseFloat(ticketdata.effort2) : this.deletetotaleffort = this.deletetotaleffort;
      ticketdata.effort3 != "" ? this.deletetotaleffort += parseFloat(ticketdata.effort3) : this.deletetotaleffort = this.deletetotaleffort;
      ticketdata.effort4 != "" ? this.deletetotaleffort += parseFloat(ticketdata.effort4) : this.deletetotaleffort = this.deletetotaleffort;
      ticketdata.effort5 != "" ? this.deletetotaleffort += parseFloat(ticketdata.effort5) : this.deletetotaleffort = this.deletetotaleffort;
      ticketdata.effort6 != "" ? this.deletetotaleffort += parseFloat(ticketdata.effort6) : this.deletetotaleffort = this.deletetotaleffort;
      ticketdata.effort7 != "" ? this.deletetotaleffort += parseFloat(ticketdata.effort7) : this.deletetotaleffort = this.deletetotaleffort;

      this.Deleteticket = ticketdata;
      this.Deleteticket.activityId = parseInt(ticketdata.activityId);
      if (ticketdata.type == "ND"){
        this.Deleteticket.activityId = this.bkpticketdetails.find(s => s.ticketId == ticketdata.ticketId && s.projectId == ticketdata.projectId
          && s.ticketDescription == ticketdata.ticketDescription).activityId
      }
    }
    else {
      this.setValidationMessage('TimesheetCannotBeDeleted');
      setTimeout(() => { this.ValidationMessage = '' }, 3000);
      
    }
  }
  cleartotalhours() {
    this.Totaleffort1 = "";
    this.Totaleffort2= "";
    this.Totaleffort3= "";
    this.Totaleffort4= "";
    this.Totaleffort5= "";
    this.Totaleffort6= "";
    this.Totaleffort7 = "";
    this.effort1 = 0.00;
    this.effort2 = 0.00;
    this.effort3 = 0.00;
    this.effort4 = 0.00;
    this.effort5 = 0.00;
    this.effort6 = 0.00;
    this.effort7 = 0.00;
    
  }
  onChange(date: any): void{  
    
    this.selectedDate = new Date(date);
    this.cleartotalhours();
    this.Todaydate = new Date();
    this.displaydate = new Date();
    this.dateselection(date);
    this.headerService.languageDataEmitter.next(date);
  }
  GetNonDeliveryList(){
    return this.ticketdetails.filter(x => x.type == 'ND');
  }
  dateselection(date: any): void{   
    
    const curr = date;
    const first = curr.getDate() - curr.getDay();
    const last = first + 6;
    this.firstday = this.dateformatecomversion(this.datePipe.transform(new Date(curr.setDate(first)).toDateString(), 'MM/dd/yyyy'), 0);
    this.lastday = this.dateformatecomversion(this.datePipe.transform(new Date(curr.setDate(last)).toDateString(), 'MM/dd/yyyy'), first <= 0 ? 1 : 0);
    this.selectdate = this.firstday + ' - ' + this.lastday;
    localStorage.setItem("Firstdayofweek", this.dateformatechange(this.firstday));
    localStorage.setItem("Lastdayofweek", this.dateformatechange(this.lastday));
    this.timesheetentry.Dynamic = this.timesheetentry.Dynamic == true ? false : true;
    this.Loadgriddata();
  }
  dateformatecomversion(date: string, monthincrement: number): string {
    let arrdate = date.split("/");
    let day = arrdate[1];
    let month = parseInt(arrdate[0]) + monthincrement;
    let datemonth = month <= 9 ? "0" + month : month;
    let year = arrdate[2];
    if (datemonth == "13" && monthincrement == 1) {
      datemonth = "01"
      year = (parseInt(year) + 1).toString();
    }
    return this.dateformate = day + "/" + datemonth + "/" + year;
  }
  dateformatechange(date: string): string {
    let arrdate = date.split("/");
    let day = arrdate[0];
    let month = arrdate[1];
    let year = arrdate[2];
    return this.dateformate = month + "/" + day + "/" + year;
  }
  closeErrCorrectionModal(event) {
    this.erroredTicketDialog = (event)?false: true;
    this.checkErrorTickCount();
    this.Loadgriddata();
  }
  openAttributesModal(td) {
    this.openAttr = true;
    this.getPopUpAttributeDetails(td);
  }
  closeAttributesModal(td) {
    let isAttrUpdted;
    if(td.isAttributeUpdated == "N" || td.isAttributeUpdated == "0" || td.isAttributeUpdated == null ){
      isAttrUpdted = 0;
    }
    else{
      isAttrUpdted = parseInt(td.isAttributeUpdated);
    }
    this.openAttr = false;
    td.isAttributeUpdated = isAttrUpdted;
    this.closeStatus = "N";
    this.displayAttributes= false;
  }
  getPopUpAttributeDetails(td) {
    let ticketTypeId;
    let serviceId;
    let employeeId = this.EmployeeId;
    let projectId = td.projectId;
    let assigneeId = td.assignedTo;
    let ticketId = td.ticketId;
    let supportTypeId = td.supportTypeId;
    let gracePeriod = td.gracePeriod;
    let closedDate = td.closedDate;
    let completedDate = td.completedDate;
    let prjStatusId = td.ticketStatusMapId;
    let dartStatusId = td.dartStatusId;
    let timeTickerId = td.timeTickerId;
    ticketTypeId = parseInt(td.ticketTypeMapId);
    let isCognizant = this.IsCognizant;
    if (isCognizant != false) {
      serviceId = td.serviceId;
      ticketTypeId = 0;
    }
    else {
      serviceId = 0;
    }
    if (td.type == 'W') {
      this.displayAttributes = false;     
      this.updateWorkItemServiceandStatus(projectId, ticketId, timeTickerId, employeeId, serviceId, prjStatusId);
    }
    else {
      if (dartStatusId == 8 || dartStatusId == 9) {
        if ((dartStatusId == 8 && closedDate != null && closedDate != "")
          || (dartStatusId == 9 && completedDate != null && completedDate != "")) {
          var currentDate = new Date();
          var validTillDate = new Date();
          if (dartStatusId == 8) {
            validTillDate = new Date(closedDate);
            validTillDate.setDate(validTillDate.getDate() + parseInt(gracePeriod));
          }
          else {
            validTillDate = new Date(completedDate);
            validTillDate.setDate(validTillDate.getDate() + parseInt(gracePeriod));
          }
          if (currentDate > validTillDate) {
            this.ticketdetails.filter(x=>x.ticketId == ticketId && x.projectId == projectId)[0].isGracePeriodMet = true;        
          }
        }
      }
      if ((isCognizant == true && ((serviceId != 0 || supportTypeId == 2) && prjStatusId != 0)) || (isCognizant == false && ticketTypeId != 0 && prjStatusId != 0)){
          if (assigneeId != "0") {
            this.updateIsAttributeByTicketId(td,serviceId,prjStatusId,null,null,this.mode,false,this.closeStatus)
            }
          else {
            this.selectedTicketDetails = td;
            this.displayAttributes= true;           
          }
      }  
      else{
           this.selectedTicketDetails = td;
           this.displayAttributes = false;
      }         
    }
  }
  updateWorkItemServiceandStatus(projectId, ticketId, timeTickerId, employeeId, serviceId, tickStatusId) {
    let inputParams = {
      TimeTickerID: timeTickerId,
      ProjectID: projectId,
      TicketID: ticketId,
      ServiceID: serviceId,
      DARTStatusId: tickStatusId,
      EmployeeID: employeeId,
    }
    this.dynamicgridservice.updateWorkItemServiceandStatus(inputParams).subscribe(x => {
      this.spinneservice.hide();
    })
  }
  updateIsAttributeByTicketId(td,serviceId,prjStatusId,closedDate,completedDate,mode,openAttrSave,closeStatus) {
    let args = {
      ProjectId: parseInt(td.projectId),
      TicketId: td.ticketId,
      ServiceId: parseInt(serviceId),
      IsCognizant: td.isCognizant,
      DARTStatusId: parseInt(prjStatusId),
      TicketTypeMapId: parseInt(td.ticketTypeMapId),
      SupportTypeId: parseInt(td.supportTypeId)
    }
    if(prjStatusId != ""){
    this.dynamicgridservice.updateIsAttributeByTicketId(args).subscribe(x => {   
      if (x != null) {
        this.spinneservice.hide();
        this.closeStatus = closeStatus;
        if(this.closeStatus == "Y"){
          if(x == "1"){
            td.isAttributeUpdated = 1;
          }
          else{
            td.isAttributeUpdated = 0;
          }
          this.dynamicgridservice.checkGraceperiodEmitter.emit(td);
        }
        else if(x == "0"){
          td.closedDate = closedDate;
          td.completedDate = completedDate;
          td.isAttributeUpdated = 0;
          this.selectedTicketDetails = td;
          this.displayAttributes= true;           
        }
        else if (this.openAttr || openAttrSave) {
          this.openAttr = false;
          openAttrSave = false;
          td.closedDate = closedDate;
          td.completedDate = completedDate;
          td.isAttributeUpdated = 1;
          this.selectedTicketDetails = td;
          this.displayAttributes= true;    
        }
      }
    });
  }
  }
  freezeDebtAttributes(event){
    this.updateIsAttributeByTicketId(event.ticketDetails,event.serviceId,event.ticketStatusMapId,
      event.closeDte,event.completeDte,event.mode,event.opnAttr,event.closeStatus);
  }
  freezeTicketAttributes(event){
    this.ticketdetails.filter(x=>x.ticketId == event.ticketDetails.ticketId && x.projectId == event.ticketDetails.projectId)[0].isGracePeriodMet = true;
    this.ticketdetails.filter(x=>x.ticketId == event.ticketDetails.ticketId && x.projectId == event.ticketDetails.projectId)[0].ServiceTitle = Constants.GracePeriodMessage;
    this.ticketdetails.filter(x=>x.ticketId == event.ticketDetails.ticketId && x.projectId == event.ticketDetails.projectId)[0].StatusTitle = Constants.GracePeriodMessage;
  }

  EffortKeyPress(evt,value) {
    let charCode = (evt.which) ? evt.which : evt.keyCode;
    let number = value.split('.');
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    if (number.length > 1 && charCode == 46) {
        return false;
    }
  return true;
  }
  checkErrorTickCount(){
    let errorParams = {
    UserId: parseInt(this.CustomerId),
    EmployeeId: this.EmployeeId
  };
  this.dynamicgridservice.getCountErroredTickets(errorParams).subscribe(y => {
    this.glowErrorCorrection = y == "True" ? true : false;
    if (document.getElementById("erroredTickets")) {
      document.getElementById("erroredTickets").className = this.glowErrorCorrection ? "clserrorTicketsImage" : "clsErrorCorrection";
    }
  })
}
}
