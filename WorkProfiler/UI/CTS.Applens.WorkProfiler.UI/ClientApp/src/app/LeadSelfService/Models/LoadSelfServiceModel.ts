// Copyright (c) Applens. All Rights Reserved.
export interface DebtReviewModel
{
    id: number;
    application: string
    assignedTo: string
    avoidableFlag: string
    avoidableFlagText: string
    causeCode: string
    causeCodeMapId: string
    causeId: string
    customerId: string
    debtClassification: string
    debtClassificationId: string
    debtClassificationMapId: string
    employeeId: string
    flexField1: string
    flexField1ProjectWise: string
    flexField2: string
    flexField2ProjectWise: string
    flexField3: string
    flexField3ProjectWise: string
    flexField4: string
    flexField4ProjectWise: string
    isAHTagged: boolean
    isApproved: number
    isCognizant: number
    isFlexField1Modified: string
    isFlexField2Modified: string
    isFlexField3Modified: string
    isFlexField4Modified: string
    natureOfTheTicketName: string
    projectId: number
    residualDebt: string
    residualDebtMapId: string
    resolutionCode: string
    resolutionCodeMapId: string
    resolutionId: string
    serviceName: string
    ticketDescription: string
    ticketId: string
    ticketType: string
}

export interface DebtReviewPostModel
{
    application: string
    assignedTo: string
    avoidableFlag: string
    causeCode: string
    causeCodeMapId: string
    causeId: string
    customerId: string
    debtClassification: string
    debtClassificationId: string
    debtClassificationMapId: string
    employeeId: string
    flexField1: string
    flexField1ProjectWise: string
    flexField2: string
    flexField2ProjectWise: string
    flexField3: string
    flexField3ProjectWise: string
    flexField4: string
    flexField4ProjectWise: string
    isAHTagged: boolean
    isApproved: number
    isCognizant: number
    isFlexField1Modified: string
    isFlexField2Modified: string
    isFlexField3Modified: string
    isFlexField4Modified: string
    natureOfTheTicketName: string
    projectId: number
    residualDebt: string
    residualDebtMapId: string
    resolutionCode: string
    resolutionCodeMapId: string
    resolutionId: string
    serviceName: string
    ticketDescription: string
    ticketId: string
    ticketType: string
}

export interface DropdownModel
{
    text: string;
    value: string;
}

export interface ProjectDropdownModel
{
    ProjectID: number;
    ProjectName: string;
    SupportTypeId: number;
    IsDDAutoClassified: string;
    IsManual: string;
    IsTicketApprovalNeeded: string;
}
export interface DebtReviewUpload
 {
    StartDate: string;
    CloseDate: string;
    ProjectID: number;
    IsCognizant: number;
    ReviewStatus: number;
    EmployeeID: string;
 }

 export interface ApproveDebtModel
 {
   ticketDetails: string
   EmployeeID: string
   ProjectID: number
 }

 export interface SearchDebtReviewDataModel
 {
  searchTicketID: string;
  searchApplication: string;
  searchDescription: string;
  searchCauseCode: string;
  searchResolutionCode: string;
  searchDebtCategory: string;
  searchAvoidableFlag: string;
  searchResidualDebt: string;
  searchService: string;
  searchAssignedTo: string;
  searchf1: string;
  searchf2: string;
  searchf3: string;
  searchf4: string;
 }
 export class PortFolioModel
 {
    IsDDAutoClassified: string;
    IsManual: string;
    IsTicketApprovalNeeded: string;
    ProjectId: number;
    ProjectName: string;
    SupportTypeId: number;
 }