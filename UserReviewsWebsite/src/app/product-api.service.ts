import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http'
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductApiService {

  readonly productAPIUrl = "https://localhost:7143/api";

  constructor(private http: HttpClient) { }

  GetProducts(query: any): Observable<any[]> {

    const params = new HttpParams().set('SearchPhrase', query.SerchPhrase).set('PageNumber', query.PageNumber).set('PageSize', query.PageSize)
      .set('SortBy', query.SortBy).set('SortDirection', query.SortDirection);

    return this.http.get<any>(this.productAPIUrl + '/Product', { params: params });
  }

  GetProduct(id: number): Observable<any> {
    return this.http.get<any>(this.productAPIUrl + `/Product/${id}`);
  }

  AddProduct(data: any): Observable<any> {
    var tokenHeader = new HttpHeaders({ 'Authorization': 'Bearer ' + localStorage.getItem('token') })
    return this.http.post(this.productAPIUrl + '/Product', data, { headers: tokenHeader });
  }

  UpdateProduct(id: number | string, data: any) {
    var tokenHeader = new HttpHeaders({ 'Authorization': 'Bearer ' + localStorage.getItem('token') })
    return this.http.put(this.productAPIUrl + `/Product/${id}`, data, { headers: tokenHeader });
  }

  DeleteProduct(id: number) {
    var tokenHeader = new HttpHeaders({ 'Authorization': 'Bearer ' + localStorage.getItem('token') })
    return this.http.delete(this.productAPIUrl + `/Product/${id}`, { headers: tokenHeader });
  }

}
