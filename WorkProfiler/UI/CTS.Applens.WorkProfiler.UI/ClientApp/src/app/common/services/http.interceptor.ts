// Copyright (c) Applens. All Rights Reserved.
import { SpinnerService } from './spinner.service';
import { MessageService } from 'primeng/api';
import { Injectable, Injector } from '@angular/core';
import {
    HttpEvent,
    HttpInterceptor,
    HttpHandler,
    HttpRequest,
    HttpResponse,
    HttpErrorResponse,
    HttpHeaders
  } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
//KeyClapk added Codes
import { AppSettingsConfig } from '../../../app/app.settings.config';
import { KeyCloakConstants } from '../KeyCloakConfigurationFiles/KeyCloakEum';

@Injectable({
  providedIn: 'root'
})
export class HttpServiceInterceptor implements HttpInterceptor {
    constructor(private messageService: MessageService, 
      private spinner: SpinnerService,
      private translate: TranslateService) {}
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
     //KeyCloak Related Codes
     const accessToken = sessionStorage.getItem(KeyCloakConstants.AccessToken);
     if (AppSettingsConfig.settings && AppSettingsConfig.settings.KeyCloakEnabled && accessToken ) {
         const keyCloakAccessToken = accessToken.replace(/^"(.*)"$/, '$1');//To replaces extra quotes bgin&end. The replacement pattern $1 indicates 
         //that the matched substring is to be replaced by the first captured group
       request = request.clone({
         setHeaders: {
           Authorization: `${KeyCloakConstants.Bearer} ${keyCloakAccessToken}`
         },
         withCredentials: true
       });
     }
     else if(request.url.includes('realms')){
       request = request.clone({
         withCredentials: true
       });
     }
     else{
       request = request.clone(
        {
          setHeaders: {
            'Access-Control-Allow-Origin': '*'
          },
            withCredentials: true
          });
        }
        //KeyCloak Related End
      return next.handle(request)
        .pipe(
          catchError((error: HttpErrorResponse) => {
            let errorMessage = '';
            if (error.error instanceof ErrorEvent) {
              errorMessage = `Error: ${error.error.message}`;
            } else {
              errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
            }
            this.translate.get('AnerroroccurredwhileprocessingyourrequestPleasetryagain')
            .subscribe(message => {
              this.messageService.add(
                {severity:'error', summary: 'Error in Request',
                detail: message, life: 5000}
              );  
            });                   
            this.spinner.hide();
            return throwError(error);
          })
        );
    }
  }
