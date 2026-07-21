import { authGuard } from './core/auth/auth-guard';
import { Routes } from '@angular/router';
import { roleGuard } from './core/auth/role-guard';

export const routes: Routes = [
    {
        path: 'login',
        loadComponent: () => import('./features/auth/login/login').then(m=>m.Login),
    },
    {
        path: 'register',
        loadComponent:()=> import('./features/auth/register/register').then(m=>m.Register),
    },
    {
        path: 'users/pending',
        loadComponent:()=> import('./features/users/pending/pending').then(m=>m.Pending),
        canActivate: [roleGuard], data: {roles: ['Admin']}
    },
    {
        path: '',
        loadComponent: () => import('./layout/shell/shell').then(m=>m.Shell),
        canActivate: [authGuard]
    }
];
