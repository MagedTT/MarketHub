import { PaginationMetadata } from "./paginationMetadata.interface";

export interface PagedResult<T> {
    items: T[];
    metadata: PaginationMetadata;
}