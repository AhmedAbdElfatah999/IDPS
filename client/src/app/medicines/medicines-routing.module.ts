import { NgModule } from '@angular/core';
import { MedicinesComponent } from './medicines.component';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', component: MedicinesComponent},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class MedicinesRoutingModule { }
