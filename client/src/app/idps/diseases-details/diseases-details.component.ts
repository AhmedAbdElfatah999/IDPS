import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IDiseases } from 'src/app/shared/models/diseases';
import { IDPSService } from '../idps.service';

@Component({
  selector: 'app-diseases-details',
  templateUrl: './diseases-details.component.html',
  styleUrls: ['./diseases-details.component.scss']
})
export class DiseasesDetailsComponent implements OnInit {
  disease: IDiseases;

  constructor(private idpsService: IDPSService, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadDiseases();
  }
  // tslint:disable-next-line: typedef
  loadDiseases(){
    // tslint:disable-next-line: deprecation
    this.idpsService.getDiseasDetails(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(
      diseases => {
        this.disease = diseases;
      },
      error => {
        console.log(error);
      }
      );
  }

}
