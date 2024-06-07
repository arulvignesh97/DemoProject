// Copyright (c) Applens. All Rights Reserved.
import { MasterDataModel } from './../../Layout/models/header.models';
import { HeaderService } from './../../Layout/services/header.service';
import { BsDatepickerDirective, BsDatepickerModule, DatepickerModule } from 'ngx-bootstrap/datepicker';
import { AddReasonCompDate } from './../Models/add-reason-compdate';
import { ErrorLog } from './../Models/error-log';
import { ApplicationDetailSave } from './../Models/application-detail-save';
import { DataDictionaryGrid } from './../Models/data-dictionary-grid';
import { ResolutionCode } from './../Models/resolution-code';
import { ResidualDebt } from './../Models/residual-debt';
import { DebtClassification } from './../Models/debt-classification';
import { CauseCode } from './../Models/cause-code';
import { AvoidableFlag } from './../Models/avoidable-flag';
import { ApplicationDetail } from './../Models/application-detail';
import { DataDictionaryService } from './../Service/data-dictionary.service';
import { SpinnerService } from './../../common/services/spinner.service';
import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { Table } from 'primeng/table';
import { Constants } from 'src/app/common/constants/constants';
import { FileUpload } from 'primeng/fileupload';
import { TranslateService } from '@ngx-translate/core';
import { DatepickerDateCustomClasses } from 'ngx-bootstrap/datepicker';
import { LayoutService } from 'src/app/common/services/layout.service';
import { AppSettingsConfig } from 'src/app/app.settings.config';

@Component({
  selector: 'app-data-dictionary',
  templateUrl: './data-dictionary.component.html',
  styleUrls: ['./data-dictionary.component.css'],
  providers: [DatePipe]
})
export class DataDictionaryComponent implements OnInit {

  public addpatternFG: FormGroup = new FormGroup({});
  @ViewChild('dtGrid') dtGrid: Table;
  @ViewChild('errorLogGrid') errorLogGrid: Table;
  @ViewChild('upload') excelupload: FileUpload;

  addPatternDialog: boolean;
  optionalPatternDialog: boolean;
  errorLogDialog: boolean;
  dictionaryloading = false;
  errorLogloading = false;
  disabledSignOff = true;
  params = {};
  dataDictionaryEnabled = false;
  dataDisable = false;
  searchDisabled = false;

  selectedDictionaries = [];
  dictionaryDataSource: any[] = [];
  datadictionary: any;
  signOff: any;
  todayDate: string;
  optionalReasonResidual: any;
  optionalCompletionDate: any;

  searchCreatedBy: any;
  searchApplicationName: any;
  searchCauseCode: any;
  searchResolutionCode: any;
  searchDebtCategory: any;
  searchAvoidableFlag: any;
  searchResidualDebt: any;

  errorApplicationName: any;
  errorCauseCode: any;
  errorResolutionCode: any;
  errorDebtCategory: any;
  errorAvoidableFlag: any;
  errorResidualDebt: any;
  errorReasonForResidual: any;
  errorExpectedCompletionDate: any;
  errorRemarks: any;

  projects = [];
  selectedProject: any;
  portfolios = [];
  selectedPortfolios = [];
  applications = [];
  selectedApplications = [];

  toggleFilter = true;
  toggleErrorFilter = true;
  toggleicon = Constants.toggleIconOn;
  toggleErroricon = Constants.toggleIconOn;
  debtDefault = true;
  virtualItemSize = 34;
  heightValue = 38;
  scrollheight = this.heightValue + 'vh';
  saveClicked = false;
  searchClicked = false;
  reasonResidualHide = false;
  completionDateHide = false;
  optionalReasonResidualHide = false;
  optionalCompletionDateHide = false;
  conflictPattern = false;
  dataLoaded = false;
  rowsPerPage = Constants.rowsPerPage;
  excelName:string;
  isError = false;
  isSuccess = false;
  isPatternError = false;
  isLengthError = true;
  errorMessage = ''
  successMessage = '';
  errorPatternMessage = '';

