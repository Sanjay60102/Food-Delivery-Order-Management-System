import { Observable } from 'rxjs';
import { OrderStatus } from '../enums/order-status.enum';
import { Order } from '../models/order.model';
import { DashboardSummary } from '../models/dashboard-summary.model';

export interface IOrderService {
  getOrders(): Observable<Order[]>;
  getOrderById(id: number): Observable<Order>;
  createOrder(order: Order): Observable<Order>;
  updateOrder(id: number, order: Order): Observable<Order>;
  updateStatus(id: number, status: OrderStatus): Observable<Order>;
  deleteOrder(id: number): Observable<void>;
  searchOrders(searchTerm: string): Observable<Order[]>;
  getSummary(): Observable<DashboardSummary>;
}
