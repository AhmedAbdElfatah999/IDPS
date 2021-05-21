import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { Routes, RouterModule } from '@angular/router';
import { DoctorLoginComponent } from './doctor-login/doctor-login.component';
import { MessagesResolver } from './messages.resolver';
import { MessagesComponent } from './messages/messages.component';
import { PatientMessagesComponent } from './patient-messages/patient-messages.component';
import { DoctorProfileComponent } from './doctor-profile/doctor-profile.component';
import { PatientProfileComponent } from './patient-profile/patient-profile.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'doctor-login', component: DoctorLoginComponent },
  { path: 'register', component: RegisterComponent },
  {path: 'patient-messages', component: PatientMessagesComponent },
  {path: 'doctor-profile', component: DoctorProfileComponent },
  {path: 'patient-profile', component: PatientProfileComponent },
  {path: 'messages', component: MessagesComponent, resolve: {messages: MessagesResolver}}
];
@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccountRoutingModule {}
