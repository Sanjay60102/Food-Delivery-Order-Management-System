import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';

import { DashboardSummary } from '../../models/dashboard-summary.model';
import { OrderService } from '../../services/order.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container py-4">
      <h2 class="mb-4">Dashboard</h2>

      <div *ngIf="loading" class="alert alert-info">Loading summary...</div>
      <div *ngIf="error" class="alert alert-danger">{{ error }}</div>

      <div *ngIf="summary" class="row g-3">
        <div class="col-md-3">
          <div class="card text-white bg-dark h-100">
            <div class="card-body">
              <div class="text-uppercase small">Total Orders</div>
              <h3 class="mt-2 mb-0">{{ summary.totalOrders }}</h3>
            </div>
          </div>
        </div>

        <div class="col-md-3">
          <div class="card h-100 border-primary">
            <div class="card-body">
              <div class="text-uppercase small text-primary">Placed</div>
              <h3 class="mt-2 mb-0">{{ summary.placedOrders }}</h3>
            </div>
          </div>
        </div>

        <div class="col-md-3">
          <div class="card h-100 border-warning">
            <div class="card-body">
              <div class="text-uppercase small text-warning">Preparing</div>
              <h3 class="mt-2 mb-0">{{ summary.preparingOrders }}</h3>
            </div>
          </div>
        </div>

        <div class="col-md-3">
          <div class="card h-100 border-info">
            <div class="card-body">
              <div class="text-uppercase small text-info">Out For Delivery</div>
              <h3 class="mt-2 mb-0">{{ summary.outForDeliveryOrders }}</h3>
            </div>
          </div>
        </div>

        <div class="col-md-3">
          <div class="card h-100 border-success">
            <div class="card-body">
              <div class="text-uppercase small text-success">Delivered</div>
              <h3 class="mt-2 mb-0">{{ summary.deliveredOrders }}</h3>
            </div>
          </div>
        </div>

        <div class="col-md-3">
          <div class="card h-100 border-danger">
            <div class="card-body">
              <div class="text-uppercase small text-danger">Cancelled</div>
              <h3 class="mt-2 mb-0">{{ summary.cancelledOrders }}</h3>
            </div>
          </div>
        </div>

        <div class="col-md-3">
          <div class="card h-100 bg-success-subtle">
            <div class="card-body">
              <div class="text-uppercase small text-success">Revenue</div>
              <h3 class="mt-2 mb-0">{{ summary.totalRevenue | currency }}</h3>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class DashboardComponent implements OnInit {
  summary?: DashboardSummary;
  loading = false;
  error = '';

  constructor(private readonly orderService: OrderService) {}

  ngOnInit(): void {
    this.loading = true;
    this.orderService
      .getSummary()
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: (data) => (this.summary = data),
        error: (err: Error) => (this.error = err.message),
      });
  }
}
