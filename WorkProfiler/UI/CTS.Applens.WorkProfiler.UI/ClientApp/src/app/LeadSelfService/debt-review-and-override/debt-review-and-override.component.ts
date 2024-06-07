// Copyright (c) Applens. All Rights Reserved.
import { AfterViewChecked, ChangeDetectorRef, Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { SelectItem } from 'primeng/api';
import { Table } from 'primeng/table';
import { Observable } from 'rxjs';
import { AppSettingsConfig } from 'src/app/app.settings.config';
import { Constants } from 'src/app/common/constants/constants';
import { LayoutService } from 'src/app/common/services/layout.service';
import { SpinnerService } from 'src/app/common/services/spinner.service';
import { MasterDataModel } from 'src/app/Layout/models/header.models';
import { HeaderService } from 'src/app/Layout/services/header.service';
import { LeadselfService } from '../leadself.service';
import { ApproveDebtModel, DebtReviewModel, DebtReviewPostModel, DebtReviewUpload, DropdownModel, PortFolioModel, ProjectDropdownModel } from '../Models/LoadSelfServiceModel';


@Component({
  selector: 'app-debt-review-and-override',
  templateUrl: './debt-review-and-override.component.html',
  styleUrls: ['./debt-review-and-override.component.css']
})
export class DebtReviewAndOverrideComponent implements OnInit, AfterViewChecked {

  scrHeight: number;
  scrWidth: number;
  isExpanded: boolean = true;
  ProjectFilterDropdown: SelectItem[] = [];
  selectedProjectFilterDropdown: number = 0;
  closedDateFrom: Date = new Date();
  closedDateTo: Date = new Date();
  cardHeight: number = 0;
  debtReviewMasterList: DebtReviewModel[]=[];
  // debtReviewViewList: DebtReviewModel[]=[];
  // debtFilterMasterList: DebtReviewModel[]=[];
  showSearchBox: boolean = true;
  CauseCodeDropdown: DropdownModel[] = [];
  ResolutionCodeDropdown: DropdownModel[] = [];
  DebtOverrideReviewDropdown: DropdownModel[] = [];
  ResidualMasterDropdown: DropdownModel[] = [];
  AvoidableMasterDropdown: DropdownModel[] = [];
  ProjectDropdownList: ProjectDropdownModel[] = [];
  currentPage: number = 1;
  previousPage: number = 0;
  itemsPerPage: number = 100;
  itemsPerPageDropdown: string[] = ["100","200","300","400"];
  ReviewMasterDropdown: DropdownModel[] = [];
  SelectedReviewItems: DropdownModel;
  totalPages: number = 0;
  searchTicketID: string  = null;
  searchApplication: string = null;
  searchDescription: string = null;
  searchCauseCode: string = null;
  searchResolutionCode: string = null;
  searchDebtCategory: string = null;
  searchAvoidableFlag: string = null;
  searchResidualDebt: string = null;
  searchService: string = null;
  searchAssignedTo: string = null;
  searchf1:string = null;
  searchf2:string = null;
  searchf3: string = null;
  searchf4:string = null;
  employeeId: string = null;
  customerId: number = 0;
  file: File;
  extension: string;
  rowIdArray: number[] = [];
  selectedDebtData: DebtReviewPostModel[] = [];
  displaySpinner: boolean = false;
  displayTable: boolean = false;
  isCognizant: number = 0;
  isDebtEngineEnabled: boolean = true;
  isReviewRecordsDisplayed: boolean = false;
  isFlexField1Modified: boolean = false;
  isFlexField2Modified: boolean = false;
  isFlexField3Modified: boolean = false;
  isFlexField4Modified: boolean = false;
  flexField1ProjectWise: boolean = false;
  flexField2ProjectWise: boolean = false;
  flexField3ProjectWise: boolean = false;
  flexField4ProjectWise: boolean = false;
  updatedSucessMessage: boolean = false;
  approvedSucessMessage: boolean = false;
  selectItemsAlert: boolean = false;
  portFolioList: PortFolioModel[] = [];
  errorLogloading = false;
  @ViewChild('debtGrid', { static: false }) debtGrid: Table;
  toggleFilter = true;
  toggleErrorFilter = true;
  toggleicon = Constants.toggleIconOn;
  toggleErroricon = Constants.toggleIconOn;
  maxdate: Date = new Date();
  invalidDate: boolean = false;
  public searchbox = '';
  noRecordMsgFlag:boolean;
  flexField1ProjectName = '';
  flexField2ProjectName = '';
  flexField3ProjectName = '';
  flexField4ProjectName = '';  
  displayExemptedMsg=false;
  exemptedMsg:string;
  hiddendata: any;
  
  constructor(private layoutService: LayoutService, private leadSelf: LeadselfService, 
   private cdRef: ChangeDetectorRef, private headerService: HeaderService, private spinner: SpinnerService) {
  this.ReviewMasterDropdown.push({
    text: "Reviewed",
    value: "1"
  },
  {
    text: "Pending Review",
    value: "2"
  });
  this.SelectedReviewItems = this.ReviewMasterDropdown[1];
   }
  @HostListener('window:onLoad')
    @HostListener('window:resize')
    getScreenSize() {
          this.scrHeight = window.innerHeight;
          this.scrWidth = window.innerWidth;
    }

  ngAfterViewChecked(): void {
    if(this.layoutService.headerHeight > 0 && this.layoutService.footerHeight > 0){
      document.getElementById('debtContainer').style.height = this.scrHeight - (this.layoutService.headerHeight + this.layoutService.footerHeight + 30) + 'px';
    }
    if(document.getElementsByClassName('p-datepicker') != undefined){
      for (let index = 0; index < document.getElementsByClassName('p-datepicker').length; index++) {
        document.getElementsByClassName('p-datepicker')[index].setAttribute("style","top: 2px;z-index:1;padding:0;");
        for (let j = 0; j < document.getElementsByClassName('p-datepicker')[index].getElementsByTagName("td").length; j++) {
          document.getElementsByClassName('p-datepicker')[index].getElementsByTagName("td")[j].setAttribute("style","padding:0");
        }
      }
    }
    if(document.getElementsByClassName('p-dropdown-label') != undefined){
      for (let index = 0; index < document.getElementsByClassName('p-dropdown-label').length; index++) {
        document.getElementsByClassName('p-dropdown-label')[index].setAttribute("style","padding:0px; font-size: 0.8rem;");
      }
    }
    if(document.getElementsByClassName('p-datepicker p-datepicker-header') != undefined){
      for (let index = 0; index < document.getElementsByClassName('p-datepicker p-datepicker-header').length; index++) {
        document.getElementsByClassName('p-datepicker p-datepicker-header')[index].setAttribute("style","padding:0px;");
      }
    }
    if(document.getElementsByClassName('p-datatable-tbody') != undefined){
      for(let index = 0; index < document.getElementsByClassName('p-datatable-tbody').length; index++)
      {
        document.getElementsByClassName('p-datatable-tbody')[index].setAttribute("style","width: min-content;");
      }
    }
    if(document.getElementsByClassName('p-inputtext') != undefined){
      for(let index = 0; index < document.getElementsByClassName('p-inputtext').length; index++)
      {
        document.getElementsByClassName('p-inputtext')[index].setAttribute("style","padding: 9.5px;font-size:0.8rem;");
      }
    }
  }

  ngOnInit(): void {
   
    this.searchbox = '';
    this.closedDateFrom.setMonth(this.closedDateTo.getMonth() - 1);
    this.getScreenSize();
    this.spinner.show();
    this.headerService.masterDataEmitter.subscribe((masterData: MasterDataModel) => {
      this.spinner.hide();
      this.ProjectFilterDropdown = [];
      if(masterData != null){
        this.hiddendata = masterData.hiddenFields;
        this.employeeId = masterData.hiddenFields.employeeId;
        this.isCognizant = masterData.hiddenFields.isCognizant;
        if(masterData.hiddenFields.lstProjectUserID.length > 0){
          masterData.hiddenFields.lstProjectUserID.forEach(f=>{
            this.ProjectFilterDropdown.push({
              value: f.projectId,
              label: f.projectName
            })
          });
          this.selectedProjectFilterDropdown = this.ProjectFilterDropdown[0].value;
          this.customerId = +masterData.selectedCustomer.customerId;
          this.switchProject();
          this.leadSelf.GetDropDownValuesProjectPortfolio(window.btoa(this.employeeId),this.customerId).subscribe((data: any) => {
            this.portFolioList = [];
            if(data != undefined){
              this.portFolioList = JSON.parse(data).Project;
            }
            this.FetchDebtReviewDetails();
          });
        }
      }
    });
  }
  OpenDropdownPanel(){
    this.isExpanded = !this.isExpanded;
    if(this.isExpanded){
      this.cardHeight = 50;
    }
  }
  isEmpty(event){
    if(event.filteredValue.length ==0){
        this.noRecordMsgFlag = true;
    }
    else{
        this.noRecordMsgFlag = false;
    }
    }
  switchProject(){
      this.displayTable=false;   
      this.displayExemptedMsg=this.hiddendata.lstProjectUserID
      .filter(x=>x.projectId==this.selectedProjectFilterDropdown && x.isExempted==true).length>0
      ?true:false;
      this.switchOnProject();
      if(this.displayExemptedMsg){
      this.exemptedMsg=Constants.exemptedMessage;
      this.debtReviewMasterList=[];
    }
  }


  switchOnProject(){
    this.closedDateFrom = new Date();
    this.closedDateTo = new Date();
    this.closedDateFrom.setMonth(this.closedDateTo.getMonth() - 1);
    this.SelectedReviewItems = this.ReviewMasterDropdown[1];
  }
  FetchDebtReviewDetails()
  {
    this.currentPage = 1;
    this.displayTable = true;
    this.CauseCodeDropdown = [];
    this.ResolutionCodeDropdown = [];
    this.DebtOverrideReviewDropdown = [];
    this.AvoidableMasterDropdown = [];
    this.ResidualMasterDropdown = [];
    let tempStartDate: string = (this.closedDateFrom.getMonth() + 1) + '/' + this.closedDateFrom.getDate() +
     '/' + this.closedDateFrom.getFullYear() + ' ' + this.closedDateFrom.toLocaleTimeString();
    let tempEndDate: string = (this.closedDateTo.getMonth() + 1) + '/' + this.closedDateTo.getDate() +
     '/' + this.closedDateTo.getFullYear() + ' ' + this.closedDateTo.toLocaleTimeString();
    let review: number = (this.SelectedReviewItems != undefined) ? +this.SelectedReviewItems.value : 0;
    this.isDebtEngineEnabled = (this.portFolioList != null && 
      this.portFolioList.length > 0 && 
      this.portFolioList.find(f=>f.ProjectId == this.selectedProjectFilterDropdown) != undefined) ?
      (this.portFolioList.find(f=>f.ProjectId == this.selectedProjectFilterDropdown).IsTicketApprovalNeeded == 'Y'): false;
    if(this.isDebtEngineEnabled == true && this.closedDateFrom < this.closedDateTo){
      this.spinner.show();
      this.leadSelf.getDebtOverrideAndDictionaryData(tempStartDate, tempEndDate, this.customerId,
         this.employeeId, this.selectedProjectFilterDropdown, review).subscribe((res: any) => {
        const selectedReview: string = (this.ReviewMasterDropdown.find(f=>+f.value == review) != undefined)?(this.ReviewMasterDropdown.find(f=>+f.value == review).text): null;
        this.isReviewRecordsDisplayed = (selectedReview == "Reviewed");
        this.leadSelf.GetDebtclassification().subscribe((data) => {
          for (let index = 0; index < data.length; index++) {
            this.DebtOverrideReviewDropdown.push({
              value: data[index].debtClassificationId,
              text: data[index].debtClassificationName
            });
          }
          this.leadSelf.GetCausecode(this.selectedProjectFilterDropdown).subscribe((data)=>{
            for (let index = 0; index < data.length; index++) {
              this.CauseCodeDropdown.push({
                value: data[index].causeId,
                text: data[index].causeCode
              });
            }
          });
    
    
          this.leadSelf.GetResolutioncode(this.selectedProjectFilterDropdown).subscribe((data) => {
            for (let index = 0; index < data.length; index++) {
              this.ResolutionCodeDropdown.push({
                value: data[index].resolutionId,
                text: data[index].resolutionCode
              })
            }
          });
          
          this.leadSelf.GetResidualDebt().subscribe((data) => {
            this.spinner.hide();
            for (let index = 0; index < data.length; index++) {
              this.ResidualMasterDropdown.push({
                value: data[index].residualDebtId,
                text: data[index].residualDebtName
              });
            }
          })
        });
        this.debtReviewMasterList = [];
        let avoidableObj: DropdownModel;
          if(this.AvoidableMasterDropdown.length == 0){
            this.leadSelf.GetAvoidableFlag().subscribe((data) => {
              for (let index = 0; index < data.length; index++) {
                this.AvoidableMasterDropdown.push({
                  value: data[index].avoidableFlagId,
                  text: data[index].avoidableFlagName
                });
              }
              for(var i=0;i<res.debtOverRideReviews.length; i++)
              {
                avoidableObj = this.AvoidableMasterDropdown.find(f=>f.value == res.debtOverRideReviews[i].avoidableFlag);
                this.LoadViewModel(res, i, avoidableObj);                
              }
            }
            )
          }
          else
          {
            for(var i=0;i<res.debtOverRideReviews.length; i++)
              {
                this.LoadViewModel(res, i, avoidableObj);
              }
          }          
      },(error : Error) => {
      })
    }
    else if(this.closedDateFrom >= this.closedDateTo)
    {
      this.invalidDate = true;
      setTimeout(() => {
        this.invalidDate = false
      }, 1000);
    }
  }

  private LoadViewModel(res: any, i: number, avoidableObj: DropdownModel) {
    this.debtReviewMasterList.push({
      application: res.debtOverRideReviews[i].application,
      assignedTo: res.debtOverRideReviews[i].assignedTo,
      avoidableFlag: res.debtOverRideReviews[i].avoidableFlag.toString(),
      avoidableFlagText: res.debtOverRideReviews[i].avoidableFlagName.toString(),
      causeCodeMapId: res.debtOverRideReviews[i].causeCodeMapId.toString(),
      debtClassificationId: res.debtOverRideReviews[i].debtClassificationId.toString(),
      ticketDescription: res.debtOverRideReviews[i].ticketDescription,
      id: (i + 1),
      residualDebtMapId: res.debtOverRideReviews[i].residualDebtMapId.toString(),
      resolutionId: res.debtOverRideReviews[i].resolutionId.toString(),
      serviceName: res.debtOverRideReviews[i].serviceName,
      ticketId: res.debtOverRideReviews[i].ticketId,
      causeCode: res.debtOverRideReviews[i].causeCode,
      causeId: res.debtOverRideReviews[i].causeId,
      customerId: res.debtOverRideReviews[i].customerId,
      debtClassification: res.debtOverRideReviews[i].debtClassification,
      debtClassificationMapId: res.debtOverRideReviews[i].debtClassificationMapId,
      employeeId: res.debtOverRideReviews[i].employeeId,
      flexField1: res.debtOverRideReviews[i].flexField1,
      flexField1ProjectWise: res.debtOverRideReviews[i].flexField1ProjectWise === '0' ? null : res.debtOverRideReviews[i].flexField1ProjectWise != '' ? 'Flex1' : null,
      flexField2: res.debtOverRideReviews[i].flexField2,
      flexField2ProjectWise: res.debtOverRideReviews[i].flexField2ProjectWise === '0' ? null : res.debtOverRideReviews[i].flexField2ProjectWise != '' ? 'Flex2' : null,
      flexField3: res.debtOverRideReviews[i].flexField3,
      flexField3ProjectWise: res.debtOverRideReviews[i].flexField3ProjectWise === '0' ? null : res.debtOverRideReviews[i].flexField3ProjectWise != '' ? 'Flex3' : null,
      flexField4: res.debtOverRideReviews[i].flexField4,
      flexField4ProjectWise: res.debtOverRideReviews[i].flexField4ProjectWise === '0' ? null : res.debtOverRideReviews[i].flexField4ProjectWise != '' ? 'Flex4' : null,
      isAHTagged: res.debtOverRideReviews[i].isAHTagged,
      isApproved: res.debtOverRideReviews[i].isApproved,
      isCognizant: res.debtOverRideReviews[i].isCognizant,
      isFlexField1Modified: res.debtOverRideReviews[i].isFlexField1Modified,
      isFlexField2Modified: res.debtOverRideReviews[i].isFlexField2Modified,
      isFlexField3Modified: res.debtOverRideReviews[i].isFlexField3Modified,
      isFlexField4Modified: res.debtOverRideReviews[i].isFlexField4Modified,
      natureOfTheTicketName: res.debtOverRideReviews[i].natureOfTheTicketName,
      projectId: res.debtOverRideReviews[i].projectId,
      residualDebt: res.debtOverRideReviews[i].residualDebt,
      resolutionCode: res.debtOverRideReviews[i].resolutionCode,
      resolutionCodeMapId: res.debtOverRideReviews[i].resolutionCodeMapId,
      ticketType: res.debtOverRideReviews[i].ticketType
    });
    if(this.debtReviewMasterList.length > 0){
      this.flexField1ProjectWise = this.flexField1ProjectWise ? true : this.debtReviewMasterList[i].flexField1ProjectWise == 'Flex1' ? true : false;
      this.flexField2ProjectWise = this.flexField2ProjectWise ? true : this.debtReviewMasterList[i].flexField2ProjectWise == 'Flex2' ? true : false;
      this.flexField3ProjectWise = this.flexField3ProjectWise ? true : this.debtReviewMasterList[i].flexField3ProjectWise == 'Flex3' ? true : false;
      this.flexField4ProjectWise = this.flexField4ProjectWise ? true : this.debtReviewMasterList[i].flexField4ProjectWise == 'Flex4' ? true : false;
      this.flexField1ProjectName = res.debtOverRideReviews[0].flexField1ProjectWise;
      this.flexField2ProjectName = res.debtOverRideReviews[0].flexField2ProjectWise;
      this.flexField3ProjectName = res.debtOverRideReviews[0].flexField3ProjectWise;
      this.flexField4ProjectName = res.debtOverRideReviews[0].flexField4ProjectWise;
    }
  }

  private GetAvoidableMasterDropdown(): Observable<any> {
    return new Observable(observer => {
    this.leadSelf.GetAvoidableFlag().subscribe((data) => {
      for (let index = 0; index < data.length; index++) {
        this.AvoidableMasterDropdown.push({
          value: data[index].avoidableFlagId,
          text: data[index].avoidableFlagName
        });
      }
    },() => {},() => {
    });
    observer.complete();
    });
  }

  DownloadFile()
  {
    let tempStartDate: string = (this.closedDateFrom.getMonth() + 1) + '/' + 
    this.closedDateFrom.getDate() + '/' + this.closedDateFrom.getFullYear() + ' ' + this.closedDateFrom.toLocaleTimeString();
    let tempEndDate: string = (this.closedDateTo.getMonth() + 1) + '/' + 
    this.closedDateTo.getDate() + '/' + this.closedDateTo.getFullYear() + ' ' + this.closedDateTo.toLocaleTimeString();
    let review: number = (this.SelectedReviewItems != undefined) ? +this.SelectedReviewItems.value : 0;
    this.spinner.show();
    const searchModel = {
      StartDate: tempStartDate,
      EndDate: tempEndDate,
      CustomerID: this.customerId,
      EmployeeID: this.employeeId,
      ProjectID: this.selectedProjectFilterDropdown,
      ReviewStatus: review,
      isCognizant: this.isCognizant
    }
    this.leadSelf.DownloadFile(searchModel).subscribe(data => {
      this.spinner.hide();
      const contentDisposition = data.headers.get('Content-Disposition');
      const filename = this.getFilenameFromContentDisposition(contentDisposition);
      const blob = new Blob([data.body], {type: 'application/vnd.ms-excel'});
      let link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.download = filename;
      link.href = window.URL.createObjectURL(blob);
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(link.href);
    },(error: Error) => {
    })
  }

  private getFilenameFromContentDisposition(contentDisposition: string): string {
    const regex = /filename=(?<filename>[^,;]+);/g;
    const match = regex.exec(contentDisposition);
    const filename = match.groups.filename;
    return filename;
  }

  UploadFile(event)
  {
    this.extension = null;
    if(event != null && event.target.files.length > 0){
        this.file = event.target.files[0];
        this.extension = this.file.name.split('.')[1];
      }
      if(this.extension == "xlsm"){
        let tempStartDate: string = (this.closedDateFrom.getMonth() + 1) + '/' + 
        this.closedDateFrom.getDate() + '/' + this.closedDateFrom.getFullYear() + ' ' + this.closedDateFrom.toLocaleTimeString();
        let tempEndDate: string = (this.closedDateTo.getMonth() + 1) + '/' + 
        this.closedDateTo.getDate() + '/' + this.closedDateTo.getFullYear() + ' ' + this.closedDateTo.toLocaleTimeString();
        let uploadModel: DebtReviewUpload = {
          StartDate: tempStartDate,
          CloseDate: tempEndDate,
          EmployeeID: this.employeeId,
          IsCognizant: this.isCognizant,
          ProjectID: this.selectedProjectFilterDropdown,
          ReviewStatus: (this.SelectedReviewItems != undefined) ? +this.SelectedReviewItems.value : 0
        }
        this.spinner.show();
        this.leadSelf.fileChange(this.file, uploadModel).subscribe((data) => {
          this.updatedSucessMessage = true;
          this.spinner.hide();
          (<HTMLButtonElement>(document.getElementById('search-btn'))).click();
          (<HTMLInputElement>(document.getElementById('btnUpload'))).value = null;
          setTimeout(() => {
            this.updatedSucessMessage = false;
          },1000);
        });
      }
  }
  ClickFileUpload(){
    document.getElementById('btnUpload').click();
  }

  ToggleSelectAllStatus(event)
  {
    this.debtReviewMasterList.forEach(f=>{
      if(f.isAHTagged == false){
        this.ToggleSelectStatus(event, f);
      }
    });
  }

  ToggleSelectStatus(event,debtDetails: DebtReviewModel)
  {
    if(event.target.checked && this.rowIdArray.findIndex(f=>f == debtDetails.id) == -1){
      this.rowIdArray.push(debtDetails.id);
      (<HTMLInputElement>document.getElementById(debtDetails.id.toString())).checked = true;
    }
    else if(!event.target.checked){
      this.rowIdArray.splice(this.rowIdArray.findIndex(f=>f == debtDetails.id), 1);
      (<HTMLInputElement>document.getElementById(debtDetails.id.toString())).checked = false;
    }
    (<HTMLSelectElement>document.getElementById('CauseCode'+debtDetails.id)).disabled = !event.target.checked;
      (<HTMLSelectElement>document.getElementById('resolutionCode'+debtDetails.id)).disabled = !event.target.checked;
      (<HTMLSelectElement>document.getElementById('debtClassification'+debtDetails.id)).disabled = !event.target.checked;
      (<HTMLSelectElement>document.getElementById('avoidableFlag'+debtDetails.id)).disabled = !event.target.checked;
      (<HTMLSelectElement>document.getElementById('residualDetails'+debtDetails.id)).disabled = !event.target.checked;
      if(debtDetails.flexField1ProjectWise == 'Flex1'){
        (<HTMLInputElement>document.getElementById('flexField1'+debtDetails.id)).disabled = !event.target.checked;
      }
      if(debtDetails.flexField2ProjectWise == 'Flex2'){
        (<HTMLInputElement>document.getElementById('flexField2'+debtDetails.id)).disabled = !event.target.checked;
      }
      if(debtDetails.flexField3ProjectWise == 'Flex3'){
        (<HTMLInputElement>document.getElementById('flexField3'+debtDetails.id)).disabled = !event.target.checked;
      }
      if(debtDetails.flexField4ProjectWise == 'Flex4'){
        (<HTMLInputElement>document.getElementById('flexField4'+debtDetails.id)).disabled = !event.target.checked;
      }
    this.cdRef.detectChanges();
  }

  ApproveSelectedData()
  {
    if(this.rowIdArray.length > 0){
      this.spinner.show();
    let approveDebtModel: ApproveDebtModel = {
      EmployeeID: this.employeeId,
      ProjectID: this.selectedProjectFilterDropdown,
      ticketDetails: null
    };
    this.rowIdArray.forEach(f=>{
      var data = this.debtReviewMasterList.find(q=>q.id == f);
      if(data != undefined){
        this.selectedDebtData.push({
          application : data.application,
          assignedTo : data.employeeId,
          avoidableFlag : data.avoidableFlag.toString(),
          causeCodeMapId : data.causeCodeMapId.toString(),
          debtClassificationId : data.debtClassificationId.toString(),
          ticketDescription : data.ticketDescription,
          residualDebtMapId : data.residualDebtMapId.toString(),
          resolutionId : data.resolutionId.toString(),
          serviceName : data.serviceName,
          ticketId : data.ticketId,
          causeCode: (this.CauseCodeDropdown.find(f=>f.value == data.causeCodeMapId) != undefined)?(this.CauseCodeDropdown.find(f=>f.value == data.causeCodeMapId).text):null,
          causeId: data.causeId,
          customerId: data.customerId,
          debtClassification: this.DebtOverrideReviewDropdown.find(f=>f.value == data.debtClassificationId).text,
          debtClassificationMapId: data.debtClassificationId,
          employeeId: data.employeeId,
          flexField1: data.flexField1,
          flexField1ProjectWise: data.flexField1ProjectWise,
          flexField2: data.flexField2,
          flexField2ProjectWise: data.flexField2ProjectWise,
          flexField3: data.flexField3,
          flexField3ProjectWise: data.flexField3ProjectWise,
          flexField4: data.flexField4,
          flexField4ProjectWise: data.flexField4ProjectWise,
          isAHTagged: data.isAHTagged,
          isApproved: data.isApproved,
          isCognizant: data.isCognizant,
          isFlexField1Modified: data.isFlexField1Modified,
          isFlexField2Modified: data.isFlexField2Modified,
          isFlexField3Modified: data.isFlexField3Modified,
          isFlexField4Modified: data.isFlexField4Modified,
          natureOfTheTicketName: data.natureOfTheTicketName,
          projectId: data.projectId,
          residualDebt: this.ResidualMasterDropdown.find(f=>f.value == data.residualDebtMapId.toString()).text,
          resolutionCode: (this.ResolutionCodeDropdown.find(f=>f.value == data.resolutionId) != undefined) ? (this.ResolutionCodeDropdown.find(f=>f.value == data.resolutionId).text) : null,
          resolutionCodeMapId: data.resolutionId,
          ticketType: data.ticketType
        });
      }
    });
    approveDebtModel.ticketDetails = JSON.stringify(this.selectedDebtData);
    this.leadSelf.ApproveSelectedData(approveDebtModel).subscribe((data) => {
      let tempStartDate: string = (this.closedDateFrom.getMonth() + 1) + '/' + 
      this.closedDateFrom.getDate() + '/' + this.closedDateFrom.getFullYear() + ' ' + this.closedDateFrom.toLocaleTimeString();
    let tempEndDate: string = (this.closedDateTo.getMonth() + 1) + '/' + 
    this.closedDateTo.getDate() + '/' + this.closedDateTo.getFullYear() + ' ' + this.closedDateTo.toLocaleTimeString();
    let review: number = (this.SelectedReviewItems != undefined) ? +this.SelectedReviewItems.value : 0;
      this.leadSelf.getDebtOverrideAndDictionaryData(tempStartDate, tempEndDate, this.customerId, 
        this.employeeId,this.selectedProjectFilterDropdown, review).subscribe((res: any)=>{
        this.rowIdArray = [];
        approveDebtModel = {
          EmployeeID: this.employeeId,
          ProjectID: this.selectedProjectFilterDropdown,
          ticketDetails: null
        };
        this.spinner.hide();
        this.debtReviewMasterList = [];
      for(var i=0;i<res.debtOverRideReviews.length; i++)
      {
        var avoidableObj = this.AvoidableMasterDropdown.find(f=>f.value == res.debtOverRideReviews[i].avoidableFlag);
        this.LoadViewModel(res, i, avoidableObj);
      }
      });
    });
    this.approvedSucessMessage = true;
    setTimeout(() => {
      this.approvedSucessMessage = false;
    },1000);
    }
    else if(this.rowIdArray.length == 0){
      this.selectItemsAlert = true;
      setTimeout(() => {
        this.selectItemsAlert = false;
      },1000);
    }
  }

  clearErrorLogFilter(modelName: string, fieldName: string, searchOptions: string): void {
    this[modelName] = null;
    this.debtGrid.filter(null, fieldName, searchOptions);
  }
  toggleErrorLog(event): void {
    if (this.toggleErrorFilter) {
      this.toggleErrorFilter = false;
      this.toggleErroricon = Constants.toggleiconOff;
    } else {
      this.toggleErrorFilter = true;
      this.toggleErroricon = Constants.toggleIconOn;
    }
  }
  getTitle(values: any, id: any): string {
    var data = values.find(x => x.value == id);
    if (values && id && data != undefined) {
      return data.label;
    }
    return '';
  }
  ClearFilterData()
  {
    this.noRecordMsgFlag = false;
    this.debtGrid.clear();
    this.rowIdArray = [];
    this.searchTicketID = null;
    this.searchApplication = null;
    this.searchAssignedTo = null;
    this.searchAvoidableFlag = null;
    this.searchCauseCode = null;
    this.searchDescription = null;
    this.searchResidualDebt = null;
    this.searchResolutionCode = null;
    this.searchService = null;
    this.searchDebtCategory = null;
    this.debtReviewMasterList = [];
    let tempStartDate: string = (this.closedDateFrom.getMonth() + 1) + '/' + 
    this.closedDateFrom.getDate() + '/' + this.closedDateFrom.getFullYear() + ' ' + this.closedDateFrom.toLocaleTimeString();
    let tempEndDate: string = (this.closedDateTo.getMonth() + 1) + '/' + 
    this.closedDateTo.getDate() + '/' + this.closedDateTo.getFullYear() + ' ' + this.closedDateTo.toLocaleTimeString();
    let review: number = (this.SelectedReviewItems != undefined) ? +this.SelectedReviewItems.value : 0;
    this.leadSelf.getDebtOverrideAndDictionaryData(tempStartDate, tempEndDate, this.customerId, 
      this.employeeId,this.selectedProjectFilterDropdown, review).subscribe((res: any)=>{
      for(var i=0;i<res.debtOverRideReviews.length; i++)
      {
        var avoidableObj = this.AvoidableMasterDropdown.find(f=>f.value == res.debtOverRideReviews[i].avoidableFlag);
        this.LoadViewModel(res, i, avoidableObj);
      }
    });
  }

}
