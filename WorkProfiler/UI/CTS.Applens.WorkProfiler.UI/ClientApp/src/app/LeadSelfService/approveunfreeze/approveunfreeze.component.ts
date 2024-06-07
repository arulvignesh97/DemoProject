// Copyright (c) Applens. All Rights Reserved.
import { Component, OnInit, AfterViewChecked} from '@angular/core';
import { BsDatepickerModule, DatepickerModule } from 'ngx-bootstrap/datepicker';
import { DatePipe } from '@angular/common';
import { DropdownModule } from 'primeng/dropdown';
import { ApproveunfreezeService } from './../Service/approveunfreeze.service';
import { SpinnerService } from './../../common/services/spinner.service';
import { AppSettingsConfig } from './../../app.settings.config';
import { ScrollableView } from 'primeng/table';
import { HeaderService } from 'src/app/Layout/services/header.service';
import { LanguageModel, MasterDataModel } from '../../Layout/models/header.models';
import * as Chart from 'chart.js'
import * as moment from 'moment';
import { TranslateService } from '@ngx-translate/core';
import { Constants } from 'src/app/common/constants/constants';


@Component({
  selector: 'app-approveunfreeze',
  templateUrl: './approveunfreeze.component.html',
  styleUrls: ['./approveunfreeze.component.css'],
  providers: [DatePipe]
})

export class ApproveunfreezeComponent implements OnInit, AfterViewChecked {
  public isDaily: boolean;
  public userId: string;
  public customerId: number;
  public customername : string;
  public approverId:string;
  public UnfreezeDay: number;
  selectstartdate: string;
  selectenddate: string;
  selectweekdate: string;
  weekfirstday: string;
  weeklastday: string;
  specialWeekDate: string;
  isSuccess: boolean;
  successMessage = '';
  public FromdatePickerConfigStart: Partial<BsDatepickerModule>;
  public FromdatePickerConfig: Partial<BsDatepickerModule>;
  public FromdatePickerConfigEnd: Partial<BsDatepickerModule>;
  public defaultersList: any;
  defaultselection: string[] = [];
  public assgineList: any;
  assigneselection: string[] = [];
  //jsondata: any;
  public approveunfreezedaily: any[] = [];
  public approveunfreezeweekly: any[] = [];
  public dailyTimeSheetData: any[] = [];
  public isApproveAll = true;
  public isUnfreezeAll = true;
  public isApproveAllchk = false;
  public isUnfreezeAllchk = false;
  public approveunfreezeSelected: any[] = [];
  todaydate: Date;
  weeklastdate: Date;
  enableSubmit = false;
  values: Date[];
  Todaydate:Date;
  displaydate:Date;
  //Ticket Details Popup
  displayTicketDetails: boolean = false;
  cols = [];
  ticketDetails = [];
  ticketData = [];
  ticketDetailEmployeeID: any;
  ticketDetailStartDate: string;
  ticketDetailEndDate: string;
  downloadButtonShow: boolean = true;
  dailyFromMinDate: Date = new Date();
  dailyFromMaxDate: Date = new Date();
  dailyToMinDate: Date = new Date();
  dailyToMaxDate: Date = new Date();
  weeklyFromMinDate: Date = new Date();
  weeklyToMaxDate: Date = new Date();
  //End
  //Chart
  dateRangeSet: boolean = false;
  graphStartDate: string;
  graphEndDate: string;
  chartData = [];
  day: string;
  chart: Chart;
  yearData2 = [];
  unfreezeGracePeriod: string;
  translatedData: string;
  //End
  montstartDate: Date = new Date();
  headerdata: any = [];
  public firstday: Date;
  public lastday: Date;
  public currentdate: Date;
  public firstweek: string;
  public lastdayweek:string;
  isCognizant: boolean = true;
  isADMApplicableforCustomer : boolean = false;

  constructor(private datePipe: DatePipe, private approveService: ApproveunfreezeService, private spinner: SpinnerService,
    private headerService: HeaderService, private translate: TranslateService) {
   }

  ngOnInit(): void {

    this.todaydate = new Date();


    
    this.headerService.masterDataEmitter.subscribe((masterData: MasterDataModel) => {
      if (masterData != null) {
        this.spinner.show();
        this.userId = masterData.hiddenFields.employeeId;
        this.customerId = masterData.selectedCustomer.customerId;
        this.customername=masterData.selectedCustomer.customerName;
        this.approverId=masterData.hiddenFields.employeeId;
        this.isCognizant = masterData.hiddenFields.isCognizant == 1 ? true : false;
        this.isADMApplicableforCustomer = AppSettingsConfig.settings.ADMApplicableforCustomer;
        if (masterData.hiddenFields.isDaily == 1) {
          this.isDaily = true;
        }
        else {
          this.isDaily = false;
        }

        if (this.isDaily) {
          this.FromdatePickerConfigStart = Object.assign({},
            {
              showWeekNumbers: false,
              dateInputFormat: 'DD/MM/YYYY'
              
            });
          this.FromdatePickerConfigEnd = Object.assign({},
            {
              showWeekNumbers: false,
              dateInputFormat: 'DD/MM/YYYY'
              
            });
          var currentDate = new Date();
          var firstDay =
            new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
          this.montstartDate = firstDay;
          this.datestartselection(currentDate);
          this.dateendselection(currentDate);
        }
        else {
          this.FromdatePickerConfig = Object.assign({},
            {
              showWeekNumbers: false,
              dateInputFormat: 'DD/MM/YYYY'
            });
          var currentDate = new Date();
          var firstDay =
            new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
          this.montstartDate = firstDay;
          const curr = currentDate;
          const first = curr.getDate() - curr.getDay();
          const last = first + 6;
          
          this.weeklastdate = new Date(curr.setDate(last));
    
          this.weekfirstday = this.datePipe.transform(new Date(curr.setDate(first)), Constants.dateInputFormatApprove);
         
          this.weeklastday = this.datePipe.transform(new Date(curr.setDate(last)), Constants.dateInputFormatApprove);
          var calenderDate = new Date();
          var datestr = new Date(calenderDate).toDateString();
          this.dateweekselection(datestr);
     
        }
        this.dateRangeSet = false;
        this.populateAssigneandDefaulters();
      }
    });

    this.headerService.languageDataEmitter.subscribe((languageData: LanguageModel) => {
      if (languageData != null) {
       //this.getChartData();
      }
    });

    //Calendar Date Set
   
    let start = new Date();
    start.setDate(start.getDate() - start.getDay());
    let end = new Date(start);
    end.setDate(start.getDate() + 6);
    this.datePipe.transform(start, 'yyyy/MM/dd')
    this.datePipe.transform(end, 'yyyy/MM/dd')
    this.values = [start,end];

  }