  applicationNames = [];
  applicationDetails = [];
  avoidableFlags: AvoidableFlag[] = [];
  causeCodes: CauseCode[] = [];
  debtClassifications: DebtClassification[] = [];
  residualDebts: ResidualDebt[] = [];
  resolutionsCodes: ResolutionCode[] = [];
  reasonForResidual = [];
  dictionaries: DataDictionaryGrid[] = [];
  errorLogs: ErrorLog[] = [];

  public datePickerConfig: Partial<BsDatepickerModule>;
  minCompletionDate: Date = new Date();  
  @ViewChild('signoffDatePick', { static: false }) signOffDatepicker: BsDatepickerDirective;
  dateCustomClasses: DatepickerDateCustomClasses[];

  public hiddendata: any;
  projectId: any;
  employeeId: any;
  customerId: any;
  esaProjectId: any;
  displayExemptedMsg=false;
  exemptedMsg:string;

  constructor(private datadictionaryService: DataDictionaryService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private spinner: SpinnerService,
    private fb: FormBuilder,
    private datePipe: DatePipe,
    private el: ElementRef,
    private renderer: Renderer2,
    private headerService: HeaderService,
    private translate: TranslateService, private layoutService:LayoutService) { }

  ngOnInit(): void {
       this.todayDate = this.getDate(new Date().toString());
       this.headerService.masterDataEmitter.subscribe((masterData: MasterDataModel
        ) => {
          if (masterData) {
            this.hiddendata = masterData.hiddenFields;
            if (this.hiddendata) {
              this.employeeId = this.hiddendata.employeeId;
              this.customerId = Number(this.hiddendata.customerId);
              this.searchDisabled = false;             
              this.getProjectDetails();
            }
            else {
              this.searchDisabled = true;
              this.emptyFilterValues();
            }
            this.dictionaries = [];
            this.dictionaryDataSource = [];
            this.clearAllFilters();
        } else {
          this.searchDisabled = true;
          this.emptyFilterValues();
        }
        this.searchClicked = false;
        this.isLengthError = true;
        this.dataLoaded = false;
       });
       this.addpatternFG = this.fb.group({
        applicationName: new FormControl(),
        causeCode: new FormControl(),
        resolutionCode: new FormControl(),
        debtCategory: new FormControl(),
        avaidableFlag: new FormControl(),
        residualFlag: new FormControl(),
        reasonForResidual: new FormControl(),
        completionDate: new FormControl(new Date()),
      });
       this.datePickerConfig = Object.assign({},
        {
          showWeekNumbers: false,
          dateInputFormat: Constants.dateInputFormat,
          todayHighlight: true         
        });
        this.dateCustomClasses = [
          { date: new Date(), classes: ['backgblue'] }
        ];
       this.minCompletionDate.setDate(this.minCompletionDate.getDate() + 1);
       this.getDebtClassifications();
       this.getAvoidableFlags();
       this.getResidualDebt();
       this.spinner.show();
  }

  SearchDictionaries(): void {
  if(this.selectedProject) {
   this.spinner.show();
   this.clearGrid();   
   this.searchClicked = true;
   this.dataLoaded = false;
   this.params = {
      Projectid : this.projectId,
      ApplicationIDs: this.selectedApplications.map(x => ({ID: x.applicationId}))
    };
   this.datadictionaryService.GetGriddetails(this.params)
    .subscribe((data: DataDictionaryGrid[]) => {
      this.dictionaries = data;
      this.dictionaryDataSource = JSON.parse(JSON.stringify(data));
      this.isLengthError = this.dictionaries.length === 0;
      this.dataLoaded = true;
      this.spinner.hide();
      this.clearAllFilters();
    });
    this.getProjectDebtDetails();
  } else {
    this.setErrorMessage('PleaseFillTheHighlightedFields');
  }
  }

  clearGrid(): void {
    if (this.dtGrid) {
      this.dtGrid.clear();
      this.dtGrid.clearCache();
    }
    this.selectedDictionaries = [];
  }

