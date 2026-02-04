import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import Swal from 'sweetalert2';
import { Snackbar } from '../../shared/components/snackbar/snackbar';
import { IProduct } from '../../shared/interfaces/product';
import { IResponseDialog } from '../../shared/interfaces/response-dialog';
import { UserData } from '../../shared/interfaces/user-data';
import { AuthService } from '../../shared/services/auth.service';
import { ProductService } from '../../shared/services/product.service';
import { ReportService } from '../../shared/services/report.service';
import { AddEditProduct } from './components/add-edit-product/add-edit-product';

@Component({
  selector: 'app-inventory',
  standalone: true,
  imports: [
    MatProgressSpinnerModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
  templateUrl: './inventory.html',
  styleUrl: './inventory.scss',
})
export class Inventory implements OnInit {
  private _snackBar = inject(MatSnackBar);
  readonly dialog = inject(MatDialog);
  displayedColumns: string[] = ['name', 'description', 'price', 'stockQuantity', 'actions'];
  data = signal<IProduct[]>([]);
  resultsLength = 0;
  loading = signal<boolean>(true);
  user = signal<UserData>(undefined!);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private readonly productService: ProductService,
    private readonly reportService: ReportService,
    private readonly authService: AuthService,
  ) {
    this.user.set(authService.user);
  }

  ngOnInit(): void {
    this.getData();
  }

  getData() {
    this.loading.set(true);
    this.productService.findAll().subscribe({
      next: (response) => {
        this.data.set(response.data);
        this.resultsLength = response.data.length;
        console.log(response);
      },
      error: () => {
        console.log('errpor');
      },
      complete: () => {
        this.loading.set(false);
      },
    });
  }

  applyFilter(event: KeyboardEvent) {}

  onAdd() {
    this.dialog.open(AddEditProduct, {
      width: '70%',
      enterAnimationDuration: '0ms',
      exitAnimationDuration: '0ms',
      data: {
        type: 1,
      },
    });
  }

  onEdit(product: IProduct) {
    const dialogRef = this.dialog.open(AddEditProduct, {
      width: '70%',
      enterAnimationDuration: '0ms',
      exitAnimationDuration: '0ms',
      data: {
        type: 2,
        id: product.id,
      },
    });
    dialogRef.afterClosed().subscribe((result: IResponseDialog) => {
      if (result?.isConfirmed) {
        console.log('object');
        this.getData();
      }
    });
  }

  onDelete(product: IProduct) {
    Swal.fire({
      title: '¿Estás seguro?',
      text: 'Esta acción no se puede deshacer',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar',
    }).then((result) => {
      if (result.isConfirmed) {
        this.loading.set(true);
        this.productService.deleted(product.id).subscribe({
          next: (response) => {},
          error: (err) => {
            this.loading.set(false);
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
            this.loading.set(false);
            this.getData();
          },
        });
      }
    });
  }

  onLowStockAlert(product: IProduct) {
    Swal.fire({
      title: '¿Estás seguro?',
      text: 'Se reportará el producto como inventario bajo',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, reportar',
      cancelButtonText: 'Cancelar',
    }).then((result) => {
      if (result.isConfirmed) {
        this.reportService
          .created({
            userId: this.user().id,
            productId: product.id,
            reason: 'Stock de inventario bajo',
          })
          .subscribe({
            next: (res) => {},
            error: (err) => {},
            complete: () => {},
          });
      }
    });
  }
}