  ngAfterViewChecked(): void {
    if (this.isDaily) {
      $('#weekdatepick').val(this.specialWeekDate);
    }
    else {
      $('#weekdatepick').val(this.weekfirstday + ' - ' + this.weeklastday);
    }
  }

  onstartChange(date: any): void {
    this.datestartselection(date);
    this.dailyToMinDate = date;
    this.populateAssigneandDefaulters();
  }

  onendChange(date: any): void {
    this.dateendselection(date);
    this.dailyFromMaxDate = date;
    this.populateAssigneandDefaulters();
  }
  onChange(date: any): void{  
    
    let start = new Date(date);
    start.setDate(start.getDate() - start.getDay());
    this.values[0] = start;
    let end = new Date(start);
    end.setDate(start.getDate() + 6);
    this.values[1] = end;
    this.Todaydate = new Date();
    this.displaydate = new Date();
    
    this.dateweekselection(start);
    this.populateAssigneandDefaulters();
    
    
    this.headerService.languageDataEmitter.next(date);
   
  }
  onWeekChange(date: any): void {
   
    var datestr = new Date(date).toString();
    this.dateweekselection(datestr);
   
    this.populateAssigneandDefaulters();
  }
  datestartselection(date: any): void {
    const curr = date;
     this.selectstartdate = this.datePipe.transform(new Date(curr), Constants.dateInputFormatApprove);
  }
  dateendselection(date: any): void {
    const curr = date;
    this.selectenddate = this.datePipe.transform(new Date(curr), Constants.dateInputFormatApprove);
  }
  dateweekselection(date: any): void {
   
    const curr = new Date(date);
    const dateend = new Date(date);
    const first = curr.getDate() - curr.getDay();
    const last = first + 6;
    this.firstweek = new Date(curr.setDate(first)).toDateString();
    this.lastdayweek = new Date(dateend.setDate(last)).toDateString();
    this.weeklastdate = new Date(curr.setDate(last));
    this.weekfirstday = this.datePipe.transform(this.firstweek, Constants.dateInputFormatApprove);
    this.weeklastday = this.datePipe.transform(this.lastdayweek, Constants.dateInputFormatApprove);
    this.specialWeekDate = this.weekfirstday + '-' + this.weeklastday;
    this.selectweekdate = this.datePipe.transform(new Date(date), Constants.dateInputFormatApprove);
  }
  defaulterschange() {

  }
  assignerschange() {

  }
  searchTimesheets() {
    this.bindGridOnSearch();
  }

  populateAssigneandDefaulters() {
    this.defaultselection = [];
    this.assigneselection = [];
    var StartDate = '';
    var EndDate = '';
    var Dates = [];
    if (this.isDaily) {
      StartDate = this.selectstartdate;
      EndDate = this.selectenddate;
      if (StartDate == '' || EndDate == '') {
        var currentDate = new Date().getDate();
        var currentMonth = new Date().getMonth() + 1;
        var currDate = '';
        var currMonth = '';
        if (currentDate < 10) {
          currDate = '0' + currentDate;
        }
        else {
          currDate = currentDate.toString();
        }
        if (currentMonth < 10) {
          currMonth = '0' + currMonth;
        }
        var currYear = new Date().getFullYear();
        StartDate = currDate + '/' + currMonth + '/' + currYear;
        EndDate = currDate + '/' + currMonth + '/' + currYear;

      }
    }
    else {
      
      StartDate = this.weekfirstday; 
      EndDate = this.weeklastday;
     }

    var data = {
      EmployeeId: this.userId,
      CustomerId: Number(this.customerId),
      StartDate: StartDate,
      EndDate: EndDate
    }

    this.approveService.GetAssignessOrDefaulters(data).subscribe(x => {
      this.defaultersList = [];
      this.assgineList = [];
      this.unfreezeGracePeriod = x.unfreezeGracePeriod;
      this.defaultselection = [];
      this.assigneselection = [];
      if (x.assignessOrDefaulters != null && x.assignessOrDefaulters.length > 0) {
        for (let i = 0; i < x.assignessOrDefaulters.length; i++) {
          if (x.assignessOrDefaulters[i].isDefaulter == 'true') {
            this.defaultersList.push({
              "value": x.assignessOrDefaulters[i].employeeId,
              "label": x.assignessOrDefaulters[i].employeeName
            });
            this.defaultselection.push(x.assignessOrDefaulters[i].employeeId);
            this.assgineList.push({
              "value": x.assignessOrDefaulters[i].employeeId,
              "label": x.assignessOrDefaulters[i].employeeName
            });
            this.assigneselection.push(x.assignessOrDefaulters[i].employeeId);
          }
          else {
            this.assgineList.push({
              "value": x.assignessOrDefaulters[i].employeeId,
              "label": x.assignessOrDefaulters[i].employeeName
            });
            this.assigneselection.push(x.assignessOrDefaulters[i].employeeId);
          }
        }
      }
      if (this.dateRangeSet == false) {
        this.graphDateRange();
        this.dateRangeSet = true;
      }
      this.getChartData();
      this.bindGridOnSearch();
    });
  }

