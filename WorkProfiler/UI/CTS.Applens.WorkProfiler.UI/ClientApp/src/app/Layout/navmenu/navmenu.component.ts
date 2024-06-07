// Copyright (c) Applens. All Rights Reserved.
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Routes, RouterModule, ActivatedRoute, Params, Router } from '@angular/router';
import { AppSettingsConfig } from 'src/app/app.settings.config';
import { MasterDataModel } from '../models/header.models';
import { HeaderService } from '../services/header.service';
import { TicketEffortUploadServices } from 'src/app/LeadSelfService/Service/ticketeffortupload.service';

@Component({
  selector: 'app-navmenu',
  templateUrl: './navmenu.component.html',
  styleUrls: ['./navmenu.component.css']
})
export class NavmenuComponent implements OnInit {
  imagepresent: boolean;
  isNavCloseEnabled: boolean;
  public menuAnalystSelfServiceEnable: boolean = true;
  public menuLeadSelfServiceEnable: boolean = true;
  public subMenuTimesheetEnable: boolean = true;
  public subMenuApproveUnFreezeEnable: boolean = true;
  public subMenuTicketEffortUploadEnable: boolean = true;
  public subMenuErrorLogEnable: boolean = true;
  public subMenuDataDictionaryEnable = true;
  public subMenuDebtReviewEnable = true;
  public menuMainspringMetricsEnable = true;
  public menuSearchTicketsEnable = true;
  public subMenuBasemeasureEnable = true;
  public subMenuSprintReviewEnable = true;
  public subMenuTimesheetComplianceEnable = true;
  public subMenuDetailedTimesheetEnable = true;
  public subMenuAdMetricsEnable = true;
  public subMenubaseMeasureReportEnable = true;
  public subMenuTicketSummaryReportEnable = true;

  UserData: any;
  UserImage: string;
  UserName: string;
  UserRole: string;
  UserId: string;
  imagePresent: boolean;
  designation: string;
  showMask: boolean = false;
  IsSAAbove: string;
  customerName: string;

  roleMenus = [];
  AnalystSelfRole = false;
  LeadSelfServiceRole = false;
  datadictionaryEnabled = false;
  debtreviewEnabled = false;
  navMenUrl: any = {};

  customerId: number;
  employeeId: string;
  isCognizant: boolean;
  AnalystHasProject:boolean=false;

  menuRole:any;
  role:any;

  @Input('collapsed') collapsed;
  @Output('collapseClicked') collapseClicked = new EventEmitter<boolean>();
  tabdisplay: boolean =false;

  constructor(private headerService: HeaderService,private ticketUploadService:TicketEffortUploadServices) {
  }

  ngOnInit() {
    this.headerService.masterDataEmitter.subscribe((masterData: MasterDataModel) => {
      if (masterData) {
        this.customerName = masterData.hiddenFields.customerName;
        this.roleMenus = masterData.roleMenus;
        if (this.roleMenus.length > 0) {
          this.AnalystSelfRole = this.isValid(this.roleMenus.find(x => x.menuName === 'TicketingModule'));
          this.LeadSelfServiceRole = this.roleMenus[0].menuRole === 'Lead';
        }
        this.employeeId = masterData.hiddenFields.employeeId;
        this.isCognizant = (masterData.hiddenFields.isCognizant === 1);
        this.tabdisplay = (this.isCognizant || ((!this.isCognizant) && AppSettingsConfig.settings.ADMApplicableforCustomer)) == true ? true : false;
        if(masterData.hiddenFields.customerId == null){
          this.AnalystSelfRole = false;
        }
        if (masterData.customerDetails && masterData.customerDetails[0]) {
         this.customerId=masterData.hiddenFields.customerId;
        }
        this.checkAnalystProject();
      }
    });
    this.getMenuURL();
  }

  GetRoles(): void {
    const params = {
      mode: 'GetRoleUser',
      customerid: this.customerId,
      userid: this.employeeId
    };
    this.headerService.GetRoles(params)
      .subscribe(data => {

      });
  }

  GetTicketRoles(employeeId: string, customerId: number, projectId: number): void {
    const params = {
      employeeID: employeeId,
      customerID: Number(customerId),
      projectID: projectId
    };
    this.headerService.GetTicketRoles(params)
      .subscribe(data => {
        if (data.length > 0) {
          if (data[0].roleId == "101") {
            if (data.length > 1 && data[1].roleId == "102") {
              this.debtreviewEnabled = true;
              this.datadictionaryEnabled = true;
            }
          }
          else {
            this.debtreviewEnabled = false;
            this.datadictionaryEnabled = false;
          }
        }
        else {
          this.debtreviewEnabled = false;
          this.datadictionaryEnabled = false;
        }
      });
  }

  getMenuEnabled(menu: string) {
    if (this.roleMenus) {
      return this.isValid(this.roleMenus.find(x => x.menuName === menu));
    }
    return false;
  }
  isValid(value: any): boolean {
    return (value != null && value != undefined);
  }

  checkAnalystProject():void{
        const params={
          EmployeeID:this.employeeId,
          CustomerID:this.customerId,
          MenuRole:'Analyst'
        }
        this.ticketUploadService.ProjectDetails(params).subscribe(x=>{
          if(x.userDetails.length==0){
            this.AnalystHasProject=false;
          }
          else
          {
            this.AnalystHasProject=true;
          }
        });
        }

