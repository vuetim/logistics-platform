import { Directive, Input } from '@angular/core';
import { AuthFacade } from '../../core/auth/auth.facade';

@Directive()
// Directive sepse nuk ka template
export abstract class BaseCrudTabComponent<
    TItem,
    TCreateDto,
    TUpdateDto
> {
    @Input({ required: true }) customerId!: string;

    items: TItem[] = [];

    showModal = false;
    editing?: TItem;
    loading = false;

    // permissions
    canView = false;
    canCreate = false;
    canUpdate = false;
    canDelete = false;

    protected constructor(
        protected auth: AuthFacade,
        private permissions: {
            view: string;
            create?: string;
            update?: string;
            delete?: string;
        }
    ) {
        this.canView = auth.hasPermission(permissions.view);
        this.canCreate = permissions.create
            ? auth.hasPermission(permissions.create)
            : false;
        this.canUpdate = permissions.update
            ? auth.hasPermission(permissions.update)
            : false;
        this.canDelete = permissions.delete
            ? auth.hasPermission(permissions.delete)
            : false;
    }

    // -------- ABSTRACT API --------
    protected abstract fetch(customerId: string): void;
    protected abstract create(dto: TCreateDto): void;
    protected abstract update(id: string, dto: TUpdateDto): void;
    protected abstract remove(id: string): void;
    // --------------------------------

    load() {
        if (!this.canView) return;
        this.fetch(this.customerId);
    }

    openAddModal() {
        if (!this.canCreate) return;
        this.editing = undefined;
        this.showModal = true;
    }

    openEditModal(item: TItem) {
        if (!this.canUpdate) return;
        this.editing = item;
        this.showModal = true;
    }

    onModalClose(saved: boolean) {
        this.showModal = false;
        this.editing = undefined;

        if (saved) {
            this.load();
        }
    }

    deleteItem(id: string) {
        if (!this.canDelete) return;
        if (!confirm('Are you sure?')) return;

        this.remove(id);
    }
}
