import { MarkerService } from './services/marker.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { GoogleMapsModule } from '@angular/google-maps';
import { HttpClientModule } from '@angular/common/http';
import { NgxGaugeModule } from 'ngx-gauge';
import { NgxChartsModule } from '@swimlane/ngx-charts';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { FooterComponent } from './components/common/footer/footer.component';
import { HeaderComponent } from './components/common/header/header.component';
import { PlacesComponent } from './components/places/places.component';
import { AboutComponent } from './components/about/about.component';
import { InformationComponent } from './components/information/information.component';
import { LoginComponent } from './components/login/login.component';
import { GoogleMapHomeComponent } from './components/home/google-map-home/google-map-home.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { DetailMarkerComponent } from './components/detail-marker/detail-marker.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    FooterComponent,
    HeaderComponent,
    PlacesComponent,
    AboutComponent,
    InformationComponent,
    LoginComponent,
    GoogleMapHomeComponent,
    PageNotFoundComponent,
    DetailMarkerComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    GoogleMapsModule,
    HttpClientModule,
    NgxGaugeModule,
    NgxChartsModule
  ],
  providers: [MarkerService],
  bootstrap: [AppComponent]
})
export class AppModule { }
