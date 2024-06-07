// Copyright (c) Applens. All Rights Reserved.
import { Component, OnInit } from '@angular/core';
import { Chart } from 'chart.js';
import { param } from 'jquery';
import { EffortgraphService } from 'src/app/AnalystSelfService/Service/effortgraph.service';
import { HeaderService } from 'src/app/Layout/services/header.service';
import { MasterDataModel, LanguageModel } from 'src/app/Layout/models/header.models';
import { DatePipe } from '@angular/common';
import { TranslateService } from '@ngx-translate/core';
import { DynamicgridService } from 'src/app/AnalystSelfService/Service/dynamicgrid.service';

@Component({
  selector: 'app-effortgraph',
  templateUrl: './effortgraph.component.html',
  styleUrls: ['./effortgraph.component.scss'],
  providers: [DatePipe]
})
export class EffortgraphComponent implements OnInit {
  Days = [];
  color = [];
  mandatoryHours: any;
  maxcount: any;
  DaysSplit = [];
  partialeffort = [];
  bkppartialeffort = [];
  TicketedEffort = [];
  NonTicketedEffort = [];
  public lastsat: Date;
  public firstsat: Date;
  public date: Date;
  public date1: Date;
  CustomerId: string;
  EmployeeId: string;
  public isShow:boolean = true;
  public show: boolean = false;
  public Assigndays: boolean = false;
  TicketEffort : string;
  NonTicketEffort :string;
  public tooldata: any;
  showgraph: boolean = true;
  constructor( private effortGraphService: EffortgraphService,private  readonly datePipe: DatePipe,
    private headerService : HeaderService ,
    private translate: TranslateService,
    private dynamicgridservice: DynamicgridService) { }
  ngOnInit(): void {
    this.headerService.masterDataEmitter.subscribe((masterData: MasterDataModel) => {
      if (masterData != null) {
          this.closegraph();
          this.CustomerId = masterData.hiddenFields.customerId;
          this.EmployeeId = masterData.hiddenFields.employeeId;
          this.showgraph = masterData.hiddenFields.isEffortConfigured == 0 ? false : true;
        }
    });
    this.headerService.languageDataEmitter.subscribe((Lang: LanguageModel) => {
      if (Lang != null) {
        this.closegraph();
      }
    });
    this.dynamicgridservice.DatechangeEmitter.subscribe((date: Date) => {
      if (date != null) {
        this.closegraph();
      }
    });
}
toggle() {
    this.show = !this.show;
    if(this.show)  {
      this.GetEffortChartDetails();
      this.isShow = false;
    }
    else{
        this.isShow = true;
    }
  }
  closegraph(){
    if (this.show) {
      this.isShow = true;
      this.show = false;
    }

  }

