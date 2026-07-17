import { Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { OrderListComponent } from './components/order-list/order-list.component';
import { OrderFormComponent } from './components/order-form/order-form.component';

export const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'orders', component: OrderListComponent },
  { path: 'orders/new', component: OrderFormComponent },
  { path: 'orders/:id/edit', component: OrderFormComponent },
  { path: '**', redirectTo: '/dashboard' },
];
