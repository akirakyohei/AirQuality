import { Injectable } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { Router } from '@angular/router';
import { RsaCypherService } from './rsa-cypher.service';
import { NgxNotifierService } from 'ngx-notifier';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  invalidLogin = true;
  constructor(private http: HttpClient, private router: Router, private rsa: RsaCypherService, private ngxNotifierService: NgxNotifierService,) { }
  login(form: NgForm) {

    // form.setValue({ 'password': this.rsa.encrypt(form.value.password) });
    var obj = {
      username: form.value.username,
      password: this.rsa.encrypt(form.value.password)
    }
    const credential = JSON.stringify(obj);
    const url = '/api/Auth/login';
    this.http.post(environment.uri + url, credential, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    }).subscribe(response => {
      const token = (response as any).token;
      localStorage.setItem('jwt', token);
      this.invalidLogin = false;
      this.ngxNotifierService.createToast("Login success", "success", 3000);
      setTimeout(() => {
        this.router.navigate(['/admin']), 2000
      })


    }, err => {
      this.invalidLogin = true;
      this.ngxNotifierService.createToast("Login failed", "danger", 3000);
    })
  }

  logOut() {
    var url = '/api/Auth/logout';
    this.http.get(url);
    localStorage.removeItem('jwt');
  }

  updateAccount(form: NgForm) {
    var obj = {
      username: form.value.username,
      password: this.rsa.encrypt(form.value.newPassword),
      lastPassword: this.rsa.encrypt(form.value.oldPassword)
    }
    const credential = JSON.stringify(obj);

    const url = '/api/Auth/updateAccount';
    this.http.put(environment.uri + url, credential, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    }).subscribe(response => {
      console.log(response)
      this.invalidLogin = true;
      localStorage.removeItem('jwt');
      this.ngxNotifierService.createToast("Update success", "success", 3000);
      setTimeout(() => {
        this.router.navigate(['/login']);
      }, 2000)
    }, err => {
      console.log(err);
    })
  }
}
