import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  login(form: NgForm) {
    this.authService.login(form);
  }

  isShowPassword = false;
  toggleShowPassword() {
    this.isShowPassword = !this.isShowPassword;
  }

}
