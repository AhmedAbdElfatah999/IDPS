import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IPagination } from '../shared/models/pagination';
import { ISpecialization } from '../shared/models/specialization';
import { map } from 'rxjs/operators';
import { IdpsParams } from '../shared/models/idpsParams';
import { IDiseases } from '../shared/models/diseases';

@Injectable({
  providedIn: 'root'
})
export class IDPSService {
 baseUrl = 'https://localhost:5001/api/';
 constructor(private http: HttpClient){}
 // tslint:disable-next-line: typedef
 getDiseases(idpsParams: IdpsParams){
   let params = new HttpParams();
   if (idpsParams.specId) {
     params = params.append('specId', idpsParams.specId.toString());
   }
   return this.http.get<IPagination>(this.baseUrl + 'diseases/Diseases?pageSize=50', {observe: 'response', params})
   .pipe(
     map(
       response => {
         return response.body;
        }
     )
   );
 }

 // tslint:disable-next-line: typedef
 getSpecialization(){
   return this.http.get<ISpecialization[]>(this.baseUrl + 'diseases/specializations');
 }

 // tslint:disable-next-line: typedef
 getDiseasDetails(id: number){
   return this.http.get<IDiseases>(this.baseUrl + 'diseases/' + id);
 }
}
