// Copyright (c) Applens. All Rights Reserved.
import { Component, ElementRef, HostListener, Input, OnChanges, OnInit, Renderer2, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BsDatepickerDirective, BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { Table } from 'primeng/table';
import { Constants } from 'src/app/common/constants/constants';
import { SpinnerService } from 'src/app/common/services/spinner.service';
import { BasemeasuresComponent } from '../basemeasures/basemeasures.component';
import { MainspringMetricsService } from '../Service/mainspring-metrics.service';

@Component({
  selector: 'app-ticket-summary-odc',
  templateUrl: './ticket-summary-odc.component.html',
  styleUrls: ['./ticket-summary-odc.component.css']
})
export class TicketSummaryOdcComponent implements OnInit, OnChanges {

  @Input('basemeasures') basemeasures = [];

  isError = false;
  isSuccess = false;
  toggleFilter = true;
  errorMessage = '';
  successMessage = '';

  searchServiceName: any;
  searchBaseMeasureName: any;
  searchBaseMeasureValue: any;
  searchUOMDESC: any;
  searchMainspringPriorityName: any;
  searchSupportCategory: any;
  searchTechnology: any;
  

  heightValue = 38;
  scrollHeight = this.heightValue + 'vh';
  toggleicon = Constants.toggleIconOn;

  public datePickerConfig: Partial<BsDatepickerModule>;
  minCompletionDate: Date = new Date();

  @ViewChild('dtGrid') dtGrid: Table;
  @ViewChild(BsDatepickerDirective, { static: false }) datepicker: BsDatepickerDirective;

  constructor(private basemeasureservice: MainspringMetricsService,
    public basemeasurecomp: BasemeasuresComponent,
    private el: ElementRef,
    private renderer: Renderer2,
    private translate: TranslateService,
    private spinner: SpinnerService) { }

  ngOnInit(): void {

    this.datePickerConfig = Object.assign({},
      {
        showWeekNumbers: false,
        dateInputFormat: Constants.dateInputFormat,
        todayHighlight: true
      });
    this.basemeasures = JSON.parse(JSON.stringify(this.basemeasurecomp.basemeasureTicketSummaryOdc));    
  }

  ngOnChanges() {
    if (this.basemeasures) {
      this.clearFilters();
    }
  }

  @HostListener('window:scroll')
  onScrollEvent() {
    if (this.datepicker) {
      this.datepicker.hide();
      }   
  }
    
  SaveTimesheetMeasure(): void {
    this.spinner.show();
    const modelList = [];
    this.basemeasures.forEach(x => 
      modelList.push({
        ServiceID: x.serviceId,
        TicketSummaryBaseMeasureID: x.ticketSummaryBaseId,
        MainspringPriorityID: x.priorityId,
        MainspringSUPPORTCATEGORYID: x.supportCategory,       
        TicketBaseMeasureValue: x.ticketSummaryValue,
    }));
    const params = {
      ProjectID: Number(this.basemeasurecomp.selectedProjects), 
      FrequencyID: Number(this.basemeasurecomp.selectedfrequency), 
      ServiceIDs: this.basemeasurecomp.selectedddlservice.join(','), 
      MetricsIDs: this.basemeasurecomp.selectedddlmetrics.join(','), 
      ReportFrequencyID: Number(this.basemeasurecomp.selectedReportingPeriod), 
      lstTicketSummaryBaseODC: modelList, 
      UserId: this.basemeasurecomp.userId
    };
    this.basemeasureservice.SaveTicketSummaryBaseMeasureODC(params)
    .subscribe(result => {
      this.spinner.hide();
      if (result == "Success") {
        this.setSuccessMessage('SuccessfullySaved');  
        this.basemeasurecomp.LoadTicketSummaryODC(true);      
      } else {
        this.setErrorMessage('Someerroroccured');
      }
    });    
  }

  dateValueUpdate(event, basemeasure): void {    
    basemeasure.baseMeasureValue = this.basemeasurecomp.getDate(event);
  }

  toggle(event): void {
    if (this.toggleFilter) {
      this.toggleFilter = false;
      this.toggleicon = Constants.toggleiconOff;
      this.setBodyheight(true);
    } else {
      this.toggleFilter = true;
      this.toggleicon = Constants.toggleIconOn;
      this.setBodyheight(false);
    }
  }

  setBodyheight(toggle: boolean): void {
    const tableBody: HTMLElement = (this.el.nativeElement as HTMLElement)
      .querySelector('.p-datatable-scrollable-body table tbody');
    if (tableBody) {
      if (toggle) {
        this.heightValue = (this.heightValue + 7);
        this.renderer.setStyle(tableBody, 'height', this.heightValue + 'vh');
        this.scrollHeight = this.heightValue + 'vh';
      } else {
        this.heightValue = (this.heightValue - 7);
        this.renderer.setStyle(tableBody, 'height', this.heightValue + 'vh');
        this.scrollHeight = this.heightValue + 'vh';
      }
    }
  }

  clearFilter(modelName: string, fieldName: string, searchOptions: string): void {
    this[modelName] = null;
    this.dtGrid.filter(null, fieldName, searchOptions);
  }
  clearAllFilters(): void {
    this.clearGrid();    
    this.basemeasurecomp.LoadTicketSummaryODC(false);
    this.searchServiceName = null;
    this.searchBaseMeasureName = null;
    this.searchBaseMeasureValue = null;   
    this.searchMainspringPriorityName = null;
    this.searchSupportCategory = null;   
  }

  clearFilters(): void {
    this.clearGrid();        
    this.searchServiceName = null;
    this.searchBaseMeasureName = null;
    this.searchBaseMeasureValue = null;   
    this.searchMainspringPriorityName = null;
    this.searchSupportCategory = null;   
  }

  clearGrid(): void {
    if (this.dtGrid) {
      this.dtGrid.clear();
      this.dtGrid.clearCache();
    }
  }

  ValidateDecimalKeyPress(event: any): void {
    const input = event.target;
    if (event.shiftKey == true) {
      event.preventDefault();
    }
    if ((event.keyCode >= 48 && event.keyCode <= 57) || 
    (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 8 || 
    event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || 
    event.keyCode == 46 || event.keyCode == 190 || event.keyCode == 110) {
      //CCAP Fix

    } else {
      event.preventDefault();
    }
    if (input) {
      if (input.value.indexOf('.') !== -1 && (event.keyCode == 190 || event.keyCode == 110))
        event.preventDefault();
    }
  }
  ValidateDecimalBlur(event: any): void {
    const input = event.target;
    if (input) {
      if (input.value == "" || input.value == 0.0) {
        //CCAP Fix

      }
      if (event.shiftKey == true) {
        event.preventDefault();
      }

      if ((event.keyCode >= 48 && event.keyCode <= 57) || 
      (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 8 || 
      event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || 
      event.keyCode == 46 || event.keyCode == 190 || event.keyCode == 110) {
        //CCAP Fix

      } else {
        event.preventDefault();
      }

      if (input.value.indexOf('.') !== -1 && (event.keyCode == 190 || event.keyCode == 110))
        event.preventDefault();
    }
  }

  ValidateRatingKeyPress(event: any): void {
    const input = event.target;
    if (input) {
      if (event.shiftKey == true) {
        event.preventDefault();
      }

      if ((event.keyCode >= 49 && event.keyCode <= 53) || 
      (event.keyCode >= 97 && event.keyCode <= 101) || 
      event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || 
      event.keyCode == 39 || event.keyCode == 46) {
        //CCAP Fix

      } else {
        event.preventDefault();
      }

      if (input.value.indexOf('.') !== -1 && (event.keyCode == 190 || event.keyCode == 110)) {
        event.preventDefault();
      }
    }
  }

  ValidateRatingBlur(event: any): void {
    const input = event.target;
    if (input) {
      if (input.value == "" || input.value == 0.0) {
        //CCAP Fix
      }
      if (event.shiftKey == true) {
        event.preventDefault();
      }

      if ((event.keyCode >= 49 && event.keyCode <= 53) || 
      (event.keyCode >= 97 && event.keyCode <= 101) || 
      event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || 
      event.keyCode == 39 || event.keyCode == 46) {
        //CCAP Fix

      } else {
        event.preventDefault();
      }

      if (input.value.indexOf('.') !== -1 && (event.keyCode == 190 || event.keyCode == 110)) {
        event.preventDefault();
      }
    }
  }

  ValidateNumberKeyPress(event: any): void {
    const input = event.target;
    if (input) {
      if (event.shiftKey == true) {
        event.preventDefault();
      }

      if ((event.keyCode >= 48 && event.keyCode <= 57) || 
      (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 8 || 
      event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 46) {
        //CCAP Fix

      } else {
        event.preventDefault();
      }

      if (input.value.indexOf('.') !== -1 && (event.keyCode == 190 || event.keyCode == 110)) {
        event.preventDefault();
      }
    }
  }

  ValidateNumberBlur(event: any): void {
    const input = event.target;
    if (input) {
      if (input.value == "" || input.value == 0.0) {
        //CCAP Fix
      }
      if (event.shiftKey == true) {
        event.preventDefault();
      }

      if ((event.keyCode >= 48 && event.keyCode <= 57) || 
      (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 8 ||
       event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 46) {
         //CCAP Fix

      } else {
        event.preventDefault();
      }

      if (input.value.indexOf('.') !== -1 && (event.keyCode == 190 || event.keyCode == 110))
        event.preventDefault();
    }
  }

  ValidateDateKeyPress(event: any): void {
    if ((event.keyCode !== 8)) {
      event.preventDefault();
    }
  }

  setSuccessMessage(pattern: string): void {
    this.translate.get(pattern)
      .subscribe(message => {
        this.isSuccess = true;
        this.successMessage = message;
        setTimeout(() => {
          this.isSuccess = false;
          this.successMessage = '';
        }, Constants.lifeSpan);
      });
  }
  setErrorMessage(pattern): void {
    this.translate.get(pattern)
      .subscribe(message => {
        this.isError = true;
        this.errorMessage = message;
        setTimeout(() => {
          this.isError = false;
          this.errorMessage = '';
        }, Constants.lifeSpan);
      });
  }
}
