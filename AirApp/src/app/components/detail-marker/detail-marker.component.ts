import { Report } from './../../models/report';
import { ColorAqiService } from './../../services/color-aqi.service';
import { PointAirDetail } from './../../models/point-air-detail';
import { ActivatedRoute, Params } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { ReportAqiService } from 'src/app/services/report-aqi.service';
import { PointAir } from 'src/app/models/point-air';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-detail-marker',
  templateUrl: './detail-marker.component.html',
  styleUrls: ['./detail-marker.component.css'],
})
export class DetailMarkerComponent implements OnInit {
  // line chart

  private timer1: any;
  private timer2: any;
  pointInCity: PointAir[];
  pointInCityObserve: any;
  reportAqi = new Array();
  reportAqiObserve: any;
  infoAir: PointAirDetail;
  infoAirObserve: any;
  airIndex: AirIndex[];
  date: any;
  namePoint: string;
  selectedReport = 1;
  reports = [
    { id: 1, name: 'Daily' },
    { id: 2, name: 'Hours' }
  ]





  // line gauge
  airOptions = {
    type: 'arch',
    size: 200,
    min: 0,
    max: 500,
    cap: 'round',
    thick: 10,
    label: 'Air Quality',
    // foreground_color:'green',
    thresholds: {
      0: { color: '#1A93D9' },
      1: { color: '#23DA27' },
      50: { color: '#FFFD38' },
      100: { color: '#FC7D23' },
      150: { color: '#FC0D1B' },
      200: { color: '#97084D' },
      300: { color: '#7C0425' },
    },
  };
  airIndexthresholds = [
    { value: '0', color: '#1A93D9' },
    { value: '1', color: '#23DA27' },
    { value: '50', color: '#FFFD38' },
    { value: '100', color: '#FC7D23' },
    { value: '150', color: '#FC0D1B' },
    { value: '200', color: '#97084D' },
    { value: '300', color: '#7C0425' },
  ]


  constructor(private activatedRoute: ActivatedRoute, private reportAqiService: ReportAqiService, private colorAqiService: ColorAqiService, private datePipe: DatePipe) {

  }

  ngOnInit(): void {
    this.setInitialInfo();
    this.getPointInCity();
    this.getAqiInhourByPointId();
    this.timer2 = setInterval(() => {
      this.getAqiInhourByPointId();
    }, 5 * 60 * 1000);
    this.date = Date.now();
    this.timer1 = setInterval(() => {
      this.date = Date.now();
    }, 1000);
  }

  setInitialInfo() {
    this.activatedRoute.queryParams.subscribe((params: Params) => {
      const idPoint = params['id'];
      this.namePoint = params['namePoint'];
      this.infoAirObserve = this.reportAqiService.getInfoAirCurrentByPointId(idPoint).subscribe((data: PointAirDetail) => {
        this.infoAir = data;
        this.airIndex = [
          {
            min: 0,
            max: 500,
            value: this.infoAir != undefined ? this.infoAir.co : 0,
            name: 'CO',
            unit: 'µg/m³',
          },
          {
            min: 0,
            max: 500,
            value: this.infoAir != undefined ? this.infoAir.sO2 : 0,
            name: 'SO2',
            unit: 'µg/m³',
          },
          {
            min: 0,
            max: 500,
            value: this.infoAir != undefined ? this.infoAir.o3 : 0,
            name: 'O3',
            unit: 'µg/m³',
          },
          {
            min: 0,
            max: 500,
            value: this.infoAir != undefined ? this.infoAir.nO2 : 0,
            name: 'NO2',
            unit: 'µg/m³',
          },
          {
            min: 0,
            max: 500,
            value: this.infoAir != undefined ? this.infoAir.pM2_5 : 0,
            name: 'pm2.5',
            unit: 'µg/m³',
          },
          {
            min: 0,
            max: 500,
            value: this.infoAir != undefined ? this.infoAir.pM10_0 : 0,
            name: 'pm10.0',
            unit: 'µg/m³',
          }

        ]
      })
    });
  }

  getPointInCity() {
    this.activatedRoute.queryParams.subscribe((params: Params) => {

      const cityId = params['cityId'];
      this.pointInCityObserve = this.reportAqiService.getinfoPointAir(cityId).subscribe((data: any) => {
        this.pointInCity = data;
      });

    });
  }
  getAqiInhourByPointId() {

    this.activatedRoute.queryParams.subscribe((params: Params) => {

      const pointId = params['id'];



      this.reportAqiService.getAqiAirByPointId(pointId);
      this.reportAqiObserve = this.reportAqiService.reportAqiList.subscribe((data: PointAirDetail[]) => {

        while (this.reportAqi.length) {

          this.reportAqi.pop();
        }
        data.forEach(element => {
          console.log('hg')
          this.reportAqi.push({
            name: this.datePipe.transform(element.dateTime, 'yyyy/MM/dd HH:mm:ss')
            , value: [this.datePipe.transform(element.dateTime, 'yyyy/MM/dd HH:mm:ss'),
            element.aqiInHour]
          });
        });
        console.log(this.reportAqi);

      });
    });
  }

  gethealthy(value) {
    return this.colorAqiService.getAirHealthy(value);
  }
  getColorHealthy(value) {
    return this.colorAqiService.setColor(value);
  }

  ngOnDestroy() {
    clearInterval(this.timer1);
    clearInterval(this.timer2);
    this.infoAirObserve.unsubscribe();
    this.reportAqiObserve.unsubscribe();
    this.pointInCityObserve.unsubscribe();
  }
}


interface AirIndex {
  min: Number;
  max: Number;
  value: Number;
  name: string;
  unit: string;
}