  bindGridOnSearch() {
    this.spinner.show();
    var Dates = [];
    var Startdate = '';
    var EndDate = '';
    var Project = [];
    this.isApproveAll = true;
    this.isUnfreezeAll = true;
    this.isUnfreezeAllchk = false;
    this.isApproveAllchk = false;

    if (this.isDaily) {
      Startdate = this.selectstartdate;
      EndDate = this.selectenddate;
    }
    else {
     Startdate = this.weekfirstday; 
     EndDate = this.weeklastday;
    }

    var assignessOrDefaultersId = "";
    var defaultersId = "";
    if (this.defaultselection != null && this.defaultselection.length > 0) {
      defaultersId = this.defaultselection.join(',');
    }
    if (this.assigneselection != null && this.assigneselection.length > 0) {
      assignessOrDefaultersId = this.assigneselection.join(',');
    }

    var params = {
      AssignessOrDefaultersID: assignessOrDefaultersId,
      DefaulterId: defaultersId,
      EndDate: EndDate,
      StartDate: Startdate,
      UserID: this.userId,
      CustomerId: Number(this.customerId),
      IsDaily: this.isDaily ? 1 : 0
    }

    if (Startdate != "" && EndDate != "" && Startdate != undefined && EndDate != undefined && Startdate != null && EndDate != null && assignessOrDefaultersId != null) {
      if (this.isDaily) {
        this.approveService.GetTimeSheetDataDaily(params).subscribe(x => {
          this.approveunfreezedaily = [];
          this.approveunfreezedaily = x;
          if (this.approveunfreezedaily != null && this.approveunfreezedaily.length > 0) {
            for (var i = 0; i < this.approveunfreezedaily.length; i++) {
              if (this.approveunfreezedaily[i].timeSheetStatus.toLowerCase() == 'submitted') {
                this.isApproveAll = false;
                this.isUnfreezeAll = false;
              }
              else if (this.approveunfreezedaily[i].timeSheetStatus.toLowerCase() != 'submitted' && this.approveunfreezedaily[i].isFreezed == true) {
                this.isUnfreezeAll = false;
              }
            }
          }
          if (this.isApproveAll != false) {
            this.isApproveAll = true
          }
          if (this.isUnfreezeAll != false) {
            this.isUnfreezeAll = true
          }
          this.spinner.hide();
        });
      }
      else {
        this.approveService.GetTimeSheetDataWeekly(params).subscribe(x => {
          this.approveunfreezeweekly = [];
          this.approveunfreezeweekly = x;
          if (this.approveunfreezeweekly != null && this.approveunfreezeweekly.length > 0) {
            this.dailyTimeSheetData = this.approveunfreezeweekly[0].dailyTimeSheetData;
            for (var i = 0; i < this.approveunfreezeweekly.length; i++) {
              if (this.approveunfreezeweekly[i].timeSheetStatus.toLowerCase() == 'submitted') {
                this.isApproveAll = false;
                this.isUnfreezeAll = false;
              }
              else if (this.approveunfreezeweekly[i].timeSheetStatus.toLowerCase() != 'submitted' && this.approveunfreezeweekly[i].isFreezed == true) {
                this.isUnfreezeAll = false;
              }
            }
          }
          if (this.isApproveAll != false) {
            this.isApproveAll = true
          }
          if (this.isUnfreezeAll != false) {
            this.isUnfreezeAll = true
          }
          this.spinner.hide();
        });
        if (document.getElementsByClassName('p-datatable-scrollable-view').length > 0) {
          var d = document.getElementsByClassName('p-datatable-scrollable-view')[0];
          d.setAttribute('style', 'overflow:hidden')
        }
      }
    }
    else {
      this.spinner.hide();
    }
  }

