import { WishListItemDto } from "./WishListItemDto.interface";


export interface wishlistDto {
    id: string;
    userId: string;
    wishlistItems: WishListItemDto[];
}