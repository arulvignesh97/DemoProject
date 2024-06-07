// Copyright (c) Applens. All Rights Reserved.
import { Component, OnInit } from '@angular/core';
import { ErrorLogServices } from '../Service/errorlog.service';
import { SpinnerService } from './../../common/services/spinner.service';
import { Constants } from 'src/app/common/constants/constants';
import { HeaderService } from 'src/app/Layout/services/header.service';
import { MasterDataModel } from '../../Layout/models/header.models';
import { MessageService } from 'primeng/api';
import { DatePipe } from '@angular/common';
import { TranslateService } from '@ngx-translate/core';

import { Routes, RouterModule, ActivatedRoute, Params, Router } from '@angular/router';
import { LayoutService } from 'src/app/common/services/layout.service';
import { AppSettingsConfig } from 'src/app/app.settings.config';


@Component({
  selector: 'app-errorlog',
  templateUrl: './errorlog.component.html',
  styleUrls: ['./errorlog.component.css'],
  providers: [MessageService, DatePipe]
})

export class ErrorlogComponent implements OnInit {
  public projectListData = [];
  public errorLog = [];
  public employeeID;
  public customerID;
  public projectID;
  public isCognizant;
  public choice;
  public noData: boolean = false;
  public ticketLogShow: boolean = false;
  public errorLogShow: boolean = false;
  public errorMessageShow: boolean = false;
  public errorMessage: string;
  
  id:number;
  displayExemptedMsg=false;
  exemptedMsg:string;
  hiddendata: any;

  constructor(private ErrorLogServices: ErrorLogServices, private messageService: MessageService,
    private spinner: SpinnerService, private datePipe: DatePipe, private headerService: HeaderService, private translate: TranslateService,
        private router:ActivatedRoute,
        private layoutService:LayoutService) { }


  ngOnInit(): void {
    this.spinner.show();

    this.router.params.subscribe((params:Params)=>{
      this.id=params['id']
    })


    this.headerService.masterDataEmitter.subscribe((masterData: MasterDataModel) => {
      if (masterData != null) {
        this.hiddendata = masterData.hiddenFields;
        this.employeeID = masterData.hiddenFields.employeeId;
        this.customerID = masterData.selectedCustomer.customerId;
        this.isCognizant = masterData.hiddenFields.isCognizant;
        const Params = {
          EmployeeID: this.employeeID,
          CustomerID: this.customerID,
          MenuRole:this.id==1?'Analyst':'Lead'
        };
        this.ErrorLogServices.ProjectDetails(Params).subscribe(x => {
          if (x.length > 0) {
            this.projectListData = x;
            this.projectID = this.projectListData[0].projectId;
            this.choice = "1";
            this.SetProject(this.projectID);
            this.GetConfigDetails();
          }
          else {        
            this.projectListData = [];
            this.errorLog = [];
            this.ticketLogShow = false;
            this.errorLogShow = false;
            this.choice = null;
            this.projectID = null;
            this.spinner.hide();
          }
        });
      }
    });
  }

  Choice(value: any) {
    this.choice = value.toString();
    this.GetLog();
  }

  GetConfigDetails() {
    const Params = {
      ProjectID: this.projectID
    };
    this.ErrorLogServices.GetConfigDetails(Params).subscribe(x => {
      if (x[0].isActive == "1" && x[1].isActive == "1") {
        this.ticketLogShow = true;
        this.errorLogShow = true;
        this.choice = "1";
        this.GetLog();
        }
      else if (x[0].isActive == "1") {
        this.ticketLogShow = false;
        this.errorLogShow = true;
        this.choice = "0";
        this.GetLog();
        }
      else if (x[1].isActive == "1") {
        this.ticketLogShow = true;
        this.errorLogShow = false;
        this.choice = "1";
        this.GetLog();
        }
      else {
        this.ticketLogShow = false;
        this.errorLogShow = false;
        this.noData = true;
      }
      this.spinner.hide();
    });
  }

  SetProject(val: any) {
      this.displayExemptedMsg=this.hiddendata.lstProjectUserID
      .filter(x=>x.projectId==val && x.isExempted==true).length>0
      ?true:false;
      this.onProjectChange(val);
      if(this.displayExemptedMsg){       
      this.exemptedMsg=Constants.exemptedMessage;
      }   
  }
 onProjectChange(val:any){
  this.spinner.show();
  this.projectID = val;
  this.GetConfigDetails();
 }

  ExportExcelClick(fileName: any, value: any) {
    if (fileName != '') {
      this.spinner.show();
      const Params = {
        FileName: fileName,
        Choice: value.toString()
      };
          this.ErrorLogServices.Download(Params).subscribe(x => {
            const blob = new Blob([x], { type: Constants.ExcelType });
            const url = window.URL.createObjectURL(blob);
            const downloadTag = document.createElement('a');
            downloadTag.download = fileName;
            downloadTag.href = url;
            downloadTag.click();
          });
        this.spinner.hide();
    }
  }

  GetLog() {
    const Params = {
      ProjectID: this.projectID,
      Choice: this.choice
    };
    this.ErrorLogServices.ErrorLog(Params).subscribe(x => {
      if (x.length != 0) {
        this.errorLog = x;
        this.noData = false;
        for (let i = 0; i < this.errorLog.length; i++) {
          this.errorLog[i].uploadedDate = this.getDate(this.errorLog[i].uploadedDate);
        }
      }
      else {
        this.noData = true;
      }
    });
  }

  getDate(date: string): string {
    if (date.toString() == "") {
      return "";
    }
    else {
      return this.datePipe.transform(new Date(date), 'MM/dd/yyyy hh:mm:ss a');
    }
  }

  setErrorMessage(pattern): void {
    this.translate.get(pattern)
      .subscribe(message => {
        this.errorMessageShow = true;
        this.errorMessage = message;
        setTimeout(() => {
          this.errorMessageShow = false;
          this.errorMessage = '';
        }, Constants.lifeSpan);
      });
  }

}
