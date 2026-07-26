export interface Employee{
    id : string,
    employeeNumber: string,
    firstName: string,
    lastName: string,
    email: string,
    department: string,
    jobTitle?: string | null,
    isActive: boolean,
    hireDate: string,
    createdAt: string,
    updatedAt: string
}

export interface EmployeeDto{
    employeeNumber: string,
    firstName: string,
    lastName: string,
    email: string,
    department: string,
    jobTitle?: string,
    hireDate: string
}