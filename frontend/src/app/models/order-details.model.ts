import { Product } from "./product.model";
import { Order } from "./order.model";

export class OrderDetail {
    orderDetailId?: number;
    orderId?: number;
    order?: Order;
    productId?: number;
    product?: Product;
    quantity?: number;
    unitPric?: number;
}
