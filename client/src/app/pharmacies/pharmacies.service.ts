import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPagination } from '../shared/models/pagination';
import { IdpsParams } from '../shared/models/idpsParams';

@Injectable({
  providedIn: 'root'
})
export class PharmaciesService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) {}
   // tslint:disable-next-line: typedef
   getPharmacies(pharmaciesParams: IdpsParams){
     let params = new HttpParams();
     if (pharmaciesParams.search){
       params = params.append('search', pharmaciesParams.search);
     }
     params = params.append('sort', pharmaciesParams.sort);
     params = params.append('pageIndex', pharmaciesParams.pageNumber.toString());
     params = params.append('pageSize', pharmaciesParams.pageNumber.toString());
     return this.http.get<IPagination>(this.baseUrl + 'pharmacy/AllPharmacies');
    }
}
