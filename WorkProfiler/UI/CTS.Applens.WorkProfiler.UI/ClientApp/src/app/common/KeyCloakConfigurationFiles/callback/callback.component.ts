/* © [2022] Cognizant. All rights reserved.  Cognizant Confidential and/or Trade Secret. 
NOTICE: This unpublished material is proprietary to Cognizant and its suppliers, if any. 
The methods, techniques and technical concepts herein are considered Cognizant confidential 
and/or trade secret information. 
This material may be covered by U.S. and/or foreign patents or patent applications. 
Use, distribution or copying, in whole or in part, is forbidden, 
except by express written permission of Cognizant. */

import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { KeyCloakConstants } from '../KeyCloakEum';

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.css']
})
export class CallbackComponent implements OnInit {

  constructor(private readonly router: Router) { }

  ngOnInit(): void {
    const currenturl = sessionStorage.getItem(KeyCloakConstants.CurrentRoutedUrl);
    this.router.navigate([currenturl ? currenturl : '']);
  }

}



