import { Component, OnInit } from '@angular/core';
import { AuthService } from "../services/auth.service";
import { Router } from "@angular/router";

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  private _opened: boolean = false;
constructor(private authService:AuthService,private router:Router){}
 _toggleSidebar() {
    this._opened = !this._opened;
  }

  ngOnInit(): void {
  }
logout(){
  this.authService.logOut();
  this.router.navigate(['/']);

}

}
