import { Customer } from "./customer.model";
import { OrderDetail } from "./order-details.model";

export class Order {
    orderId: string = '';
    shortOrderId: string = '';
    customerId: string = '';
    customer: Customer = new Customer();
    orderDate: Date = new Date();
    totalPrice: number = 0;
    status: string = '';
    orderDetails: OrderDetail[] = [];
}
