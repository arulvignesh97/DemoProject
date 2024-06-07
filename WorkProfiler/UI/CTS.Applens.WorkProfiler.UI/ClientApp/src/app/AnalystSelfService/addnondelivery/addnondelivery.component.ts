// Copyright (c) Applens. All Rights Reserved.
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { TimesheetentryComponent} from 'src/app/AnalystSelfService/timesheetentry/timesheetentry.component'
import { AddnondeliveryService } from 'src/app/AnalystSelfService/Service/addnondelivery.service';
import { DynamicgridComponent } from 'src/app/AnalystSelfService/dynamicgrid/dynamicgrid.component';
import { Constants } from 'src/app/AnalystSelfService/Constants/Constants';
import { DatePipe } from '@angular/common';
import { HeaderService} from 'src/app/Layout/services/header.service';
import { MasterDataModel } from 'src/app/Layout/models/header.models';
import { SelectItem } from 'primeng/api';
import { TranslateService } from '@ngx-translate/core';
import { SpinnerService } from 'src/app/common/services/spinner.service';
import { AppSettingsConfig } from 'src/app/app.settings.config';
import { LayoutService } from 'src/app/common/services/layout.service';
@Component({
  selector: 'app-addnondelivery',
  templateUrl: './addnondelivery.component.html',
  styleUrls: ['./addnondelivery.component.scss'],
  providers: [DatePipe]
})

export class AddnondeliveryComponent implements OnInit {
  AddNonDeliverypopup : boolean = true;
  ProjectDropdown: SelectItem[] = [];
  selectedProjectDropdown: number = 0;
  ActivityDropdown: SelectItem[] = [];
  selectedActivityDropdown: number = 0;
  activityNamelist = [];
  toAddDetails = [];
  SaveCalled : boolean =false;
  isEligible : boolean;
  projectdetails=[];
  Activitydetails=[]; 
  NonDeliveryDetails = [];
  public IsMandateMessageempty : boolean;
  public IsMandateMessageActivity : boolean; 
  public IsMandateMessageproject : boolean; 
  public IsMandateMessagelength : boolean;
  public IsMandateSugisnotvalid : boolean;
  
  public IsMandatealreadyexists : boolean;
  public Remarksvalue : string; 
  projectlist = [];
  activitylist = [];
  activityList = [];
  excludedWordslist = [];
  NonDeliveryValidationMessage : string = '';
  nonDeliveryList = [];
  CustomerId: string;
  EmployeeId: string;
  public fdate: Date;
  public tdate: Date;
  today: any = this.datePipe.transform(new Date(), 'MM/dd/yyyy');
  nondeliveryTicket :  FormGroup= new FormGroup({});
  displayExemptedMsg=false;
  exemptedMsg:string;
  hiddendata: any;

  constructor(private fb: FormBuilder,private timesheetentry:TimesheetentryComponent,
    private addNonDeliveryService : AddnondeliveryService, 
    private dynamicGridComponent : DynamicgridComponent, private readonly datePipe: DatePipe,
    private headerService : HeaderService,private translate: TranslateService,
    private spinnerService : SpinnerService,
    private layoutService:LayoutService) { }

