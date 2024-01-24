import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Product } from '../models/product.model';
import { PaginatedResult } from '../models/paginated-result.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private apiBaseUrl = environment.apiUrl;
  private productsUrl = `${this.apiBaseUrl}/products`;
  private mediaUrl = `${this.apiBaseUrl}/media`;

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
    return this.http.get<Product[]>(`${this.productsUrl}`);
  }

  getProductsByPage(page: number, itemsPerPage: number = 10): Observable<PaginatedResult<Product[]>> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('itemsPerPage', itemsPerPage.toString());

    return this.http.get<PaginatedResult<Product[]>>(`${this.productsUrl}`, { params });
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