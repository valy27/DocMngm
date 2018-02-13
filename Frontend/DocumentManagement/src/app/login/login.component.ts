import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import {LoginService} from '../services/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  errorMessage;
  loginInfo: FormGroup = new FormGroup({
    userName: new FormControl(null, Validators.required),
    password: new FormControl(null, Validators.required)
  });
  hide = true;

  constructor(
    private loginService: LoginService,
    private router: Router
  ) {}

  ngOnInit() {
  }

  login() {
    this.loginService.login(this.loginInfo.value).subscribe(
      result => {
        this.loginService.saveToken(result);
        this.router.navigate(
          this.redirectUser(this.loginService.getUserRole())
        );
      },
      err => {
        console.log(err.error);
      }
    );
  }

  private redirectUser(userRole: string) {
    if (userRole === 'Admin') {
      return ['/documents'];
    }
    return ['/'];
   }

}
