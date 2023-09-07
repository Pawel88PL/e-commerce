import { Category } from "./category.model";
import { ProductImage } from "./product-image.model";
import { OrderDetail } from "./order-details.model";

export class Product {
    amountAvailable = 0;
    categoryId?: number;
    dateAdded?: Date;
    description?: string;
    name?: string;
    popularity = 0;
    price = 0;
    priority = 0;
    productId = 0;
    weight = 0;
    productImages?: ProductImage[];
    orderDetails?: OrderDetail[];
    category?: Category;
}

