import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class HospitalsService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }
  // tslint:disable-next-line: typedef
  getHospitals(){
    return this.http.get(this.baseUrl + 'hospital/AllHospitals');
  }
}
