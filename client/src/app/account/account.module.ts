import { PatientMessagesComponent } from './patient-messages/patient-messages.component';
import { HttpClientModule } from '@angular/common/http';
import { MessagesComponent } from './messages/messages.component';
import { MessagesForUserComponent } from './messages-for-user/messages-for-user.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AccountRoutingModule } from './account-routing.module';
import { SharedModule } from '../shared/shared.module';
import { DoctorLoginComponent } from './doctor-login/doctor-login.component';
import { PatientLoginComponent } from './patient-login/patient-login.component';
import { PatientProfileComponent } from './patient-profile/patient-profile.component';
import { DoctorProfileComponent } from './doctor-profile/doctor-profile.component';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { JwtModule } from '@auth0/angular-jwt';
import {  TabsModule } from 'ngx-bootstrap/tabs';
import {   ButtonsModule } from 'ngx-bootstrap/buttons';
@NgModule({
  declarations: [LoginComponent,
    RegisterComponent,
    DoctorLoginComponent,
    PatientLoginComponent,
    PatientProfileComponent,
    DoctorProfileComponent,
    MessagesForUserComponent,
  MessagesComponent,
  PatientMessagesComponent

],
  imports: [
    CommonModule,
    AccountRoutingModule,
    SharedModule
, BrowserModule,
HttpClientModule,
FormsModule,
ReactiveFormsModule,
RouterModule,
PaginationModule,
TabsModule.forRoot(),
JwtModule,
ButtonsModule



  ],
})
export class AccountModule {}
