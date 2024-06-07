// Copyright (c) Applens. All Rights Reserved.
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SpinnerService {

constructor() { }
public status: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(null);

  show(): void {
    this.status.next(true);
  }
  hide(): void {
    this.status.next(false);
  }
}
