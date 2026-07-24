import { ApproveUserRequest, PendingUser } from '../../shared/models/user.model';
import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { Observable } from "rxjs";
import { EmployeeDto } from '../../shared/models/employee.model';

@Injectable({providedIn: 'root'})
export class UsersServices{
    private http = inject(HttpClient);
    private base = `${environment.apiUrl}/users`

    getPending(): Observable<PendingUser[]>{
        return this.http.get<PendingUser[]>(`${this.base}/pending`);
    }

    approve(id: string, request: ApproveUserRequest) : Observable<EmployeeDto>{
        return this.http.put<EmployeeDto>(`${this.base}/${id}/approve`, request)
    }

    reject(id: string): Observable<void>{
        return this.http.put<void>(`${this.base}/${id}/reject`,{});
    }
    
}