  checkboxapproveallValidation(event) {

    //Daily timesheet
    if (this.isDaily) {
      for (var i = 0; i < this.approveunfreezedaily.length; i++) {
        if (event.target.checked) {

          if (this.approveunfreezedaily[i].timeSheetStatus.toLowerCase() == 'submitted') {
            this.approveunfreezedaily[i].isApprove = true;
            this.approveunfreezedaily[i].isUnfreeze = false;
            this.enableSubmit = true;
          }
          if (this.approveunfreezedaily[i].timeSheetStatus.toLowerCase() != 'submitted' && this.approveunfreezedaily[i].isFreezed == true) {
            this.approveunfreezedaily[i].isUnfreeze = false;
          }
          this.isUnfreezeAllchk = false;
        }
        else {
          this.approveunfreezedaily[i].isApprove = false;
          this.approveunfreezedaily[i].isUnfreeze = false;
          this.isUnfreezeAllchk = false;
        }
      }
      if (this.approveunfreezedaily.filter(a => a.isApprove == true).length == 0) {
        this.enableSubmit = false;
      }
     
    }
    //Weekly timesheet
    else {
      for (var i = 0; i < this.approveunfreezeweekly.length; i++) {
        if (event.target.checked) {
          if (this.approveunfreezeweekly[i].timeSheetStatus.toLowerCase() == 'submitted') {
            this.approveunfreezeweekly[i].isApprove = true;
            this.approveunfreezeweekly[i].isUnfreeze = false;
            this.enableSubmit = true;
          }
          if (this.approveunfreezeweekly[i].timeSheetStatus.toLowerCase() != 'submitted' && this.approveunfreezeweekly[i].isFreezed == true) {
            this.approveunfreezeweekly[i].isUnfreeze = false;
          }
          this.isUnfreezeAllchk = false;
        }
        else {
          this.approveunfreezeweekly[i].isApprove = false;
          this.approveunfreezeweekly[i].isUnfreeze = false;
          this.isUnfreezeAllchk = false;
        }
      }
      if (this.approveunfreezeweekly.filter(a => a.isApprove == true).length == 0) {
        this.enableSubmit = false;
      }
      
    }
  }
  checkboxunfreezeallValidation(event) {

    //Daily timesheet
    if (this.isDaily) {
      for (var i = 0; i < this.approveunfreezedaily.length; i++) {
        if (event.target.checked) {
          if (this.approveunfreezedaily[i].timeSheetStatus.toLowerCase() == 'submitted') {
            this.approveunfreezedaily[i].isApprove = false;
            this.approveunfreezedaily[i].isUnfreeze = true;
            this.enableSubmit = true;
          }
          if (this.approveunfreezedaily[i].timeSheetStatus.toLowerCase() != 'submitted' && this.approveunfreezedaily[i].isFreezed == true) {
            this.approveunfreezedaily[i].isUnfreeze = true;
            this.enableSubmit = true;
          }
          this.isApproveAllchk = false;
        }
        else {
          this.approveunfreezedaily[i].isApprove = false;
          this.approveunfreezedaily[i].isUnfreeze = false;
          this.isApproveAllchk = false;
          this.enableSubmit = false;
        }
      }
      if (this.approveunfreezedaily.filter(a => a.isUnfreeze == true).length == 0) {
        this.enableSubmit = false;
      }
    }
    //Weekly timesheet
    else {
      for (var i = 0; i < this.approveunfreezeweekly.length; i++) {
        if (event.target.checked) {
          if (this.approveunfreezeweekly[i].timeSheetStatus.toLowerCase() == 'submitted') {
            this.approveunfreezeweekly[i].isApprove = false;
            this.approveunfreezeweekly[i].isUnfreeze = true;
            this.enableSubmit = true;
          }
          if (this.approveunfreezeweekly[i].timeSheetStatus.toLowerCase() != 'submitted' && this.approveunfreezeweekly[i].isFreezed == true) {
            this.approveunfreezeweekly[i].isUnfreeze = true;
            this.approveunfreezeweekly[i].isApprove = false;
            this.enableSubmit = true;
          }
          this.isApproveAllchk = false;
        }
        else {
          this.approveunfreezeweekly[i].isApprove = false;
          this.approveunfreezeweekly[i].isUnfreeze = false;
          this.isApproveAllchk = false;
          this.enableSubmit = false;
        }
      }
      if (this.approveunfreezeweekly.filter(a => a.isUnfreeze == true).length == 0) {
        this.enableSubmit = false;
      }
    }

  }

  downloadTimesheetdate() {
    this.spinner.show();
    var Dates = [];
    var Startdate = '';
    var EndDate = '';
    if (this.isDaily) {
      Startdate = this.selectstartdate;
      EndDate = this.selectenddate;
    }
    else {
      
      Startdate = this.weekfirstday; 
      EndDate = this.weeklastday;
    }
    var assignessOrDefaultersId = "";
    var defaultersId = "";
    if (this.defaultselection != null && this.defaultselection.length > 0) {
      assignessOrDefaultersId = this.defaultselection.join(',');
    }
    if (this.assigneselection != null && this.assigneselection.length > 0) {
      defaultersId = this.assigneselection.join(',');
    }

    var params = {
      AssignessOrDefaultersID: assignessOrDefaultersId,
      DefaulterId: defaultersId,
      EndDate: EndDate,
      StartDate: Startdate,
      UserID: this.userId,
      CustomerId: Number(this.customerId),
      IsDaily: this.isDaily ? 1 : 0
    }
    if (Startdate != "" && EndDate != "" && Startdate != undefined && EndDate != undefined && Startdate != null && EndDate != null && assignessOrDefaultersId != null) {
      this.approveService.GetAssignessDownload(params).subscribe(x => {
        var resultset = x;

        var fileparam = {
          FileName: resultset
        }
        if (resultset != "") {
          this.approveService.DownloadExcel(fileparam).subscribe(y => {
            this.downloadFile(y);
          });
        }
      });
    }
    this.spinner.hide();
  }

  downloadFile(data: any) {
    const blob = new Blob([data], { type: 'application/vnd.ms-excel' });
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var MM = String(today.getMonth() + 1).padStart(2, '0');
    var yyyy = today.getFullYear();
    var hh = today.getHours();
    var mm = today.getMinutes();
    var ss = today.getSeconds();
    var excelName = "";
    var td = yyyy + '_' + MM + '_' + dd + '_' + hh + '_' + mm + '_' + ss;
    if (this.isDaily) {
      excelName = "ApproveUnfreeze_" + td + ".xlsx";
    } else {
      excelName = "ApproveUnfreezeWeekly_" + td + ".xlsx";
    }
    const url = window.URL.createObjectURL(blob);
    const noice = document.createElement("a");
    noice.download = excelName;
    noice.href = url;
    noice.click();
  }


