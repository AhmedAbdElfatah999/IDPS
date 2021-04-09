import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PharmaciesComponent } from './pharmacies.component';
import { PharmaciesRoutingModule } from './pharmacies-routing.module';



@NgModule({
  declarations: [PharmaciesComponent],
  imports: [
    CommonModule,
    PharmaciesRoutingModule
  ]
})
export class PharmaciesModule { }
