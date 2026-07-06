import { Routes } from "@angular/router";

export const PRODUCTS_ROUTES: Routes = [
    {
        path: '',
        loadComponent: () =>
            import('./pages/products/products')
                .then(c => c.Products)
    },
    {
        path: ':id',
        loadComponent: () =>
            import('./pages/product-details/product-details')
                .then(c => c.ProductDetails)
    },
    {
        path: 'wishlist/:id',
        loadComponent: () =>
            import('../wishlist/pages/wishlist/wishlist')
                .then(c => c.Wishlist)
    }
];