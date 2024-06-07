// Copyright (c) Applens. All Rights Reserved.
import { formatDate } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import { ErrorTicketModel, SearchModel } from '../Models/ErrorTicketsModels';

@Pipe({
  name: 'errorTicketFilter'
})
export class ErrorTicketFilterPipe implements PipeTransform {

  transform(value: ErrorTicketModel[], searchData: SearchModel, masterData : any) : ErrorTicketModel[] {
    let tempDetails: ErrorTicketModel[] = value;
    if((searchData.searchTicketType == null || searchData.searchTicketType === '') && 
    (searchData.searchApplicationName == null || searchData.searchApplicationName === '') && 
    (searchData.searchTicketId == null || searchData.searchTicketId === '') && 
    (searchData.searchTicketDescription == null || searchData.searchTicketDescription === '') && 
    (searchData.searchStatus == null || searchData.searchStatus === '') && 
    (searchData.searchSeverity == null || searchData.searchSeverity === '') && 
    (searchData.searchResolutionCode == null || searchData.searchResolutionCode === '') && 
    (searchData.searchResidualDebt == null || searchData.searchResidualDebt === '') && 
    (searchData.searchPriority == null || searchData.searchPriority === '') && 
    (searchData.searchOpenDate == null || searchData.searchOpenDate === '') && 
    (searchData.searchIsPartialAutomated == null || searchData.searchIsPartialAutomated === '') && 
    (searchData.searchDebtCategory == null || searchData.searchDebtCategory === '') && 
    (searchData.searchCauseCode == null || searchData.searchCauseCode === '') && 
    (searchData.searchTower == null || searchData.searchTower === '') && 
    (searchData.searchAssignment == null || searchData.searchAssignment === '')){
      return tempDetails;
    }
    else{
      if(searchData.searchTicketId !== null && searchData.searchTicketId !== undefined){
        tempDetails = tempDetails.filter(f=> {
          return f.ticketId.toLocaleLowerCase().trim().replace(/\s/g,'').indexOf(searchData.searchTicketId.toLocaleLowerCase().trim().replace(/\s/g,'')) > -1
        });
      }
      if(searchData.searchTicketDescription !== null && searchData.searchTicketDescription !== undefined){
        tempDetails = tempDetails.filter(f=> {
          return f.ticketDescription.toLocaleLowerCase().trim().replace(/\s/g,'').indexOf(searchData.searchTicketDescription.toLocaleLowerCase().trim().replace(/\s/g,'')) > -1
        });
      }
      if(searchData.searchApplicationName !== null && searchData.searchApplicationName !== undefined){
        masterData.applicationList.forEach(s=>{
          if(!s.label.toLocaleLowerCase().trim().replace(/\s/g,'').includes(searchData.searchApplicationName.toLocaleLowerCase().trim().replace(/\s/g,''))){
            let data = tempDetails.filter(c=>c.applicationId === s.value);
            if(data !== undefined && data.length > 0){
              data.forEach(f=>{
                const index: number = tempDetails.findIndex(c=>c.applicationId === f.applicationId);
                tempDetails.splice(index,1);
              })
              
            }
          }
        })
      }
      if(searchData.searchTicketType !== null && searchData.searchTicketType !== undefined){
        masterData.ticketTypeList.forEach(s=>{
          if(!s.label.toLocaleLowerCase().trim().replace(/\s/g,'').includes(searchData.searchTicketType.toLocaleLowerCase().trim().replace(/\s/g,''))){
            let data = tempDetails.filter(c=>c.ticketTypeId === s.value);
            if(data !== undefined && data.length > 0){
              data.forEach(f=>{
                const index: number = tempDetails.findIndex(c=>c.ticketTypeId === f.ticketTypeId);
                tempDetails.splice(index,1);
              })
              
            }
          }
        })
      }
      if(searchData.searchPriority !== null && searchData.searchPriority !== undefined){
        masterData.priorityList.forEach(s=>{
          if(!s.label.toLocaleLowerCase().trim().replace(/\s/g,'').includes(searchData.searchPriority.toLocaleLowerCase().trim().replace(/\s/g,''))){
            let data = tempDetails.filter(c=>c.priorityId === s.value);
            if(data !== undefined && data.length > 0){
              data.forEach(f=>{
                const index: number = tempDetails.findIndex(c=>c.priorityId === f.priorityId);
                tempDetails.splice(index,1);
              })
             
            }
          }
        })
      }
      if(searchData.searchStatus !== null && searchData.searchStatus !== undefined){
        masterData.statusList.forEach(s=>{
          if(!s.label.toLocaleLowerCase().trim().replace(/\s/g,'').includes(searchData.searchStatus.toLocaleLowerCase().trim().replace(/\s/g,''))){
            let data = tempDetails.filter(c=>c.statusId === s.value);
            if(data !== undefined && data.length > 0){
              data.forEach(f=>{
                const index: number = tempDetails.findIndex(c=>c.statusId === f.statusId);
                tempDetails.splice(index,1);
              })
              
            }
          }
        })
      }
      if(searchData.searchOpenDate !== null && searchData.searchOpenDate !== undefined){
        tempDetails = tempDetails.filter(f=> {
          return formatDate(f.openDate,'MM-dd-yyyy','en-US').toLocaleLowerCase().trim().replace(/\s/g,'')
          .indexOf(searchData.searchOpenDate.toLocaleLowerCase().trim().replace(/\s/g,'')) > -1
        });
      }
      if(searchData.searchSeverity !== null && searchData.searchSeverity !== undefined){
        masterData.severityList.forEach(s=>{
          if(!s.label.toLocaleLowerCase().trim().replace(/\s/g,'').includes(searchData.searchSeverity.toLocaleLowerCase().trim().replace(/\s/g,''))){
            let data = tempDetails.filter(c=>c.severityId === s.value);
            if(data !== undefined && data.length > 0){
              data.forEach(f=>{
                const index: number = tempDetails.findIndex(c=>c.severityId === f.severityId);
                tempDetails.splice(index,1);
              });
            }
          }
        });
      }
      if(searchData.searchCauseCode !== null && searchData.searchCauseCode !== undefined){
        masterData.causeList.forEach(s=>{
          if(!s.label.toLocaleLowerCase().trim().replace(/\s/g,'').includes(searchData.searchCauseCode.toLocaleLowerCase().trim().replace(/\s/g,''))){
            let data = tempDetails.filter(c=>c.causeCodeId === s.value);
            if(data !== undefined && data.length > 0){
              data.forEach(f=>{
                const index: number = tempDetails.findIndex(c=>c.causeCodeId === f.causeCodeId);
                tempDetails.splice(index,1);
              });
            }
          }
        });
      }
      if(searchData.searchResolutionCode !== null && searchData.searchResolutionCode !== undefined){
        masterData.resolutionList.forEach(s=>{
          if(!s.label.toLocaleLowerCase().trim().replace(/\s/g,'').includes(searchData.searchResolutionCode.toLocaleLowerCase().trim().replace(/\s/g,''))){
            let data = tempDetails.filter(c=>c.resolutionId === s.value);
            if(data !== undefined && data.length > 0){
              data.forEach(f=>{
                const index: number = tempDetails.findIndex(c=>c.resolutionId === f.resolutionId);
                tempDetails.splice(index,1);
              });
            }
          }
        });
      }
      if(searchData.searchDebtCategory !== null && searchData.searchDebtCategory !== undefined){
        masterData.debtList.forEach(s=>{
          if(!s.label.toLocaleLowerCase().trim().replace(/\s/g,'').includes(searchData.searchDebtCategory.toLocaleLowerCase().trim().replace(/\s/g,''))){
            let data = tempDetails.filter(c=>c.debtClassificationId === s.value);
            if(data !== undefined && data.length > 0){
              data.forEach(f=>{
                const index: number = tempDetails.findIndex(c=>c.debtClassificationId === f.debtClassificationId);
                tempDetails.splice(index,1);
              });
            }
          }
        });
      }
      if(searchData.searchAvoidable !== null && searchData.searchAvoidable !== undefined){
        masterData.avoidableFlgList.forEach(s=>{
          if(!s.label.toLocaleLowerCase().trim().replace(/\s/g,'').includes(searchData.searchAvoidable.toLocaleLowerCase().trim().replace(/\s/g,''))){
            let data = tempDetails.filter(c=>c.avoidableFlagId === s.value);
            if(data !== undefined && data.length > 0){
              data.forEach(f=>{
                const index: number = tempDetails.findIndex(c=>c.avoidableFlagId === f.avoidableFlagId);
                tempDetails.splice(index,1);
              });
            }
          }
        });
      }
      if(searchData.searchResidualDebt !== null && searchData.searchResidualDebt !== undefined){
        masterData.residualFlgList.forEach(s=>{
          if(!s.label.toLocaleLowerCase().trim().replace(/\s/g,'').includes(searchData.searchResidualDebt.toLocaleLowerCase().trim().replace(/\s/g,''))){
            let data = tempDetails.filter(c=>c.residualDebtId === s.value);
            if(data !== undefined && data.length > 0){
              data.forEach(f=>{
                const index: number = tempDetails.findIndex(c=>c.residualDebtId === f.residualDebtId);
                tempDetails.splice(index,1);
              });
            }
          }
        });
      }
      if(searchData.searchIsPartialAutomated !== null && searchData.searchIsPartialAutomated !== undefined){
        masterData.partialList.forEach(s=>{
          if(!s.label.toLocaleLowerCase().trim().replace(/\s/g,'').includes(searchData.searchIsPartialAutomated.toLocaleLowerCase().trim().replace(/\s/g,''))){
            let data = tempDetails.filter(c=>c.isPartiallyAutomated === s.value);
            if(data !== undefined && data.length > 0){
              data.forEach(f=>{
                const index: number = tempDetails.findIndex(c=>c.isPartiallyAutomated === f.isPartiallyAutomated);
                tempDetails.splice(index,1);
              });
            }
          }
        });
      }
      if(searchData.searchTower !== null && searchData.searchTower !== undefined){
        masterData.towerList.forEach(s=>{
          if(!s.label.toLocaleLowerCase().trim().replace(/\s/g,'').includes(searchData.searchTower.toLocaleLowerCase().trim().replace(/\s/g,''))){
            let data = tempDetails.filter(c=>c.towerId === s.value);
            if(data !== undefined && data.length > 0){
              data.forEach(f=>{
                const index: number = tempDetails.findIndex(c=>c.towerId === f.towerId);
                tempDetails.splice(index,1);
              });
            }
          }
        });
      }
      if(searchData.searchAssignment !== null && searchData.searchAssignment !== undefined){
        masterData.towerList.forEach(s=>{
          if(!s.label.toLocaleLowerCase().trim().replace(/\s/g,'').includes(searchData.searchAssignment.toLocaleLowerCase().trim().replace(/\s/g,''))){
            let data = tempDetails.filter(c=>c.assignmentGroupId === s.value);
            if(data !== undefined && data.length > 0){
              data.forEach(f=>{
                const index: number = tempDetails.findIndex(c=>c.assignmentGroupId === f.assignmentGroupId);
                tempDetails.splice(index,1);
              });
            }
          }
        });
      }
    }
    return tempDetails;
  }

}
