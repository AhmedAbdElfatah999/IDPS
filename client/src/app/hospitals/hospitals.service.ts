import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { IdpsParams } from '../shared/models/idpsParams';
import { HPagination } from '../shared/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class HospitalsService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }
  // tslint:disable-next-line: typedef
  getHospitals(hospitalsParams: IdpsParams){
    let params = new HttpParams();
    params = params.append('pageIndex', hospitalsParams.pageNumber.toString());
    params = params.append('pageIndex', hospitalsParams.pageSize.toString());
    return this.http.get<HPagination>(this.baseUrl + 'hospital/AllHospitals?pageSize=8', {observe: 'response', params})
    .pipe(
      map(
        response => {
          return response.body;
        }
      )
    );
  }
}
