import { ActivatedRoute, Router } from '@angular/router';
import { Marker } from './../../../models/marker';
import { MarkerService } from './../../../services/marker.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { GoogleMap } from '@angular/google-maps';


@Component({
  selector: 'app-google-map-home',
  templateUrl: './google-map-home.component.html',
  styleUrls: ['./google-map-home.component.css']
})

export class GoogleMapHomeComponent implements OnInit {
@ViewChild('googlemap',{static:true}) public googleMap :GoogleMap;


  zoom = 12;
  center: google.maps.LatLngLiteral;

  options: google.maps.MapOptions = {
    //  mapTypeId: 'hybrid',
    zoomControl: true,
    scrollwheel: true,
    disableDoubleClickZoom: true,
    mapTypeControl: true,
    maxZoom: 15,
    minZoom: 5
  };


  optionsMarker: google.maps.MarkerOptions = {
    animation: google.maps.Animation.DROP

  };

  icons: Record<string, { icon: string }> = {
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

  constructor(private markerService: MarkerService,private router:Router,private route:ActivatedRoute) {

  }

  ngOnInit() {

    this.getLocation();

    this.getMarker();

  }

  getLocation() {
    navigator.geolocation.getCurrentPosition((position) => {
      (this.center = {
        lat: position.coords.latitude,
        lng: position.coords.longitude,
      }),
        (error: any) => {

          console.log('not read location' + error);
        },
        { timeout: 10000 };
    });
  }
  getMarker() {

    this.markerService.getMarker().subscribe((data: Marker[]) => {

      data.forEach(element => {
        const type = this.getAirType(element.AQI);

        const marker = new google.maps.Marker({
          position: new google.maps.LatLng(element.lat, element.lng),
          icon: {
            url: this.icons[type].icon,
            scaledSize:new google.maps.Size(46,43),

          },
          label: {
            color: 'white'
            , text: (element.AQI).toString(),

          },
          map:this.googleMap.googleMap


        });
        marker.set('id',element.idMarker);
        marker.addListener('click',()=>{
          this.router.navigate(['/detail-marker'],{ queryParams: {id:marker.get('id')} });
        });
      });
    });
  }

  getAirType(x: number) {
    if (x <= 50) {
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



}
