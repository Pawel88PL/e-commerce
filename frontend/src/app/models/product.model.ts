import { Category } from "./category.model";
import { ProductImage } from "./product-image.model";
import { OrderDetail } from "./order-details.model";

export class Product {
    amountAvailable = 0;
    categoryId?: number;
    categoryName?: string;
    dateAdded?: Date;
    description?: string;
    name?: string;
    orderDetails?: OrderDetail[];
    popularity = 0;
    price = 0;
    priority = 0;
    productId = 0;
    productImages?: ProductImage[];
    weight = 0;
}