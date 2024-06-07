// Copyright (c) Applens. All Rights Reserved.
import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { DatePipe } from '@angular/common';
import { parseDate } from 'ngx-bootstrap/chronos';

@Component({
  selector: 'app-timesheetentry',
  templateUrl: './timesheetentry.component.html',
  styleUrls: ['./timesheetentry.component.scss'],
  providers: [DatePipe]
})
export class TimesheetentryComponent implements OnInit {
  AddWorkItempopup: boolean;
  public addTicketDialog: boolean;
  public chooseTicketDialog: boolean;
  AddNonDeliverypopup: boolean;
  public erroredTicketDialog = false;
  Dynamic: boolean = false;
 

  constructor(private datePipe: DatePipe) {
  }

  ngOnInit(): void {
  
   
  }
 
  public searchPopup(param: boolean): void{
      this.chooseTicketDialog = param;
    }
  erroredTicketsPopup(template: TemplateRef<any>): void {
        this.erroredTicketDialog = true;
  }
}
