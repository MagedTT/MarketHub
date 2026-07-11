export interface RegisterRequest {
    firstName: string;
    lastName: string;
    userName: string;
    phoneNumber?: string;
    email: string;
    password: string;
    confirmPassword: string;
    permission: string;
    roles: string[];
};