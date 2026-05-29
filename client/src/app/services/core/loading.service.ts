import { inject, Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
private spinnerService = inject(NgxSpinnerService);
requestCount=0;

loading(){
  this.requestCount++;
  this.spinnerService.show(undefined, {
    bdColor: "rgba(0, 0, 0, 0.8)",
    size: "large",
    color: "#f97316",
    type: "square-jelly-box",
    fullScreen: true,
    showSpinner:true
  });

}

hideLoader(){
    this.requestCount--;
  if(this.requestCount <= 0)
    this.spinnerService.hide();
}

}
