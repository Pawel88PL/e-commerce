import { Category } from "./category.model";
import { ProductImage } from "./product-image.model";
import { OrderDetail } from "./order-details.model";

export class Product {
    productId = 0;
    priority = 0;
    name?: string;
    description?: string;
    price = 0;
    weight = 0;
    amountAvailable = 0;
    popularity = 0;
    dateAdded?: Date;
    productImages?: ProductImage[];
    orderDetails?: OrderDetail[];
    categoryId?: number;
    category?: Category;
}

