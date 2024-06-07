// Copyright (c) Applens. All Rights Reserved.
import { DataDictionaryComponent } from './LeadSelfService/data-dictionary/data-dictionary.component';
import { TicketeffortuploadComponent } from './LeadSelfService/ticketeffortupload/ticketeffortupload.component';
import { ErrorlogComponent } from './LeadSelfService/errorlog/errorlog.component';
import { ApproveunfreezeComponent } from './LeadSelfService/approveunfreeze/approveunfreeze.component';
import { BasemeasuresComponent } from './mainspring-metrics/basemeasures/basemeasures.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HeaderComponent } from './Layout/header/header.component';
import { NavmenuComponent } from './Layout/navmenu/navmenu.component';
import {SearchticketComponent} from './searchticket/searchticket.component';
import { TimesheetentryComponent } from './AnalystSelfService/timesheetentry/timesheetentry.component';
import { DebtReviewAndOverrideComponent } from './LeadSelfService/debt-review-and-override/debt-review-and-override.component';
import { AppRoutes } from './common/constants/constants';
//KeyCloak Integratd Imports
import { AuthGuard } from './common/KeyCloakConfigurationFiles/Azure-AD/auth.guard';
import { CallbackComponent } from './common/KeyCloakConfigurationFiles/callback/callback.component';
//KeyCloak ENd
export const routingComponents = [HeaderComponent, NavmenuComponent];

export const routes: Routes = [
  { component: TimesheetentryComponent, path: AppRoutes.timesheetEntry, pathMatch: 'full',canActivate: [AuthGuard] },
  { component: DataDictionaryComponent, path: AppRoutes.datadictionary, pathMatch: 'full',canActivate: [AuthGuard] },
  { component: TicketeffortuploadComponent, path: AppRoutes.ticketeffortupload, canActivate: [AuthGuard] },
  { component: ErrorlogComponent, path: AppRoutes.errorlog, canActivate: [AuthGuard] },
  { component: ApproveunfreezeComponent, path: AppRoutes.approveunfreeze, pathMatch: 'full', canActivate: [AuthGuard] },
  { component: DebtReviewAndOverrideComponent, path: AppRoutes.debtreview , canActivate: [AuthGuard]},
  { component: BasemeasuresComponent, path: AppRoutes.basemeasures, pathMatch: 'full', canActivate: [AuthGuard] },
  { component: SearchticketComponent, path: 'searchticket', pathMatch: 'full', canActivate: [AuthGuard] },
  { path: 'callback', component: CallbackComponent,pathMatch: 'full' },
  { path: '', component: TimesheetentryComponent, pathMatch: 'full',canActivate: [] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { onSameUrlNavigation:'reload' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
