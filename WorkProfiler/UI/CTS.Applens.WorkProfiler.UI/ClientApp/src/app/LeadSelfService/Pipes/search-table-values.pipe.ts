// Copyright (c) Applens. All Rights Reserved.
import { Pipe, PipeTransform } from '@angular/core';
import { LeadselfService } from '../leadself.service';
import { DebtReviewModel, SearchDebtReviewDataModel } from '../Models/LoadSelfServiceModel';

@Pipe({
  name: 'searchTableValues'
})
export class SearchTableValuesPipe implements PipeTransform {

  transform(value: DebtReviewModel[], searchData: SearchDebtReviewDataModel,loadSelf: LeadselfService): DebtReviewModel[] {
    let tempDetails: DebtReviewModel[] = value;
    if((searchData.searchTicketID == null || searchData.searchTicketID == '') && 
    (searchData.searchApplication == null || searchData.searchApplication == '') && 
    (searchData.searchDescription == null || searchData.searchDescription == '') && 
    (searchData.searchResolutionCode == null || searchData.searchResolutionCode == '') && 
    (searchData.searchResidualDebt == null || searchData.searchResidualDebt == '') && 
    (searchData.searchDebtCategory == null || searchData.searchDebtCategory == '') && 
    (searchData.searchCauseCode == null || searchData.searchCauseCode == '') && 
    (searchData.searchAssignedTo == null || searchData.searchAssignedTo == '') && 
    (searchData.searchAvoidableFlag == null || searchData.searchAvoidableFlag == '') && 
    (searchData.searchService == null || searchData.searchService == '') && 
    (searchData.searchf1 == null || searchData.searchf1 == '') && 
    (searchData.searchf2 == null || searchData.searchf2 == '') && 
    (searchData.searchf3 == null || searchData.searchf3 == '') && 
    (searchData.searchf4 == null || searchData.searchf4 == '')){
      return value;
    }
    else{
      if(searchData.searchTicketID != null && searchData.searchTicketID != undefined){
        tempDetails = tempDetails.filter(f=> {
          return f.ticketId.toLocaleLowerCase().trim().replace(/\s/g,'').indexOf(searchData.searchTicketID.toLocaleLowerCase().trim().replace(/\s/g,'')) > -1
        });
      }
      if(searchData.searchDescription != null && searchData.searchDescription != undefined){
        tempDetails = tempDetails.filter(f=> {
          return f.ticketDescription.toLocaleLowerCase().trim().replace(/\s/g,'').indexOf(searchData.searchDescription.toLocaleLowerCase().trim().replace(/\s/g,'')) > -1
        });
      }
      if(searchData.searchApplication != null && searchData.searchApplication != undefined){
        tempDetails = tempDetails.filter(f=> {
          return f.application.toLocaleLowerCase().trim().replace(/\s/g,'').indexOf(searchData.searchApplication.toLocaleLowerCase().trim().replace(/\s/g,'')) > -1
        });
      }
      if(searchData.searchCauseCode != null && searchData.searchCauseCode != undefined){
        loadSelf.CauseCodeDropdown.forEach(s=>{
          if(!s.text.toLocaleLowerCase().trim().replace(/\s/g,'').includes(searchData.searchCauseCode.toLocaleLowerCase().trim().replace(/\s/g,''))){
            var data = value.filter(c=>c.causeCodeMapId == s.value);
            if(data != undefined && data.length > 0){
              
              data.forEach(f=>{
                const index: number = tempDetails.findIndex(c=>c.causeCodeMapId == f.causeCodeMapId);
                tempDetails.splice(index,1);
              });
            }
          }
        })
      }
      if(searchData.searchResolutionCode != null && searchData.searchResolutionCode != undefined){
        loadSelf.ResolutionCodeDropdown.forEach(s=>{
          if(!s.text.toLocaleLowerCase().trim().replace(/\s/g,'').includes(searchData.searchResolutionCode.toLocaleLowerCase().trim().replace(/\s/g,''))){
            var data = tempDetails.filter(c=>c.resolutionId == s.value);
            if(data != undefined && data.length > 0){
              data.forEach(f=>{
                const index: number = tempDetails.findIndex(c=>c.resolutionId == f.resolutionId);
                tempDetails.splice(index,1);
              });
            }
          }
        });
      }
      if(searchData.searchDebtCategory != null && searchData.searchDebtCategory != undefined){
        loadSelf.DebtOverrideReviewDropdown.forEach(s=>{
          if(!s.text.toLocaleLowerCase().trim().replace(/\s/g,'').includes(searchData.searchDebtCategory.toLocaleLowerCase().trim().replace(/\s/g,''))){
            var data = tempDetails.filter(c=>c.debtClassificationId == s.value);
            if(data != undefined && data.length > 0){
              data.forEach(f=>{
                const index: number = tempDetails.findIndex(c=>c.debtClassificationId == f.debtClassificationId);
                tempDetails.splice(index,1);
              });
            }
          }
        });
      }
      if(searchData.searchAvoidableFlag != null && searchData.searchAvoidableFlag != undefined){
        loadSelf.AvoidableMasterDropdown.forEach(s=>{
          if(!s.text.toLocaleLowerCase().trim().replace(/\s/g,'').includes(searchData.searchAvoidableFlag.toLocaleLowerCase().trim().replace(/\s/g,''))){
            var data = tempDetails.filter(c=>c.avoidableFlag == s.value);
            if(data != undefined && data.length > 0){
              data.forEach(f=>{
                const index: number = tempDetails.findIndex(c=>c.avoidableFlag == f.avoidableFlag);
                tempDetails.splice(index,1);
              });
            }
          }
        });
      }
      if(searchData.searchResidualDebt != null && searchData.searchResidualDebt != undefined){
        loadSelf.ResidualMasterDropdown.forEach(s=>{
          if(!s.text.toLocaleLowerCase().trim().replace(/\s/g,'').includes(searchData.searchResidualDebt.toLocaleLowerCase().trim().replace(/\s/g,''))){
            var data = tempDetails.filter(c=>c.residualDebtMapId == s.value);
            if(data != undefined && data.length > 0){
              data.forEach(f=>{
                const index: number = tempDetails.findIndex(c=>c.residualDebtMapId == f.residualDebtMapId);
                tempDetails.splice(index,1);
              });
            }
          }
        });
      }
      if(searchData.searchService != null && searchData.searchService != undefined){
        tempDetails = tempDetails.filter(f=> {
          return f.serviceName.toLocaleLowerCase().trim().replace(/\s/g,'').indexOf(searchData.searchService.toLocaleLowerCase().trim().replace(/\s/g,'')) > -1
        });
      }
      if(searchData.searchAssignedTo != null && searchData.searchAssignedTo != undefined){
        tempDetails = tempDetails.filter(f=> {
          return f.assignedTo.toLocaleLowerCase().trim().replace(/\s/g,'').indexOf(searchData.searchAssignedTo.toLocaleLowerCase().trim().replace(/\s/g,'')) > -1
        });
      }
      if(searchData.searchf1 != null && searchData.searchf1 != undefined){
        tempDetails = tempDetails.filter(f=> {
          return f.flexField1.toLocaleLowerCase().trim().replace(/\s/g,'').indexOf(searchData.searchf1.toLocaleLowerCase().trim().replace(/\s/g,'')) > -1
        });
      }
      if(searchData.searchf2 != null && searchData.searchf2 != undefined){
        tempDetails = tempDetails.filter(f=> {
          return f.flexField2.toLocaleLowerCase().trim().replace(/\s/g,'').indexOf(searchData.searchf2.toLocaleLowerCase().trim().replace(/\s/g,'')) > -1
        });
      }
      if(searchData.searchf3 != null && searchData.searchf3 != undefined){
        tempDetails = tempDetails.filter(f=> {
          return f.flexField3.toLocaleLowerCase().trim().replace(/\s/g,'').indexOf(searchData.searchf3.toLocaleLowerCase().trim().replace(/\s/g,'')) > -1
        });
      }
      if(searchData.searchf4 != null && searchData.searchf4 != undefined){
        tempDetails = tempDetails.filter(f=> {
          return f.flexField4.toLocaleLowerCase().trim().replace(/\s/g,'').indexOf(searchData.searchf4.toLocaleLowerCase().trim().replace(/\s/g,'')) > -1
        });
      }
    }
    return tempDetails;
  }

}
