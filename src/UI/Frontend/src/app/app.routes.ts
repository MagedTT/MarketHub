import { Routes } from '@angular/router';
import { AuthLayout } from './core/layouts/auth-layout/auth-layout';
import { MainLayout } from './core/layouts/main-layout/main-layout';
import { sellerGuard } from './core/guards/seller-guard';
import { buyerGuard } from './core/guards/buyer-guard';

export const routes: Routes = [
    {
        path: '',
        component: MainLayout,
        children: [
            // {
            //     path: '',
            //     redirectTo: 'products',
            //     pathMatch: 'full'
            // },
            {
                path: 'products',
                // canActivate: [buyerGuard],
                loadChildren: () =>
                    import('./features/products/products.routes')
                        .then(c => c.PRODUCTS_ROUTES)
            },
            {
                path: 'wishlist/:id',
                // canActivate: [buyerGuard],
                loadComponent: () =>
                    import('./features/wishlist/pages/wishlist/wishlist')
                        .then(c => c.Wishlist)
            },
            {
                path: 'cart',
                // canActivate: [buyerGuard],
                loadComponent: () =>
                    import('./features/cart/pages/cart/cart')
                        .then(c => c.Cart)
            },
            {
                path: 'orders',
                // canActivate: [buyerGuard],
                loadComponent: () =>
                    import('./features/orders/pages/orders-list/orders-list')
                        .then(c => c.OrdersList)
            },
            // {
            //     path: '',
            //     redirectTo: 'seller-dashboard',
            //     pathMatch: 'full'
            // },
            {
                path: 'seller-dashboard',
                canActivate: [sellerGuard],
                loadComponent: () =>
                    import('./features/sellers/pages/dashboard/dashboard')
                        .then(c => c.Dashboard)
            },
            {
                path: 'seller-orders',
                canActivate: [sellerGuard],
                loadComponent: () =>
                    import('./features/sellers/pages/seller-orders/seller-orders')
                        .then(c => c.SellerOrders)
            },
            {
                path: 'seller-order-details/:id',
                canActivate: [sellerGuard],
                loadComponent: () =>
                    import('./features/sellers/pages/seller-order-details/seller-order-details')
                        .then(c => c.SellerOrderDetails)
            },
            {
                path: 'seller-products',
                canActivate: [sellerGuard],
                loadComponent: () =>
                    import('./features/sellers/pages/seller-products/seller-products')
                        .then(c => c.SellerProducts)
            },
            {
                path: 'seller-product-details/:id',
                canActivate: [sellerGuard],
                loadComponent: () =>
                    import('./features/sellers/pages/seller-product-details/seller-product-details')
                        .then(c => c.SellerProductDetails)
            },
            {
                path: 'add-product/:storeId',
                canActivate: [sellerGuard],
                loadComponent: () =>
                    import('./features/sellers/pages/add-product/add-product')
                        .then(c => c.AddProduct)
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
