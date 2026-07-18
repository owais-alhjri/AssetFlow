import { CanActivateFn, Router } from '@angular/router';
import { Auth } from './auth';
import { inject } from '@angular/core';

export const roleGuard: CanActivateFn = (route) => {
  const auth = inject(Auth);
  const router = inject(Router);
  const allowed = route.data['roles'] as string[] | undefined;
  const ok = auth.isLoggedIn() && (!allowed || allowed.includes(auth.getRole() ?? ''));
  return ok ? true : router.createUrlTree(['/login']);
};
