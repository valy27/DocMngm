import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { JwtHelperService } from '@auth0/angular-jwt';

import { environment } from '../../environments/environment.prod';

import { LoginModel } from '../shared/models/login.model';

@Injectable()
export class LoginService {
  _loginEndpoint = `${environment.apiUrlBase}/accounts/login`;

  constructor(
    private http: HttpClient,
    private router: Router,
    private jwtHelperService: JwtHelperService
  ) {}

  login(loginInfo): Observable<any> {

       const credentials: LoginModel = {
           userName: loginInfo.userName,
           password: loginInfo.password
       };

      return this.http.post(this._loginEndpoint, credentials, {
          headers: new HttpHeaders().set(
              'Content-Type',
              'application/json;charset=utf-8;'
          ),
          responseType: 'text'
      });
  }

  logout() {
      this.deleteToken();
      this.router.navigate(['/login']);
  }

  loggedIn() {
      const token = this.getToken();
      if (!token) {
          return false;
      }

      const expired = this.jwtHelperService.isTokenExpired(token);
      return !expired;
  }

  saveToken(tokenData) {
      const data = JSON.parse(tokenData);
      localStorage.setItem(environment.userRole, data.userRole);
      localStorage.setItem(environment.accessToken, data.auth_token);
  }

  getToken(): string {
    return localStorage.getItem(environment.accessToken);
  }

  getUserRole(token?) {
    return localStorage.getItem(environment.userRole);
  }

  deleteToken() {
    localStorage.removeItem(environment.userRole);
    localStorage.removeItem(environment.accessToken);
  }

}