  getProjectDetails(): void {
    this.params = {
      employeeID: window.btoa(this.employeeId),
      customerID: this.customerId
    };
    this.datadictionaryService.GetDropDownValuesProjectPortfolio(this.params)
    .subscribe(data => {
        this.projects = JSON.parse(data).Project;
        if (this.projects) {
          this.selectedProject = this.projects[0];
          this.projectId = this.selectedProject.ProjectId;
          this.params = {
            employeeID: this.employeeId,
            customerID: this.customerId,
            projectID: this.projectId
           };
          this.projectChange(this.selectedProject);
          this.isDictionaryEnabled(this.selectedProject);
          this.getFilterValues(this.params);
          this.getProjectValues();
          this.searchDisabled = false;
        } else {
          this.spinner.hide();
          this.searchDisabled = true;
          this.dataDictionaryEnabled = false;
          this.dataDisable = true;
          this.projects = [];
          this.emptyFilterValues();
        }
    });
  }
  emptyFilterValues(): void {
    this.portfolios = [];
    this.applications = [];
    this.selectedPortfolios = [];
    this.selectedApplications = [];
  }
  getFilterValues(param): void {
    this.spinner.show();
    this.datadictionaryService.GetDropDownValuesDataDictionary(param)
    .subscribe(data => {
      if (param.mode && param.mode === Constants.mode) {
        this.selectedApplications = [];
        this.applications = data.lstApplicationData;
        this.applications.map(x => this.selectedApplications.push(x));
      } else {
        this.selectedApplications = [];
        this.selectedPortfolios = [];
        this.portfolios = data.lstPortfolioData;
        this.portfolios.map(x => this.selectedPortfolios.push(x));
        this.applications = data.lstApplicationData;
        this.applications.map(x => this.selectedApplications.push(x));
      }
      this.spinner.hide();
    });
  }
  getProjectValues(): void {
    this.getProjectDebtDetails();
    this.getResolutioncode();
    this.getCausecode();
    this.getreasonforresidual();
    this.getApplicationdetails();
  }
  getProjectDebtDetails(): void {
    this.params = {
      ProjectID: this.projectId
     };
    this.datadictionaryService.ProjectDebtDetails(this.params)
    .subscribe(data => {
      if (data) {
        this.signOff = this.getDate(data);
        this.disabledSignOff = true;
      } else {
        this.signOff = null;
        this.disabledSignOff = false;
      }
    });
  }

  getResolutioncode(): void {
    this.params = {
      ProjectID: this.projectId
     };
    this.datadictionaryService.GetResolutioncode(this.params)
    .subscribe(data => {
     this.resolutionsCodes = data.map(x => {
      return { label: x.resolutionCode, value: Number(x.resolutionId)};
      });
    });
  }
  getCausecode(): void {
    this.params = {
      ProjectID: this.projectId
     };
    this.datadictionaryService.GetCausecode(this.params)
    .subscribe(data => {
     this.causeCodes = data.map(x => ({label: x.causeCode,
        value: Number(x.causeId) }));
    });
  }
  getreasonforresidual(): void {
    this.params = {
      ProjectID: this.projectId
     };
    this.datadictionaryService.Getreasonforresidual(this.params)
    .subscribe(data => {
     this.reasonForResidual = data.map(x => {
      return { label: x.reasonResidualName, value: Number(x.reasonResidualId)};
      });
    });
  }
  getApplicationdetails(): void {
    this.params = {
      ProjectID: this.projectId
     };
    this.datadictionaryService.GetApplicationdetails(this.projectId)
    .subscribe((data: ApplicationDetail[]) => {
      this.esaProjectId = data[0].esaProjectId;
      const conflictcount = data[0].conflictPatternsCount;
      if (conflictcount > 0) {
        this.conflictPattern = true;
      } else {
        this.conflictPattern = false;
      }
      this.applicationDetails = data.map(x => {
            return { label: x.applicationName, value: x.applicationId};
        });
    });
  }

