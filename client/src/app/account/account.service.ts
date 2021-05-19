import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IUser } from '../shared/models/user';
import { localStorageSync } from 'ngrx-store-localstorage';
@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<IUser>(null);
  currentUser$ = this.currentUserSource.asObservable();
  MyToken= null;

  constructor(private http: HttpClient, private router: Router) {}
  private loginStatus = new BehaviorSubject<boolean>(this.checkLoginStatus());

  // tslint:disable-next-line: typedef
  getCurrentUserValue() {
    return this.currentUserSource.value;
  }

  // tslint:disable-next-line: typedef
  loadCurrentUser(token: string) {

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${this.MyToken}`);

    return this.http.get(this.baseUrl + 'admin/Account', { headers }).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', this.MyToken);
          this.currentUserSource.next(user);
        }
      })
    );
  }
  // tslint:disable-next-line: typedef
  login(values: any) {
    return this.http.post(this.baseUrl + 'admin/login', values).pipe(
      map((user: IUser) => {
        if (user) {
          this.MyToken=user.token;
          localStorage.setItem('token', user.token);
          localStorage.setItem('loginstatus','1');
          this.currentUserSource.next(user);
        }
      })
    );
  }
  // tslint:disable-next-line: typedef
  register(values: any) {
    return this.http.post(this.baseUrl + 'admin/register', values).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }
  // tslint:disable-next-line: typedef
  Logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }
  // tslint:disable-next-line: typedef
  checkEmailExists(email: string) {
    return this.http.get(this.baseUrl + 'admin/emailexists?email=' + email);
  }

  checkLoginStatus():boolean{
    var loginCookie=localStorage.getItem('loginstatus');
    if (loginCookie=='1') {
       return true;
    }
   return true;
  }
  get isLoggesIn()
  {
      return this.loginStatus.asObservable();
  }
}