  ToggleSubMenu(menuLevel, showOrHide) {
    this.subMenuTimesheetEnable = true;
    this.subMenuApproveUnFreezeEnable = true;
    this.subMenuTicketEffortUploadEnable = true;
    this.subMenuErrorLogEnable = true;
    this.subMenuDataDictionaryEnable = true;
    this.subMenuDebtReviewEnable = true;
    this.subMenuBasemeasureEnable = true;
    this.subMenuSprintReviewEnable = true;
    this.subMenuTimesheetComplianceEnable = true;
    this.subMenuDetailedTimesheetEnable = true;
    this.subMenubaseMeasureReportEnable = true;
    this.subMenuTicketSummaryReportEnable = true;
    this.subMenuAdMetricsEnable = true;

    if (menuLevel == 1) {
      this.subMenuTimesheetEnable = false;
    }
    else if (menuLevel == 2) {
      this.subMenuApproveUnFreezeEnable = false;
    }
    else if (menuLevel == 3) {
      this.subMenuTicketEffortUploadEnable = false;
    } else if (menuLevel === 4) {
      this.subMenuErrorLogEnable = false;
    }
    else if (menuLevel === 5) {
      this.subMenuDataDictionaryEnable = false;
    } else if (menuLevel === 6) {
      this.subMenuDebtReviewEnable = false;
    }
    else if (menuLevel === 7) {
      this.subMenuBasemeasureEnable = false;
    } else if (menuLevel === 8) {
      this.subMenuSprintReviewEnable = false;
    } else if (menuLevel === 9) {
      this.subMenuTimesheetComplianceEnable = false;
    } else if (menuLevel === 10) {
      this.subMenuDetailedTimesheetEnable = false;
    }
    else if (menuLevel === 11) {
      this.subMenubaseMeasureReportEnable = false;
    }
    else if (menuLevel === 12) {
      this.subMenuTicketSummaryReportEnable = false;
    }
    else if (menuLevel === 13) {
      this.subMenuAdMetricsEnable = false;
    }
  }

  ToggleMainMenu(menuLevel, showOrHide) {
    this.menuAnalystSelfServiceEnable = true;
    this.menuLeadSelfServiceEnable = true;
    this.menuMainspringMetricsEnable = true;
    this.menuSearchTicketsEnable = true;
    if (menuLevel == 1) {
      if (!this.collapsed) {
        this.menuAnalystSelfServiceEnable = false;
      } else {
        this.menuAnalystSelfServiceEnable = !showOrHide;
      }
      
    }
    else if (menuLevel == 2) {
      if (!this.collapsed) {
        this.menuLeadSelfServiceEnable = false;
      } else {
        this.menuLeadSelfServiceEnable = !showOrHide;
      }
    }
    else if (menuLevel == 3) {
      if (!this.collapsed) {
        this.menuMainspringMetricsEnable = false;
      } else {
        this.menuMainspringMetricsEnable = !showOrHide;
      }
    }
    else if (menuLevel == 4) {
      if (!this.collapsed) {
        this.menuSearchTicketsEnable = false;
      } else {
        this.menuSearchTicketsEnable = !showOrHide;
      }
    }
    if (this.collapsed) {
      this.ToggleSubMenu(0, false);
    } else {
      this.collapsed = true;
      this.collapseClicked.emit(true);
    }
  }

  getMenuURL(): void {
    this.headerService.GetNavMenuUrl()
      .subscribe(urls => {
        this.navMenUrl = urls;
      });
  }

  openSprintReview(): void {
    this.ToggleSubMenu(8, false);
    if (this.navMenUrl) {
      window.open(this.navMenUrl.sprintReviewClosureUrl, '_blank');
    }
  }

  openTimesheetComplience(): void {
    this.ToggleSubMenu(9, false);
    if (this.navMenUrl) {
      const CustomerID = this.customerId;
      const newurl = this.navMenUrl.qlikSenseUrl;
      window.open(newurl, '_blank');
    }
  }
  openDetailedTimesheetReport(): void {
    this.ToggleSubMenu(10, false);
    if (this.navMenUrl) {
      const CustomerID = this.customerId;
      const newurl = this.navMenUrl.detailedTimesheetReportUrl;
      window.open(newurl, '_blank');
    }
  }
  openbaseMeaureReport(): void {
    this.ToggleSubMenu(11, false);
    if (this.navMenUrl) {
      const newurl = this.navMenUrl.basemeasureReportUrl;
      window.open(newurl, '_blank');
    }
  }
  openTicketSummaryReport(): void {
    this.ToggleSubMenu(12, false);
    if (this.navMenUrl) {
      const newurl = this.navMenUrl.servicePerformanceReportUrl;
      window.open(newurl, '_blank');
    }
  }

  openAdMetricsReport(): void {
    this.ToggleSubMenu(13, false);
    if (this.navMenUrl) {
      const newurl = this.navMenUrl.smartExeMetricsUrl;
      window.open(newurl, '_blank');
    }
  }

  subMenuCollapse() {
    
  }

  subMenuExpand() {
    
  }

  onClick(view) {
  }
}
