import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { HeaderComponent } from './header/header.component';
import { RouterModule } from '@angular/router';
import { PharmaciesModule } from '../pharmacies/pharmacies.module';


@NgModule({
  declarations: [NavBarComponent, HeaderComponent],
  imports: [
    CommonModule,
    RouterModule,
    PharmaciesModule
  ],
  exports: [NavBarComponent, HeaderComponent]
})
export class CoreModule { }
