/***************************************************************************
*COGNIZANT CONFIDENTIAL AND/OR TRADE SECRET
*
*Copyright [2018] - [2021] Cognizant. All rights reserved.
*
*
*NOTICE: This unpublished material is proprietary to Cognizant and
*
*its suppliers, if any. The methods, techniques and technical
*
concepts herein are considered Cognizant confidential and/or trade secret information. 
*
*This material may be covered by U.S. and/or foreign patents or patent applications.
* 
*Use, distribution or copying, in whole or in part, is forbidden, except by express written permission of Cognizant.
***************************************************************************/

export interface Inputelements{
  HeaderIcons:Icon;
  HelpDocumentUrl:string;
  IsMyActivity:boolean;
  ApplicationName:string;
  IsCustomerPresent:boolean;
  ServiceAnalytics?:ServiceAnalytics;
  WorkProfiler?:WorkProfiler;
  BusinessProfiler?:boolean;
  AssociateData:AssociateInputs;
  RoleList:Role[];
}
export interface Icon{
  IsBreadcrumbs:boolean;
  IsHome:boolean;
}
  export interface AssociateInputs{
    IsMultiSelect:boolean;
    RoleEnabled:boolean;
    IsLogout:boolean;
    AssociateData:Associate;
    IsAvatarIcon:boolean;
  }


export interface WorkProfiler{
  IsAvatarIcon:boolean;
  Languages:Languagedropdown[];
  Projects:Projectdropdown[];
}
export interface DropdownElement{
  Id:number;
  Name:string;
}
export interface Projectdropdown{
  customerId: number;
  customerName: string;
}
export interface Languagedropdown{
  employeeId: string
isSelected: boolean
language: string
languageCode: string
languageNameInEnglish:string
}

export interface Roledropdown{
  IsMultiSelect:boolean;
  Role:Role[];
}
export interface Role {
  Id: number;
  Name: string;
  BussinesUnits: BusinessUnit[];
}

export interface BusinessUnit {
  Id: number;
  Name: string;
  Accounts: Account[];
}

export interface Account {
  Id: number;
  EsaCustomerId: string;
  Name: string;
  Projects: Project[];
}

export interface Project {
  Id: number;
  Name: string;
  EsaProjectId: string;
}

export class MyActivityCountModel{
    NotificationCount = 0 ;
    ApprovalCount = 0;
    TaskCount = 0;
    TotalCount = 0;
}
 
export interface HeaderDropdown{
  values:any[];
  optionlabel:string;
}

export interface Associate {
  ID: string;
  Name: string;
  UserProfileImageString: string;
  IsSuperAdmin: boolean;
  extension:string;
  userGroups:any[];
}
export interface RoleSelected{

  selectall:boolean;

  selectedrole:Role[];

}

export interface ServiceAnalytics{
  IsAnalyticsIcon: boolean;
  Values:string[];
}

