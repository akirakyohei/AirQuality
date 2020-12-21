import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class PointService {

  constructor(private http: HttpClient) { }
  getPointList() {
    const url = environment.uri + '/api/ManagementDevice/listAll';
    return this.http.get(url, );
  }
  addPoint(object) {
    const url = environment.uri + '/api/ManagementDevice/register';
    return this.http.post(url, object);
  }
  removePoint(pointId) {

    const url = environment.uri + '/api/ManagementDevice/remove';
    const params = new HttpParams().set('deviceId', pointId);
    return this.http.delete(url, {  params });
  }
  updatePoint(point, id) {
    const url = environment.uri + '/api/ManagementDevice/update';
    const params = new HttpParams().set('id', id);
    return this.http.put(url, point, { params });
  }
}
