// Copyright (c) Applens. All Rights Reserved.
import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: '[appDisableRightClick]'
})
export class DisableRightClickDirective {

  constructor() { }

  

  BM = 2; // button middle
  BR = 3; // button right
  msg = "MOUSE RIGHT CLICK IS NOT SUPPORTED ON THIS PAGE";

  @HostListener('document:mousedown', ['$event']) 
  onMousedownHandler(event: any) { 
    try { 
      if (event.button == this.BM || event.button == this.BR) 
      { 
        this.stopEvent(event);
        return false; 
      } 
    }
    catch (e) { 
      if (e.which == this.BR) 
      {
        this.stopEvent(event);
         return false; 
      } 
    }
  }
  @HostListener('document:contextmenu', ['$event'])
  onRightClick(event: any) {
    this.stopEvent(event);
  }

  @HostListener('document:dragstart', ['$event'])
  ondragstart(event: any) {
    this.stopEvent(event);
  }

  @HostListener('document:select', ['$event'])
  onselect(event: any) {
    this.stopEvent(event);
  }
  @HostListener('document:selectstart', ['$event'])
  onselectstart(event: any) {
    this.stopEvent(event);
  }

  @HostListener('document:keydown', ['$event']) 
  onKeydownHandler(e: any) {
    var keystroke = String.fromCharCode(e.keyCode).toLowerCase();

    
    if (e.ctrlKey && (e.key == "P" || e.key == "C" || e.key == "A" || e.key == "p"
        || e.key == "c" || e.key == "a" || e.charCode == 16 || e.charCode == 112
        || e.keyCode == 80) || (e.keyCode == 44) || (e.keyCode == 123)) {
        
        this.stopEvent(e);
    }

    if (e.keyCode > 111 && e.keyCode < 124) {
        
        this.stopEvent(e);
    }
    if (e.key == "F11" || e.key == "f11") {
       
        this.stopEvent(e);
    }
  }

  @HostListener('document:keyup', ['$event'])
  onkeyup(e: any) {
    if (e.keyCode == 44) {
      var aux = document.createElement("input");
      aux.setAttribute("value", "print screen disabled!");
      document.body.appendChild(aux);
      aux.focus();
      aux.select();
      document.execCommand("copy");
      // Remove it from the body
      document.body.removeChild(aux);
      //alert("Print screen disabled!")
      this.stopEvent(e);
    }

    if (e.keyCode > 111 && e.keyCode < 124) {
        
        this.stopEvent(e);
    }
    if (e.key == "F11" || e.key == "f11") {
       
        this.stopEvent(e);
    }
  }

  stopEvent(e: any): void {
    e.cancelBubble = true;
    e.preventDefault();
    e.stopImmediatePropagation();
  }

}
