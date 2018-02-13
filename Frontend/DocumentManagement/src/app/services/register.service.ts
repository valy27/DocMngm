import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';

import { environment } from '../../environments/environment.prod';

import { RegisterModel } from '../shared/models/register.model';

@Injectable()
export class RegisterService {
  _loginEndpoint = `${environment.apiUrlBase}/accounts/create`;

  constructor(
    private http: HttpClient,
    private router: Router
  ) {}

  register(registerInfo): Observable<any> {

       const registerData: RegisterModel = {
           userName: registerInfo.userName,
           firstName: registerInfo.firstName,
           lastName: registerInfo.lastName,
           age: registerInfo.age,
           role: registerInfo.role,
           password: registerInfo.password
       };

      return this.http.post(this._loginEndpoint, registerData, {
          headers: new HttpHeaders().set(
              'Content-Type',
              'application/json;charset=utf-8;'
          ),
          responseType: 'text'
      });
  }

}
