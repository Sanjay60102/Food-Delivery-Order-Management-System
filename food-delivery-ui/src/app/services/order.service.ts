import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { IOrderService } from '../interfaces/order-service.interface';
import { OrderStatus } from '../enums/order-status.enum';
import { Order } from '../models/order.model';
import { DashboardSummary } from '../models/dashboard-summary.model';

@Injectable({ providedIn: 'root' })
export class OrderService implements IOrderService {
  private readonly apiUrl = `${environment.apiUrl}/orders`;

  constructor(private readonly http: HttpClient) {}

  getOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(this.apiUrl).pipe(catchError(this.handleError<Order[]>('getOrders')));
  }

  getOrderById(id: number): Observable<Order> {
    return this.http.get<Order>(`${this.apiUrl}/${id}`).pipe(catchError(this.handleError<Order>('getOrderById')));
  }

  createOrder(order: Order): Observable<Order> {
    return this.http.post<Order>(this.apiUrl, order).pipe(catchError(this.handleError<Order>('createOrder')));
  }

  updateOrder(id: number, order: Order): Observable<Order> {
    return this.http.put<Order>(`${this.apiUrl}/${id}`, order).pipe(catchError(this.handleError<Order>('updateOrder')));
  }

  updateStatus(id: number, status: OrderStatus): Observable<Order> {
    return this.http.patch<Order>(`${this.apiUrl}/${id}/status`, { status }).pipe(catchError(this.handleError<Order>('updateStatus')));
  }

  deleteOrder(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(catchError(this.handleError<void>('deleteOrder')));
  }

  searchOrders(searchTerm: string): Observable<Order[]> {
    return this.http
      .get<Order[]>(`${this.apiUrl}/search`, {
        params: { searchTerm },
      })
      .pipe(catchError(this.handleError<Order[]>('searchOrders')));
  }

  getSummary(): Observable<DashboardSummary> {
    return this.http.get<DashboardSummary>(`${this.apiUrl}/summary`).pipe(catchError(this.handleError<DashboardSummary>('getSummary')));
  }

  private handleError<T>(operation: string) {
    return (error: HttpErrorResponse): Observable<T> => {
      const message = error.error?.message || `Request failed while ${operation}.`;
      console.error(`${operation} failed: ${message}`);
      return throwError(() => new Error(message));
    };
  }
}