  ApproveUnfreeze() {
    this.spinner.show();
    var usercount = 0;
    this.approveunfreezeSelected=[];
    // Daily submit
    if (this.isDaily) {
      if (this.approveunfreezedaily != null && this.approveunfreezedaily.length > 0) {

        for (var i = 0; i < this.approveunfreezedaily.length; i++) {
          if (this.approveunfreezedaily[i].isApprove == true || this.approveunfreezedaily[i].isUnfreeze == true) {
            usercount = 1;
            var datas = {
              isApproval: this.approveunfreezedaily[i].timesheetId == 0 ? false : true,
              statusId: this.approveunfreezedaily[i].isApprove == true ? 3 : 4,
              comments: this.approveunfreezedaily[i].rejectionCommects,
              timesheetId: this.approveunfreezedaily[i].timesheetId,
              timeSheetDate: this.approveunfreezedaily[i].timeSheetDate,
              employeeId: this.approveunfreezedaily[i].employeeId,
              submitterId: this.approveunfreezedaily[i].employeeId,
              customerId: Number(this.customerId)
            };
            this.approveunfreezeSelected.push(datas);
          }
        }
      }
    }
    // Weekly submit
    else {
      if (this.approveunfreezeweekly != null && this.approveunfreezeweekly.length > 0) {

        for (var i = 0; i < this.approveunfreezeweekly.length; i++) {
          if (this.approveunfreezeweekly[i].isUnfreeze == true || this.approveunfreezeweekly[i].isApprove == true) {
            usercount = 1;
            if (this.approveunfreezeweekly[i].dailyTimeSheetData.length > 0) {
              for (var j = 0; j < this.approveunfreezeweekly[i].dailyTimeSheetData.length; j++) {
                var datas = {
                  isApproval: this.approveunfreezeweekly[i].dailyTimeSheetData[j].timesheetId == 0 ? false : true,
                  statusId: this.approveunfreezeweekly[i].isApprove == true ? 3 : 4,
                  comments: this.approveunfreezeweekly[i].rejectionCommects,
                  timesheetId: this.approveunfreezeweekly[i].dailyTimeSheetData[j].timesheetId,
                  timeSheetDate: this.approveunfreezeweekly[i].dailyTimeSheetData[j].timeSheetDate,
                  employeeId: this.approveunfreezeweekly[i].employeeId,
                  submitterId: this.approveunfreezeweekly[i].employeeId,
                  customerId: Number(this.customerId)
                };
                this.approveunfreezeSelected.push(datas);
              }
            }
          }
        }
      }
    }
    // API call
    if (usercount == 1) {
      this.approveService.UpdateTimeSheetData(this.approveunfreezeSelected,this.isDaily,this.userId).subscribe(x => {
        this.spinner.hide();
        this.isSuccess = true;
        this.setSuccessMessage('Approved/UnfreezedSuccessfully');
        setTimeout(() => {
          this.isSuccess = false;
        }, Constants.lifeSpan);
        this.bindGridOnSearch();
        this.getChartData();
      });
    }
  }

  checkboxApproveValidation(dailydata, event) {

    if (event.target.checked) {
      this.enableSubmit = true;
      dailydata.isUnfreeze = false;
      this.isUnfreezeAllchk = false;
      if (this.isDaily) {
        var submittedcount = this.approveunfreezedaily.filter(a => a.timeSheetStatus.toLowerCase() == 'submitted').length;
        if (this.approveunfreezedaily.filter(a => a.isApprove == true).length == submittedcount) {
          this.isApproveAllchk = true;
        }
        else {
          this.isApproveAllchk = false;
        }
      }
      else {
        var submittedcount = this.approveunfreezeweekly.filter(a => a.timeSheetStatus.toLowerCase() == 'submitted').length;
        if (this.approveunfreezeweekly.filter(a => a.isApprove == true).length == submittedcount) {
          this.isApproveAllchk = true;
        }
        else {
          this.isApproveAllchk = false;
        }
      }
    }
    else {
      this.isApproveAllchk = false;
      this.isUnfreezeAllchk = false;
      if (this.isDaily) {
        if (this.approveunfreezedaily.filter(a => a.isApprove == true).length == 0) {
          this.enableSubmit = false;
        }
      }
      else {
        if (this.approveunfreezeweekly.filter(a => a.isApprove == true).length == 0) {
          this.enableSubmit = false;
        }
      }
    }

  }
  checkboxUnfreezeValidation(dailydata, event) {

    if (event.target.checked) {
      this.enableSubmit = true;
      dailydata.isApprove = false;
      this.isApproveAllchk = false;
      if (this.isDaily) {
        var submittedcount = this.approveunfreezedaily.filter(a => a.timeSheetStatus.toLowerCase() == 'submitted').length;
        var notsubmittedcount = this.approveunfreezedaily.filter(a => a.timeSheetStatus.toLowerCase() != 'submitted' && a.isFreezed == true).length;

        if (this.approveunfreezedaily.filter(a => a.isUnfreeze == true).length == (submittedcount + notsubmittedcount)) {
          this.isUnfreezeAllchk = true;
        }
        else {
          this.isUnfreezeAllchk = false;
        }
      }
      else {
        var submittedcount = this.approveunfreezeweekly.filter(a => a.timeSheetStatus.toLowerCase() == 'submitted').length;
        var notsubmittedcount = this.approveunfreezeweekly.filter(a => a.timeSheetStatus.toLowerCase() != 'submitted' && a.isFreezed == true).length;

        if (this.approveunfreezeweekly.filter(a => a.isUnfreeze == true).length == (submittedcount + notsubmittedcount)) {
          this.isUnfreezeAllchk = true;
        }
        else {
          this.isUnfreezeAllchk = false;
          
        }
      }
    }
    else {
      this.isUnfreezeAllchk = false;
      this.isApproveAllchk = false;
      if (this.isDaily) {
        if (this.approveunfreezedaily.filter(a => a.isUnfreeze == true).length == 0) {
          this.enableSubmit = false;
        }
      }
      else {
        if (this.approveunfreezeweekly.filter(a => a.isUnfreeze == true).length == 0) {
          this.enableSubmit = false;
        }
      }
    }
  }

