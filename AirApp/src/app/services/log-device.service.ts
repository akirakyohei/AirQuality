import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LogDeviceService {

  private logConnectionBehaivor = new BehaviorSubject<any>(new Array());
  logConnections = this.logConnectionBehaivor.asObservable();
  constructor(private http: HttpClient) { }

  LogConnection(deviceId: any) {
    let url = environment.uri + '/api/ManagementDevice/Log/Connection';
    let param = new HttpParams().set('deviceId', deviceId);
    this.http.get(url, { params: param }).subscribe((data) => {
      console.log(data);
      this.logConnectionBehaivor.next(data);

    })
    return this.logConnections;
  }


}
