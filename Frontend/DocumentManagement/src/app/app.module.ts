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


@NgModule({
  declarations: [
    AppComponent,
    ValuesComponent,
    LoginComponent,
    HeaderComponent,
    RegisterComponent,
    DocumentsComponent
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
  providers: [LoginService, RegisterService],
  bootstrap: [AppComponent]
})
export class AppModule { }
