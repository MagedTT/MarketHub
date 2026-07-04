import { Routes } from "@angular/router";

export const AUTH_LAYOUT_ROUTES: Routes = [
    {
        path: '',
        loadChildren: () =>
            import('../../../features/auth/auth.routes')
                .then(c => c.AUTH_ROUTES)
    }
];