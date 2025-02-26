import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Observable } from 'rxjs';
import { Sale } from '../../../core/models/sale.model';
import { PaginatedResponse } from '../../../core/models/api-response.model';
import { HttpParams } from '@angular/common/http';
import { map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SalesService {
  constructor(private apiService: ApiService) {}

  getSales(
    page: number = 1,
    pageSize: number = 5,
    customer?: string,
    startDate?: Date | null,
    endDate?: Date | null,
    sortOrder: 'asc' | 'desc' = 'desc'
  ): Observable<PaginatedResponse<Sale>> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('sortOrder', sortOrder);

    if (customer) {
      params = params.set('customer', customer);
    }

    if (startDate) {
      const start = new Date(startDate);
      start.setHours(0, 0, 0, 0);
      const formattedStart = `${(start.getMonth() + 1).toString().padStart(2, '0')}/${start.getDate().toString().padStart(2, '0')}/${start.getFullYear()}`;
      params = params.set('startDate', formattedStart);
    }

    if (endDate) {
      const end = new Date(endDate);
      end.setHours(23, 59, 59, 999);
      const formattedEnd = `${(end.getMonth() + 1).toString().padStart(2, '0')}/${end.getDate().toString().padStart(2, '0')}/${end.getFullYear()}`;
      params = params.set('endDate', formattedEnd);
    }

    console.log('Final request params:', params.toString());

    return this.apiService.getPaginated<Sale>('Sales/filtered', params).pipe(
      tap(response => console.log('Raw API response:', response)),
      map(response => ({
        data: Array.isArray(response) ? response : response.data || [],
        currentPage: page,
        totalPages: Math.ceil((Array.isArray(response) ? response : response.data || []).length / pageSize),
        totalCount: (Array.isArray(response) ? response : response.data || []).length,
        success: true,
        message: ''
      }))
    );
  }

  getSaleById(id: string): Observable<Sale> {
    return this.apiService.get<Sale>(`Sales/${id}`).pipe(
      tap(rawResponse => console.log('Raw sale detail response:', rawResponse)),
      map(response => {
        if (Array.isArray(response)) {
          return response[0];
        }
        return response.data || response;
      }),
      tap(mappedResponse => console.log('Mapped sale detail:', mappedResponse))
    );
  }

  createSale(sale: Omit<Sale, 'id'>): Observable<Sale> {
    const saleData = {
      ...sale,
      storeBranch: sale.storeBranch || 'Main Branch',
      items: sale.items.map(item => ({
        ...item,
        product: item.product || 'Default Product'
      }))
    };
    
    return this.apiService.post<Sale>('Sales', saleData).pipe(
      map(response => response.data)
    );
  }

  updateSale(id: string, sale: Sale): Observable<void> {
    return this.apiService.put<void>(`Sales/${id}`, sale).pipe(
      tap(response => console.log('Update response:', response)),
      map(() => undefined)
    );
  }

  deleteSale(id: string): Observable<void> {
    return this.apiService.delete<void>(`Sales/${id}`).pipe(
      tap(response => console.log('Delete response:', response)),
      map(() => undefined)
    );
  }
} 