  setTicketEffort(pattern: string): void { 
    this.translate.get(pattern)
          .subscribe(message => {           
            this.TicketEffort = message;          
    });    
  }
  setNonTicketEffort(pattern: string): void { 
    this.translate.get(pattern)
          .subscribe(message => {           
            this.NonTicketEffort = message;          
    });    
  }
GetEffortChartDetails(){
    let fdate = new Date(localStorage.getItem("Firstdayofweek"));
    const month = fdate.getMonth()+1;
    const year = fdate.getFullYear();
    let date = new Date();
    let firstDay = new Date(fdate.getFullYear(), fdate.getMonth(), 1);
    let lastDay = new Date(fdate.getFullYear(), fdate.getMonth() + 1, 0);
    let firstsaturday = this.getNextSaturday(firstDay);
    let lastsaturday = this.getNextSaturday(lastDay);
    this.lastsat = lastsaturday;
    this.firstsat = firstsaturday;
    let firstWeek = []; 
    let lastWeek = [];
   

    firstWeek.push(this.datePipe.transform(new Date(this.firstsat),'MM/dd/yyyy'))
    lastWeek.push(this.datePipe.transform(new Date(this.lastsat),'MM/dd/yyyy'))
    for(let i = 1; i <= 6; i++){
        this.date = new Date(firstsaturday);
        this.date.setDate(firstsaturday.getDate() - i);
        firstWeek.push(this.datePipe.transform(this.date,'MM/dd/yyyy'));
    }
    for(let i = 1; i <= 6; i++){
        this.date1 = new Date(lastsaturday);
        this.date1.setDate(lastsaturday.getDate() - i);
        lastWeek.push(this.datePipe.transform(this.date1,'MM/dd/yyyy'));
    }
    let fweek = firstWeek[6]+','+firstWeek[5]+','+firstWeek[4]+','+firstWeek[3]+','+firstWeek[2]+','+firstWeek[1]+','+firstWeek[0];
    let lweek = lastWeek[6]+','+lastWeek[5]+','+lastWeek[4]+','+lastWeek[3]+','+lastWeek[2]+','+lastWeek[1]+','+lastWeek[0];
    let Params= {
    EmployeeId: this.EmployeeId,
    FirstWeek: fweek,
    LastWeek: lweek,
    CustomerId: this.CustomerId,
    Month: month,
    Year: year
  }  

this.effortGraphService.GetEffortChartDetails(Params).subscribe(x => {  
     this.Days = x.debtMonthTrend.days;
    this.partialeffort = x.debtMonthTrend.partialEffort;
    if(x.totalMandatoryHours<=0){
      this.mandatoryHours = 9.00;
    }
    else{
      this.mandatoryHours = x.totalMandatoryHours;
    }
  if (this.mandatoryHours > 0) {
    let roundoffvalue = Math.round(Math.max(...x.debtMonthTrend.partialEffort) / this.mandatoryHours);
    this.maxcount = roundoffvalue != 0 ? (roundoffvalue < (Math.max(...x.debtMonthTrend.partialEffort) / 
    this.mandatoryHours) ? this.mandatoryHours * ((Math.round(Math.max(...x.debtMonthTrend.partialEffort) / this.mandatoryHours)) + 1)
      : this.mandatoryHours * Math.round(Math.max(...x.debtMonthTrend.partialEffort) / this.mandatoryHours)) : this.mandatoryHours
  }
  else {
    this.maxcount = Math.round(Math.max(...x.debtMonthTrend.partialEffort))
  }
let myBar = null;
for (let j = 0; j < this.partialeffort.length; j++) {
  if (this.partialeffort[j] == 0){
    this.partialeffort[j] = 0.199;
  }
  else{
    this.partialeffort[j] = this.partialeffort[j];
  }
}
for(let i = 0; i < this.partialeffort.length; i++){
if(this.partialeffort[i] >=  this.mandatoryHours){
    this.color[i]="#0f5acf";
}
else if(this.partialeffort[i] == 0.199){
     this.color[i]="rgb(204, 102, 119)";
}
else if(this.partialeffort[i] == -1){
     this.color[i]="white";
}
else{
    this.color[i]="rgb(221, 204, 119)";
}
}
this.setTicketEffort('TicketEffort');
this.setNonTicketEffort('NonTicketEffort');

 this.tooldata = {
  tickmesg : this.TicketEffort,
  nontickmesg : this.NonTicketEffort
}
Chart.defaults.global.datasets.bar.categoryPercentage = 0.6;
Chart.defaults.global.datasets.bar.barPercentage = 1;
const barChartData = {
     labels: this.Days,
     datasets: [
         {
             label: 'Effort',
             backgroundColor: this.color,
             data: this.partialeffort,
             xAxisID: 'xAxis1',
         },       
        {
          tooldata:this.tooldata,
        }
         
     ]
};

myBar = new Chart ('myChart',{
     type: 'bar',
     data: barChartData,
     options: {
         animationEnabled: true,
         responsive: true,
         maintainAspectRatio: false,

       title: {
         display: false,
         text: this.Days,
        },

         legend: {
             display: false,
             position: 'bottom',
             labels: {
                 display: true,
                 padding: 25,
                 boxWidth: 10,
                 fontColor: 'black',
                 fontSize: 10,
                 fontFamily: 'Segoe UI, Segoe UI Web Regular, Segoe UI Symbol,Helvetica Neue, BBAlpha Sans, S60 Sans, Arial, sans-seri'
             }
         },
         tooltips: {
             callbacks: {
             label: function (tooltipItem: any, Days: any): string {
                 if (!this.Assigndays) {
                   this.Days = Days.labels.map(x => x);
                   this.Assigndays = true;
                 }                 
                 const label = this.Days [tooltipItem.index].split("/")[0];
                 const ticketedeffortValue = tooltipItem.yLabel - parseFloat(this.Days[tooltipItem.index].split("/")[2]);
                 const NonTicktedEffortValue = parseFloat(this.Days[tooltipItem.index].split("/")[2]);
                  let tooltipMsg = barChartData.datasets[1].tooldata.tickmesg + ticketedeffortValue + barChartData.datasets[1].tooldata.nontickmesg + NonTicktedEffortValue;
                  const value = ticketedeffortValue + NonTicktedEffortValue;
                  if (value === 0.199){
                      tooltipMsg = barChartData.datasets[1].tooldata.tickmesg + 0 + barChartData.datasets[1].tooldata.nontickmesg + 0;
                  }
                  for (let i = 0; i < Days.labels.length; i++) {
                    Days.labels[i] = Days.labels[i].split("/")[0]
                  }
                 return tooltipMsg;
                 }
             }
         },
         scales: {
             xAxes: [
                   {
                       display: true,
                       id: 'xAxis1',
                       type: 'category',                     

                       ticks: {
                           background: 'Lightgrey',
                           fontColor: 'black',
                           fontSize: 9,
                           fontFamily: 'Proxima Nova, Regular',
                           autoSkip: false,
                           representZero: false,
                           callback: function (label) {
                               const day = label.split('/')[0];
                               const dayname = label.split('/')[1];
                               return day;
                           }
                       },
                       gridLines: {
                           zeroLineColor: '#fff',
                           borderDash: [2, 4],
                           color: 'rgba(255, 255, 255, 0.20)'
                       }
                   },
                 {
                     display: true,
                     id: 'xAxis2',
                     barPercentage: 1,
                   categoryPercentage: 0.6,

                   ticks: {
                         backgroundColor: 'Lightgrey',
                         fontColor: '#1d6da2',                         
                         fontSize: 10,
                         fontFamily: 'Proxima Nova, Regular',
                         autoSkip: false,
                         callback: function (label) {
                                 const day = label.split('/')[0];
                                 const dayname = label.split('/')[1];
                                 return day === '' || dayname ==="0" ? '' : dayname;
                             }
                     },
                     gridLines: {
                         zeroLineColor: '#fff',
                         borderDash: [2, 4],
                         color: 'rgba(255, 255, 255, 0.20)'
                     }
                 },
             ],
             yAxes: [
                 {
                     type: 'linear',
                     stacked: true,
                     display: true,
                     ticks: {
                         fontColor: 'black',
                         fontSize: 10,
                         fontFamily: 'Segoe UI, Segoe UI Web Regular, Segoe UI Symbol,Helvetica Neue, BBAlpha Sans, S60 Sans, Arial, sans-serif',
                         beginAtZero: true,
                       min: 0,
                       max: this.maxcount,
                         stepSize: this.mandatoryHours           
                     },
                     scaleLabel: {
                         display: true,
                         labelString: 'Hours',
                         fontColor: '#67757c',
                         fontSize: 12,
                         fontFamily: 'Segoe UI, Segoe UI Web Regular, Segoe UI Symbol,Helvetica Neue, BBAlpha Sans, S60 Sans, Arial, sans-serif'
                     },
                     gridLines: {
                         display: true,
                         borderDash: [2, 4],
                         color: 'rgba(124, 63, 63, 0.20)'
                     }
                 }]
         }
     }
 });
});
}
  getNextSaturday(d){
    let t = new Date(d);
      t.setDate(t.getDate() + (6 + 7 - t.getDay()) % 7);
      return t;
  }
}