  ngOnInit(): void {
    this.fdate = new Date(localStorage.getItem('Firstdayofweek'));
    this.tdate = new Date(localStorage.getItem('Lastdayofweek'));
    this.headerService.masterDataEmitter.subscribe((masterData: MasterDataModel) => {
     if(masterData != null){
    this.hiddendata = masterData.hiddenFields;
     this.CustomerId = masterData.hiddenFields.customerId;
     this.EmployeeId = masterData.hiddenFields.employeeId;
     this.AddNonDeliverypopup = true;
     this.LoadNonTicketActivity();
     this.nondeliveryTicket = this.fb.group({
      ProjectName : new FormControl(),
      Activity  : new FormControl(),
      SuggestedActivity : new FormControl(),
      Remarks : new FormControl()    
    });

    this.nondeliveryTicket.addControl('HdnProjectName',new FormControl(
                         { Visible :true,  IsMandatory :false, IsInvalid :false})); 
    this.nondeliveryTicket.addControl('HdnActivity',new FormControl(
                         { Visible :true, IsMandatory :false,IsInvalid :false}));
    this.nondeliveryTicket.addControl('HdnSuggestedActivity',new FormControl(
                         {  Visible :true,  IsMandatory :false, IsInvalid :false  }));
    this.nondeliveryTicket.addControl('HdnRemarks',new FormControl(
                         {  Visible :true,  IsMandatory :false, IsInvalid :false  }));
    this.nondeliveryTicket.controls['SuggestedActivity'].disable();
      }
    });
  }
  LoadNonTicketActivity(){
    
    let FromDate = this.datePipe.transform(this.fdate,Constants.DateFormat);
    let ToDate =  this.datePipe.transform(this.tdate,Constants.DateFormat);
    this.spinnerService.show();
    let Params = {
      CustomerID: this.CustomerId,
      EmployeeID: this.EmployeeId,
      FirstDateOfWeek: FromDate,
      LastDateOfWeek: ToDate
} 
this.addNonDeliveryService.GetNonTicketPopup(Params).subscribe(x => {
this.NonDeliveryDetails = x;
this.projectlist = x.projectModelDetails;
if(this.projectlist.length > 0){
  this.projectlist.forEach(f=>{
    this.ProjectDropdown.push({
      value: f.projectId,
      label: f.projectName
    })
  });
}

if (new Date(this.today) < new Date(FromDate)) {
  this.activityList = x.nonTicketPopupDetail.lstNonTicketActivity.filter(n => n.id == 1);
}
else{
  this.activityList = x.nonTicketPopupDetail.lstNonTicketActivity;
}
if(this.projectlist.length == 1) {
  this.selectedProjectDropdown = this.projectlist[0].projectId;
  this.LoadActivityDetails();
}
this.excludedWordslist = x.nonTicketPopupDetail.lstExcludedWords;
this.spinnerService.hide();
});
  }
  closenondelivery(){
    this.AddNonDeliverypopup = false;
    this.dynamicGridComponent.AddNonDeliverypopup = false;
  }
  clearnondelivery(){
    this.displayExemptedMsg=false;
    if(this.projectlist.length != 1){
    this.selectedProjectDropdown = 0;
    }
    this.selectedActivityDropdown = 0;
    this.nondeliveryTicket.controls['Remarks'].reset();
    this.nondeliveryTicket.controls['SuggestedActivity'].reset();
  }
  LoadActivityDetails(){
      this.displayExemptedMsg=this.hiddendata.lstProjectUserID
      .filter(x=>x.projectId==this.selectedProjectDropdown && x.isExempted==true).length>0
      ?true:false;
        if(!this.displayExemptedMsg){
          this.LoadActivityList();
        }
        else{
          this.exemptedMsg=Constants.exemptedMessage;
        }
  }

    LoadActivityList(){
    this.selectedActivityDropdown = 0;
    this.activitylist = []; 
    this.ActivityDropdown = [];
    this.activitylist = this.activityList;
    if(this.activitylist.length > 0){
      this.activitylist.forEach(f=>{
        this.ActivityDropdown.push({
          value: f.id,
          label: f.nonTicketedActivity
        })
      });
    }
  }
  SuggestedActivityValidation(event: any){ 
    this.nondeliveryTicket.controls['SuggestedActivity'].reset();
    if(event == 8){
      this.nondeliveryTicket.controls['SuggestedActivity'].enable();
    }
    else{
      this.nondeliveryTicket.controls['SuggestedActivity'].disable();
    }
  }

