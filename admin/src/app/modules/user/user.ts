import { Component, OnInit, signal, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { IUser } from '../../shared/interfaces/user';
import { UserData } from '../../shared/interfaces/user-data';
import { AuthService } from '../../shared/services/auth.service';
import { UserService } from '../../shared/services/user.service';

@Component({
  selector: 'app-user',
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
  templateUrl: './user.html',
  styleUrl: './user.scss',
})
export class User implements OnInit {
  loading = signal<boolean>(true);
  displayedColumns: string[] = ['fullname', 'email', 'createdAt', 'role'];
  data = signal<IUser[]>([]);
  user = signal<UserData>(undefined!);
  resultsLength = 0;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private readonly userService: UserService,
    private readonly authService: AuthService,
  ) {
    this.user.set(authService.user);
  }

  ngOnInit(): void {
    this.getData();
  }

  getData() {
    this.loading.set(true);
    this.userService.findAll().subscribe({
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
}
