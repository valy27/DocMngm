import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';

import { LoginService } from '../../services/login.service';

@Injectable()
export class LoginGuard implements CanActivate {

   constructor(private loginService: LoginService) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
       const loggedIn = this.loginService.loggedIn();
       if (!loggedIn) {
         this.loginService.logout();
       }
       return loggedIn;
  }
}
