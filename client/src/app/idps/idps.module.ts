import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IDPSComponent } from './idps.component';
import { DiseasesChileComponent } from './diseases-chile/diseases-chile.component';
import { SharedModule } from '../shared/shared.module';
import { DiseasesDetailsComponent } from './diseases-details/diseases-details.component';
import { IdpsRoutingModule } from './idps-routing.module';


@NgModule({
  declarations: [IDPSComponent, DiseasesChileComponent, DiseasesDetailsComponent],
  imports: [
    CommonModule,
    SharedModule,
    IdpsRoutingModule
  ],
  exports: [IDPSComponent]
})
export class IDPSModule { }
