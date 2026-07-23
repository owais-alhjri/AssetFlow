import { authGuard } from './core/auth/auth-guard';
import { Routes } from '@angular/router';
import { roleGuard } from './core/auth/role-guard';
import { Shell } from './layout/shell/shell';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login').then((m) => m.Login),
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register/register').then((m) => m.Register),
  },
  {
    path: '',
    component: Shell,
    canActivate: [authGuard],
    children: [
      {
        path: 'users/pending',
        loadComponent: () => import('./features/users/pending/pending').then((m) => m.Pending),
        canActivate: [roleGuard],
        data: { roles: ['Admin'] },
      },
      {path: '', redirectTo: 'assets', pathMatch: 'full'}
    ],
  },
];
