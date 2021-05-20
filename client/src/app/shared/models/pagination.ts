import { IDiseases } from './diseases';
import { IHospitals } from './hospitals';
import { IPharmacies } from './pharmacies';

export interface IPagination{
    pageSize: number;
    pageIndex: number;
    count: number;
    data: IDiseases[];
}

export interface PhPagination{
    pageSize: number;
    pageIndex: number;
    count: number;
    data: IPharmacies[];
}

export interface HPagination{
    pageSize: number;
    pageIndex: number;
    count: number;
    data: IHospitals[];
}
export interface Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
}
export class PaginatedResult<T> {
  result: T;
  pagination: Pagination;
}
