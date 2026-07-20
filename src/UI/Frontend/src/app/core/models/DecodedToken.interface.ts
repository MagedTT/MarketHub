export interface DecodedToken {
    sub: string;
    storeId?: string | null;
    isActive?: string | null;
    name: string;
    email: string;
    roles: string[];
    exp: number;
    iat: number;
}