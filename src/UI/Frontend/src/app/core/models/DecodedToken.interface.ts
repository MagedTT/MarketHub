export interface DecodedToken {
    sub: string;
    name: string;
    email: string;
    roles: string[];
    exp: number; // seconds
    iat: number;
}