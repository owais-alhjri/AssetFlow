export interface PendingUser {
  id: string;
  email: string;
  status: string;
  requestedAt: string;
}
export interface ApproveUserRequest {
  employeeNumber: string;
  firstName: string;
  lastName: string;
  department: string;
  jobTitle?: string | null;
  hireDate: string;
}