  setNonDeliveryValidationMessage(pattern: string): void { 
    this.translate.get(pattern)
          .subscribe(message => {           
            this.NonDeliveryValidationMessage = message;
            setTimeout(() => {
              this.NonDeliveryValidationMessage = '';
            }, 4000);
    });    
  }
    
  Savenondelivery(event){        
    this.isEligible = this.IsEligibleforSave();
      if (this.isEligible){
        let NonTicketActivityID = (this.nondeliveryTicket.controls['Activity'].value).toString();
        let ProjectID = (this.nondeliveryTicket.controls['ProjectName'].value).toString();
        this.activityNamelist = this.activitylist.filter(x => x.id == NonTicketActivityID)
        let ActivityName = this.activityNamelist[0].nonTicketedActivity;
        this.nonDeliveryList = this.dynamicGridComponent.GetNonDeliveryList();
        if(this.nonDeliveryList.filter(x => x.projectId == ProjectID && x.ticketDescription.toLowerCase().trim() == ActivityName.toLowerCase().trim()).length > 0){
          this.setNonDeliveryValidationMessage('ThechosenNonDeliveryActivityalreadyexistsintheTimeSheetGrid');
          setTimeout(() => { this.NonDeliveryValidationMessage = '' }, 4000);
        }
        else{
          this.spinnerService.show();
          let FromDate = this.datePipe.transform(this.fdate,Constants.DateFormat);
          let ToDate =  this.datePipe.transform(this.tdate,Constants.DateFormat);
          let Params = {
          EmployeeID: this.EmployeeId,
          CustomerID: this.CustomerId,
          FirstDateOfWeek: FromDate, 
          LastDateOfWeek: ToDate,
          TicketDescription: this.nondeliveryTicket.controls['Remarks'].value,
          NonTicketActivityID: NonTicketActivityID,
          ProjectID : ProjectID,
          SuggestedActivityName: this.nondeliveryTicket.controls['SuggestedActivity'].value,
        };
      this.addNonDeliveryService.SaveNonTicketDetails(Params).subscribe(x=>{       
        x.lstOverallTicketDetails[0].type = 'ND';
        x.lstOverallTicketDetails[0].ticketDescription = ActivityName;
        x.lstOverallTicketDetails[0].ticketId = Constants.NonDelivery;
        this.dynamicGridComponent.BindSelectedTicketsorWorkItems(x.lstOverallTicketDetails);
        this.closenondelivery();
        this.spinnerService.hide();
      });
      }
    }     
  
    }
    IsEligibleforSave(){    
      this.SaveCalled = true;
      if(this.nondeliveryTicket.controls['ProjectName'].value == 0 && this.nondeliveryTicket.controls["Activity"].value == 0){
        this.nondeliveryTicket.controls['HdnProjectName'].value.IsInvalid= true; 
        this.nondeliveryTicket.controls['HdnActivity'].value.IsInvalid= true;
        this.setNonDeliveryValidationMessage('PleaseCaptureAllTheMandatoryFields');
        setTimeout(() => { this.NonDeliveryValidationMessage = '' }, 4000);
        return false;
      }
      else if (this.nondeliveryTicket.controls["Activity"].value == 0 && this.nondeliveryTicket.controls["ProjectName"].value != 0){
        this.nondeliveryTicket.controls["HdnActivity"].value.IsInvalid= true;
        this.nondeliveryTicket.controls["HdnProjectName"].value.IsInvalid=false;
        this.setNonDeliveryValidationMessage('PleaseSelectANonTicketActivity');
        setTimeout(() => { this.NonDeliveryValidationMessage = '' }, 4000);
        return false;
      }
      else if(this.nondeliveryTicket.controls["ProjectName"].value == 0 && this.nondeliveryTicket.controls["Activity"].value != 0){
        this.nondeliveryTicket.controls["HdnProjectName"].value.IsInvalid= true; 
        this.nondeliveryTicket.controls["HdnActivity"].value.IsInvalid=false;
        this.setNonDeliveryValidationMessage('PleaseCaptureAllTheMandatoryFields');
        setTimeout(() => { this.NonDeliveryValidationMessage = '' }, 4000);
        return false;
      }
      else{
        this.nondeliveryTicket.controls["HdnProjectName"].value.IsInvalid = false; 
        this.nondeliveryTicket.controls["HdnActivity"].value.IsInvalid = false;
      }

      if(this.nondeliveryTicket.controls["Activity"].value == 8){
        this.nondeliveryTicket.controls["HdnProjectName"].value.IsInvalid = false; 
        this.nondeliveryTicket.controls["HdnActivity"].value.IsInvalid = false;
        let Remarks = this.nondeliveryTicket.controls["Remarks"].value;
        let SuggestedActivity = this.nondeliveryTicket.controls["SuggestedActivity"].value;
        if(Remarks == null){
          Remarks = "";
        }
        if(SuggestedActivity == null){
          SuggestedActivity = "";
        }

        if(Remarks.trim() == "" && SuggestedActivity.trim() == ""){
          this.nondeliveryTicket.controls["HdnSuggestedActivity"].value.IsInvalid= true; 
          this.nondeliveryTicket.controls["HdnRemarks"].value.IsInvalid= true;
          this.setNonDeliveryValidationMessage('PleaseCaptureAllTheMandatoryFields');
          setTimeout(() => { this.NonDeliveryValidationMessage = '' }, 4000);
          return false;
        }
        else if(Remarks.trim() != "" && SuggestedActivity.trim() == ""){
          this.nondeliveryTicket.controls["HdnSuggestedActivity"].value.IsInvalid = true; 
          this.nondeliveryTicket.controls["HdnRemarks"].value.IsInvalid = false;
          this.setNonDeliveryValidationMessage('PleaseCaptureAllTheMandatoryFields');
          setTimeout(() => { this.NonDeliveryValidationMessage = '' }, 4000);
          return false;
        }
        else if(Remarks.trim() == "" && SuggestedActivity.trim() != ""){
          this.nondeliveryTicket.controls["HdnSuggestedActivity"].value.IsInvalid=false; 
          this.nondeliveryTicket.controls["HdnRemarks"].value.IsInvalid= true;
          this.setNonDeliveryValidationMessage('PleaseCaptureAllTheMandatoryFields');
          setTimeout(() => { this.NonDeliveryValidationMessage = '' }, 4000);
          return false;
        }
        else{
          this.nondeliveryTicket.controls["HdnSuggestedActivity"].value.IsInvalid = false; 
          this.nondeliveryTicket.controls["HdnRemarks"].value.IsInvalid = false;
          let excludedcount = this.excludedWordslist.filter(x=>x.name.toLowerCase().trim() == SuggestedActivity.toLowerCase().trim()).length;
          if(excludedcount > 0){
            this.nondeliveryTicket.controls["HdnSuggestedActivity"].value.IsInvalid=true; 
            this.setNonDeliveryValidationMessage('Suggestedactivityisnotvalid');
            setTimeout(() => { this.NonDeliveryValidationMessage = '' }, 4000);
            return false;
          }
        }
        if(SuggestedActivity.trim().length <= 4){
          this.nondeliveryTicket.controls["HdnSuggestedActivity"].value.IsInvalid=true; 
          this.setNonDeliveryValidationMessage('Minimumcharacters');
          setTimeout(() => { this.NonDeliveryValidationMessage = '' }, 4000);
          return false;
        }
      }
      return true;
    }

    ExcludeSpecialCharacters(evt) {      
      let charCode = (evt.which) ? evt.which : evt.keyCode;
      if ((charCode >=48  && charCode <= 57) || (charCode >=65  && charCode <= 90) ||
      (charCode >= 97 && charCode <= 122) || (charCode == 32)){
          return true;
      }
      else{
          return false;
      }
    }
}