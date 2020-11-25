
import { environment } from './../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { Injectable } from '@angular/core';
import { PointAirDetail } from '../models/point-air-detail';
import { PointAir } from '../models/point-air';
import { tap } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class ReportAqiService {
  //
  private reportAqiBehavior = new BehaviorSubject<PointAir[]>(new Array<PointAir>());
  reportAqi = this.reportAqiBehavior.asObservable();
  //
  private reportAqiInfoBehavior= new BehaviorSubject<PointAir[]>(new Array());
  reportAqiInfo= this.reportAqiInfoBehavior.asObservable();
  //
private reportAqiListBehavior= new BehaviorSubject<PointAirDetail[]>(new Array());
reportAqiList=this.reportAqiListBehavior.asObservable();


  constructor(private http: HttpClient) { }
  OnInit() {

  }

  //
  getinfoPointAir(id: string) {
    const url = environment.uri+'/api/ReportAirQuality/aqi/point';
    const param = new HttpParams().set('cityId', id);

    this.http.get(url, { params: param }).subscribe(
      (data: PointAir[]) => {
        this.reportAqiBehavior.next(data);
      }, error => {
        console.error(error);
      }
    )
    return this.reportAqi;
  }
  getInfoPointAir(){
const url=environment.uri+'/api/ReportAirQuality/aqi/point/list';
this.http.get(url).subscribe((data:PointAir[])=>{
 this.reportAqiInfoBehavior.next(data);
},error=>{
  console.error(error);
})
return this.reportAqiInfo;
  }

  getInfoAirCurrentByPointId(id:string){
const url = environment.uri+'/api/AirQuality/current';
const param= new HttpParams().set('pointId',id);
return this.http.get(url,{params:param});
  }
  getAqiAirByPointId(id:string){
const url= environment.uri+'/api/AirQuality/listById';

const param = new HttpParams().set('pointId', id);

this.http.get(url, { params: param }).subscribe(
  (data: PointAirDetail[]) => {
  console.log('hh'+data);
    this.reportAqiListBehavior.next(data);
  }, error => {
    console.error(error);
  }
)

// return this.reportAqiList;
  }

}
