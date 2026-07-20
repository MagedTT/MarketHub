export interface User {
    id: string;
    storeId?: string | null;
    isActive?: string | null;
    userName: string;
    email: string;
    roles: string[];
}