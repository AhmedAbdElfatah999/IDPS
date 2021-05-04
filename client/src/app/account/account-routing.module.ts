import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { Routes, RouterModule } from '@angular/router';
import { DoctorLoginComponent } from './doctor-login/doctor-login.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'doctor-login', component: DoctorLoginComponent },
  { path: 'register', component: RegisterComponent },
];
@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccountRoutingModule {}
