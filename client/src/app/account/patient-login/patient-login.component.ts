import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-patient-login',
  templateUrl: './patient-login.component.html',
  styleUrls: ['./patient-login.component.scss']
})
export class PatientLoginComponent implements OnInit {
  patientLoginForm: FormGroup;
  constructor(private accountService: AccountService, private router: Router) {}

  ngOnInit(): void {
    this.createloginForm();
    this.accountService.loadCurrentUser(localStorage.getItem('token'));
  }

  // tslint:disable-next-line: typedef
  createloginForm() {
    this.patientLoginForm = new FormGroup({
      email: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
      rememberMe:new FormControl(true,Validators.required)
    });
  }
  // tslint:disable-next-line: typedef
  onSubmit() {
    this.accountService.doctorLogin(this.patientLoginForm.value).subscribe(
      () => {
        console.log(localStorage.getItem('Id'));
      },
      (error) => {
        console.log(error);
      }
    );
  }
}

