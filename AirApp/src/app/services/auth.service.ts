import { Injectable } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
invalidLogin=true;
  constructor(private http:HttpClient,private router:Router) { }
  login(form:NgForm){
    const credential= JSON.stringify(form.value);
    const url='/api/Auth/login';
this.http.post(environment.uri+url,credential,{
  headers:new HttpHeaders({'Content-Type':'application/json'})
}).subscribe(response=>{
  const token = (response as any).token;
  localStorage.setItem('jwt',token);
  this.invalidLogin=false;
  this.router.navigate(['/admin']);
},err=>{
  this.invalidLogin= true;
})
  }

  logOut(){
    localStorage.removeItem('jwt');
  }
}
