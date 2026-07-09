export interface DecodedToken {
    sub: string;
    storeId?: string | null;
    name: string;
    email: string;
    roles: string[];
    exp: number; // seconds
    iat: number;
}