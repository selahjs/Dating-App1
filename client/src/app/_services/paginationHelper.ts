import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs/operators";
import { PaginatedResult } from "../_models/pagination";

export function getPaginatedResult<T>(url, params, http: HttpClient) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
    return http.get<T>(url, { observe: 'response', params }).pipe(
      // we are getting all the response the header and the body so we need to separate them
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

  export function getPaginationHeaders(pageNumber: number, pageSize: number){
    let params = new HttpParams();
    
    params = params.append("pageNumber", pageNumber.toString());
    params = params.append("pageSize", pageSize.toString());
    
    return params;
  }