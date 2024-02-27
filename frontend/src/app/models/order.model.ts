import { Customer } from "./customer.model";
import { OrderDetails } from "./order-details.model";

export class Order {
    orderId: string = '';
    shortOrderId: string = this.orderId.substring(0, 8);
    customerId: string = '';
    customerName: string = '';
    customer: Customer = new Customer();
    orderDate: Date = new Date();
    totalPrice: number = 0;
    isPickupInStore: boolean = false;
    status: string = '';
    orderDetails: OrderDetails[] = [];
}

export class AdminOrder {
    orderId: string = '';
    customerName: string = '';
    shortOrderId: string = this.orderId.substring(0, 8);
    orderDate: Date = new Date();
    totalPrice: number = 0;
    isPickupInStore: boolean = false;
    status: string = '';
}
