import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse, ApiResponseWithData, PaginatedResponse } from '../models/api-response.model';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  get<T>(path: string, params?: HttpParams): Observable<ApiResponseWithData<T>> {
    console.log(`Making GET request to ${path}`, params);
    return this.http.get<ApiResponseWithData<T>>(`${this.baseUrl}/${path}`, { params })
      .pipe(
        tap(response => console.log(`Response from ${path}:`, response))
      );
  }

  post<T>(path: string, body: any): Observable<ApiResponseWithData<T>> {
    const url = `${this.baseUrl}/${path}`;
    console.log(`Making POST request to ${url}:`, body);
    
    return this.http.post<ApiResponseWithData<T>>(url, body, {
      headers: {
        'Content-Type': 'application/json'
      }
    }).pipe(
      tap(response => console.log(`Response from ${path}:`, response))
    );
  }

  put<T>(path: string, body: any): Observable<ApiResponseWithData<T>> {
    return this.http.put<ApiResponseWithData<T>>(`${this.baseUrl}/${path}`, body);
  }

  delete<T>(path: string): Observable<ApiResponseWithData<T>> {
    return this.http.delete<ApiResponseWithData<T>>(`${this.baseUrl}/${path}`);
  }

  getPaginated<T>(path: string, params?: HttpParams): Observable<PaginatedResponse<T>> {
    return this.http.get<PaginatedResponse<T>>(`${this.baseUrl}/${path}`, { params });
  }
} 