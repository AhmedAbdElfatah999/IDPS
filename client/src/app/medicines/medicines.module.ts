import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MedicinesComponent } from './medicines.component';
import { MedicinesRoutingModule } from './medicines-routing.module';



@NgModule({
  declarations: [MedicinesComponent],
  imports: [
    CommonModule,
    MedicinesRoutingModule
  ]
})
export class MedicinesModule { }
