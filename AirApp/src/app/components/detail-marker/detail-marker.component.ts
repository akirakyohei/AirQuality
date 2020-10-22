
import { Component, OnInit } from '@angular/core';



@Component({
  selector: 'app-detail-marker',
  templateUrl: './detail-marker.component.html',
  styleUrls: ['./detail-marker.component.css']
})
export class DetailMarkerComponent implements OnInit {
  gaugeOptions ={
    type:'arch',
    size:200,
    min:0,
    max:500,
    cap:'round',
    thick:10,
    label:'Air Quality',
   // foreground_color:'green',
    thresholds:{
      0:{color:'#23DA27'},
      50:{color:'#FFFD38'},
      100:{color:'#FC7D23'},
      150:{color:'#FC0D1B'},
      200:{color:'#97084D'},
      300:{color:'#7C0425'},
    }

  }
  gaugeValue =300;

  lineargaugeOption={

    view:[200,200]
    ,scheme:['#ab3452'],
    min:0,
    max:500,
    unit:'μg/m³'
  }

  co2_value={
    value:300,
    title:'CO2'
  }



  constructor() { }

  ngOnInit(): void {

  }

}
