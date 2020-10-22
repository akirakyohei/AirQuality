import { DetailMarkerComponent } from './components/detail-marker/detail-marker.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';

import { LoginComponent } from './components/login/login.component';
import { InformationComponent } from './components/information/information.component';
import { AboutComponent } from './components/about/about.component';
import { PlacesComponent } from './components/places/places.component';
import { HomeComponent } from './components/home/home.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {path: '', redirectTo: 'home', pathMatch: 'full'},
  { path: 'home', component: HomeComponent },
  { path: 'places', component: PlacesComponent },
  { path: 'about', component: AboutComponent },
  { path: 'information', component: InformationComponent },
  {path:'detail-marker',component:DetailMarkerComponent},
  { path: 'login', component: LoginComponent },
  {path: '**', component:PageNotFoundComponent}

];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