  userTicketDetails(employeeId: any, date: any) {
    this.spinner.show();
    this.ticketDetailEmployeeID = employeeId;
    this.currentdate = date;
    if (this.isDaily) {
      this.ticketDetailStartDate = this.datePipe.transform(new Date(date), Constants.dateInputFormatApprove);
      this.ticketDetailEndDate = this.datePipe.transform(new Date(date), Constants.dateInputFormatApprove);
    }
    else {
      this.ticketDetailStartDate = this.weekfirstday; 
      this.ticketDetailEndDate = this.weeklastday;
    }

    var data = {
      CustomerID: Number(this.customerId),
      SubmitterId: employeeId,
      FromDate: this.ticketDetailStartDate,
      ToDate: this.ticketDetailEndDate,
      TsApproverId:this.approverId
    }
    this.approveService.GetTicketDetailsPopUp(data).subscribe(x => {
      this.displayTicketDetails = true;
      this.ticketDetails = [];
      this.cols = [];

      //Label Tanslate
      if(!this.isCognizant && !this.isADMApplicableforCustomer){
        this.cols.push({ field: 'ticketId', header: 'Ticket ID' });
      }
      else{
        this.cols.push({ field: 'ticketId', header: 'Ticket/WorkItem ID' });
      }
   
      this.translateData('Description');
      this.cols.push({ field: 'description', header: this.translatedData });
      this.translateData('Application');
      this.cols.push({ field: 'applicationName', header: this.translatedData });
      this.translateData('Tower');
      this.cols.push({ field: 'tower', header: this.translatedData });
      this.translateData('TimesheetDate');
      this.cols.push({ field: 'timesheetDate', header: this.translatedData });
      if(this.isCognizant || (!this.isCognizant && this.isADMApplicableforCustomer)){
        this.translateData('Service');
        this.cols.push({ field: 'service', header: this.translatedData });
        this.translateData('Activity');
        this.cols.push({ field: 'activity', header: this.translatedData });
      }
      this.translateData('Remarks');
      this.cols.push({ field: 'remarks', header: this.translatedData });
      this.translateData('DebtType');
      this.cols.push({ field: 'debtClassification', header: this.translatedData });
      this.translateData('ResolutionCode');
      this.cols.push({ field: 'resolutionCode', header: this.translatedData });
      this.translateData('CauseCode');
      this.cols.push({ field: 'causeCode', header: this.translatedData });
      this.translateData('AvoidableFlag');
      this.cols.push({ field: 'avoidableFlag', header: this.translatedData });
      this.translateData('ResidualDebt');
      this.cols.push({ field: 'residualDebt', header: this.translatedData });
      this.translateData('EffortHours');
      this.cols.push({ field: 'effortTillDate', header: this.translatedData });
      this.translateData('ProjectId');
      this.cols.push({ field: 'projectId', header: this.translatedData });
      this.ticketData = x;
      if (this.ticketData.length > 0) {
        this.downloadButtonShow = true;
      }
      else {
        this.downloadButtonShow = false;
      }
      for (var i = 0; i < this.ticketData.length; i++) {
        this.createJSON(this.ticketData[i]);
      }
      this.spinner.hide();
    });
  }
   
  createJSON(ticketData) {
    var itemJson = {};
    itemJson["ticketId"] = ticketData.ticketId;
    itemJson["description"] = ticketData.description;
    itemJson["applicationName"] = ticketData.applicationName;
    itemJson["tower"] = ticketData.tower;
    ticketData.timesheetDate = this.getDate(ticketData.timesheetDate);
    itemJson["timesheetDate"] = ticketData.timesheetDate;
    itemJson["service"] = ticketData.service;
    itemJson["activity"] = ticketData.activity;
    itemJson["remarks"] = ticketData.remarks;
    itemJson["debtClassification"] = ticketData.debtClassification;
    itemJson["resolutionCode"] = ticketData.resolutionCode;
    itemJson["causeCode"] = ticketData.causeCode;
    itemJson["avoidableFlag"] = ticketData.avoidableFlag;
    itemJson["residualDebt"] = ticketData.residualDebt;
    itemJson["effortTillDate"] = ticketData.effortTillDate;
    itemJson["projectId"] = ticketData.projectId;
    this.ticketDetails.push(itemJson);
  }

  Close() {
    this.displayTicketDetails = false;
  }

