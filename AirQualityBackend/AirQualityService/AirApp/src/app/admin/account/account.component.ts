import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgxNotifierService } from 'ngx-notifier';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  checkform(form: NgForm) {
    if (!form.value.username) return false;
    if (form.value.username.length < 8 ||
      form.value.newPassword.length < 8) {

      return false;
    }
    if (form.value.newPassword !== form.value.confirmPassword) {

      return false;
    }
    if (form.value.oldPassword.length < 3) {
      return false;
    }
    return true;
  }

  updateAccount(form: NgForm) {
    this.authService.updateAccount(form);
  }

  isShowNewPassword = false;
  isShowConfirmPassword = false;
  isShowOldPassword = false;

  toggleShowNewPassword() {
    this.isShowNewPassword = !this.isShowNewPassword;
  }
  toggleShowConfirmPassword() {
    this.isShowConfirmPassword = !this.isShowConfirmPassword;
  }
  toggleShowOldPassword() {
    this.isShowOldPassword = !this.isShowOldPassword;
  }
}
