import { Component, Input, OnInit } from '@angular/core';
import { IPharmacies } from 'src/app/shared/models/pharmacies';

@Component({
  selector: 'app-pharmacies-child',
  templateUrl: './pharmacies-child.component.html',
  styleUrls: ['./pharmacies-child.component.scss']
})
export class PharmaciesChildComponent implements OnInit {
 @Input() pharmacies: IPharmacies;
  constructor() { }

  ngOnInit(): void {
  }

}
