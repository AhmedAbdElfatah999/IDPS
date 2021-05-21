import { AppComponent } from './../../app.component';
import { AccountService } from './../../account/account.service';
import { Component, OnInit } from '@angular/core';
import { from, Observable } from 'rxjs';

import { IUser } from 'src/app/shared/models/user';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
})
export class NavBarComponent implements OnInit {


  constructor(private accountService: AccountService,private appService:AppComponent) {}
  LoginStatus$ : Observable<boolean>;
  currentUser$: any;

  ngOnInit(): void {
    this.currentUser$ =JSON.parse(localStorage.getItem('user'));
    localStorage.setItem('token',this.appService.Mytoken);
    console.log(this.appService.Mytoken+"from nav bar component");
    console.log(localStorage.getItem('user'));
   this.LoginStatus$=this.accountService.isLoggesIn;
  }
  logout() {
    localStorage.removeItem('token');
    localStorage.clear();

  }
}
