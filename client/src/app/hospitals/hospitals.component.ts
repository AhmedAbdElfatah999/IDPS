import { Component, OnInit } from '@angular/core';
import { IHospitals } from '../shared/models/hospitals';
import { HospitalsService } from './hospitals.service';

@Component({
  selector: 'app-hospitals',
  templateUrl: './hospitals.component.html',
  styleUrls: ['./hospitals.component.scss']
})
export class HospitalsComponent implements OnInit {
  hospitals;

  constructor(private hospitalsServices: HospitalsService) { }

  ngOnInit(): void {
    this.getHospitals();
  }
  // tslint:disable-next-line: typedef
  getHospitals(){
    // tslint:disable-next-line: deprecation
    this.hospitalsServices.getHospitals().subscribe(
      response => {
        this.hospitals = response;
      },
      error => {
        console.log(error);
      }
    );
  }

}
