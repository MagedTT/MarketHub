import { Routes } from '@angular/router';
import { AuthLayout } from './core/layouts/auth-layout/auth-layout';
import { MainLayout } from './core/layouts/main-layout/main-layout';

export const routes: Routes = [
    {
        path: '',
        component: MainLayout,
        children: [
            {
                path: '',
                redirectTo: 'products',
                pathMatch: 'full'
            },
            {
                path: 'products',
                loadChildren: () =>
                    import('./features/products/products.routes')
                        .then(c => c.PRODUCTS_ROUTES)
            },
            {
                path: 'wishlist/:id',
                loadComponent: () =>
                    import('./features/wishlist/pages/wishlist/wishlist')
                        .then(c => c.Wishlist)
            },
            {
                path: 'cart',
                loadComponent: () =>
                    import('./features/cart/pages/cart/cart')
                        .then(c => c.Cart)
            },
            {
                path: 'orders',
                loadComponent: () =>
                    import('./features/orders/pages/orders-list/orders-list')
                        .then(c => c.OrdersList)
            }
        ]
    },
    {
        path: 'auth',
        component: AuthLayout,
        children: [
            {
                path: '',
                loadChildren: () =>
                    import('./features/auth/auth.routes')
                        .then(m => m.AUTH_ROUTES)
            }
        ]
    }
];
