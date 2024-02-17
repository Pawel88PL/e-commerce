import { Order } from "./order.model";

export class OrderDetail {
    orderDetailId?: number;
    orderId?: number;
    order?: Order;
    productId?: number;
    quantity: number = 0;
    unitPrice: number = 0;
    productName: string = '';
}
