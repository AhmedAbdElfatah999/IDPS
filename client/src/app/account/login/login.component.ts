import { Route } from '@angular/compiler/src/core';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  constructor(private accountService: AccountService, private router: Router) {}

  ngOnInit(): void {
    this.createloginForm();
    this.accountService.loadCurrentUser(this.accountService.MyToken);
  }

  // tslint:disable-next-line: typedef
  createloginForm() {
    this.loginForm = new FormGroup({
      email: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
      rememberMe:new FormControl('',Validators.required)
    });
  }
  // tslint:disable-next-line: typedef
  onSubmit() {
    this.accountService.login(this.loginForm.value).subscribe(
      () => {
        console.log('user logged in');
        this.accountService.checkLoginStatus;
        localStorage.setItem('token', this.accountService.MyToken);
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
