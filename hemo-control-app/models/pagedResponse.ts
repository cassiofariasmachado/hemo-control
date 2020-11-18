export type PagedResponse<T> = {
    items: T[],
    limit: number,
    offset: number,
    total: number
}