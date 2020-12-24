import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ColorAqiService {

  constructor() { }

  setColor(value:number){
    if(value==0){
return '#1A93D9';
    }else if(value<50){
    return '#23DA27';
  }else if(value<100){
    return '#FFFD38';
  }else if(value<150){
    return '#FC7D23';
  }else if(value<200){
    return '#FC0D1B';
  }else if(value<300){
    return '#97084D';
  }else {
    return '#7C0425';
  }
  }
  getAirType(x: number) {
    if(x===0){
      return 'default';
    }else if (x <= 50) {
      return 'good';
    } else if (x <= 100) {
      return 'moderate';
    } else if (x <= 150) {
      return 'unhealthyForGroups';
    } else if (x <= 200) {
      return 'unhealthy';
    } else if (x <= 300) {
      return 'veryunhealthy';
    } else {
      return 'hazardous';
    }
  }

  getAirHealthy(x: number) {
    if(x===0){
      return 'No Data';
    }else if (x <= 50) {
      return 'Good';
    } else if (x <= 100) {
      return 'Moderate';
    } else if (x <= 150) {
      return 'Unhealthy for groups';
    } else if (x <= 200) {
      return 'Unhealthy';
    } else if (x <= 300) {
      return 'Very unhealthy';
    } else {
      return 'Hazardous';
    }
  }
}
