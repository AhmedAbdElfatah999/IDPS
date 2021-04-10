import { NgModule } from '@angular/core';
import { HospitalsComponent } from './hospitals.component';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', component: HospitalsComponent},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class HospitalsRoutingModule { }
