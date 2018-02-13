import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs/Observable';

import { environment } from '../../environments/environment';

@Injectable()
export class TokenExpirationInterceptor implements HttpInterceptor {
  constructor(
    private router: Router,
    private jwtHelperService: JwtHelperService
  ) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const token = localStorage.getItem(environment.accessToken);
    if (token) {
      const expired = this.jwtHelperService.isTokenExpired(token);
      if (expired) {
        localStorage.removeItem(environment.accessToken);
        this.router.navigate(['login']);
      } else {
        request = request.clone({
           setHeaders: {
            Authorization: `Bearer ${token}`
           }
        });
      }
    } else {
      this.router.navigate(['login']);
    }
    return next.handle(request);
  }
}
