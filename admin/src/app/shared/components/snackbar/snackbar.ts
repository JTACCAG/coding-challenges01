import { NgClass } from '@angular/common';
import { Component, Inject, ViewEncapsulation } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material/snack-bar';
import { SnackbarData } from '../../interfaces/snackbar-data';

@Component({
  standalone: true,
  encapsulation: ViewEncapsulation.None,
  selector: 'app-snackbar',
  imports: [MatIconModule, MatButtonModule, NgClass],
  templateUrl: './snackbar.html',
  styleUrl: './snackbar.scss',
})
export class Snackbar {
  constructor(
    @Inject(MAT_SNACK_BAR_DATA) public data: SnackbarData,
    private matDialogRef: MatSnackBarRef<Snackbar>,
  ) {}

  get messageClass() {
    return `snackbar-${this.data.type}-message`;
  }

  close() {
    this.matDialogRef.dismiss();
  }

  get icon(): string {
    switch (this.data.type) {
      case 'success':
        return 'check_circle';
      case 'warning':
        return 'warning';
      case 'error':
        return 'error';
      case 'info':
        return 'info';
      default:
        return 'info';
    }
  }
}
