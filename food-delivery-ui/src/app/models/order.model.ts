import { OrderStatus } from '../enums/order-status.enum';

export interface Order {
  id: number;
  customerName: string;
  customerPhone: string;
  foodItem: string;
  quantity: number;
  price: number;
  deliveryAddress: string;
  status: OrderStatus;
  orderDate: string;
}
