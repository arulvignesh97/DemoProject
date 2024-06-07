// Copyright (c) Applens. All Rights Reserved.
export class BaseInformationModel{
    EmployeeId: string;
    CustomerId: string;
    FirstDateOfWeek: string;
    LastDateOfWeek: string;
    TicketDescription: string;
    TicketId: string;
    NonTicketActivityId: string;
    ProjectId: string;
    SuggestedActivityName: string;
    Language: string;
    UserId: string;
    SupportTypeId: number;
    ApplicationId: number;
    PortfolioId: number;
    CauseCode: string;
}
export class SelectedTicketModel{
    public CustomerID: string;
    public  EmployeeID: string;
    public  FirstDateOfWeek: string;
    public  LastDateOfWeek: string;
    public  Mode: string;
    public  TicketID_Desc: any[];
    public  ProjectID: string;
    public isCognizant: number;
}