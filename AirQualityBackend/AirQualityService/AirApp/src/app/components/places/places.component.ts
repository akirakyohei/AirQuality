import { Router } from '@angular/router';
import { ColorAqiService } from './../../services/color-aqi.service';
import { PointAir } from './../../models/point-air';
import { PlacesService } from './../../services/places.service';
import { Component, OnInit} from '@angular/core';
import { City } from 'src/app/models/city';
import { ReportAqiService } from 'src/app/services/report-aqi.service';

@Component({
  selector: 'app-places',
  templateUrl: './places.component.html',
  styleUrls: ['./places.component.css']
})
export class PlacesComponent implements OnInit {
  cities: City[];
  reportAqi:PointAir[];
  nameCity:string='';
  private cityId='';
  constructor(private placesService: PlacesService,private reportAqiService:ReportAqiService,private colorAqiService:ColorAqiService,private router:Router) { }

  ngOnInit() {
    this.placesService.getCity().subscribe(
      (data) => {

        this.cities = data;
        console.log(this.cities);
      }
    )
    this.reportAqi = new Array();
  }

  provinceClick(id: string,nameCity:string) {
    this.nameCity=nameCity;
    this.reportAqiService.getinfoPointAir(id).subscribe((data:PointAir[])=>{
      this.reportAqi=data;
      this.cityId=id;
      console.log(this.reportAqi);
    },error=>{
      console.error(error);
    })
    let el= document.getElementById('point');
    el.scrollIntoView({behavior:'smooth',block:'center'});
  }

  getColorAqi(value:number){
   return this.colorAqiService.setColor(value);
  }
getPointInfoClick(id: string,nameCity:string){
  this.router.navigate(['/detail-marker'],{ queryParams: {id:id,namePoint:nameCity,cityId:this.cityId}});
}

}
