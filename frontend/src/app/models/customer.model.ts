import { Order } from "./order.model";

export class Customer {
    customerId?: string;
    name?: string;
    surname?: string;
    phoneNumber?: string;
    email?: string;
    city?: string;
    street?: string;
    address?: string;
    postalCode?: string;
    orders?: Order[];
}
