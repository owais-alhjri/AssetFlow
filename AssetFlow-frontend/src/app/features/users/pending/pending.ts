import { Component, inject, OnInit, signal } from '@angular/core';
import { Users } from '../../../core/services/users';
import { PendingUser } from '../../../shared/models/user.model';
import { MatDialog } from '@angular/material/dialog';
import { ApproveDialog } from '../../../shared/components/approve-dialog/approve-dialog';
import { RejectDialog } from '../../../shared/components/reject-dialog/reject-dialog';

@Component({
  selector: 'app-pending',
  imports: [],
  templateUrl: './pending.html',
  styleUrl: './pending.scss',
})
export class Pending implements OnInit {
  private usersService = inject(Users);
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
    const dialogRef = this.dialog.open(RejectDialog, {
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
