import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private apiUrl = 'https://localhost:5047/Products';
  private imageUploadUrl = 'https://localhost:5047/api/Media';

  constructor(private http: HttpClient) { }

  createProduct(productData: any, imagePaths?: string[]): Observable<Product> {
    const payload = {
      ...productData,
      ImagePaths: imagePaths
    }
    return this.http.post<Product>(this.apiUrl, payload);
  }

  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  updateProduct(id: number, productData: any, imagePaths?: string[]): Observable<Product> {
    const payload = {
      ...productData,
      ImagePaths: imagePaths
    }
    return this.http.put<Product>(`${this.apiUrl}/${id}`, payload);
  }

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl)
  }

  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${id}`);
  }

  uploadProductImages(data: FormData): Observable<any> {
    return this.http.post(this.imageUploadUrl, data);
  }
}