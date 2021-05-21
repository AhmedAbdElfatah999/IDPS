import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IUser } from '../shared/models/user';
import { localStorageSync } from 'ngrx-store-localstorage';
import { Message } from '../shared/models/message';
import { PaginatedResult } from '../shared/models/pagination';
import {  HttpParams } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<IUser>(null);
  currentUser$ = this.currentUserSource.asObservable();
  MyToken= null;
  decodedToken: any;
  jwtHelper = new JwtHelperService();
  User=null;

  constructor(private http: HttpClient, private router: Router) {}
  private loginStatus = new BehaviorSubject<boolean>(this.checkLoginStatus());

  // tslint:disable-next-line: typedef
  getCurrentUserValue() {
    return this.currentUserSource.value;
  }

  // tslint:disable-next-line: typedef
  loadCurrentUser(token: string) {


          this.MyToken=this.User.token;
          localStorage.setItem('Id', this.User.id);
          localStorage.setItem('token', token);
          this.currentUserSource.next(this.User);


  }
  // tslint:disable-next-line: typedef
  login(values: any) {
    return this.http.post(this.baseUrl + 'patient/login', values).pipe(
      map((user: IUser) => {
        if (user) {
          localStorage.setItem('Id', user.id);
          this.User=user;
          this.MyToken=user.token;
          localStorage.setItem('token', user.token);
          localStorage.setItem('loginstatus','1');
          localStorage.setItem('user', JSON.stringify(user));
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }
// tslint:disable-next-line: typedef
    doctorLogin(values: any) {
      return this.http.post(this.baseUrl + 'doctor/login', values).pipe(
        map((user: IUser) => {
          if (user) {
            localStorage.setItem('Id', user.id);
            this.User=user;
            this.MyToken=user.token;
            localStorage.setItem('token', user.token);
            this.decodedToken = this.jwtHelper.decodeToken(user.token);
            this.currentUserSource.next(user);
          }
        })
      );
    }
// tslint:disable-next-line: typedef
patientLogin(values: any) {
  return this.http.post(this.baseUrl + 'patient/login', values).pipe(
    map((user: IUser) => {
      if (user) {
        localStorage.setItem('Id', user.id);
        this.User=user;
        this.MyToken=user.token;
        localStorage.setItem('token', user.token);
        this.decodedToken = this.jwtHelper.decodeToken(user.token);
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
          this.User=user;
          localStorage.setItem('pic', user.photoUrl);
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }
  // tslint:disable-next-line: typedef
  Logout() {
    localStorage.removeItem('token');
    localStorage.clear();
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

//Messaging Part Start From Here


  getMessages(id: string, page?, itemsPerPage?, messageContainer?) {
    const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();

    let params = new HttpParams();

    params = params.append('MessageContainer', messageContainer);

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    return this.http.get<Message[]>(this.baseUrl + 'user/' + id + '/messages', {observe: 'response', params})
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') !== null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }

          return paginatedResult;
        })
      );
  }




  getMessageThread(id: string, recipientId: string) {
    return this.http.get<Message[]>(this.baseUrl + 'user/' + id + '/messages/thread/' + recipientId);
  }

  sendMessage(id: string, message: Message) {
    return this.http.post(this.baseUrl + 'user/' + id + '/messages', message);
  }

  deleteMessage(id: number, userId:string) {
    return this.http.post(this.baseUrl + 'user/' + userId + '/messages/' + id, {});
  }

  markAsRead(userId: string, messageId: number) {
    this.http.post(this.baseUrl + 'user/' + userId + '/messages/' + messageId + '/read', {})
      .subscribe();
  }





}
