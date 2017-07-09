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
  // login error message
  errorMessage: string;

  constructor(private http: Http, private route: Router) { }

  ngOnInit() {
  }

  onSubmit(value: ISignIn) {
    this.errorMessage = '';

    // login
    this.http.post(env.apiBaseAddress + 'api/signin', value, {withCredentials: true}).subscribe(res => {
        // success
        console.log(res.json().userName);
        this.route.navigate(['/projects']);
    }, error => {
        // fail
        this.errorMessage = 'Invalid username or password.';
        console.log(error);
    });
  }
}

interface ISignIn {
  userName: string;
  password: string;
}
