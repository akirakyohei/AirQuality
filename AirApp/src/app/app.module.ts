import { ColorAqiService } from './services/color-aqi.service';
import { ReportAqiService } from './services/report-aqi.service';
import { PointService } from './services/point.service';
import { PlacesService } from './services/places.service';
import { BrowserModule } from '@angular/platform-browser';
import { LOCALE_ID, NgModule } from '@angular/core';
import { GoogleMapsModule } from '@angular/google-maps';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgxGaugeModule } from 'ngx-gauge';
import { JwtModule } from '@auth0/angular-jwt';
import { NgxProgressHttpModule } from '@kken94/ngx-progress';
import { AnimateOnScrollModule } from 'ng2-animate-on-scroll';
import { NgxEchartsModule } from 'ngx-echarts';

import { LazyLoadImageModule, LAZYLOAD_IMAGE_HOOKS, ScrollHooks } from 'ng-lazyload-image';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import localeVi from '@angular/common/locales/vi';
// tslint:disable-next-line: no-use-before-declare
registerLocaleData(localeVi);
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
import { LinearGaugeComponent } from './components/detail-marker/linear-gauge/linear-gauge.component';
import { LineChartComponent } from './components/detail-marker/line-chart/line-chart.component';
import { HttpconfigInterceptor } from "./services/httpconfig.interceptor";
import { DatePipe, registerLocaleData } from '@angular/common';
import { NgxNotifierModule, NgxNotifierService } from 'ngx-notifier';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { AdminModule } from './admin/admin.module';
import { AuthGuard } from './services/auth.guard';
import { AuthService } from './services/auth.service';
import { AdminRoutingModule } from './admin/admin-routing.module';
import { SidebarModule } from 'ng-sidebar';
import { RsaCypherService } from './services/rsa-cypher.service';
import { LogDeviceService } from './services/log-device.service';

export function tokenGetter() {
  return localStorage.getItem('jwt');
}


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
    DetailMarkerComponent,
    LinearGaugeComponent,
    LineChartComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    GoogleMapsModule,
    HttpClientModule,
    NgxGaugeModule,
    NgxProgressHttpModule,
    AnimateOnScrollModule.forRoot(),
    NgxEchartsModule.forRoot({ echarts: import('echarts') }),
    LazyLoadImageModule,
    NgSelectModule,
    FormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
      }
    }),
    AdminModule,
    AdminRoutingModule,
    Ng2SmartTableModule,
    NgxNotifierModule,
    SidebarModule.forRoot()
  ],
  providers: [{ provide: LAZYLOAD_IMAGE_HOOKS, useClass: ScrollHooks },
  { provide: HTTP_INTERCEPTORS, useClass: HttpconfigInterceptor, multi: true },
  { provide: LOCALE_ID, useValue: 'vi' },
    PlacesService,
    ReportAqiService,
    ColorAqiService,
    DatePipe,
    AuthService,
    PointService,
    NgxNotifierService,
    RsaCypherService,
    LogDeviceService,
    AuthGuard],
  bootstrap: [AppComponent],
})
export class AppModule { }
