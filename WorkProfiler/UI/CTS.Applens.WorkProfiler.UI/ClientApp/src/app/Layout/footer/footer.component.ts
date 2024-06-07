// Copyright (c) Applens. All Rights Reserved.
import { Component, OnInit, Input, AfterViewChecked } from '@angular/core';
import { LayoutService } from 'src/app/common/services/layout.service';
@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit, AfterViewChecked {

  constructor(private layoutService: LayoutService) { }
  ngAfterViewChecked(): void {
    if(document.getElementById('footer') != undefined)
    {
      this.layoutService.footerHeight = document.getElementById('footer').clientHeight;
    }
  }
  
  footerValue: string;
  ngOnInit() {
    this.layoutService.Getfooter().subscribe(x => {
      this.footerValue = x;
    })
  }
}
