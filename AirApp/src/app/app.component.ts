import { AfterViewInit, Component, OnInit } from '@angular/core';
import { NgxNotifierService } from 'ngx-notifier';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, AfterViewInit {
  title = 'AirApp';

constructor(private notifier:NgxNotifierService){}
  ngAfterViewInit(): void {
    // const main = document.getElementById('main');
    // const header = document.getElementById('header-nav');
    //  main.style.top = '4.6rem';

   // main.style.height = (window.screen.height - 90).toString();
    //   window.screen.height - Number.parseInt(header.style.height)
    // ).toString();
  }

  ngOnInit() {

  }
}
