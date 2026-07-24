import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Asset, CreateAssetRequest, UpdateAssetRequest } from "../../shared/models/asset.model";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { PagedResult } from "../../shared/models/paged-result.model";

@Injectable({providedIn: 'root'})
export class AssetsServices{
    private http = inject(HttpClient);
    private base = `${environment.apiUrl}/assets`;

    getAssets(params:{
       pageNumber?: number;
       pageSize?: number;
       search?: string;
       status?: string;
       categoryId?:string;
    }): Observable<PagedResult<Asset>>{
        let httpParams = new HttpParams();
        if (params.pageNumber) httpParams = httpParams.set('pageNumber', params.pageNumber);
        if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize);
        if (params.search) httpParams = httpParams.set('search', params.search);
        if (params.status) httpParams = httpParams.set('status', params.status);
        if (params.categoryId) httpParams = httpParams.set('categoryId', params.categoryId);
        return this.http.get<PagedResult<Asset>>(this.base, {params: httpParams});
    }

    getAssetById(id: string):Observable<Asset>{
        return this.http.get<Asset>(`${this.base}/${id}`);
    }
    
    createAsset(request: CreateAssetRequest): Observable<Asset>{
        return this.http.post<Asset>(this.base, request);
    }

    updateAsset(id: string, request: UpdateAssetRequest): Observable<Asset>{
        return this.http.put<Asset>(`${this.base}/${id}`,request);
    }
    deleteAsset(id: string): Observable<void>{
        return this.http.delete<void>(`${this.base}/${id}`);
    }
}