import { Component, OnInit } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';
import { Point } from '../../models/point';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { PointService } from 'src/app/services/point.service';
import { NgxNotifierService } from 'ngx-notifier';


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  showEditModal = false;
  showResultAdd=false;
  resultAdd:any={
    deviceId:'',
    token:''
  }
  selectedPoint:any ={};
  addPoint:Point=new Point();
  listCities:any;

  settings = {
    columns: {
      pointId: {
        title: 'ID',
        width: '10%',
        class: 'col-title',
        type: 'html'
      },
      nameLocation: {
        title: 'Location',
        width: '20%',
        class: 'col-title'
      },
      address: {
        title: 'Address',
        width: '30%'
      },
      cityName: {
        title: 'City',
        width: '10%'
      },
      lat: {
        title: 'Latitude',
        width: '10%'
      },
      lng: {
        title: 'Longitude',
        width: '10%'
      }
    },
    actions: {
      columnTitle: '',
      edit: true,
      delete: true,
      position: 'right',
      add: false
    },
    edit: {
      editButtonContent: '<i class="fas fa-edit "aria-hidden="true"></i>'
    },
    delete: {
      deleteButtonContent: '<i class="fa fa-trash" aria-hidden="true"></i>'
    },
    noDataMessage: 'Not data found',
    filter: {
      inputClass: 'filter-class'
    },
    mode: 'external',
    pager: {
      perPage: 5
    }
  }
  source: LocalDataSource;

  constructor(private pointService:PointService,private ngxNotifierService:NgxNotifierService, private http: HttpClient) {
   pointService.getPointList().subscribe((data:any[])=>{
     console.log(data);
  this.source = new LocalDataSource(data);
   })

  }

  ngOnInit(): void {
this.getNameCities();
  }
  onSearch(query: string = '') {
    this.source.setFilter([
      {
        field: 'id',
        search: query
      },
      {
        field: 'location',
        search: query
      },
      {
        field: 'address',
        search: query
      },
      {
        field: 'city',
        search: query
      }
      ,
      {
        field: 'lat',
        search: query
      }
      ,
      {
        field: 'lng',
        search: query
      }

    ], false);
  }

  onDelete(event) {

    console.log(event.data);
    //  // Delete item from array
    //  let index = this.data.indexOf(event.data);
    //  console.log(index);
    //  this.data.splice(index, 1);

    //  // Update the array in the service class


    //  // Update the local datasource
    //  this.source = new LocalDataSource(this.data);
    this.pointService.removePoint(event.data.pointId).subscribe(data=>{
      console.log(data);
    });
    this.source.remove(event.data);
  }
  pointTemp:any;
  onEdit(e) {
    this.selectedPoint = e.data;
    this.pointTemp=this.selectedPoint;
    console.log(e.data);
    this.showEditModal = true;
  }
  Edit() {

    this.source.update(this.pointTemp,this.selectedPoint);
    this.pointService.updatePoint(this.selectedPoint,this.selectedPoint.pointId).subscribe(
      result=>{
        this.ngxNotifierService.createToast('update succes!','success');
      },error=>{
        this.ngxNotifierService.createToast('Update error','danger');
      }
    )
  }
  onCloseEdit() {
    this.showEditModal = false;
  }
  getLocation() {
    navigator.geolocation.getCurrentPosition((position) => {

      (
        this.selectedPoint.lat = position.coords.latitude.toString(),
        this.selectedPoint.lng = position.coords.longitude.toString()
      )
        ,
        (error: any) => {

          console.log('not read location' + error);
        },
        { timeout: 20000 };
    });
  }
  getLocationAdd() {
    navigator.geolocation.getCurrentPosition((position) => {

      (
        this.addPoint.lat = position.coords.latitude.toString(),
        this.addPoint.lng = position.coords.longitude.toString()
      )
        ,
        (error: any) => {

          console.log('not read location' + error);
        },
        { timeout: 20000 };
    });
  }

  getNameCities() {
    this.http.get(environment.uri + '/api/City/listName').subscribe((data: JSON) => {
      console.log(data);
      this.listCities=data;
    }, error => {
      console.error(error)
    })
  }
  onAddPoint(){
if((!this.isFloat(this.addPoint.lat))||(!this.isFloat(this.addPoint.lng))||this.addPoint.lat.length===0||this.addPoint.lng.length===0){
  this.ngxNotifierService.createToast('Latitude or Longitude must be number','danger');
  return;
}
if((this.addPoint.pointName.length===0)){
  this.ngxNotifierService.createToast('Location Name can\'t not empty','danger');
  return;
}
if((this.addPoint.city.length===0)){
  this.ngxNotifierService.createToast('City can\'t not empty','danger');
  return;
}

   let object={
      pointId: '',
      cityName: this.addPoint.city,
      nameLocation: this.addPoint.pointName,
      address: this.addPoint.pointAddress,
      lat: this.addPoint.lat,
      lng:this.addPoint.lng
    }
    this.pointService.addPoint(object).subscribe((data:any)=>{
      this.source.add(data.point);
      let messIbm=data.ibmResult;
      let jsonMess=JSON.parse(messIbm);
      console.log(messIbm);
      console.log(jsonMess);
      console.log(jsonMess[0].success)
      if(jsonMess[0].success){
        this.resultAdd= {
          deviceId:jsonMess[0].deviceId,
          token:jsonMess[0].authToken
        }
        this.showResultAdd=true;
        this.ngxNotifierService.createToast(' Registered device!','success');
      }else{
        this.ngxNotifierService.createToast(' Error register device!','danger');
      }

      console.log(jsonMess);
      console.log(data);
    })
  }
  copied(event,type){
    console.log(event);
this.ngxNotifierService.createToast(type+ ' copied!','info');
  }
  copyerror(event,type){
    console.log(event);
    this.ngxNotifierService.createToast(type+'copy error!','danger');
  }
  notifiertion(){
    this.ngxNotifierService.createToast('jhnj','info');
  }
  closeResultAdd(){
    this.showResultAdd=false;
  }
  isFloat(n) {
    return parseFloat(n.match(/^-?\d*(\.\d+)?$/))>0;
}
}
