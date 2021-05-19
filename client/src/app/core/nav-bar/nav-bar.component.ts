import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { IUser } from 'src/app/shared/models/user';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
})
export class NavBarComponent implements OnInit {
  currentUser$: Observable<IUser>;

  constructor(private accountServive: AccountService) {}
  LoginStatus$ : Observable<boolean>;
  ngOnInit(): void {
    this.currentUser$ = this.accountServive.currentUser$;
    localStorage.setItem('token', this.accountServive.MyToken);
   this.LoginStatus$=this.accountServive.isLoggesIn;
  }
}
