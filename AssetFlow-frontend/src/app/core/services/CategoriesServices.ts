import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { Category } from "../../shared/models/category.model";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class CategoriesServices {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/categories`;

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.base);
  }
}