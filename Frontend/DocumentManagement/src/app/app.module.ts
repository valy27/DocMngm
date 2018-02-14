import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';

import { AppRoutingModule } from './app-routing.module';

import { LoginService } from './services/login.service';
import { RegisterService } from './services/register.service';

import { AppComponent } from './app.component';
import {SharedModule} from '../app/shared/shared.module';
import { ValuesComponent } from './values/values.component';
import { LoginComponent } from './login/login.component';

import { environment } from '../environments/environment';
import { HeaderComponent } from './header/header.component';
import { RegisterComponent } from './register/register.component';
import { DocumentsComponent } from './documents/documents.component';
import { DocumentService } from './services/document.service';
import { DialogComponent } from './documents/dialog/dialog.component';


@NgModule({
  declarations: [
    AppComponent,
    ValuesComponent,
    LoginComponent,
    HeaderComponent,
    RegisterComponent,
    DocumentsComponent,
    DialogComponent
  ],
  imports: [
    BrowserModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: () =>
          localStorage.getItem(environment.accessToken),
        whitelistedDomains: [environment.apiDomain]
      }
    }),
    SharedModule.forRoot(),
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [LoginService, RegisterService, DocumentService],
  bootstrap: [AppComponent],
  entryComponents: [DialogComponent]
})
export class AppModule { }
