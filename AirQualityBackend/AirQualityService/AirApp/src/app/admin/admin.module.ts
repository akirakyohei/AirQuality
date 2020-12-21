import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AdminComponent } from './admin.component';
import { AccountComponent } from './account/account.component';
import { AdminRoutingModule } from './admin-routing.module';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule } from '@angular/forms';
import { ClipboardModule } from 'ngx-clipboard';
import { NgxNotifierModule, NgxNotifierService } from 'ngx-notifier';
import { SidebarModule } from 'ng-sidebar';
import { PointService } from '../services/point.service';
import { AuthService } from '../services/auth.service';
import { LogComponent } from './log/log.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { GoogleMapsModule } from '@angular/google-maps';




@NgModule({
  declarations: [DashboardComponent, AdminComponent, AccountComponent, LogComponent],
  imports: [

    CommonModule,
    AdminRoutingModule,
    Ng2SmartTableModule,
    NgSelectModule,
    GoogleMapsModule,
    FormsModule,
    ClipboardModule,
    NgxNotifierModule,
    NgxPaginationModule,
    SidebarModule.forRoot()

  ],
  providers: [PointService, NgxNotifierService, AuthService],
  exports: [AdminRoutingModule]
})
export class AdminModule { }
