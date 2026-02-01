import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(
      withFetch(),
      withInterceptors([
        // loadingInterceptor
        // authInterceptor,
      ]),
    ),
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes)
  ]
};
