export interface StoreOrderShippingAddressDto {
    id: string;
    userId: string;
    fullName: string;
    phoneNumber: string;
    country: string;
    governorate: string;
    city: string;
    street: string;
    buildingNumber: string;
    floor: string | null;
    apartment: string | null;
    postalCode: string;
    isDefault: boolean;
}