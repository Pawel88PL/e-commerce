import { Order } from "./order.model";

export class OrderDetails {
    orderDetailId: number = 0;
    orderId: string = '';
    order: Order = new Order();
    productId: number = 0;
    quantity: number = 0;
    unitPrice: number = 0;
    productName: string = '';
}
