import { Directive, Input, TemplateRef, ViewContainerRef } from "@angular/core";
import { AuthFacade } from "../../core/auth/auth.facade";

@Directive({
    selector: '[hasPermission]',
    standalone: true
})
export class HasPermissionDirective {

    private permissions: string[] = [];

    constructor(
        private auth: AuthFacade,
        private tpl: TemplateRef<any>,
        private vcr: ViewContainerRef
    ) { }

    @Input()
    set hasPermission(permission: string) {
        this.vcr.clear();

        if (this.auth.hasPermission(permission)) {
            this.vcr.createEmbeddedView(this.tpl);
        }
    }
}
