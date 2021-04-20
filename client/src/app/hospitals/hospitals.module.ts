import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HospitalsComponent } from './hospitals.component';
import { HospitalsRoutingModule } from './hospitals-routing.module';
import { HospitalsChildComponent } from './hospitals-child/hospitals-child.component';



@NgModule({
  declarations: [HospitalsComponent, HospitalsChildComponent],
  imports: [
    CommonModule,
    HospitalsRoutingModule
  ]
})
export class HospitalsModule { }
