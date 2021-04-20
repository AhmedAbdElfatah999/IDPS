import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { PharmaciesService } from './pharmacies.service';
import { IdpsParams } from '../shared/models/idpsParams';

@Component({
  selector: 'app-pharmacies',
  templateUrl: './pharmacies.component.html',
  styleUrls: ['./pharmacies.component.scss']
})
export class PharmaciesComponent implements OnInit {
  @ViewChild('search', {static: true}) searchTerm: ElementRef;
  pharmacies;
  pharmaciesParams = new IdpsParams();
  totalCount: number;
  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Number Of Branches: high to low', value: 'branchAsc'},
    {name: 'Number Of Branches: low to high', value: 'branchDesc'}
  ];

  constructor(private pharmaciesServices: PharmaciesService) { }

  // tslint:disable-next-line: typedef
  ngOnInit() {
    this.getPharmacies();
  }

  // tslint:disable-next-line: typedef
  getPharmacies(){
    // tslint:disable-next-line: deprecation
    this.pharmaciesServices.getPharmacies(this.pharmaciesParams).subscribe(
      response => {
        this.pharmacies = response;
        this.pharmaciesParams.pageNumber = response.pageIndex;
        this.pharmaciesParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error => {
        console.log(error);
      }
    );

  }

  // tslint:disable-next-line: typedef
  onSortSelected(sort: string){
    this.pharmaciesParams.sort = sort;
    this.getPharmacies();
  }
  // tslint:disable-next-line: typedef
  onPageChange(event: any){
    this.pharmaciesParams.pageNumber = event;
    this.getPharmacies();
  }
  // tslint:disable-next-line: typedef
  onSearch(){
    this.pharmaciesParams.search = this.searchTerm.nativeElement.value;
    this.getPharmacies();
  }
}
