export enum UserRole {
  None = 0,
  Admin = 1,
  User = 2
}

export enum UserStatus {
  Unknown = 0,
  Active = 1,
  Inactive = 2,
  Blocked = 3
}

export interface User {
  id: number;
  username: string;
  email: string;
  phone: string;
  status: UserStatus;
  role: UserRole;
} 