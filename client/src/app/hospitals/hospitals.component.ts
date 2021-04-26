import { Component, OnInit } from '@angular/core';
import { IHospitals } from '../shared/models/hospitals';
import { IdpsParams } from '../shared/models/idpsParams';
import { HospitalsService } from './hospitals.service';

@Component({
  selector: 'app-hospitals',
  templateUrl: './hospitals.component.html',
  styleUrls: ['./hospitals.component.scss']
})
export class HospitalsComponent implements OnInit {
  hospitals: IHospitals[];
  hospitalsParams = new IdpsParams();
  totalCount: number;
  constructor(private hospitalsServices: HospitalsService) { }

  ngOnInit(): void {
    this.getHospitals();
  }
  // tslint:disable-next-line: typedef
  getHospitals(){
    // tslint:disable-next-line: deprecation
    this.hospitalsServices.getHospitals(this.hospitalsParams).subscribe(
      response => {
        this.hospitals = response.data;
        this.hospitalsParams.pageNumber = response.pageIndex;
        this.hospitalsParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error => {
        console.log(error);
      }
    );
  }
  // tslint:disable-next-line: typedef
  onPageChange(event: any){
    this.hospitalsParams.pageNumber = event;
    this.getHospitals();
  }
}
