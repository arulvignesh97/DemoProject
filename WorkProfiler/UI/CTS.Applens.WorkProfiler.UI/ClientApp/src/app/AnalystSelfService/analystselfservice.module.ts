// Copyright (c) Applens. All Rights Reserved.
import { AddnondeliveryComponent } from './addnondelivery/addnondelivery.component';
import { TicketAttributesComponent } from './ticket-attributes/ticket-attributes.component';
import { EffortgraphComponent } from './effortgraph/effortgraph.component';
import { TimesheetentryComponent } from './timesheetentry/timesheetentry.component';
import { AddworkitemComponent } from './addworkitem/addworkitem.component';
import { InfoiconComponent} from './infoicon/infoicon.component';

import { NgModule } from '@angular/core';
import { ErrorTicketFilterPipe } from './Pipes/error-ticket-filter.pipe';
@NgModule({
    declarations: [
    AddnondeliveryComponent,
    TicketAttributesComponent,
    TimesheetentryComponent,
    EffortgraphComponent,
    AddworkitemComponent,
    InfoiconComponent,
    ErrorTicketFilterPipe
],
imports: [
],
})

export class Analystselfservicemodule { }