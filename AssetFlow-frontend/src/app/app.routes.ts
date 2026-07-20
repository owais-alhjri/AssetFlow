import { authGuard } from './core/auth/auth-guard';
import { Routes } from '@angular/router';

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
        path: '',
        loadComponent: () => import('./layout/shell/shell').then(m=>m.Shell),
        canActivate: [authGuard]
    }
];
