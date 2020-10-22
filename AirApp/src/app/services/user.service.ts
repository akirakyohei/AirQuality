import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { User } from './../models/user';
import { BehaviorSubject, Observable, pipe } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private currentUserSubject = new BehaviorSubject<User>(new User());
  public currentUser = this.currentUserSubject.asObservable();
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  public isAuthenticated = this.isAuthenticatedSubject.asObservable();

  constructor(private http: HttpClient) {

  }

  setAuth(user: User) {
    this.currentUserSubject.next(user);
    this.isAuthenticatedSubject.next(true);
  }

  // attemptAuth(type, credentials): Observable<User>{
  //   let route = (type === 'login') ? '/login' : '';

  // }
  // getCurrentUser(): User{
  //   return this.currentUserSubject.value;
  // }
}
