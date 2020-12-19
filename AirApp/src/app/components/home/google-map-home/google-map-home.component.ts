import { PlacesService } from './../../../services/places.service';
import { environment } from './../../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ColorAqiService } from './../../../services/color-aqi.service';
import { PointAir } from './../../../models/point-air';
import { ReportAqiService } from 'src/app/services/report-aqi.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';
import { GoogleMap } from '@angular/google-maps';
import { catchError, timeout } from 'rxjs/operators';
import { of } from 'rxjs';
import { isUndefined } from 'util';



@Component({
  selector: 'app-google-map-home',
  templateUrl: './google-map-home.component.html',
  styleUrls: ['./google-map-home.component.css']
})

export class GoogleMapHomeComponent implements OnInit {
  @ViewChild('googlemap', { static: true }) public googleMap: GoogleMap;


  zoom = 12;
  center: google.maps.LatLngLiteral;

  options: google.maps.MapOptions = {
    //  mapTypeId: 'hybrid',
    zoomControl: true,
    scrollwheel: true,
    disableDoubleClickZoom: true,
    mapTypeControl: true,
    maxZoom: 15,
    minZoom: 5,
    center: new google.maps.LatLng(21.039005939041772, 105.83822167275451)
  };


  optionsMarker: google.maps.MarkerOptions = {
    animation: google.maps.Animation.DROP

  };
  markerLocal: any;

  icons: Record<string, { icon: string }> = {
    default: {
      icon: '../../../../assets/img/aqi/default.png',
    },
    good: {
      icon: '../../../../assets/img/aqi/good.png',
    },
    moderate: {
      icon: '../../../../assets/img/aqi/moderate.png',
    },
    unhealthyForGroups: {
      icon: '../../../../assets/img/aqi/unhealthyForGroups.png',
    },
    unhealthy: {
      icon: '../../../../assets/img/aqi/unhealthy.png',
    },
    veryunhealthy: {
      icon: '../../../../assets/img/aqi/veryunhealthy.png',
    },
    hazardous: {
      icon: '../../../../assets/img/aqi/hazardous.png',
    }
  };

  height = '90vh';
  markerLocalLocal: any;

  constructor(private reportAqiService: ReportAqiService,
    private router: Router,
    private route: ActivatedRoute,
    private colorAqiService: ColorAqiService,
    private placesService: PlacesService) {

  }

  ngOnInit() {

    this.getLocation();

    this.getMarker();

  }

  getLocation() {
    navigator.geolocation.getCurrentPosition((position) => {

      this.center = {
        lat: position.coords.latitude,
        lng: position.coords.longitude,
      }
      this.getMarkerLocation();

    },
      (error: any) => {

        console.log('not read location' + error);
      },
      // tslint:disable-next-line: no-unused-expression
      { timeout: 20000 }
    )
  }
  getMarkerLocation() {
    if (isUndefined(this.center))
      return;
    this.markerLocal = new google.maps.Marker({
      position: this.center,
      draggable: true,
      animation: google.maps.Animation.DROP,
      title: '',
      map: this.googleMap.googleMap
    });
    this.markerLocal.addListener('click', this.toggleBounce);
  }
  toggleBounce() {
    if (this.markerLocalLocal.getAnimation() !== null) {
      this.markerLocal.setAnimation(null);
    } else {
      this.markerLocal.setAnimation(google.maps.Animation.BOUNCE);
    }
  }

  getMarker() {

    this.reportAqiService.getInfoPointAir().subscribe((data: PointAir[]) => {

      data.forEach(element => {
        const type = this.colorAqiService.getAirType(element.aqi);
        const marker = new google.maps.Marker({
          position: new google.maps.LatLng(Number.parseFloat(element.lat), Number.parseFloat(element.lng)),
          icon: {
            url: this.icons[type].icon,
            scaledSize: new google.maps.Size(35, 32),

          },
          label: {
            color: 'white'
            , text: element.aqi > 0 ? element.aqi.toString() : '---',
          },
          title: element.pointName
          ,
          map: this.googleMap.googleMap


        });
        marker.set('id', element.pointId);
        marker.addListener('click', () => {

          this.placesService.getCityIdComposePoint(element.pointId).pipe(timeout(2000), catchError(() => { return of(null) })).subscribe(data => {
            console.log(data);
            this.router.navigate(['/detail-marker'], { queryParams: { id: marker.get('id'), namePoint: element.pointName, cityId: data.cityId } });

          })
        }
        );
      });
    });
  }
}




