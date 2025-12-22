import { Injectable } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class BreadcrumbService {
    breadcrumbs: string[] = [];

    constructor(private router: Router) {

        // 1️⃣ BUILD ON INITIAL LOAD (refresh fix)
        const root = this.router.routerState.root;
        this.breadcrumbs = this.build(root);

        // 2️⃣ BUILD ON EVERY NAVIGATION
        this.router.events
            .pipe(filter(e => e instanceof NavigationEnd))
            .subscribe(() => {
                const root = this.router.routerState.root;
                this.breadcrumbs = this.build(root);
            });
    }

    private build(route: ActivatedRoute, acc: string[] = []): string[] {
        if (route.snapshot.data['breadcrumb']) {
            acc = acc.concat(route.snapshot.data['breadcrumb']);
        }

        if (route.firstChild) {
            return this.build(route.firstChild, acc);
        }

        return acc;
    }
}
