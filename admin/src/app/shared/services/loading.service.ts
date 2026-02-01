import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LoadingService {
  private _loading = signal(0);
  readonly loading = this._loading.asReadonly();

  show() {
    this._loading.update((v) => v + 1);
  }

  hide() {
    this._loading.update((v) => Math.max(0, v - 1));
  }
}
