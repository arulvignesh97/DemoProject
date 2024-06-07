// Copyright (c) Applens. All Rights Reserved.
import { Component, ElementRef, HostListener, Input, OnChanges, OnInit, QueryList, Renderer2, ViewChild, ViewChildren } from '@angular/core';
import { MainspringMetricsService } from '../Service/mainspring-metrics.service';
import { BasemeasuresComponent} from '../basemeasures/basemeasures.component';
import { Constants } from 'src/app/common/constants/constants';
import { BsDatepickerDirective, BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { Table } from 'primeng/table';
import { TranslateService } from '@ngx-translate/core';
import { SpinnerService } from 'src/app/common/services/spinner.service';
import { DatePipe } from '@angular/common';
import { DatepickerDateCustomClasses } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-basemeasure-userdefined',
  templateUrl: './basemeasure-userdefined.component.html',
  styleUrls: ['./basemeasure-userdefined.component.css'],
  providers: [DatePipe]
})
export class BasemeasureUserdefinedComponent implements OnInit, OnChanges {

  @Input('basemeasures') basemeasures = [];
  baseMeasureDataSource = [];

  isError = false;
  isSuccess = false;
  toggleFilter = true;
  errorMessage = '';
  successMessage = '';

  searchServiceName: any;
  searchBaseMeasureName: any;
  searchBaseMeasureValue: any;
  searchUOMDESC: any;
  coun : any;

  heightValue = 40;
  scrollHeight = this.heightValue + 'vh';
  toggleicon = Constants.toggleIconOn;

  public datePickerConfig: Partial<BsDatepickerModule>;
  minCompletionDate: Date = new Date();
  dateCustomClasses: DatepickerDateCustomClasses[];
  
  @ViewChild('dtGrid') dtGrid: Table;
  @ViewChildren('baseMeasuredp') datepicker: QueryList<BsDatepickerDirective>;

  constructor(private basemeasureservice: MainspringMetricsService,
    public basemeasurecomp: BasemeasuresComponent,
    private el: ElementRef,
    private renderer: Renderer2,
    private translate: TranslateService,
    private spinner: SpinnerService,
    private datePipe: DatePipe) { }

  ngOnInit(): void {
    this.datePickerConfig = Object.assign({},
      {
        showWeekNumbers: false,
        dateInputFormat: Constants.dateInputFormat,
        todayHighlight: true         
      });
      this.dateCustomClasses = [
        { date: new Date(), classes: ['backgblue'] }
      ];
      this.basemeasures = JSON.parse(JSON.stringify(this.basemeasurecomp.basemeasuresUserDefinedModel));
      this.basemeasures.map( x => {
        if (x.uomDataType === 'Date') {
          x.baseMeasureValue = this.getDate(x.baseMeasureValue);
        }
      });            
  }
  ngOnChanges() {
    if (this.basemeasures) {
      this.clearFilters();
    }
  }

  @HostListener('window:scroll')
  onScrollEvent() {
    if (this.datepicker) {
    //CCAP Fix
    }    
  }
  
  SaveUserDefinedBaseMeasure(): void {
    this.spinner.show();
    const modelList = [];
    this.coun =[];
    this.coun= this.basemeasures.filter(x => x.baseMeasureValue != "")
    this.coun.forEach(x => 
      modelList.push({
        BaseMeasureId:x.baseMeasureId,
        ServiceId:x.serviceId, 
        BaseMeasureValue: x.baseMeasureValue,
    }));
    const params = {
      ProjectID: Number(this.basemeasurecomp.selectedProjects), 
      UserId: this.basemeasurecomp.userId,
      lstBaseMeasures: modelList
    };
    this.basemeasureservice.SaveBaseMeasureData(params)
    .subscribe(result => {
      this.spinner.hide();
      if (result == "Success") {
        this.setSuccessMessage('SuccessfullySaved');   
        this.basemeasurecomp.GetBaseMeasureProjectwiseSearchUserDefined(true);     
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
    const tableBody: HTMLElement = ( this.el.nativeElement as HTMLElement)
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
    this.basemeasurecomp.GetBaseMeasureProjectwiseSearchUserDefined(false);
    this.searchServiceName = null;
    this.searchBaseMeasureName = null;
    this.searchBaseMeasureValue = null;
    this.searchUOMDESC = null;    
  }
  clearFilters(): void {
    this.clearGrid();       
    this.searchServiceName = null;
    this.searchBaseMeasureName = null;
    this.searchBaseMeasureValue = null;
    this.searchUOMDESC = null;    
  }

  clearGrid(): void {
    if (this.dtGrid) {
      this.dtGrid.clear();
      this.dtGrid.clearCache();
    }  
  }

  getDate(date: Date) {
    if (date) {
    return this.datePipe.transform(new Date(date), Constants.dateFormat);
    }
  }
  getFormat(): string {
    return Constants.dateFormat;
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
    (event.keyCode >= 97 && event.keyCode <= 101) || event.keyCode == 8 || 
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
    (event.keyCode >= 97 && event.keyCode <= 101) || event.keyCode == 8 || 
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
    } else {
        let currentPicker = this.datepicker.filter(x => (x.isOpen))[0];
        if(currentPicker) {
          currentPicker.bsValue = null;
          currentPicker.outsideClick = true;
          currentPicker.toggle();
          currentPicker.ngOnDestroy();
          currentPicker.ngOnInit();     
        }
    }
  }

  setSuccessMessage(pattern): void {
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
