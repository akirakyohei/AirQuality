import { Marker } from './../models/marker';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})

export class MarkerService {
  private dataMarker = new BehaviorSubject<Marker[]>(new Array<Marker>());
 public data = this.dataMarker.asObservable();
  constructor(private http: HttpClient) {

  }
  public getMarker(): Observable<Marker[]> {
    const url = '/assets/data/marker.test.json';

    this.http.get<Marker[]>(url).subscribe(
      (data: Marker[]) => {
        this.dataMarker.next(data);
      }
     );
    return this.data;

  }
}