  ExcelDownload() {
    var data = {
      CustomerID: Number(this.customerId),
      SubmitterId: this.ticketDetailEmployeeID,
      FromDate: this.ticketDetailStartDate,
      ToDate: this.ticketDetailEndDate,
      IsCognizant : this.isCognizant,
      IsADMApplicableforCustomer : this.isADMApplicableforCustomer,
      TsApproverId:this.approverId
    }
    this.approveService.GetTicketDetailsForDownload(data).subscribe(x => {
      this.spinner.show();
      const blob = new Blob([x], { type: 'application/vnd.ms-excel' });
      const url = window.URL.createObjectURL(blob);
      const downloadTag = document.createElement('a');
      var today = new Date();
      var dd = String(today.getUTCDate()).padStart(2, '0');
      var MM = String(today.getUTCMonth() + 1).padStart(2, '0');
      var yyyy = today.getUTCFullYear();
      var hh = today.getUTCHours();
      var mm = today.getUTCMinutes();
      var ss = today.getUTCSeconds();
      var td = yyyy + MM + + dd + + hh + mm + ss;
      downloadTag.download = "TimesheetDetails" + td + ".xlsx";
      downloadTag.href = url;
      downloadTag.click();
      this.spinner.hide();
    });
  }

  getDate(date: string): string {
    if (date == undefined || date.toString() == "") {
      return "";
    }
    else {
      this.currentdate = new Date(date);
      return this.datePipe.transform(this.currentdate, 'MM/dd/yyyy');
    }
  }

  renderChart(chartData: any) {
    var XAxis = [];
    var inputDate = [];
    var SavedData = [];
    var SubmittedData = [];
    var ApprovedData = [];
    var UnfreezedData = [];
    var NAData = [];
    var yearData = [];
    var yearData2 = [];
    this.translateData('Saved');
    var Saved = this.translatedData;
    this.translateData('Submitted');
    var Submitted = this.translatedData;
    this.translateData('Approved');
    var Approved = this.translatedData;
    this.translateData('Unfrozen');
    var Unfrozen = this.translatedData;
    this.translateData('NA');
    var NA = this.translatedData;
    var ConfigurationObject = {
      SavedTitle: Saved,
      SubmittedTitle: Submitted,
      ApprovedTitle: Approved,
      UnfreezedTitle: Unfrozen,
      NATitle: NA,
      SavedColor: "#0031A0",
      SubmittedColor: "#00B140",
      ApprovedColor: "#2D66FE",
      UnfreezedColor: "#7E57C2",
      NAColor: "#F90909",
    }


    for (var i = 0; i < chartData.length; i++) {
      inputDate = chartData[i].dayValue.toString().split("-");
      this.translateData(inputDate[0]);
      chartData[i].dayValue = this.translatedData + '-' + inputDate[1];
      XAxis.push(this.translatedData + '-' + inputDate[1])
    };


    for (var i = 0; i < chartData.length; i++) { yearData.push(chartData[i].dateValue.toString()) };

    XAxis = XAxis.filter(function (itm, i, a) {
      if (i == a.indexOf(itm)) {
        yearData2.push(yearData[i]);
      }
      return i == a.indexOf(itm);
    });

    this.yearData2 = yearData2;

    for (var k = 0; k < XAxis.length; k++) {
      for (var i = 0; i < chartData.length; i++) {
        if (chartData[i].dayValue == XAxis[k]) {
          switch (chartData[i].timesheetStatusId) {
            case 1:
              SavedData.push(chartData[i].resultValue);
              break;
            case 2:
              SubmittedData.push(chartData[i].resultValue);
              break;
            case 3:
              ApprovedData.push(chartData[i].resultValue);
              break;
            case 4:
              UnfreezedData.push(chartData[i].resultValue);
              break;
            case 5:
              NAData.push(chartData[i].resultValue);
              break;
            default:
              break;
          }
        }
      }
    }

    var barChartData = {
      labels: XAxis,
      datasets: [{
        label: ConfigurationObject.SavedTitle,
        backgroundColor: ConfigurationObject.SavedColor,
        borderColor: ConfigurationObject.SavedColor,
        data: SavedData
      }, {
        label: ConfigurationObject.SubmittedTitle,
        backgroundColor: ConfigurationObject.SubmittedColor,
        borderColor: ConfigurationObject.SubmittedColor,
        data: SubmittedData
      },
      {
        label: ConfigurationObject.ApprovedTitle,
        backgroundColor: ConfigurationObject.ApprovedColor,
        borderColor: ConfigurationObject.ApprovedColor,
        data: ApprovedData
      },
      {
        label: ConfigurationObject.UnfreezedTitle,
        backgroundColor: ConfigurationObject.UnfreezedColor,
        borderColor: ConfigurationObject.UnfreezedColor,
        data: UnfreezedData
      },
      {
        label: ConfigurationObject.NATitle,
        backgroundColor: ConfigurationObject.NAColor,
        borderColor: ConfigurationObject.NAColor,
        data: NAData
      }]
    };

    if (this.chart) {
      this.chart.destroy();
    }

    Chart.defaults.global.datasets.bar.categoryPercentage = 0.6;
    Chart.defaults.global.datasets.bar.barPercentage = 0.5;

    this.chart = new Chart("canvas", {
      type: 'bar',
      data: barChartData,
      options: {
        responsive: true,
        maintainAspectRatio: false,
        legend: {
          display: true,
          position: "right",
          labels: {
            display: true,
            padding: 5,
            boxWidth: 10,
            fontColor: '#495057',
            fontSize: 12,
            fontFamily: "Segoe UI, Segoe UI Web Regular, Segoe UI Symbol,Helvetica Neue, BBAlpha Sans, S60 Sans, Arial, sans-seri"
          }
        },
        title: {
          display: false,
          text: 'Calendar View'
        },
        scales: {
          xAxes: [{
            ticks: {
              backgroundColor: "Lightgrey",
              fontColor: "#1d6da2",
              fontSize: 11,
              fontweight: "bold",
              fontFamily: "Proxima Nova, Regular",
              autoSkip: false
            },
            gridLines: {
              zeroLineColor: "#fff",
              borderDash: [2, 4],
              color: 'rgba(255, 255, 255, 0.20)'
            }, stacked: true
          }],
          yAxes: [{
            ticks: {
              stepSize: 1
            },
            gridLines: {
              display: true,
              borderDash: [2, 4],
              color: "Lightgrey"
            }, stacked: true
          }]
        }
      }
    });
    this.spinner.hide();
  }

