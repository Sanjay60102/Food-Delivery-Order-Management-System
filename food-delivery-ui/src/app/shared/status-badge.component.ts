import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

import { OrderStatus } from '../enums/order-status.enum';

@Component({
  selector: 'app-status-badge',
  standalone: true,
  imports: [CommonModule],
  template: `
    <span class="badge rounded-pill" [ngClass]="badgeClass">
      {{ status }}
    </span>
  `,
  styles: [
    `
      .badge {
        font-size: 0.8rem;
        padding: 0.45rem 0.75rem;
      }
    `,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StatusBadgeComponent {
  @Input() status!: OrderStatus;

  get badgeClass(): string {
    switch (this.status) {
      case OrderStatus.Placed:
        return 'bg-primary';
      case OrderStatus.Preparing:
        return 'bg-warning text-dark';
      case OrderStatus.OutForDelivery:
        return 'bg-info text-dark';
      case OrderStatus.Delivered:
        return 'bg-success';
      case OrderStatus.Cancelled:
        return 'bg-danger';
      default:
        return 'bg-secondary';
    }
  }
}
