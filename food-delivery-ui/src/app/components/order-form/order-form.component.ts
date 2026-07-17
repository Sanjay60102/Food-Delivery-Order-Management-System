import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs/operators';

import { OrderStatus } from '../../enums/order-status.enum';
import { Order } from '../../models/order.model';
import { OrderService } from '../../services/order.service';

@Component({
  selector: 'app-order-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    <div class="container py-4">
      <h2 class="mb-4">{{ isEditMode ? 'Edit Order' : 'Create Order' }}</h2>

      <div *ngIf="errorMessage" class="alert alert-danger">{{ errorMessage }}</div>
      <div *ngIf="successMessage" class="alert alert-success">{{ successMessage }}</div>

      <form [formGroup]="orderForm" (ngSubmit)="onSubmit()" class="row g-3">
        <div class="col-md-6">
          <label class="form-label">Customer Name</label>
          <input class="form-control" formControlName="customerName" />
          <div class="text-danger small mt-1" *ngIf="hasError('customerName', 'required')">Customer Name is required.</div>
        </div>

        <div class="col-md-6">
          <label class="form-label">Phone</label>
          <input class="form-control" formControlName="customerPhone" />
          <div class="text-danger small mt-1" *ngIf="hasError('customerPhone', 'required')">Phone is required.</div>
          <div class="text-danger small mt-1" *ngIf="hasError('customerPhone', 'pattern')">Enter a valid phone number.</div>
        </div>

        <div class="col-md-6">
          <label class="form-label">Food Item</label>
          <input class="form-control" formControlName="foodItem" />
          <div class="text-danger small mt-1" *ngIf="hasError('foodItem', 'required')">Food Item is required.</div>
        </div>

        <div class="col-md-3">
          <label class="form-label">Quantity</label>
          <input class="form-control" type="number" formControlName="quantity" />
          <div class="text-danger small mt-1" *ngIf="hasError('quantity', 'required')">Quantity is required.</div>
          <div class="text-danger small mt-1" *ngIf="hasError('quantity', 'min')">Quantity must be greater than zero.</div>
        </div>

        <div class="col-md-3">
          <label class="form-label">Price</label>
          <input class="form-control" type="number" step="0.01" formControlName="price" />
          <div class="text-danger small mt-1" *ngIf="hasError('price', 'required')">Price is required.</div>
          <div class="text-danger small mt-1" *ngIf="hasError('price', 'min')">Price must be greater than zero.</div>
        </div>

        <div class="col-md-12">
          <label class="form-label">Address</label>
          <textarea class="form-control" rows="3" formControlName="deliveryAddress"></textarea>
          <div class="text-danger small mt-1" *ngIf="hasError('deliveryAddress', 'required')">Address is required.</div>
        </div>

        <div class="col-md-6">
          <label class="form-label">Status</label>
          <select class="form-select" formControlName="status">
            <option *ngFor="let status of statusOptions" [value]="status">{{ status }}</option>
          </select>
          <div class="text-danger small mt-1" *ngIf="hasError('status', 'required')">Status is required.</div>
        </div>

        <div class="col-md-6">
          <label class="form-label">Date</label>
          <input class="form-control" type="datetime-local" formControlName="orderDate" />
          <div class="text-danger small mt-1" *ngIf="hasError('orderDate', 'required')">Order Date is required.</div>
        </div>

        <div class="col-12 d-flex gap-2">
          <button class="btn btn-primary" type="submit" [disabled]="orderForm.invalid || submitting">{{ isEditMode ? 'Update Order' : 'Submit Order' }}</button>
          <button class="btn btn-outline-secondary" type="button" (click)="resetForm()">Reset</button>
          <a class="btn btn-outline-dark" routerLink="/orders">Back to List</a>
        </div>
      </form>
    </div>
  `,
})
export class OrderFormComponent implements OnInit {
  orderForm: FormGroup;
  isEditMode = false;
  submitting = false;
  statusOptions = Object.values(OrderStatus);
  orderId?: number;
  errorMessage = '';
  successMessage = '';

  constructor(
    private readonly fb: FormBuilder,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly orderService: OrderService,
  ) {
    this.orderForm = this.fb.group({
      customerName: ['', [Validators.required, Validators.maxLength(100)]],
      customerPhone: ['', [Validators.required, Validators.pattern(/^[0-9+()\-\s]{10,20}$/)]],
      foodItem: ['', [Validators.required, Validators.maxLength(150)]],
      quantity: [1, [Validators.required, Validators.min(1)]],
      price: [0, [Validators.required, Validators.min(0.01)]],
      deliveryAddress: ['', [Validators.required, Validators.maxLength(250)]],
      status: [OrderStatus.Placed, Validators.required],
      orderDate: [this.getDefaultOrderDateValue(), Validators.required],
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.orderId = Number(id);
      this.isEditMode = true;
      this.loadOrder(this.orderId);
    }
  }

  hasError(controlName: string, errorName: string): boolean {
    const control = this.orderForm.get(controlName);
    return !!(control && control.touched && control.hasError(errorName));
  }

  onSubmit(): void {
    if (this.orderForm.invalid) {
      this.orderForm.markAllAsTouched();
      return;
    }

    const payload: Order = {
      ...this.orderForm.getRawValue(),
      id: this.orderId ?? 0,
    };

    this.submitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    const request$ = this.isEditMode
      ? this.orderService.updateOrder(this.orderId!, payload)
      : this.orderService.createOrder(payload);

    request$.pipe(finalize(() => (this.submitting = false))).subscribe({
      next: () => {
        this.successMessage = this.isEditMode ? 'Order updated successfully.' : 'Order created successfully.';
        this.router.navigate(['/orders']);
      },
      error: (err: Error) => {
        this.errorMessage = err.message;
      },
    });
  }

  resetForm(): void {
    this.orderForm.reset({
      customerName: '',
      customerPhone: '',
      foodItem: '',
      quantity: 1,
      price: 0,
      deliveryAddress: '',
      status: OrderStatus.Placed,
      orderDate: this.getDefaultOrderDateValue(),
    });
  }

  private loadOrder(id: number): void {
    this.orderService.getOrderById(id).subscribe({
      next: (order) => {
        this.orderForm.patchValue({
          customerName: order.customerName,
          customerPhone: order.customerPhone,
          foodItem: order.foodItem,
          quantity: order.quantity,
          price: order.price,
          deliveryAddress: order.deliveryAddress,
          status: order.status,
          orderDate: this.toDateTimeLocalValue(order.orderDate),
        });
      },
      error: (err: Error) => {
        this.errorMessage = err.message;
      },
    });
  }

  private getDefaultOrderDateValue(): string {
    const date = new Date();
    const offset = date.getTimezoneOffset();
    return new Date(date.getTime() - offset * 60000).toISOString().slice(0, 16);
  }

  private toDateTimeLocalValue(value: string): string {
    const date = new Date(value);
    const offset = date.getTimezoneOffset();
    return new Date(date.getTime() - offset * 60000).toISOString().slice(0, 16);
  }
}
