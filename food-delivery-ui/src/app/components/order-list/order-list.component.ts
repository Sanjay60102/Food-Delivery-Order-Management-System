import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { finalize } from 'rxjs/operators';

import { OrderStatus } from '../../enums/order-status.enum';
import { Order } from '../../models/order.model';
import { OrderService } from '../../services/order.service';
import { StatusBadgeComponent } from '../../shared/status-badge.component';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, StatusBadgeComponent],
  template: `
    <div class="container py-4">
      <div class="d-flex justify-content-between align-items-center mb-3 flex-wrap gap-2">
        <h2 class="mb-0">Orders</h2>
        <a class="btn btn-primary" routerLink="/orders/new">Add Order</a>
      </div>

      <div *ngIf="loading" class="alert alert-info">Loading orders...</div>
      <div *ngIf="error" class="alert alert-danger">{{ error }}</div>
      <div *ngIf="successMessage" class="alert alert-success">{{ successMessage }}</div>

      <div class="row g-3 mb-3 align-items-end">
        <div class="col-md-4">
          <label class="form-label">Search by Customer Name</label>
          <input class="form-control" type="text" [(ngModel)]="searchTerm" (ngModelChange)="onSearchChange()" placeholder="Search customer name" />
        </div>

        <div class="col-md-3">
          <label class="form-label">Filter by Status</label>
          <select class="form-select" [(ngModel)]="selectedStatus" (ngModelChange)="onFilterChange()">
            <option value="">All Statuses</option>
            <option *ngFor="let status of statusOptions" [value]="status">{{ status }}</option>
          </select>
        </div>
      </div>

      <div class="table-responsive">
        <table class="table table-striped table-hover align-middle">
          <thead class="table-dark">
            <tr>
              <th><button class="btn btn-link text-white p-0" (click)="sortBy('customerName')">Customer Name</button></th>
              <th>Phone</th>
              <th>Food Item</th>
              <th>Price</th>
              <th><button class="btn btn-link text-white p-0" (click)="sortBy('orderDate')">Date</button></th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let order of paginatedOrders">
              <td>{{ order.customerName }}</td>
              <td>{{ order.customerPhone }}</td>
              <td>{{ order.foodItem }}</td>
              <td>{{ order.price | currency }}</td>
              <td>{{ order.orderDate | date: 'mediumDate' }}</td>
              <td>
                <app-status-badge [status]="order.status"></app-status-badge>
              </td>
              <td>
                <div class="d-flex gap-2 flex-wrap align-items-center">
                  <select class="form-select form-select-sm" [(ngModel)]="order.status" (change)="updateOrderStatus(order)">
                    <option *ngFor="let status of statusOptions" [value]="status">{{ status }}</option>
                  </select>
                  <a class="btn btn-sm btn-outline-primary" [routerLink]="['/orders', order.id, 'edit']">Edit</a>
                  <button class="btn btn-sm btn-outline-danger" (click)="deleteOrder(order.id)">Delete</button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <nav *ngIf="totalPages > 1" aria-label="Order pagination">
        <ul class="pagination justify-content-center">
          <li class="page-item" [class.disabled]="page === 1">
            <button class="page-link" (click)="changePage(page - 1)">Previous</button>
          </li>
          <li class="page-item" *ngFor="let pageNumber of pages" [class.active]="pageNumber === page">
            <button class="page-link" (click)="changePage(pageNumber)">{{ pageNumber }}</button>
          </li>
          <li class="page-item" [class.disabled]="page === totalPages">
            <button class="page-link" (click)="changePage(page + 1)">Next</button>
          </li>
        </ul>
      </nav>
    </div>
  `,
})
export class OrderListComponent implements OnInit {
  orders: Order[] = [];
  loading = false;
  error = '';
  successMessage = '';
  searchTerm = '';
  selectedStatus = '';
  page = 1;
  pageSize = 5;
  sortColumn = 'orderDate';
  sortDirection: 'asc' | 'desc' = 'desc';
  statusOptions = Object.values(OrderStatus);

  constructor(private readonly orderService: OrderService) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  get filteredOrders(): Order[] {
    const term = this.searchTerm.trim().toLowerCase();

    return this.orders
      .filter((order) => {
        const matchesSearch = !term || order.customerName.toLowerCase().includes(term);
        const matchesStatus = !this.selectedStatus || order.status === this.selectedStatus;
        return matchesSearch && matchesStatus;
      })
      .sort((a, b) => {
        const direction = this.sortDirection === 'asc' ? 1 : -1;
        const left = a[this.sortColumn as keyof Order];
        const right = b[this.sortColumn as keyof Order];

        if (typeof left === 'string' && typeof right === 'string') {
          return left.localeCompare(right) * direction;
        }

        return ((Number(left) || 0) - (Number(right) || 0)) * direction;
      });
  }

  get paginatedOrders(): Order[] {
    const start = (this.page - 1) * this.pageSize;
    return this.filteredOrders.slice(start, start + this.pageSize);
  }

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.filteredOrders.length / this.pageSize));
  }

  get pages(): number[] {
    return Array.from({ length: this.totalPages }, (_, index) => index + 1);
  }

  loadOrders(): void {
    this.loading = true;
    this.error = '';
    this.successMessage = '';

    this.orderService
      .getOrders()
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: (data) => {
          this.orders = data;
          this.page = 1;
        },
        error: (err: Error) => (this.error = err.message),
      });
  }

  onSearchChange(): void {
    this.page = 1;
  }

  onFilterChange(): void {
    this.page = 1;
  }

  sortBy(column: keyof Order): void {
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }
  }

  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.page = page;
    }
  }

  updateOrderStatus(order: Order): void {
    this.orderService.updateStatus(order.id, order.status).subscribe({
      next: () => {
        this.successMessage = 'Order status updated successfully.';
        this.loadOrders();
      },
      error: (err: Error) => (this.error = err.message),
    });
  }

  deleteOrder(id: number): void {
    this.orderService.deleteOrder(id).subscribe({
      next: () => {
        this.successMessage = 'Order deleted successfully.';
        this.loadOrders();
      },
      error: (err: Error) => (this.error = err.message),
    });
  }
}
