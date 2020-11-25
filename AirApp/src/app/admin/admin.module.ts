import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AdminComponent } from './admin.component';
import { AdminRoutingModule } from './admin-routing.module';
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule } from '@angular/forms';
import { ClipboardModule } from 'ngx-clipboard';
import { NgxNotifierModule,NgxNotifierService } from 'ngx-notifier';
import { SidebarModule } from 'ng-sidebar';
import { PointService } from '../services/point.service';




@NgModule({
  declarations: [DashboardComponent, AdminComponent],
  imports: [

CommonModule,
    AdminRoutingModule,
    Ng2SmartTableModule,
    NgSelectModule,
    FormsModule,
    ClipboardModule,
   NgxNotifierModule,
   SidebarModule.forRoot()

  ],
  providers: [PointService,NgxNotifierService],
  exports: [AdminRoutingModule]
})
export class AdminModule { }
