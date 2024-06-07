// Copyright (c) Applens. All Rights Reserved.
import { AfterViewChecked, Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import * as $ from 'jquery';
import { TranslateService } from '@ngx-translate/core';
import { HeaderService} from 'src/app/Layout/services/header.service';
import { DropdownModel } from 'src/app/LeadSelfService/Models/LoadSelfServiceModel';
import { LeadselfService } from 'src/app/LeadSelfService/leadself.service';
import { LayoutService } from 'src/app/common/services/layout.service';
import { LanguageModel, MasterDataModel } from '../models/header.models';
import { SpinnerService } from 'src/app/common/services/spinner.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppRoutes, Constants } from 'src/app/common/constants/constants';
import { Router } from '@angular/router';
// KeyCloak related Imports
import { KeyCloakConstants } from '../../../app/common/KeyCloakConfigurationFiles/KeyCloakEum';
import { AuthService } from '../../../app/common/KeyCloakConfigurationFiles/Azure-AD/auth.service';
import { AppSettingsConfig } from 'src/app/app.settings.config';
// KeyCloak related Imports End
//Header 
import { Associate } from 'src/app/Models/header';
import { keyframes } from '@angular/animations';
import { AssociateInputs, Inputelements, WorkProfiler } from 'src/app/Models/header';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit,AfterViewChecked {

  sidebarExpandEnable: boolean = false;
  header: string;
  imagepresent: boolean=false;
  public userimage:any;
  userimage1: any;
  public hiddenFields: any;
  IsCustomerPresent: boolean = true;
  userimage_secured: any;
  Roles: string[];
  selectedRole: string;
  languagedetails = [];
  customerdetails = [];
  languagecode = [];
  CustomerMasterDropdown: DropdownModel[] = [];
  masterDataModel: MasterDataModel;
  languageModel: LanguageModel;
  employeeName: string;
  defaultProjects = [];
  defaultPages = [];
  TSApprover = '';
  projectManager = '';
  projectAdmin = '';
  customerId = '';
  employeeId = '';
  defaultPage: any;
  defaultPageId: number;
  defaultProject: any;
  currentLang: any;
  currentProject: any;
  userClicked = false;
  IsAuth : boolean;
  projectPercentageList: any;
  refreshsidebar : boolean;
  collapsedState = false;
  associateData: Associate;
  // common header
  headerelement:Inputelements;
  //common header end

  @ViewChild('preferenceModal') public preferenceModal: ModalDirective;
  @ViewChild('userImage') public userImage: ElementRef;

   // KeyCloak related Properties
  appSettingsConfig =  AppSettingsConfig.settings;
  isKeycloakEnabled: boolean;
  isUserLogIn: boolean;
  displayExemptedMsg=false;
  exemptedMsg:string;
  // KeyCloak related Properties End
  constructor(public translate: TranslateService, 
    private headerService: HeaderService,
    private leadSelf: LeadselfService, 
    private layoutService: LayoutService,
    private spinner: SpinnerService,
    public router: Router,
    private el: ElementRef,
    private readonly idpAuthService: AuthService) 
    {
    this.Roles = ["Developer", "QA"];
    this.header = 'Work Profiler';
    this.selectedRole = this.Roles[0];
  }
  ngAfterViewChecked(): void {
    if(document.getElementsByClassName('header') != undefined){
      this.layoutService.headerHeight = document.getElementsByClassName('header')[0].clientHeight;
    }
  }
  //common header
  AssociateData:AssociateInputs=({AssociateData:({ID:'',
  IsSuperAdmin:false,Name:'',UserProfileImageString:'',extension:'',userGroups:[]}),IsAvatarIcon:true,RoleEnabled:false,
  IsMultiSelect:false,IsLogout:false}); //need
  profiler:WorkProfiler=({IsAvatarIcon:true,Languages:[],Projects:[]});
 //common header end

  ngOnInit() {  
    //KeyCloak Related Code
   this.isUserLogIn = AppSettingsConfig.isLogin;
   this.isKeycloakEnabled = this.appSettingsConfig.KeyCloakEnabled; 
   this.AssociateData.IsLogout=this.isKeycloakEnabled;
   //KeyCloak end
    this.IsAuth = true;
    this.spinner.show();
    this.masterDataModel = {
      customerDetails: null,
      hiddenFields: null,
      selectedCustomer: null,
      roleMenus: null
    }
    this.languageModel = {
      languageCode: null
    }  
    this.headerService.GetLanguageDetails().subscribe(x => {
     this.languagedetails = x;
     this.currentLang = this.languagedetails.find(x => x.languageCode === this.translate.defaultLang) ;
     this.translate.setDefaultLang('en'); 
     //common header
     this.languagedetails.forEach(lang => {
      this.profiler.Languages.push({
      languageCode: lang.languageCode,
      language: lang.languageNameInEnglish,
      languageNameInEnglish:lang.languageNameInEnglish,
      isSelected:lang.isSelected=true,
      employeeId:lang.employeeId=this.employeeId
       })
     }); 
     
    });
    //User profile pic from GraphAPI
    this.headerService.GetUserProfilePicture().subscribe((data: any) => {
      // Object.keys(data).forEach(key=>{
      //   const value=data[key];
      //   delete data[key];
      //   data[key.charAt(0).toUpperCase()+key.slice(1)]=value;
      // });
      if (data) {
        if(data.userProfileImageString)
        {
          const userdata = data;
          this.userimage=data.userProfileImageString;
          this.imagepresent = true;
       }
          this.associateData=data;
          this.associateData = {
          ID:data.id,
          IsSuperAdmin:false,
          Name:data.name,
          UserProfileImageString:data.userProfileImageString,
          extension:data.extension,
          userGroups:data.userGroups,
          }
         }
            else {
            this.imagepresent = false;
            };  
                  
  });
    this.headerService.GetCacheDetails().subscribe(x => {
      this.spinner.hide();
      this.refreshsidebar = true;
      if(x.hiddenFields.customerId == null){
        this.IsCustomerPresent = false;
      }
      else{
        this.IsCustomerPresent = true;
      }
      this.customerdetails = x.userWiseCustomer;
      this.hiddenFields = x.hiddenFields;
      if(x.hiddenFields.customerId == null){
        this.IsAuth = false;
      }
      else{
        this.IsAuth = true;
      }

        for (let index = 0; index < this.customerdetails.length; index++) {
        this.CustomerMasterDropdown.push({
          value: this.customerdetails[index].customerID,
          text: this.customerdetails[index].customerName
        });
      }
      if(x.hiddenFields.customerId != null){
      this.employeeName = this.hiddenFields.employeeName;
      this.AssociateData.AssociateData.Name= this.employeeName;
      this.masterDataModel.customerDetails = this.customerdetails;
      this.masterDataModel.hiddenFields = this.hiddenFields;
      this.masterDataModel.selectedCustomer = this.customerdetails[0];
      this.masterDataModel.roleMenus = x.rolePrevilageMenus;
      this.headerService.masterDataEmitter.next(this.masterDataModel);
      if (this.customerdetails[0]) {
      this.customerId = this.customerdetails[0].customerId;
      }
      this.employeeId = this.hiddenFields.employeeId;
      localStorage.setItem('userId', this.employeeId);
      }
      if (x.hiddenFields.customerId != null) {
        this.GetProjectDetailsforDefaultLanding(this.customerId, this.employeeId);
        this.GetDefaultLandingPageDetails(this.customerId, this.employeeId);
        this.GetCustomerwiseDefaultPage();
      }
      
     //common header
      this.customerdetails.forEach(cust => {
      this.profiler.Projects.push({
      customerId:cust.customerId,
      customerName:cust.customerName,
    });
    
    this.AssociateData.AssociateData=this.associateData
            this.headerelement={   
              ApplicationName:'Work Profiler',
              HeaderIcons:{
              IsBreadcrumbs:true,
              IsHome:false},
              HelpDocumentUrl:'',
              IsMyActivity:false,
              IsCustomerPresent:false,
              ServiceAnalytics:null,
              RoleList:null,
              AssociateData:this.AssociateData,
              WorkProfiler:this.profiler,  
             }

  });
     
    //common header end
     });
     
     this.SetCommonScripts();
     
  }
  //Common header code only for deployment
  private SetCommonScripts()
  {
  
  const origin = location.origin;
  //HeaderScripts
  let headerscript = document.createElement("script");
  const headerpath = origin + AppSettingsConfig.settings.HeaderPath;
  headerscript.setAttribute("src", headerpath);
  document.body.appendChild(headerscript);

  //FooterScripts
  let footerscript = document.createElement("script");
  const footerpath = origin + AppSettingsConfig.settings.FooterPath;
  footerscript.setAttribute("src", footerpath);
  document.body.appendChild(footerscript);

  //Background Styles
  var head = document.getElementsByTagName('HEAD')[0];
  var link  = document.createElement('link');
  link.rel  = 'stylesheet';
  link.type = 'text/css';
  link.href = origin + AppSettingsConfig.settings.CommonCss;
  link.media = 'all';
  head.appendChild(link);
  }
  //common header end   


  GetCustomerwiseDefaultPage(): void {
    
    const params = {
      CustomerID: this.customerId,
      EmployeeID: this.employeeId
    }
    this.headerService.GetCustomerwiseDefaultPage(params)
    .subscribe(data => {
      this.defaultPageId = Number(data);
      this.defaultPage = this.defaultPages.find(x => x.privilegeId === this.defaultPageId); 
      const url = this.router.url;
      const analyst: HTMLElement = document.querySelector('.analyst-self');
      const leadself: HTMLElement = document.querySelector('.lead-self-link');
      if (this.defaultPageId == 2) {
        const timesheet: HTMLElement = document.querySelector('#timesheetLink');
        if (analyst && timesheet) {
          if (url.indexOf(AppRoutes.timesheetEntry) === -1) {
            analyst.click();
          }          
          timesheet.click();
        }
      }
      else if ( this.defaultPageId == 5) {
        const ticketEffort: HTMLElement = document.querySelector('#ticket-effort-upload');
        if (leadself && ticketEffort) {
          if (url.indexOf(AppRoutes.ticketeffortupload) === -1) {
            leadself.click();
          }           
          ticketEffort.click();
        }
      }
      else if ( this.defaultPageId == 8) {
        const debtReview: HTMLElement = document.querySelector('#debt-review');
        if (leadself && debtReview) {
          if (url.indexOf(AppRoutes.debtreview) === -1) {
            leadself.click();
          }  
          debtReview.click();
        }
      }
      else if ( this.defaultPageId == 9) {
        const approve: HTMLElement = document.querySelector('#approve-unfreeze');
        if (leadself && approve) {
          if (url.indexOf(AppRoutes.approveunfreeze) === -1) {
            leadself.click();
          }  
          approve.click();
        }
      }     
      
    });
  }

  GetMyAssociateURL(): void {
    this.spinner.show();
    this.headerService.GetMyAssociateURL()
    .subscribe(data => {
      if (data) {
        this.spinner.hide();
      window.open(data);
      }
    });
  }
  GetHomePage(): void {

    this.spinner.show();

    this.headerService.GetHomePage()

      .subscribe((data: any) => {

        if (data) {

          this.spinner.hide();

          window.open(data);

        }

      });

  }

  GetProjectDetailsforDefaultLanding(customerId: any, employeeId: any): void {
    const params = {
      CustomerID: customerId,
      EmployeeID: employeeId
    }
    this.headerService.GetProjectDetailsforDefaultLanding(params)
    .subscribe(data => {
      this.defaultProjects = data;
      if (this.defaultProjects && this.defaultProjects[0]) {
        this.defaultProject = this.defaultProjects[0].projectId;
      this.GetProjectLeadDetails(this.defaultProjects[0].projectId, this.employeeId);
      }
    });
  }
      GetProjectLeadDetails(projectId: any, employeeId: any): void {
       
          this.displayExemptedMsg=this.masterDataModel.hiddenFields.lstProjectUserID
          .filter(x=>x.projectId==projectId && x.isExempted==true).length>0
          ?true:false;
          if(!this.displayExemptedMsg){
          this.GetProjectLead(projectId,employeeId);
          }
          else{
          this.exemptedMsg=Constants.exemptedMessage;
          }
      }
      
  GetProjectLead(projectId: any, employeeId: any): void {
    const params = {
      ProjectID: projectId.toString(),
      EmployeeID: employeeId
    }
    this.headerService.GetProjectLeadDetails(params)
    .subscribe(data => {
      const lead = data[0];
      if (lead) {
      this.TSApprover = lead.projectTSApprover;
      this.projectAdmin = lead.projectAdmin;
      this.projectManager = lead.projectManager;
      }
    });
  }

  GetDefaultLandingPageDetails(customerId: any, employeeId: any): void {
    const params = {
      CustomerID: customerId,
      EmployeeID: employeeId
    }
    this.headerService.GetDefaultLandingPageDetails(params)
    .subscribe(data => {
      this.defaultPages = data;
      this.defaultPage = this.defaultPages.find(x => x.privilegeId === this.defaultPageId);    
    });
  }

  SaveUserIconDetails(): void {
    this.spinner.show();
    const params = {
      EmployeeID: this.employeeId,
      AccountID: this.customerId,
      PrivilegeID: this.defaultPage.privilegeId.toString()
    };
    this.headerService.SaveDefaultLandingPageDetails(params)
    .subscribe(data => {
     if (data) {
      this.defaultPage = this.defaultPage;
      this.preferenceModal.hide();
      this.spinner.hide();
      this.GetCustomerwiseDefaultPage();
     }
    });
  }

  switchLang(lang) {
    this.spinner.show();
    this.languagecode = [];
    this.languagecode = this.languagedetails.filter(y=> y.languageNameInEnglish == lang.languageNameInEnglish);
    var langcode = this.languagecode[0].languageCode
    var Params =  {
      EmployeeId: this.masterDataModel.hiddenFields.employeeId,
      Language: lang.language,
      UserId: this.masterDataModel.hiddenFields.employeeId
    }
    this.headerService.SaveLanguageDetails(Params).subscribe(x =>{
      if(x == "Y") {
      this.translate.use(langcode);
      this.languageModel.languageCode = langcode;
      this.headerService.languageDataEmitter.next(this.languageModel);
      this.spinner.hide();
      }
    });
  }

  preferenceModalClose(): void {
    if (this.preferenceModal.isShown) {
      this.userClicked = false;
      this.preferenceModal.hide(); 
      const setDivPosition: HTMLElement = document.querySelector('div');
      setDivPosition.style.setProperty('position', 'fixed');   
    }
  }

  openPreferenceModal(): void {
    const removeDivPosition: HTMLElement = document.querySelector('div');
    removeDivPosition.style.removeProperty('position');
    if (!this.preferenceModal.isShown) {
      this.userClicked = true;
      if (this.customerId) {
      this.GetProjectDetailsforDefaultLanding(this.customerId, this.employeeId);
      this.GetDefaultLandingPageDetails(this.customerId, this.employeeId);
      this.preferenceModal.show();
      }
    }
  }

  sidebarCollapse() {
    if ($("#sidebar").length) {
      $('#sidebar').addClass('active');
      this.sidebarExpandEnable = false;
      this.getCollapseChange()
    }
  }

  sidebarExpand() {
    if ($("#sidebar").length) {
      $('#sidebar').removeClass('active');
      this.sidebarExpandEnable = true;
     this.getCollapseChange()
    }
  }

   getCollapseChange(): void {
    this.collapseChange(this.sidebarExpandEnable);
  }
 collapseChange(event): void {
     this.collapsedState = event;
  }
  collapseClicked(event): void {
    if (event) {
      this.sidebarExpand();
    }
 }
  switchCustomer(event){
    this.spinner.hide();
    this.refreshsidebar = true;
    var Params = {
      EmployeeId : this.masterDataModel.hiddenFields.employeeId,
      CustomerId : event.customerId
    }
    this.headerService.GetHiddenFields(Params).subscribe(x =>{
      this.spinner.hide();
      this.hiddenFields = x.hiddenFields;
      this.masterDataModel.customerDetails = this.customerdetails;
      this.masterDataModel.hiddenFields = x.hiddenFields;
      this.masterDataModel.selectedCustomer = this.customerdetails.filter(k=> k.customerId == event.customerId)[0];
      this.masterDataModel.roleMenus = x.rolePrevilageMenus;
      this.headerService.masterDataEmitter.next(this.masterDataModel);
      this.customerId = event.customerId;
      this.GetCustomerwiseDefaultPage();
      this.spinner.hide();
      }
    )
   
  }

  getMargin(): any {
    let left = 0;
    if (this.userImage && this.userImage.nativeElement.offsetLeft > 0) {
      left = this.userImage.nativeElement.offsetLeft - 242;
    }
    return left  + 'px';
  }

  ngAfterViewInit() {
    setTimeout(() => {
      if ($("#sidebar").length) {
        $('#sidebar').addClass('active');
      this.getCollapseChange()
      }
    }, 500);
    this.preferenceModal.config = {
      backdrop: true,
      ignoreBackdropClick: true,
    };
  }

  //KeyCloak related Method
  LogOutUser() {
    this.idpAuthService.logout();
  }
    //common header 
    SideNavToggle()
  {
    if ($("#sidebar").length && this.sidebarExpandEnable==true)
      {
       $('#sidebar').addClass('active');
       this.sidebarExpandEnable = false;
       this.getCollapseChange()
      }
    else
      {
        if ($("#sidebar").length) {
        $('#sidebar').removeClass('active');
        this.sidebarExpandEnable = true;
        this.getCollapseChange()
        }
      }    
  }
}
