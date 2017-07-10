import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Http, Headers, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';

import { environment as env } from '../../../../environments/environment';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss']
})
export class SigninComponent implements OnInit {
  submitting = false;
  // login error message
  errorMessage: string;

  constructor(private http: Http, private route: Router) { }

  ngOnInit() {
  }

  siginin(value: ISignIn) {
    this.submitting = true;
    this.errorMessage = '';

    // login
    this.http.post(env.apiBaseAddress + 'api/signin', value, { withCredentials: true }).subscribe(res => {
        // success
        this.route.navigate(['/projects']);
    }, error => {
        // fail
        console.log(error);
        this.errorMessage = 'Invalid username or password.';
        this.submitting = false;
    });
  }

  register(value: ISignUp) {
    this.submitting = true;

    this.http.post(env.apiBaseAddress + 'api/signup', value).subscribe(res => {
     // success
        this.route.navigate(['/projects']);
    }, error => {
        // fail
        console.log(error);
        this.submitting = false;
    });
  }
}

interface ISignIn {
  userName: string;
  password: string;
}

interface ISignUp extends ISignIn {
  email: string;
}
