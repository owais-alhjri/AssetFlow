import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Auth } from '../auth/auth';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token= inject(Auth).getToken();
  if(token){
    req = req.clone({setHeaders:{Authorization: `Bearer ${token}`}});
  }
  return next(req);
};
