import { QueryParameters } from "../../core/models/pagination/query-parameters.model";

export abstract class GenericListPage<
    TQuery extends QueryParameters
> {
    page = 1;
    pageSize = 10;
    totalCount = 0;

    sortBy?: string;
    sortDir?: 'asc' | 'desc';
    filtersOpen = false;
    activeFilters: Record<string, any> = {};

    protected abstract loadData(query: TQuery): void;

    protected buildQuery(): TQuery {
        return {
            page: this.page,
            pageSize: this.pageSize,
            sortBy: this.sortBy,
            sortDir: this.sortDir,
            ...this.activeFilters
        } as TQuery;
    }

    onFilterChange(e: { key: string; value: any }) {
        if (!e?.key) return;

        if (e.key === '__clear__') {
            this.activeFilters = {};
        } else if (e.value == null) {
            delete this.activeFilters[e.key];
        } else {
            this.activeFilters[e.key] = e.value;
        }

        this.page = 1;
        this.reload();
    }

    removeFilter(key: string) {
        delete this.activeFilters[key];
        this.page = 1;
        this.reload();
    }

    onPageChange(page: number) {
        this.page = page;
        this.reload();
    }

    onSortChange(sort: { field: string; dir: 'asc' | 'desc' }) {
        this.sortBy = sort.field;
        this.sortDir = sort.dir;
        this.reload();
    }

    toggleFilter() {
        this.filtersOpen = !this.filtersOpen
    }
    get activeFiltersCount(): number {
        return Object.values(this.activeFilters)
            .filter(v =>
                v !== null &&
                v !== undefined &&
                v !== ''
            ).length;
    }


    protected reload() {
        this.loadData(this.buildQuery());
    }
}
