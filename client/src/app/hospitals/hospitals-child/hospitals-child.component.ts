import { Component, Input, OnInit } from '@angular/core';
import { IHospitals } from 'src/app/shared/models/hospitals';

@Component({
  selector: 'app-hospitals-child',
  templateUrl: './hospitals-child.component.html',
  styleUrls: ['./hospitals-child.component.scss']
})
export class HospitalsChildComponent implements OnInit {
  @Input() hospitals: IHospitals;

  constructor() { }

  ngOnInit(): void {
  }

}
