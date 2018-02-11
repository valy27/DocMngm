import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { environment } from '../../environments/environment';

import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import 'rxjs/add/operator/catch';


@Component({
  selector: 'app-values',
  templateUrl: './values.component.html',
  styleUrls: ['./values.component.scss']
})
export class ValuesComponent implements OnInit {
  private _valuesUrl = `${environment.apiUrlBase}/values`;
  values: string[] = [];

  constructor(private _http: HttpClient) {}

  ngOnInit() {
    this.getValues().subscribe(response => {
          this.values = response;
    });
  }

  getValues(): Observable<string[]> {
    return this._http.get<string[]>(this._valuesUrl).catch(this.handleError);
  }

  private handleError(err: HttpErrorResponse) {
    console.log(err.message);
    return Observable.throw(err.message);
  }
}
