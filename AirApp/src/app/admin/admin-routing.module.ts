
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from './admin.component';
import { AuthGuard } from '../services/auth.guard';

const routes: Routes = [
  {path:'',redirectTo:'home',pathMatch:'full'},
  {path:'home',component:AdminComponent},

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
exports: [RouterModule]
})

export class AdminRoutingModule { }
