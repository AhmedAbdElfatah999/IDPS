import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';

import { RouterModule } from '@angular/router';
import { PharmaciesModule } from '../pharmacies/pharmacies.module';
import { FooterComponent } from './footer/footer.component';


@NgModule({
  declarations: [NavBarComponent, FooterComponent],
  imports: [
    CommonModule,
    RouterModule,
    PharmaciesModule
  ],
  exports: [NavBarComponent,FooterComponent]
})
export class CoreModule { }
