export interface ApiResponse {
  success: boolean;
  message: string;
}

export interface ApiResponseWithData<T> extends ApiResponse {
  data: T;
}

export interface AuthResponseData {
  token: string;
  email: string;
  role: string;
}

export interface ValidationError {
  propertyName: string;
  errorMessage: string;
}

export interface PaginatedResponse<T> extends ApiResponseWithData<T[]> {
  currentPage: number;
  totalPages: number;
  totalCount: number;
} 