import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';

import { LogoutGuard } from './shared/guards/logout.guard';
import { LoginGuard } from './shared/guards/login.guard';
import { AdminGuard } from './shared/guards/admin.guard';

import { LoginComponent } from './login/login.component';
import { ValuesComponent } from './values/values.component';
import { RegisterComponent } from './register/register.component';
import { DocumentsComponent } from './documents/documents.component';

const routes: Routes = [
{
  path: 'values',
  component: ValuesComponent,
  canActivate: [LoginGuard]
},
{
  path: 'documents',
  component: DocumentsComponent,
  canActivate: [LoginGuard]
},
{
  path: 'register',
  component: RegisterComponent,
  canActivate: [LoginGuard, AdminGuard]
 },
  {
    path: 'login',
    canActivate: [LogoutGuard],
    component: LoginComponent
  },
  {
    path: '**',
    redirectTo: '/documents',
    canActivate: [LoginGuard]
  },
  {
    path: '',
    redirectTo: '/documents',
    pathMatch: 'full',
    canActivate: [LoginGuard]
  },
  {
    path: '**',
    redirectTo: '/login',
    canActivate: [LogoutGuard]
  },
  {
    path: '',
    redirectTo: '/login',
    pathMatch: 'full',
    canActivate: [LogoutGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
