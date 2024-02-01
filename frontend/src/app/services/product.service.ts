import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Product } from '../models/product.model';
import { PaginatedResult } from '../models/paginated-result.model';
import { DEFAULT_ITEMS_PER_PAGE } from '../config/config';

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
    return this.http.post<void>(`${this.productsUrl}/delete/${id}`, {});
  }

  deleteImage(id: number): Observable<void> {
    return this.http.post<void>(`${this.mediaUrl}/delete/${id}`, {});
  }

  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.productsUrl}/${id}`);
  }

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.productsUrl}/all`);
  }

  getProductsByPage(page: number, itemsPerPage: number = DEFAULT_ITEMS_PER_PAGE): Observable<PaginatedResult<Product[]>> {
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
    return this.http.post<Product>(`${this.productsUrl}/${id}`, payload);
  }

  uploadProductImages(data: FormData): Observable<any> {
    return this.http.post(this.mediaUrl, data);
  }
}