import { Component, Input, OnInit } from '@angular/core';
import { LogDeviceService } from 'src/app/services/log-device.service';

@Component({
  selector: 'app-log',
  templateUrl: './log.component.html',
  styleUrls: ['./log.component.css']
})
export class LogComponent implements OnInit {

  @Input() data: any;
  @Input() id: any;

  constructor(private logService: LogDeviceService) { }

  ngOnInit() {

  }

  isEmpty() {
    if (this.data && this.data.length > 0) {
      return true;
    }
    return false;
  }

  refresh() {
    if (this.id.length > 0) {
      this.logService.LogConnection(this.id).subscribe(data => {

        console.log(data.log);
        this.data = JSON.parse(data.log.result);
      }, err => {
        console.log(err);
      });
    }
  }


}
