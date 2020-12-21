import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from './../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { City } from '../models/city';

@Injectable({
  providedIn: 'root'
})
export class PlacesService {
  private citiesBehaivor = new BehaviorSubject<City[]>(new Array());
  cities=this.citiesBehaivor.asObservable();

  constructor(private http: HttpClient) { }

  getCity() {
    const url = environment.uri+'/api/City/list';

   this.http.get(url).subscribe((data:City[])=>{
     this.citiesBehaivor.next(data);
   },error=>{
     console.error(error);
   });
   return this.cities;
  }

  getCityIdComposePoint(id:string):Observable<any>{
    const url = environment.uri + '/api/Point/pointInCity';
    const param = new HttpParams().set('id', id)
   return this.http.get<any>(url, { params: param })
  }
}
