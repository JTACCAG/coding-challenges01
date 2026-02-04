import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
  MAT_DIALOG_DATA,
  MatDialogActions,
  MatDialogContent,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Snackbar } from '../../../../shared/components/snackbar/snackbar';
import { ICategory } from '../../../../shared/interfaces/category';
import { IResponseDialog } from '../../../../shared/interfaces/response-dialog';
import { CategoryService } from '../../../../shared/services/category.service';
import { ProductService } from '../../../../shared/services/product.service';

@Component({
  selector: 'app-add-edit-product',
  imports: [
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatDialogActions,
    MatDialogContent,
  ],
  templateUrl: './add-edit-product.html',
  styleUrl: './add-edit-product.scss',
})
export class AddEditProduct implements OnInit {
  private _snackBar = inject(MatSnackBar);
  readonly data = inject<{ id: string; type: number }>(MAT_DIALOG_DATA);
  form!: FormGroup;
  loading = false;
  categories = signal<ICategory[]>([]);
  type = signal<number>(1);

  constructor(
    private readonly fb: FormBuilder,
    private readonly productService: ProductService,
    private readonly categoryService: CategoryService,
    private readonly dialogRef: MatDialogRef<AddEditProduct>,
  ) {}

  ngOnInit(): void {
    console.log('object');
    this.loadForm();
    this.getCategories();
    if (this.data.type === 2) {
      this.getCategory(this.data.id);
    }
    this.type.set(this.data.type);
  }

  onSubmit() {
    console.log(this.form.controls);
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.form.updateValueAndValidity();
      this._snackBar.openFromComponent(Snackbar, {
        duration: 3000,
        verticalPosition: 'top',
        horizontalPosition: 'right',
        data: {
          message: 'Complete los campos requeridos',
          type: 'warning',
        },
      });
      return;
    }
    this.loading = true;
    this.form.disable();
    if (this.type() === 1)
      this.productService.created(this.form.value).subscribe({
        next: (response) => {
          this._snackBar.openFromComponent(Snackbar, {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'right',
            data: {
              message: response.message,
              type: 'success',
            },
          });
        },
        error: (err) => {
          this.loading = false;
          this.form.enable();
          this._snackBar.openFromComponent(Snackbar, {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'right',
            data: {
              message: err.message || 'Error en el servidor',
              type: 'error',
            },
          });
        },
        complete: () => {
          this.loading = false;
          this.form.enable();
          this.dialogRef.close({ isConfirmed: true } as IResponseDialog);
        },
      });
    else
      this.productService.updated(this.data.id, this.form.value).subscribe({
        next: (response) => {
          this._snackBar.openFromComponent(Snackbar, {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'right',
            data: {
              message: response.message,
              type: 'success',
            },
          });
        },
        error: (err) => {
          this.loading = false;
          this.form.enable();
          this._snackBar.openFromComponent(Snackbar, {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'right',
            data: {
              message: err.message || 'Error en el servidor',
              type: 'error',
            },
          });
        },
        complete: () => {
          this.loading = false;
          this.form.enable();
          this.dialogRef.close({ isConfirmed: true } as IResponseDialog);
        },
      });
  }

  close() {
    this.dialogRef.close();
  }

  getCategories() {
    this.categoryService.findAll().subscribe({
      next: (response) => {
        this.categories.set(response.data);
      },
      complete: () => {},
    });
  }

  getCategory(id: string) {
    this.productService.findOne(id).subscribe({
      next: (response) => {
        this.form.patchValue({ ...response.data });
      },
      complete: () => {},
    });
  }

  loadForm() {
    this.form = this.fb.group({
      name: ['', [Validators.required]],
      description: ['', Validators.required],
      price: [null, [Validators.required, Validators.min(0)]],
      stockQuantity: [0, [Validators.required, Validators.min(0), Validators.pattern(/^\d+$/)]],
      categoryIds: [[], Validators.required],
    });
  }
}
