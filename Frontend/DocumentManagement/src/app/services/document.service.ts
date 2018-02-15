import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.prod';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Http, Headers, ResponseContentType, Response } from '@angular/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { CreateDocumentModel, DocumentModel } from '../shared/models/document.model';

@Injectable()
export class DocumentService {
  _documentsEndpoint = `${environment.apiUrlBase}/documents`;

  constructor(
    private http: HttpClient,
    private router: Router) {
   }

   create(documentInfo): Observable<any> {
       const documentData: CreateDocumentModel = {
         description: documentInfo.description
       };

       return this.http.post(`${this._documentsEndpoint}/create`, documentData, {
        headers: new HttpHeaders().set(
          'Content-Type',
          'application/json;charset=utf-8;'
        ),
        responseType: 'text'
       });
   }

  //  postFile(fileToUpload: FormData): Observable<any> {

  //   return this.http
  //     .post(`${environment.apiUrlBase}/files`, fileToUpload, {
  //       headers: {
  //         'Accept': 'application/json;charset=utf-8;'
  //       }
  //     });
  //   }

    postFile(fileToUpload: FormData, documentInfo): Observable<any> {
      const documentData: CreateDocumentModel = {
        description: documentInfo.value.description
      };

      fileToUpload.append('docInformation', new Blob([JSON.stringify(documentData)], {type: 'application/json'}));

      const params = new HttpParams();
      params.set('description', documentData.description);

      return this.http
        .post(`${environment.apiUrlBase}/files`, fileToUpload, {
          headers: {
            'Accept': 'application/json;charset=utf-8;'
          },
          params: params
        });
      }

   getAll(): Observable<any> {
     return this.http.get<DocumentModel>(`${this._documentsEndpoint}/getall`);
   }

   deleteDocument(id: number): Observable<any> {
     const params = new HttpParams().set('id', id.toString());
     return this.http.delete(`${this._documentsEndpoint}/remove`, {params: params});
   }

    saveFile(id): Observable<any> {
    //   const headers = new Headers({
    //     'Content-Type': 'application/json',
    //     'Accept': 'application/pdf'
    //  });
    const params = new HttpParams().set('id', id.toString());
  //  headers.append('Accept', 'arraybuffer');
    return this.http.get(`${environment.apiUrlBase}/files`, {
      responseType: 'blob',
      params: params
     });
    }

}
