import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../../features/auth/services/auth.service';

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();
  const isAuthRequest = req.url.includes('Auth');
  
  console.log({
    url: req.url,
    token: token,
    isAuthRequest,
    headers: req.headers.keys()
  });

  if (token && !isAuthRequest) {
    const authReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
    console.log('Token being sent:', authReq.headers.get('Authorization'));
    return next(authReq);
  }

  return next(req);
}; 