  getChartData() {
    var param = {
      EmployeeId: window.btoa(this.userId),
      CustomerId: Number(this.customerId),
      StartDate: this.graphStartDate,
      EndDate: this.graphEndDate
    }
    this.approveService.GetCalendarData(param).subscribe(x => {
      this.chartData = x;
      this.renderChart(this.chartData);
    });
  }

  graphDateRange() {
    var CurrFirst;
    var CurrLast;
    var CurrLastDay;
    var today = new Date();
    var pfirstDay = new Date(today.getFullYear(), today.getMonth() - 1, 1);  //previous month first day
    var cfirstDay = new Date(today.getFullYear(), today.getMonth(), 1);   // current month first day
    var EndDate1 = moment(today).format("DD");
    var ChartValue = this.unfreezeGracePeriod;

    if (this.isDaily && Number(EndDate1) <= Number(ChartValue)) {
      this.graphStartDate = moment(pfirstDay).format("YYYY-MM-DD");
      this.graphEndDate = moment(today).format("YYYY-MM-DD");
    }
    else if (this.isDaily && Number(EndDate1) > Number(ChartValue)) {
      this.graphStartDate = moment(cfirstDay).format("YYYY-MM-DD");
      this.graphEndDate = moment(today).format("YYYY-MM-DD");
    }
    else if (this.isDaily == false && Number(EndDate1) <= Number(ChartValue)) {
      var pfirst = pfirstDay.getDate() - pfirstDay.getDay();
      var pday = new Date(pfirstDay.setDate(pfirst));
      this.graphStartDate = moment(pday).format("YYYY-MM-DD");
      CurrFirst = today.getDate() - today.getDay();
      CurrLast = CurrFirst + 6;
      CurrLastDay = new Date(today.setDate(CurrLast));
      this.graphEndDate = moment(CurrLastDay).format("YYYY-MM-DD");
    }
    else if (this.isDaily == false && Number(EndDate1) > Number(ChartValue)) {
      var cfirst = cfirstDay.getDate() - cfirstDay.getDay();
      var cday = new Date(cfirstDay.setDate(cfirst));
      this.graphStartDate = moment(cday).format("YYYY-MM-DD");
      CurrFirst = today.getDate() - today.getDay();
      CurrLast = CurrFirst + 6;
      CurrLastDay = new Date(today.setDate(CurrLast));
      this.graphEndDate = moment(CurrLastDay).format("YYYY-MM-DD");
    }
    //calender min max date set
    if (this.isDaily) {
      this.dailyFromMaxDate = new Date(this.graphEndDate);
      this.dailyFromMinDate = new Date(this.graphStartDate);
      this.dailyToMaxDate = new Date(this.graphEndDate);
      this.dailyToMinDate = new Date(this.graphStartDate);
    }
    else {
      this.weeklyFromMinDate = new Date(this.graphStartDate);
      this.weeklyToMaxDate = new Date(this.graphEndDate);
    }
  }

  filterGrid(event: any) {
    
    var activePoint = this.chart.getElementAtEvent(event)[0];
    var data = activePoint._chart.data;
    var date = data.labels[activePoint._index];
    var year = this.yearData2[activePoint._index];
    var month = date.split('-')[0];
    var startDate;
    var endDate;
    var month1 = month;
    month1 = month1.toLowerCase();
    this.translateData('jan');
    var jan = this.translatedData;
    this.translateData('feb');
    var feb = this.translatedData;
    this.translateData('mar');
    var mar = this.translatedData;
    this.translateData('apr');
    var apr = this.translatedData;
    this.translateData('may');
    var may = this.translatedData;
    this.translateData('jun');
    var jun = this.translatedData;
    this.translateData('jul');
    var jul = this.translatedData;
    this.translateData('aug');
    var aug = this.translatedData;
    this.translateData('sep');
    var sep = this.translatedData;
    this.translateData('oct');
    var oct = this.translatedData;
    this.translateData('nov');
    var nov = this.translatedData;
    this.translateData('dec');
    var dec = this.translatedData;
    var months = [jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec];
    month1 = months.indexOf(month1) + 1;
    month1 = (month1 < 10) ? '0' + month1.toString() : month1.toString();
    var datevalue = date.split("-");
    var d = datevalue[1];
    var date1 = (d < 10) ? '0' + d.toString() : d.toString();
    var value = month1 + "-" + date1 + "-" + year;
    var firstDate = moment(value).weekday(0).format('DD/MM/YYYY');
    var lastDate = moment(value).weekday(6).format('DD/MM/YYYY');
    if (this.isDaily) {
      startDate = moment(value).format('DD/MM/YYYY');
      this.selectstartdate = startDate;
      endDate = moment(value).format('DD/MM/YYYY');
      this.selectenddate = startDate;
      this.dailyToMinDate = new Date(moment(value).format('YYYY/MM/DD'));
    }
    else {
      startDate = firstDate;
      this.weekfirstday = firstDate;
      endDate = lastDate;
      this.weeklastday = lastDate;
    }
    //Grid call
    this.populateAssigneandDefaulters();
  }


  translateData(pattern): void {
    this.translate.get(pattern)
      .subscribe(message => {
        this.translatedData = message;
      });
  }
  setSuccessMessage(pattern): void {
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
}
