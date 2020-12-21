
import { ColorAqiService } from './../../services/color-aqi.service';
import { PointAirDetail } from './../../models/point-air-detail';
import { ActivatedRoute, Params, Router } from '@angular/router';
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
    { id: 2, name: 'Daily' },
    { id: 1, name: 'Hours' }
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

  airIndexthresholds = [[
    { value: '0', color: '#1A93D9' },
    { value: '1', color: '#23DA27' },
    { value: '10000', color: '#FFFD38' },
    { value: '30000', color: '#FC7D23' },
    { value: '45000', color: '#FC0D1B' },
    { value: '60000', color: '#97084D' },
    { value: '90000', color: '#7C0425' },
  ], [
    { value: '0', color: '#1A93D9' },
    { value: '1', color: '#23DA27' },
    { value: '125', color: '#FFFD38' },
    { value: '350', color: '#FC7D23' },
    { value: '550', color: '#FC0D1B' },
    { value: '800', color: '#97084D' },
    { value: '1600', color: '#7C0425' },
  ], [
    { value: '0', color: '#1A93D9' },
    { value: '1', color: '#23DA27' },
    { value: '160', color: '#FFFD38' },
    { value: '200', color: '#FC7D23' },
    { value: '300', color: '#FC0D1B' },
    { value: '400', color: '#97084D' },
    { value: '800', color: '#7C0425' },
  ], [
    { value: '0', color: '#1A93D9' },
    { value: '1', color: '#23DA27' },
    { value: '100', color: '#FFFD38' },
    { value: '200', color: '#FC7D23' },
    { value: '700', color: '#FC0D1B' },
    { value: '1200', color: '#97084D' },
    { value: '2350', color: '#7C0425' },
  ], [
    { value: '0', color: '#1A93D9' },
    { value: '1', color: '#23DA27' },
    { value: '25', color: '#FFFD38' },
    { value: '50', color: '#FC7D23' },
    { value: '80', color: '#FC0D1B' },
    { value: '150', color: '#97084D' },
    { value: '3500', color: '#7C0425' },
  ]]

  maxValueIndex = [150000, 2630, 1200, 3850, 500, 600];


  constructor(private activatedRoute: ActivatedRoute, private reportAqiService: ReportAqiService, private colorAqiService: ColorAqiService, private datePipe: DatePipe, private router: Router) {

  }

  ngOnInit(): void {
    this.setInitialInfo();
    this.getPointInCity();
    this.getAqiByPointId();
    this.timer2 = setInterval(() => {
      this.getAqiByPointId();
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
  getAqiByPointId() {

    this.activatedRoute.queryParams.subscribe((params: Params) => {

      const pointId = params['id'];



      this.reportAqiService.getAqiAirByPointId(pointId);
      if (this.selectedReport == 1) {
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
      } else {
        this.reportAqiObserve = this.reportAqiService.reportAqiList.subscribe((data: any[]) => {

          while (this.reportAqi.length) {

            this.reportAqi.pop();
          }
          data.forEach(element => {
            console.log('hg')
            this.reportAqi.push({
              name: this.datePipe.transform(element.dateTime, 'yyyy/MM/dd HH:mm:ss')
              , value: [this.datePipe.transform(element.dateTime, 'yyyy/MM/dd HH:mm:ss'),
              element.aqi]
            });
          });
          console.log(this.reportAqi);

        });
      }

    });

  }

  gethealthy(value) {
    return this.colorAqiService.getAirHealthy(value);
  }
  getColorHealthy(value) {
    return this.colorAqiService.setColor(value);
  }

  onChangeReport() {
    console.log(this.selectedReport);
    this.reportAqiService.setUrlReportIndex(this.selectedReport - 1);
    this.getAqiByPointId();
  }

  onClickPoint(pointId: any) {
    this.activatedRoute.queryParams.subscribe((params: Params) => {
      const idPoint = params['id'];
      this.namePoint = params['namePoint'];
      const cityId = params['cityId'];
      this.router.navigate(['/detail-marker'], { queryParams: { id: pointId, namePoint: this.namePoint, cityId: cityId } });
    });

  }
  ngOnDestroy() {
    clearInterval(this.timer1);
    clearInterval(this.timer2);
    this.infoAirObserve.unsubscribe();
    this.reportAqiObserve.unsubscribe();
    this.pointInCityObserve.unsubscribe();
  }

  isBad() {
    return this.infoAir.aqiInHour >= 150;
  }
  isUnhealthy() {
    return this.infoAir.aqiInHour >= 100;
  }
  isMedum() {
    return this.infoAir.aqiInHour >= 50;
  }
}


interface AirIndex {
  min: Number;
  max: Number;
  value: Number;
  name: string;
  unit: string;
}
