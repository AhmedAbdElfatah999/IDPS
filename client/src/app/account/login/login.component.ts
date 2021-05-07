import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
loginForm: FormGroup;
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  this.createloginForm();
  this.loadCurrentUser();
  }

  // tslint:disable-next-line: typedef
  createloginForm(){
    this.loginForm = new FormGroup(
{
  email: new FormControl('', Validators.required),
  password: new FormControl('', Validators.required),
});
}
  // tslint:disable-next-line: typedef
  onSubmit(){
   // tslint:disable-next-line: deprecation
   this.accountService.login(this.loginForm.value).subscribe(() => {
     console.log('user logged in');
     this.loadCurrentUser();
   // tslint:disable-next-line: no-shadowed-variable
   }, error => {
console.log(error);
   });
  }

  loadCurrentUser() {
    const token = localStorage.getItem('token');
    this.accountService.loadCurrentUser(token).subscribe(() => {
      console.log('loaded user');
    }, error => {
      console.log(error);
    });
  }



}
