// Copyright (c) Applens. All Rights Reserved.
import { Component, OnInit } from '@angular/core';
import { InfoiconService} from 'src/app/AnalystSelfService/Service/infoicon.service';
import { DatePipe } from '@angular/common';
import { HeaderService} from 'src/app/Layout/services/header.service';
import { MasterDataModel } from 'src/app/Layout/models/header.models';
import { Constants} from 'src/app/AnalystSelfService/Constants/Constants';
import { AppSettingsConfig } from 'src/app/app.settings.config';

@Component({
  selector: 'app-infoicon',
  templateUrl: './infoicon.component.html',
  styleUrls: ['./infoicon.component.css'],
  providers: [DatePipe]
})
export class InfoiconComponent implements OnInit {
ClosedTicket: any;
TotalEffort: any;
TicketedEffort: any;
NonTicketedEffort: any;
ClosedWorkItem: any;
WorkItemEffort: any;
CustomerId : string;
EmployeeId : string;
fdate: Date;
tdate: Date;
isCognizant : boolean = true;
isADMApplicableforCustomer : boolean = false;
  constructor( private infoService: InfoiconService, private datePipe: DatePipe
    ,private headerService : HeaderService) { }

  ngOnInit(): void {
    this.headerService.masterDataEmitter.subscribe((masterData: MasterDataModel) => {
      if(masterData != null){
      this.CustomerId = masterData.hiddenFields.customerId;
      this.EmployeeId = masterData.hiddenFields.employeeId;
      this.fdate = new Date(localStorage.getItem("Firstdayofweek"));
      this.tdate = new Date(localStorage.getItem("Lastdayofweek"));
      this.isCognizant = masterData.hiddenFields.isCognizant == 1 ? true : false;
    let FromDate = this.datePipe.transform(this.fdate,Constants.DateFormatdate);
    let ToDate =  this.datePipe.transform(this.tdate,Constants.DateFormatdate);
    this.isADMApplicableforCustomer = AppSettingsConfig.settings.ADMApplicableforCustomer;
    var Params = {
      EmployeeId: this.EmployeeId,
      CustomerId: this.CustomerId,
      FirstDateOfWeek: FromDate,
      LastDateOfWeek: ToDate
    }
    this.infoService.GetInfoIconDetails(Params).subscribe(x=>{
      if(x.closedTicket == null){
        x.closedTicket = "0";
      }
      if(x.closedWorkItem == null){
        x.closedWorkItem = "0";
      }
      if(x.ticketedEffort == null){
        x.ticketedEffort = "0.00";
      }
      if(x.nonTicketedEffort == null){
        x.nonTicketedEffort = "0.00";
      }
      if(x.workItemEffort == null){
        x.workItemEffort = "0.00";
      }
      if(x.totalEffort == null){
        x.totalEffort = "0.00";
      }
     this.ClosedTicket = x.closedTicket;
     this.TicketedEffort = x.ticketedEffort;
     this.NonTicketedEffort = x.nonTicketedEffort;
     this.ClosedWorkItem = x.closedWorkItem;
     this.WorkItemEffort = x.workItemEffort;
     this.TotalEffort = x.totalEffort;

    });
  
    }
  });
}
}
