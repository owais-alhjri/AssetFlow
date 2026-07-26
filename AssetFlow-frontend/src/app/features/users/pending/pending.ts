import { Component, inject, OnInit, signal } from '@angular/core';
import { UsersServices } from '../../../core/services/usersServices';
import { PendingUser } from '../../../shared/models/user.model';
import { MatDialog } from '@angular/material/dialog';
import { ApproveDialog } from '../../../shared/components/approve-dialog/approve-dialog';
import { ConfirmDialog } from '../../../shared/components/confirm-dialog/confirm-dialog';
import { DatePipe } from '@angular/common';
import { MatCard } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-pending',
  imports: [DatePipe, MatCard, MatIconModule, MatButtonModule, MatProgressSpinnerModule],
  templateUrl: './pending.html',
  styleUrl: './pending.scss',
})
export class Pending implements OnInit {
  private usersService = inject(UsersServices);
  private dialog = inject(MatDialog);

  users = signal<PendingUser[]>([]);
  loading = signal(false);
  errorMessage = signal<string | null>(null);

  ngOnInit() {
    this.loadPendingUsers();
  }

  loadPendingUsers() {
    this.loading.set(true);
    this.errorMessage.set(null);

    this.usersService.getPending().subscribe({
      next: (res) => {
        this.users.set(res);
        this.loading.set(false);
      },
      error: () => {
        this.errorMessage.set('Could not load users. Please try again.');
        this.loading.set(false);
      },
    });
  }

  approveUser(userId: string) {
    const dialogRef = this.dialog.open(ApproveDialog, {
      width: '600px',
    });

    dialogRef.afterClosed().subscribe((dto) => {
      if (!dto) return;
      this.usersService.approve(userId, dto).subscribe({
        next: () => this.loadPendingUsers(),
        error: () =>
          this.errorMessage.set('Could not approve. Check the employee number is unique.'),
      });
    });
  }

  rejectUser(userId: string) {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      width: '400px',
      data: {
        title: 'Reject user',
        message: 'Are you sure you want to reject user?',
      },
    });

    dialogRef.afterClosed().subscribe((confirmed) => {
      if (!confirmed) return;
      this.usersService.reject(userId).subscribe({
        next: () => this.loadPendingUsers(),
        error: () => this.errorMessage.set('Could not reject the user. Please try again.'),
      });
    });
  }
}
