import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

import { LoginService } from './services/login.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'app';
  loggedIn: boolean;

  constructor(
    private loginService: LoginService,
    private router: Router) {}

    ngOnInit() {
    //  moment().utc();
      this.router.events.subscribe(event => {
        if (!(event instanceof NavigationEnd)) {
          return;
        }
        this.loggedIn = this.loginService.loggedIn();
        window.scrollTo(0, 0);
      });
    }
}
