// Copyright (c) Applens. All Rights Reserved.
import { AppConfig } from './common/Config/config';
import { HttpServiceInterceptor } from './common/services/http.interceptor';
import { AppSettingsConfig } from './app.settings.config';
import { LeadselfServiceModule } from './LeadSelfService/leadself-service.module';
import { MainspringMetricsModule } from './mainspring-metrics/mainspring-metrics.module';
import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule,ErrorHandler} from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppRoutingModule, routingComponents } from './app-routing.module';
import { AppComponent } from './app.component';
import { FooterComponent} from './Layout/footer/footer.component';
import { TableModule} from 'primeng/table';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import {​​​​​​​​ BsDatepickerModule, DatepickerModule }​​​​​​​​ from 'ngx-bootstrap/datepicker';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AccordionModule } from 'primeng/accordion';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { DynamicgridComponent } from './AnalystSelfService/dynamicgrid/dynamicgrid.component';
import { TimesheetentryComponent } from './AnalystSelfService/timesheetentry/timesheetentry.component';
import { EffortgraphComponent } from './AnalystSelfService/effortgraph/effortgraph.component';
import { AddTicketComponent } from './AnalystSelfService/add-ticket/add-ticket.component';
import { ChooseWorkitemTicketComponent } from './AnalystSelfService/choose-workitem-ticket/choose-workitem-ticket.component';
import {AddworkitemComponent} from './AnalystSelfService/addworkitem/addworkitem.component';
import { CalendarModule} from 'primeng/calendar';
import { DropdownModule } from 'primeng/dropdown';
import { TicketAttributesComponent } from './AnalystSelfService/ticket-attributes/ticket-attributes.component';
import { AddnondeliveryComponent } from './AnalystSelfService/addnondelivery/addnondelivery.component';
import { ErroredTicketsComponent } from './AnalystSelfService/errored-tickets/errored-tickets.component';
import { InfoiconComponent} from './AnalystSelfService/infoicon/infoicon.component';
import { AutoCompleteModule } from 'primeng/autocomplete';
import {MultiSelectModule} from 'primeng/multiselect';
import { NgMarqueeModule } from 'ng-marquee';
import { ModalModule } from 'ngx-bootstrap/modal';
import {ToastModule} from 'primeng/toast';
import{RadioButtonModule} from 'primeng/radiobutton';
import{CheckboxModule}from 'primeng/checkbox';
import { SearchticketComponent } from './searchticket/searchticket.component';
import { DisableRightClickDirective } from './common/directives/disable-right-click.directive';
import {TooltipModule} from 'primeng/tooltip';
import { ClientErrorHandler } from './common/global-error-handler/client-error-handler';
import { ServerErrorHandler } from './common/global-error-handler/server-error-handler';
import {CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA, Component, ViewChild, OnInit, AfterViewInit} from '@angular/core';
// KeyCloak Integarion Modules
import { OAuthModule } from 'angular-oauth2-oidc';
import { NgIdleKeepaliveModule } from '@ng-idle/keepalive';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { CallbackComponent } from './common/KeyCloakConfigurationFiles/callback/callback.component';
import { MsalAuthFilesModule } from '@library/msalauthfiles';
//KeyClaok Integraion Modules End
@NgModule({
  declarations: [
    AppComponent,
    routingComponents,
    FooterComponent,
    DynamicgridComponent,
    TimesheetentryComponent,
    EffortgraphComponent,     
    TicketAttributesComponent,
    AddworkitemComponent,
    AddnondeliveryComponent,
    ErroredTicketsComponent,
    AddTicketComponent,
    ChooseWorkitemTicketComponent,
    InfoiconComponent,
    SearchticketComponent,
    DisableRightClickDirective,
    CallbackComponent
  ],
  imports: [
    MultiSelectModule,
    CommonModule,
    BrowserModule,
    AppRoutingModule,
    TableModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    BsDatepickerModule.forRoot(),
    DatepickerModule,
    ButtonModule,
    DropdownModule,
    ToastModule,
    AccordionModule,
    DialogModule,
    RadioButtonModule,
    CheckboxModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: httpTranslateLoader,
        deps: [HttpClient]
      }
    }),
    LeadselfServiceModule,    
    CalendarModule,
    AutoCompleteModule,
    NgMarqueeModule,
    TooltipModule,
    MainspringMetricsModule,
    ModalModule.forRoot(),
    ReactiveFormsModule.withConfig({warnOnNgModelWithFormControl: 'never'}),
    //KeyCloak Modules
    OAuthModule.forRoot(),
    NgIdleKeepaliveModule.forRoot(),
    OverlayPanelModule,
    MsalAuthFilesModule.forRoot({ configpath: './assets/config/config.json' })
  ],
  providers: [AppConfig, AppSettingsConfig,
    {
    provide: APP_INITIALIZER,
    useFactory: initializeApp,
    deps: [AppSettingsConfig],
    multi: true
  },
  {
    provide: HTTP_INTERCEPTORS,
    useClass: HttpServiceInterceptor,
    multi: true
  },
   { provide: ErrorHandler, useClass: ClientErrorHandler },
     { provide: HTTP_INTERCEPTORS, useClass: ServerErrorHandler, multi: true }
],
  bootstrap: [AppComponent],schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA]
})
export class AppModule { }
export function httpTranslateLoader(http: HttpClient): TranslateHttpLoader {
  return new TranslateHttpLoader(http, "./assets/i18n/", ".json");
}
export function initializeApp(appSettingsConfig: AppSettingsConfig): any {
  return () => appSettingsConfig.load();
}
