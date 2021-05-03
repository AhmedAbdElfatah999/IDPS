import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: 'idps',
    loadChildren: () =>
      import('./idps/idps.module').then((mod) => mod.IDPSModule),
  },
  {
    path: 'hospitals',
    loadChildren: () =>
      import('./hospitals/hospitals.module').then((mod) => mod.HospitalsModule),
  },
  {
    path: 'pharmacies',
    loadChildren: () =>
      import('./pharmacies/pharmacies.module').then(
        (mod) => mod.PharmaciesModule
      ),
  },
  {
    path: 'medicines',
    loadChildren: () =>
      import('./medicines/medicines.module').then((mod) => mod.MedicinesModule),
  },
  {
    path: 'account',
    loadChildren: () =>
      import('./account/account.module').then((mod) => mod.AccountModule),
    data: { breadcrumb: { skip: true } }
  },
  { path: '**', redirectTo: 'not-found', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
