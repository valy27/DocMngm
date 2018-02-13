import { Component, OnInit, HostListener } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

import { LoginService } from '../services/login.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  showRegister;
  isCollapsed: boolean;

  constructor(
    private loginService: LoginService,
    private router: Router
  ) { }

  ngOnInit() {
    this.checkRoles();
    this.router.events.subscribe(event => {
      this.checkRoles();
    });
  }

  checkRoles() {
    this.showRegister  = this.loginService.getUserRole() === 'Admin';
  }

  signOut() {
    this.loginService.logout();
  }

  @HostListener('window:resize', ['$event'])
  onResize(event) {
    this.isCollapsed = !(window.innerWidth <= 575);
  }
}
