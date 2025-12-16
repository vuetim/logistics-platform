import { inject } from '@angular/core';
import {
    HttpInterceptorFn,
    HttpErrorResponse
} from '@angular/common/http';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthFacade } from './auth.facade';
import { TokenStorage } from './infrastructure/token-storage.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
    const storage = inject(TokenStorage);
    const auth = inject(AuthFacade);

    const session = storage.get();

    const authReq = session
        ? req.clone({
            setHeaders: {
                Authorization: `Bearer ${session.accessToken}`
            }
        })
        : req;

    return next(authReq).pipe(
        catchError((err: HttpErrorResponse) => {
            if (req.url.includes('/auth/refresh')) {
                auth.logout();
                return throwError(() => err);
            }

            if (err.status !== 401) {
                return throwError(() => err);
            }

            return auth.refresh().pipe(
                switchMap(() => {
                    const refreshed = storage.get();
                    if (!refreshed) {
                        auth.logout();
                        return throwError(() => err);
                    }

                    const retryReq = req.clone({
                        setHeaders: {
                            Authorization: `Bearer ${refreshed.accessToken}`
                        }
                    });

                    return next(retryReq);
                }),
                catchError(e => {
                    auth.logout();
                    return throwError(() => e);
                })
            );

        })
    );
};
