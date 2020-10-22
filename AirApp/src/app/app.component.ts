import { AfterViewInit, Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, AfterViewInit {
  title = 'AirApp';


  ngAfterViewInit(): void {
    const main = document.getElementById('main');
    const header = document.getElementById('nav-header');
    main.style.top = '68px';
   // main.style.height = (window.screen.height - 90).toString();
    //   window.screen.height - Number.parseInt(header.style.height)
    // ).toString();

  }

  ngOnInit() {

  }
}
