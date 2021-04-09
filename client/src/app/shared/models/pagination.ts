import { IDiseases } from './diseases';

export interface IPagination{
    pageSize: number;
    pageIndex: number;
    count: number;
    data: IDiseases[];
}
