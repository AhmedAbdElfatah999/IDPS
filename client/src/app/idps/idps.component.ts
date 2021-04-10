import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IDiseases } from '../shared/models/diseases';
import { ISpecialization } from '../shared/models/specialization';
import { IDPSService } from './idps.service';
import { IdpsParams } from '../shared/models/idpsParams';
@Component({
  selector: 'app-idps',
  templateUrl: './idps.component.html',
  styleUrls: ['./idps.component.scss']
})
export class IDPSComponent implements OnInit {
  @ViewChild('search', {static: true}) searchTerm: ElementRef;
  disease;
  specialization: ISpecialization[];
  idpsParams = new IdpsParams();
  totalCount: number;
  constructor(private idpsService: IDPSService){}
 // tslint:disable-next-line: typedef
  ngOnInit(){
    this.getDiseases();
    this.getSpecialization();
   }
  // tslint:disable-next-line: typedef
  getDiseases(){
    // tslint:disable-next-line: deprecation
    this.idpsService.getDiseases(this.idpsParams).subscribe(
      response => {
        this.disease = response;
        this.idpsParams.pageNumber = response.pageIndex;
        this.idpsParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error => {
        console.log(error);
      }
    );
  }

  // tslint:disable-next-line: typedef
  getSpecialization(){
    // tslint:disable-next-line: deprecation
    this.idpsService.getSpecialization().subscribe(
      response => {
        this.specialization = [{id: 0, name: 'All'}, ...response];
      },
      error => {
        console.log(error);
      }
    );
  }

  // tslint:disable-next-line: typedef
  onSpecializationSelected(specId: number){
    this.idpsParams.specId = specId;
    this.getDiseases();
  }
  // tslint:disable-next-line: typedef
  onSearch()
  {
    this.idpsParams.search = this.searchTerm.nativeElement.value;
    this.getDiseases();
  }
 }

