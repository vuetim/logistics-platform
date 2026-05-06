import { Directive, EventEmitter, Input, Output } from '@angular/core';
import { AuthFacade } from '../../core/auth/auth.facade';

/*
  UNIVERSAL CRUD TAB BASE

  Pattern:
  parent entity → children list

  Examples:
  customer → addresses
  order → items
  order → routes
  load → stops
  load → documents
  carrier → contacts

  NEVER tied to specific entity.
*/

@Directive()
// no template – used only as logic base
export abstract class BaseEntityCrudTabComponent<
    TItem,
    TCreateDto,
    TUpdateDto
> {
    /* =============================
       INPUT
    ============================== */

    // Parent entity id (customerId, orderId, loadId, etc.)
    @Input({ required: true })
    parentId!: string;
    @Output() changed = new EventEmitter<void>();

    /* 
       STATE
     */

    items: TItem[] = [];

    loading = false;

    showModal = false;
    editing?: TItem;

    /* 
       PERMISSIONS
     */

    canView = false;
    canCreate = false;
    canUpdate = false;
    canDelete = false;

    /* 
       CTOR
     */

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

    /* 
       ABSTRACT API (must implement)
     */

    protected abstract fetch(parentId: string): void;

    protected abstract create(dto: TCreateDto): void;

    protected abstract update(id: string, dto: TUpdateDto): void;

    protected abstract remove(id: string): void;

    /* 
       COMMON BEHAVIOR
    */

    load() {
        if (!this.canView || !this.parentId) return;

        this.loading = true;

        this.fetch(this.parentId);
    }

    /*  Modal  */

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

    /*  Delete  */

    deleteItem(id: string) {
        if (!this.canDelete) return;

        if (!confirm('Are you sure?')) return;

        this.remove(id);
    }

    /*  Helpers  */

    protected finishLoad(items: TItem[]) {
        this.items = items;
        this.loading = false;
    }

    protected finishSave() {
        this.showModal = false;
        this.editing = undefined;
        this.changed.emit();
        this.load();
    }

    protected finishDelete() {
        this.changed.emit();
        this.load();
    }
}
