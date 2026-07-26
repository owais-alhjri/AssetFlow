import { Component, inject, OnInit, signal } from '@angular/core';
import { Employee } from '../../../shared/models/employee.model';
import { EmployeesServices } from '../../../core/services/employeesServices';
import { DatePipe } from '@angular/common';
import { MatCard } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-employees',
  standalone: true,
  imports: [
    DatePipe,
    MatCard,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    RouterLink,
  ],
  templateUrl: './employees.html',
  styleUrl: './employees.scss',
})
export class Employees implements OnInit {
  private employeesServices = inject(EmployeesServices);

  employees = signal<Employee[] | null>(null);
  isLoading = signal(false);
  errorMessage = signal<string | null>(null);
  ngOnInit(): void {
    this.loadEmployees()
  }

  loadEmployees(){
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.employeesServices.getEmployees().subscribe({
      next: (res) => {
        this.employees.set(res);
        this.isLoading.set(false);
      },
      error: ()=>{
        this.isLoading.set(false);
        this.errorMessage.set('Could not load employees')
      }
    })
  }

}
