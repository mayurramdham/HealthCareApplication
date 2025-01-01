import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CommonService {
  constructor(private http: HttpClient) {}
  getAllCoutries(): Observable<any> {
    return this.http.get(
      'https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/dropDown/GetCountries'
    );
  }
  getAllStateByCountryId(countryId: number): Observable<any> {
    return this.http.get(
      `https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/dropDown/GetStatesByCountry/${countryId}`
    );
  }
  getAllCityByStateId(stateId: number): Observable<any> {
    return this.http.get(
      `https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/dropDown/GetCityByState/${stateId}`
    );
  }
  getAllSpecialisation(): Observable<any> {
    return this.http.get(
      'https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/Appointment'
    );
  }
  getProviderBySpecialisation(specialisationId: any): Observable<any> {
    return this.http.get(
      `https://ehrapplication-ctbnhrgzeyductct.centralindia-01.azurewebsites.net/api/Appointment/getSpecilisationById/${specialisationId}`
    );
  }
}
