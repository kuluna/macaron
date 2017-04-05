import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Http, Headers, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';

import { environment as env } from '../../../environments/environment';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss']
})
export class SigninComponent implements OnInit {
  private signin = new FormGroup({
    email: new FormControl('email', Validators.email),
    password: new FormControl('password', Validators.required)
  });

  // login error message
  errorMessage: string;

  constructor(private http: Http, private route: Router) { }

  ngOnInit() {
  }

  onSubmit(value: ISignIn) {
    this.errorMessage = '';

    // login
    this.http.post(env.apiBaseAddress + 'api/user/signin', value).subscribe(res => {
        // success
        console.log(res.json().userName);
        this.route.navigate(['/projects']);
    }, error => {
        // fail
        this.errorMessage = 'Invalid email or password.';
        console.log(error);
    });
  }
}

interface ISignIn {
  email: string;
  password: string;
}
