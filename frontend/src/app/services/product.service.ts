import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private productsUrl = 'https://localhost:5047/Products';
  private mediaUrl = 'https://localhost:5047/api/Media';

  constructor(private http: HttpClient) { }

  createProduct(productData: any, imagePaths?: string[]): Observable<Product> {
    const payload = {
      ...productData,
      ImagePaths: imagePaths
    }
    return this.http.post<Product>(this.productsUrl, payload);
  }

  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.productsUrl}/${id}`);
  }

  deleteImage(id: number): Observable<void> {
    return this.http.delete<void>(`${this.mediaUrl}/${id}`);
  }

  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.productsUrl}/${id}`);
  }
  
  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.productsUrl)
  }
  
  updateProduct(id: number, productData: any, imagePaths?: string[]): Observable<Product> {
    const payload = {
      ...productData,
      ImagePaths: imagePaths
    }
    return this.http.put<Product>(`${this.productsUrl}/${id}`, payload);
  }
  
  uploadProductImages(data: FormData): Observable<any> {
    return this.http.post(this.mediaUrl, data);
  }
}