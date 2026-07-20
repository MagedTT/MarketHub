export interface StoreDto {
    id: string;
    userId: string;
    name: string;
    userName: string;
    description?: string | null;
    logoUrl?: string | null;
    isActive: boolean;
    email: string;
    createdAt: Date;
};