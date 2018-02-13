import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms'

import { Router } from '@angular/router';

import { LoginService } from '../services/login.service';
import { RegisterService } from '../services/register.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerInfo: FormGroup = new FormGroup({
    userName: new FormControl(null, Validators.required),
    firstName: new FormControl(null, Validators.required),
    lastName: new FormControl(null, Validators.required),
    age: new FormControl(null, Validators.required),
    password: new FormControl(null, Validators.required),
    role: new FormControl(null, Validators.required),
  });
  constructor(
    private registerService: RegisterService,
    private loginService: LoginService,
    private router: Router
  ) { }

  ngOnInit() {
    this.registerInfo.reset();
  }

  register() {
   this.registerService.register(this.registerInfo.value).subscribe(
     result => {
      this.registerInfo.reset();
      this.registerInfo.setErrors(null);
     }
   );
  }

}
