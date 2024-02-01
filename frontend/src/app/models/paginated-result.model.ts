export class PaginatedResult<T> {
    items: T; // Dane, np. lista produktów
    totalItems: number; // Całkowita liczba elementów
    currentPage: number; // Aktualna strona
    totalPages: number; // Całkowita liczba stron
    itemsPerPage: number; // Liczba elementów na stronę

    constructor(data: T, totalItems: number, currentPage: number, totalPages: number, itemsPerPage: number) {
        this.items = data;
        this.totalItems = totalItems;
        this.currentPage = currentPage;
        this.totalPages = totalPages;
        this.itemsPerPage = itemsPerPage;
    }
}
