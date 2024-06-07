// Copyright (c) Applens. All Rights Reserved.
export class Weekdays {
    Date: string;
    Day : string;    
    DisplayDate : string;
    FreezeStatus: string;
    StatusID: string;
    griddispalydate: string;
    griddate: string;
    gridday: string;
    gridmonth: string;
    EnabeheaderND: string;
    IsChanged: string ;
}
export class saveddatatemp{
  lstticketData: [];
}
export class Savedata{
  lstticket: saveddatatemp;
  Flag: number;
  EmployeeID: string;
  IsDaily: string;
}
export class deleteticket{
  activityId: number;
  CustomerID: number;
  EmployeeID: string;
  projectId: string;
  serviceId: string;
  StartDate: Date;
  EndDate: Date;
  SubmitterID: string;
  supportTypeId: number;
  ticketId : string;
  timeTickerId : string;
  TxtHours :string;
  type: string;
}
