import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PharmaciesComponent } from './pharmacies.component';
import { PharmaciesRoutingModule } from './pharmacies-routing.module';
import { PharmaciesChildComponent } from './pharmacies-child/pharmacies-child.component';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [PharmaciesComponent, PharmaciesChildComponent],
  imports: [
    CommonModule,
    PharmaciesRoutingModule,
    SharedModule
  ],
  exports: [PharmaciesComponent]
})
export class PharmaciesModule { }
