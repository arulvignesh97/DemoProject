/***************************************************************************
*COGNIZANT CONFIDENTIAL AND/OR TRADE SECRET
*Copyright [2018] – [2021] Cognizant. All rights reserved.
*NOTICE: This unpublished material is proprietary to Cognizant and
*its suppliers, if any. The methods, techniques and technical
  concepts herein are considered Cognizant confidential and/or trade secret information.
 
*This material may be covered by U.S. and/or foreign patents or patent applications.
*Use, distribution or copying, in whole or in part, is forbidden, except by express written permission of Cognizant.
***************************************************************************/

// Dependency injection
import { Inject, Injectable } from '@angular/core';
import { SESSION_STORAGE, StorageService } from 'ngx-webstorage-service';
import { KeyCloakConstants } from './KeyCloakEum';

// adding root as the injectable

@Injectable(
  { providedIn: 'root' }
)
export class StorageHandlerService {

  timeoutDef;
  // constructor initialization for storage handler 
  constructor(@Inject(SESSION_STORAGE) public readonly storage: StorageService) {

  }
  //Caching the session data
  GetSessionData(key: string, isTemp = false) {
    const sessionData = this.storage['storage'][key];
    if (isTemp) {
      this.timeoutDef = setTimeout(() => {
        this.storage.remove(key);
      }, KeyCloakConstants.Thousand);
    }
    return this.storage.has(key) ? sessionData : undefined;
  }

  SetSessionData(key: string, value) {
    this.storage.set(key, value);
  }

  RemoveSessions(key: string, isAll = false) {
    if (isAll) {
      this.storage.clear();
    } else {
      if (this.storage.has(key)) {
        this.storage.remove(key);
      }
    }
  }
}