  getDebtClassifications(): void {
    this.datadictionaryService.GetDebtclassification()
    .subscribe(data => {
      this.debtClassifications = data.map(x => ({label: x.debtClassificationName,
        value: Number(x.debtClassificationId) }));
    });
  }
  getAvoidableFlags(): void {
    this.datadictionaryService.GetAvoidableFlag()
    .subscribe(data => {
      this.avoidableFlags = data.map(x => ({label: x.avoidableFlagName,
        value: Number(x.avoidableFlagId) }));
    });
  }
  getResidualDebt(): void {
    this.datadictionaryService.GetResidualDebt()
    .subscribe(data => {
      this.residualDebts = data.map(x => ({label: x.residualDebtName,
        value: Number(x.residualDebtId) }));
    });
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
  toggleErrorLog(event): void {
    if (this.toggleErrorFilter) {
      this.toggleErrorFilter = false;
      this.toggleErroricon = Constants.toggleiconOff;
    } else {
      this.toggleErrorFilter = true;
      this.toggleErroricon = Constants.toggleIconOn;
    }
  }
  toggleSignoff(ele: any): void {
    if(!this.disabledSignOff) {
      this.signOffDatepicker.toggle();
    }
  }
  setBodyheight(toggle: boolean): void {
    const tableBody: HTMLElement = ( this.el.nativeElement as HTMLElement)
      .querySelector('.p-datatable-scrollable-body table tbody');
    if (tableBody) {
    if (toggle) {
        this.heightValue = (this.heightValue + 7);
        this.renderer.setStyle(tableBody, 'height', this.heightValue + 'vh');
        this.scrollheight = this.heightValue + 'vh';
      } else {
        this.heightValue = (this.heightValue - 7);
        this.renderer.setStyle(tableBody, 'height', this.heightValue + 'vh');
        this.scrollheight = this.heightValue + 'vh';
      }
    }
  }

  onSearchTabOpen(event): void {
    if (event.index === 0 ){
      this.setBodyheight(false);
    }
  }

  onSearchTabClose(event): void {
    if (event.index === 0 ){
      this.setBodyheight(true);
    }
  }
  openNew(): void {
      this.addPatternDialog = true;
      this.addpatternFG.reset();
      this.saveClicked = false;
      this.reasonResidualHide = false;
      this.completionDateHide = false;
  }

  showOptionalPattern(datadictionary: DataDictionaryGrid): void {
    this.params = {
      ProjectID: this.projectId,
      ApplicationID: datadictionary.applicationId,
      RowID: datadictionary.id
    };
    this.datadictionaryService.GetResidualDetail(this.params)
    .subscribe(data => {
      if (data.length > 0) {
      this.optionalReasonResidual = Number(data[0].reasonForResudial);
      this.optionalCompletionDate = data[0].expectedDate;
      this.optionalPatternDialog = true;
      this.optionalReasonResidualHide = false;
      this.optionalCompletionDateHide = false;
      this.datadictionary = datadictionary;
      this.optionalChange(datadictionary);
      }
    });
  }

  showErrorLogDialog(): void {
    this.getErrorLogs();
    this.errorLogDialog = true;
  }
  getErrorLogs(): void {
    this.errorLogloading = true;
    this.errorLogs = [];
    if (this.errorLogGrid) {
    this.errorLogGrid.clear();
    this.errorLogGrid.clearCache();
    }
    this.params = {
      ProjectID : this.projectId,
    };
    this.datadictionaryService.GetDDErrorLogData(this.params)
    .subscribe((data: ErrorLog[]) => {
      this.errorLogs = data;
      this.errorLogloading = false;
    });
  }
  tableCheckChange(event, datadictionary: DataDictionaryGrid): void {
    if (this.selectedDictionaries) {
      const dictionary = this.selectedDictionaries.find(x => x.id === datadictionary.id);
      if (dictionary) {
        datadictionary.defaultFlag = true;
      } else {
        datadictionary.defaultFlag = false;
      }
    }
  }

  dictionaryHeaderChange(event): void {
   this.dictionaries.map(x => {
     x.defaultFlag = event.checked;
   });
  }

  projectChange(event): void {
    this.projectId = this.selectedProject.ProjectId;
      this.displayExemptedMsg=this.hiddendata.lstProjectUserID
      .filter(x=>x.projectId==this.projectId && x.isExempted==true).length>0
      ?true:false
      if(!this.displayExemptedMsg){
      this.onProjectChange(event);
      }
      else{
      this.getProjectValues();
      this.params = {
          employeeID: this.employeeId,
          customerID: this.customerId,
          projectID: this.projectId
         };
      this.getFilterValues(this.params);
      this.exemptedMsg=Constants.exemptedMessage;
      }
  }

  onProjectChange(event): void {
    this.getProjectValues();
    this.params = {
      employeeID: this.employeeId,
      customerID: this.customerId,
      projectID: this.projectId
     };
    this.getFilterValues(this.params);
    this.isDictionaryEnabled(this.selectedProject);
    this.searchClicked = false;
  }

  isDictionaryEnabled(project: any): void {
    if (project.IsDDAutoClassified === 'N') {
      this.dataDictionaryEnabled = false;
      this.dataDisable = true;
    }
    else {
      this.dataDictionaryEnabled = true;
      this.dataDisable = false;
    }
  }
  





  portfolioChange(event): void {
    if (event.value && event.value.length > 0) {
    this.params = {
      employeeID: this.employeeId,
      customerID: this.customerId,
      projectID: this.projectId,
      mode: Constants.mode,
      lstIDs: event.value.map(x => ({PortFolioID: x.portfolioId}))
     };
    this.getFilterValues(this.params);
    } else {
      this.applications = [];
      this.selectedApplications = [];
    }
  }

  getOptionalDisabled(datadictionary: DataDictionaryGrid): boolean {
    const residuladebtName: any = this.residualDebts.find((x: any) => x.value === datadictionary.residualDebtId);
    if (residuladebtName !== null) {
      if (residuladebtName.label.toLowerCase() === 'yes' && datadictionary.defaultFlag) {
        return false;
      }
      else {
        return true;
      }
    }
  }

  getOptionalColors(datadictionary: DataDictionaryGrid): string {
    if ((datadictionary.avoidableFlagId === 3 && datadictionary.residualDebtId === 1)) {
      if ((datadictionary.residualName != null) && (datadictionary.residualName !== ''))
      {
          return Constants.greenBg;
      }
      else {
          return Constants.redBg;
      }
    }
    else if ((datadictionary.avoidableFlagId === 2 && datadictionary.residualDebtId === 1)){
        if ((datadictionary.residualName != null) && (datadictionary.residualName !== '')
        && datadictionary.expectedCompletiondate !== '' && datadictionary.expectedCompletiondate !== undefined) {
            return Constants.greenBg;
        }
        else {
            return Constants.redBg;
        }
    }
    else {
        return Constants.greyBg;
    }
  }

  clearFilter(modelName: string, fieldName: string, searchOptions: string): void {
    this[modelName] = null;
    this.dtGrid.filter(null, fieldName, searchOptions);
  }
  clearErrorLogFilter(modelName: string, fieldName: string, searchOptions: string): void {
    this[modelName] = null;
    this.errorLogGrid.filter(null, fieldName, searchOptions);
  }

  clearAllFilters(): void {
    this.clearGrid();
    this.dictionaries =JSON.parse(JSON.stringify(this.dictionaryDataSource));
    this.dictionaries.forEach(x => x.defaultFlag = false);
    this.searchApplicationName = null;
    this.searchAvoidableFlag = null;
    this.searchCauseCode = null;
    this.searchCreatedBy = null;
    this.searchDebtCategory = null;
    this.searchResidualDebt = null;
    this.searchResolutionCode = null;
  }
  clearAllErrorLogFilters(): void {
    this.errorLogGrid.clear();
    this.errorLogs = [...this.errorLogs];
    this.errorApplicationName = null;
    this.errorCauseCode = null;
    this.errorResolutionCode = null;
    this.errorDebtCategory = null;
    this.errorAvoidableFlag = null;
    this.errorResidualDebt = null;
    this.errorReasonForResidual = null;
    this.errorExpectedCompletionDate = null;
    this.errorRemarks = null;
  }
  AddNewPattern(): void {
    if (this.addpatternFG.valid) {
      this.spinner.show();
      const pattern: ApplicationDetailSave = {
        applicationID: this.addpatternFG.get('applicationName').value,
        avoidableFlagID: this.addpatternFG.get('avaidableFlag').value,
        causeCodeID: this.addpatternFG.get('causeCode').value,
        debtClassificationID: this.addpatternFG.get('debtCategory').value,
        residualDebtID: this.addpatternFG.get('residualFlag').value,
        resolutionCodeID: this.addpatternFG.get('resolutionCode').value,
        expectedCompletionDate: this.getDate(this.addpatternFG.get('completionDate').value),
        reasonForResidualID: this.getNumber(this.addpatternFG.get('reasonForResidual').value),
        customerID: this.customerId,
        employeeID: this.employeeId,
        projectID: this.projectId,
        effectiveDate: this.signOff
      };
      this.datadictionaryService.AddApplicationDetails(pattern)
      .subscribe(result => {
        if (result.indexOf('Added Sucessfully') !== -1) {
          this.searchClicked = true;
          this.isLengthError = false;
          this.setSuccessMessage('PatternAddedSuccessfully');          
          this.addPatternDialog = false;
          setTimeout(() => {
            this.SearchDictionaries();
          }, 3000);          
        } else {
          this.setAddPatternErrorMessage('Pattern already exists');          
        }
        this.spinner.hide();
      });
    } else {
      this.saveClicked = true;
    }
  }

  SaveDataDictionaryByID(): void {
    if (this.selectedDictionaries.length > 0) {
        this.spinner.show();
        const dictionaryDetails = this.selectedDictionaries.map(x =>
            {
              return {
                ProjectID: this.projectId  ,
                DebtClassificationID: x.debtId,
                ResidualDebtID: x.residualDebtId,
                AvoidableFlagID: x.avoidableFlagId,
                ReasonForResidual: this.getNumber(x.residualId),
                ExpectedCompletiondate: this.getString(x.expectedCompletiondate),
                CreatedBy: this.employeeId,
                ModifiedBy: this.employeeId,
                ID: x.id,
                ApplicationID: x.applicationId,
                CauseCodeID: x.causeId,
                ResolutionCodeID: x.resolutionId
              };
            });
        const dictionaryString = JSON.stringify(dictionaryDetails);
        const saveData = {
          dataDictionaryDetails: dictionaryString
        };
        this.datadictionaryService.SaveDataDictionaryByID(saveData)
          .subscribe(result => {
            this.spinner.hide();
            if (result === 'success') {
              this.setSuccessMessage('PatternSavedSuccessfully');              
              setTimeout(() => {
                this.SearchDictionaries();
              }, 3000); 
            } else {
              this.setErrorMessage('ApproveUnsuccessful');
            }
          });
    } else {
      this.setErrorMessage('Choose at least one pattern');
    }
  }

  ClearPattern(): void {
    this.addpatternFG.reset();
    this.saveClicked = false;
    this.reasonResidualHide = false;
    this.completionDateHide = false;
  }

  residualChange(): void {
    const residualDebtId = this.addpatternFG.get('residualFlag').value;
    if (residualDebtId !== null) {
      const residuladebtName: any = this.residualDebts.find((x: any) => x.value === residualDebtId);
      if (residuladebtName.label.toLowerCase() === 'yes') {
        this.reasonResidualHide = true;
        const avoidableFlag: any = this.avoidableFlags.find((x: any) => x.value === this.addpatternFG.get('avaidableFlag').value);
        if (avoidableFlag.label.toLowerCase() === 'yes') {
          this.completionDateHide = true;
        } else {
          this.completionDateHide = false;
        }
      } else {
        this.reasonResidualHide = false;
        this.completionDateHide = false;
      }
    }
  }

  optionalChange(dataDictionary: DataDictionaryGrid): void {
    const residualDebtId = dataDictionary.residualDebtId;
    if (residualDebtId !== null) {
      const residuladebtName: any = this.residualDebts.find((x: any) => x.value === residualDebtId);
      if (residuladebtName.label.toLowerCase() === 'yes') {
        this.optionalReasonResidualHide = true;
        const avoidableFlag: any = this.avoidableFlags.find((x: any) => x.value === dataDictionary.avoidableFlagId);
        if (avoidableFlag.label.toLowerCase() === 'yes') {
          this.optionalCompletionDateHide = true;
        } else {
          this.optionalCompletionDateHide = false;
        }
      } else {
        this.optionalReasonResidualHide = false;
        this.optionalCompletionDateHide = false;
      }
    }
  }

  deleteDictionaries(): void {
    if (this.selectedDictionaries.length > 0) {
          this.spinner.show();
          const dictionaryDetails = this.selectedDictionaries.map(x =>
             {
              return {
                    ProjectID: this.projectId,
                    ApplicationID:  x.applicationId,
                    ResolutionCodeID: x.resolutionId,
                    CauseCodeID: x.causeId,
                    ID: x.id
                  };
            });
          const dictionaryString = JSON.stringify(dictionaryDetails);
          const deleteData = {
            dataDictionaryDetails: dictionaryString,
            EmployeeID: this.employeeId
          };
          this.datadictionaryService.DeleteDataDictionaryByID(deleteData)
          .subscribe(result => {
            if (result === '0') {
              this.setSuccessMessage('PatternDeletedSuccessfully');
              setTimeout(() => {
                this.SearchDictionaries();
              }, 3000); 
            } else {
              this.setErrorMessage('PatternDeleteUnsuccessful');
            }
            this.spinner.hide();
          });              
    } else {
      this.setErrorMessage('Choose at least one pattern');
    }
  }

  AddReasonResidualAndCompDate(): void {
    const dataDictionary = this.datadictionary;
    const pattern: AddReasonCompDate = {
      empId: dataDictionary.id,
      applicationId: dataDictionary.applicationId,
      causeId: dataDictionary.causeId,
      resolutionId: dataDictionary.resolutionId,
      debtclassiId: Number(dataDictionary.debtId),
      avoidId: dataDictionary.avoidableFlagId,
      resiId: dataDictionary.residualDebtId,
      projectId: this.projectId,
      employeeId: this.employeeId,
      reasonResiValueId: this.getNumber(this.optionalReasonResidual),
      compDateValue: this.getDate(this.optionalCompletionDate),
    };
    this.datadictionaryService.AddReasonResidualAndCompDate(pattern)
    .subscribe(result => {
      if (result === 'Added Successfully') {
        this.setSuccessMessage('ReasonForResidualAndExpectedCompletionDateAddedSuccessfully');
        this.datadictionary.expectedCompletiondate = this.getDate(this.optionalCompletionDate);
        const reason = this.reasonForResidual.find(x => x.value === this.getNumber(this.optionalReasonResidual));
        if (reason) {
        this.datadictionary.residualName = reason.label;
        } else {
          this.datadictionary.residualName = '';
        }
        this.optionalPatternDialog = false;
      }
    });
  }

  UpdateSignOffDate(): void {
    if (this.signOff) {
      this.isError = false;
      this.spinner.show();
      this.params = {
      ProjectID: this.projectId,
      ApplicationID: (this.selectedApplications[0]) ? this.selectedApplications[0].applicationId : 0,
      EffectiveDate: this.getDateTime(this.signOff),
      EmployeeID: this.employeeId
    };
      this.datadictionaryService.UpdateSignOffDate(this.params)
    .subscribe(result => {
      if (result === 'success') {
        this.setSuccessMessage('DataDictionaryIsClassifiedForYourProjectAndWillBeEffectiveFrom');
        this.successMessage = this.successMessage + this.getDate(this.signOff);
        this.getProjectDebtDetails();
      }
      this.spinner.hide();
    });
  } else {
    this.setErrorMessage('PleaseProvideTheSignoffDate');
  }
  }
  getTitle(values: any, id: any): string {
    if (values && id) {
      return values.find(x => x.value === id).label;
    }
    return '';
  }

  clearOptional(): void {
    this.optionalReasonResidual = null;
    this.optionalCompletionDate = null;
  }
  GetConflictPatternDetails(): void {
    this.spinner.show();
    this.params = {
      ProjectId: this.projectId,
      EsaProjectId: this.esaProjectId
     };
    this.datadictionaryService.GetConflictPatternDetailsForDownload(this.params)
      .subscribe(result => {
        if(result != "File does not exists") {
        this.params = {
          Path: result,
          IsMacroTemplate: false
        };
        this.datadictionaryService.DownloadTemplate(this.params)
        .subscribe(file => {
            this.downloadFile(file, result);
            this.spinner.hide();
        });
      } else {
        this.setErrorMessage('DownloadFailed');
      }
      });
  }
  DownloadDataDictionary(): void {
    if(this.selectedProject) {
    this.spinner.show();
    this.params = {
      ProjectID: this.projectId,
     };
        this.datadictionaryService.DownloadPatternTemplate(this.params)
        .subscribe(file => {
            this.downloadDataFile(file);
            this.spinner.hide();
        }, (error) => {
          this.setErrorMessage('DownloadFailed');
        });
    } else {
      this.setErrorMessage('PleaseSelectAnyOneProject');
    }
  }
  downloadDataFile(file){
    const chknumber2=2;
    const blob = new Blob([file], { type: 'application/vnd.ms-excel' });
    var today = new Date();
    var dd = String(today.getDate()).padStart(chknumber2, '0');
    var MM = String(today.getMonth() + 1).padStart(chknumber2, '0');
    var yyyy = today.getFullYear();
    var hh = today.getHours();
    var mm = today.getMinutes();
    var ss = today.getSeconds();
    var td = yyyy + '_' + MM + '_' + dd + '_' + hh + '_' + mm + '_' + ss;
    this.excelName =  Constants.datadic + this.projectId + '_' + td + Constants.FileFormatxl;
    const url = window.URL.createObjectURL(blob);
    const noice = document.createElement('a');
    noice.download = this.excelName;
    noice.href = url;
    noice.click();
  }

  excelUpload(event): void {
    this.spinner.show();
    const files = event.files;
    const frmData = this.getFormData(files);
    this.excelupload.clear();
    this.datadictionaryService.DataDictionaryUploadByProject(frmData, this.projectId, this.employeeId)
      .subscribe(result => {
        this.spinner.hide();
        if (result){
        if (result === 'Y') {
          this.searchClicked = true;
          this.isLengthError = false;
          this.setSuccessMessage('UploadedSuccessfully.KindlyCheckErrorLogForFailedPatterns');
          setTimeout(() => {
            this.SearchDictionaries();
          }, 3000); 
        } else if (result.indexOf('Please') != -1){
          this.isError = true;
          this.errorMessage = result;
          setTimeout(() => {
            this.isError = false;
            this.errorMessage = '';
          }, Constants.lifeSpan);
        } else if (result == 'Invalid Template') {
          this.setErrorMessage('InvalidTemplatePleaseDownloadTheLatestTemplateAndValidateBeforeUploading');
        } else {
          this.setErrorMessage(result);
        }
      }
      });
  }

  downloadFile(file: any, fileName: string): void {
    const blob = new Blob([file], { type: Constants.ExcelType });
    const url = window.URL.createObjectURL(blob);
    const downloadTag = document.createElement('a');
    downloadTag.download = this.getFileName(fileName);
    downloadTag.href = url;
    downloadTag.click();
  }
  getFileName(path: string): string {
    return path.replace(/^.*[\\\/]/, '');
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
            if (message) {
            this.isError = true;
            this.errorMessage = message;
            }
            setTimeout(() => {
             this.isError = false;
             this.errorMessage = '';
            }, Constants.lifeSpan);
    });    
    }
  
  setAddPatternErrorMessage(pattern: string): void {    
            this.isPatternError = true;
            this.errorPatternMessage = pattern;
            setTimeout(() => {
             this.isPatternError = false;
             this.errorPatternMessage = '';
            }, Constants.lifeSpan);      
  }

  getVirtualScroll(list: any): boolean {
    if (list && list.length > this.virtualItemSize) {
      return true;
    } else {
      return false
    }
  }
  getFormData(files): FormData {
    const frmData = new FormData();
    if (files.length > 0) {
      files.forEach((file, index) => {
        frmData.append('file' + index, file);
      });
    }
    return frmData;
  }

  getDate(date: string): any {
    if (!date) {
      return '';
    }
    return this.datePipe.transform(new Date(date), Constants.dateFormat);
  }
  getDateTime(date: string): any {
    if (!date) {
      return '';
    }
    return this.datePipe.transform(new Date(date), Constants.dateTimeFormat);
  }
  getNumber(value: any): any {
    return (value === undefined || value === null) ? 0 : value;
  }

  getString(value: any): any {
    return (value === undefined || value === null) ? '' : value;
  }

}
