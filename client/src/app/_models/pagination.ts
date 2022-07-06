export interface Pagination{
    currentPages: number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;
}

export class PaginatedResult<T>{
    result :T; //our list of memebers we get from server are going to be stored here
    pagination: Pagination; //the pagination information here..
}