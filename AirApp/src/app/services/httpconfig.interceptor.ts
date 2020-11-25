import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class HttpconfigInterceptor implements HttpInterceptor {

  constructor() {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token: string = localStorage.getItem('jwt');

request = request.clone({ headers: request.headers.set('Authorization', 'Bearer ' + token) });

request = request.clone({ headers: request.headers.set('Content-Type', 'application/json') });

request = request.clone({ headers: request.headers.set('Accept', 'application/json') });
    return next.handle(request);
  }
}
