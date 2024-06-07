// Copyright (c) Applens. All Rights Reserved.
import { Component, OnInit } from '@angular/core';
import { MainspringMetricsService } from './../Service/mainspring-metrics.service';
import { HeaderService } from 'src/app/Layout/services/header.service';
import { MasterDataModel } from '../../Layout/models/header.models';
import { SpinnerService } from '../../common/services/spinner.service';
import { BasemeasuresComponent} from '../basemeasures/basemeasures.component';

@Component({
  selector: 'app-basemeasuresearch',
  templateUrl: './basemeasuresearch.component.html',
  styleUrls: ['./basemeasuresearch.component.css']
})
export class BasemeasuresearchComponent implements OnInit {
  //odcRadiobtn: boolean = false;
  ddlprojectdropdown: boolean = false;
  //topfiltersMps: boolean = false;
  //topfiltersMpsTicketSummary: boolean = false;
  //projects: any[];
  //selectedProjects: any;
  //ddlfrequency: any[];
  //selectedfrequency: any;
  //ddlReportingPeriod: any[];
  //selectedReportingPeriod: any;
  //ddlservice: any[];
  //selectedddlservice: any;
  //ddlmetrics: any[];
  //selectedddlmetrics: any;
  public userId: string;
  public customerId: number;

  constructor(private basemeasureservice: MainspringMetricsService,
    private headerService: HeaderService, private spinner: SpinnerService,
    public basemeasurecomp: BasemeasuresComponent) {
  }

  ngOnInit(): void {
    
    this.headerService.masterDataEmitter.subscribe((masterData: MasterDataModel) => {
      if (masterData != null) {
        this.basemeasurecomp.selectedProjects
        this.spinner.show();
        this.ddlprojectdropdown = false;
        this.userId = masterData.hiddenFields.employeeId;
        this.customerId = masterData.selectedCustomer.customerId;
        var proje = this.basemeasurecomp.projects;
               
        this.spinner.hide();
      }
    });
  }

}
