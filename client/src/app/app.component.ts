import { IUser } from 'src/app/shared/models/user';
import { Observable } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './account/account.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'client';
  constructor(private accountService: AccountService) {}
  ngOnInit(): void {
this.loadCurrentUser();
  }
  Mytoken:string;
  MyUser:Observable<IUser>;

  // tslint:disable-next-line: typedef
  loadCurrentUser() {
    this.Mytoken = localStorage.getItem('token');
    console.log(this.Mytoken+"from app component");
    this.accountService.loadCurrentUser(this.Mytoken);
    this.MyUser=this.accountService.User;

  }
}
