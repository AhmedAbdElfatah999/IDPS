import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-doctor-login',
  templateUrl: './doctor-login.component.html',
  styleUrls: ['./doctor-login.component.scss'],
})
export class DoctorLoginComponent implements OnInit {
  doctorLoginForm: FormGroup;
  constructor(private accountService: AccountService, private router: Router) {}

  ngOnInit(): void {
    this.createloginForm();
    this.accountService.loadCurrentUser(localStorage.getItem('token'));
  }

  // tslint:disable-next-line: typedef
  createloginForm() {
    this.doctorLoginForm = new FormGroup({
      email: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
      rememberMe:new FormControl(true,Validators.required)
    });
  }
  // tslint:disable-next-line: typedef
  onSubmit() {
    this.accountService.doctorLogin(this.doctorLoginForm.value).subscribe(
      () => {
        console.log(localStorage.getItem('pic'));
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
