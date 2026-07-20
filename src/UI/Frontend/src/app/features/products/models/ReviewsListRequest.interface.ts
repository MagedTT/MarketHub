import { RequestParameters } from "./RequestParameters.interface";

export interface ReviewsListRequest {
    productId: string;
    requestParameters: RequestParameters;
    trackChanges: boolean;
};