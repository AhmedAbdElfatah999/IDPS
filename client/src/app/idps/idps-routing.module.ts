import { NgModule } from '@angular/core';
import { IDPSComponent } from './idps.component';
import { DiseasesDetailsComponent } from './diseases-details/diseases-details.component';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', component: IDPSComponent},
  { path: ':id', component: DiseasesDetailsComponent}
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
export class IdpsRoutingModule { }
