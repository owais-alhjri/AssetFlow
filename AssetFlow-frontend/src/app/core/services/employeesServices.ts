import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { Observable } from "rxjs";
import { Employee } from "../../shared/models/employee.model";

@Injectable({providedIn: 'root'})
export class EmployeesServices{
    private http = inject(HttpClient);
    private base = `${environment.apiUrl}`

    getEmployees(): Observable<Employee[]>{
        return this.http.get<Employee[]>(`${this.base}/employees`);
    }
}