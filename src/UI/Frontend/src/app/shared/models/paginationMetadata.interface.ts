export interface PaginationMetadata {
    CurrentPage: number;
    PageSize: number;
    TotalCount: number;
    TotalPages: number;
    HasPrevious: boolean;
    HasNext: boolean;
}