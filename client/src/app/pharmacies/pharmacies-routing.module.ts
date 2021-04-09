import { NgModule } from '@angular/core';
import { PharmaciesComponent } from './pharmacies.component';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', component: PharmaciesComponent},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class PharmaciesRoutingModule { }
