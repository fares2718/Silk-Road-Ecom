import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LoadingService } from '../services/core/loading.service';
import { delay, finalize } from 'rxjs';

export const loaderInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);
  loadingService.loading();

  return next(req).pipe(
    // delay(1000),
    finalize(()=>loadingService.hideLoader())
  );
};
