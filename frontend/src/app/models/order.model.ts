import { Customer } from "./customer.model";
import { OrderDetail } from "./order-details.model";

export class Order {
    orderId?: number;
    customerId?: string;
    customer?: Customer;
    orderDate?: Date;
    totalPrice: number = 0;
    status?: string;
    orderDetails: OrderDetail[] = [];
}
