import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HospitalsComponent } from './hospitals.component';
import { HospitalsRoutingModule } from './hospitals-routing.module';



@NgModule({
  declarations: [HospitalsComponent],
  imports: [
    CommonModule,
    HospitalsRoutingModule
  ]
})
export class HospitalsModule { }
