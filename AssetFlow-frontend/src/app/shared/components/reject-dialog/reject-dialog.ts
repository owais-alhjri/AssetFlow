import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';

export interface RejectDialogData {
  title: string;
  message: string;
}

@Component({
  selector: 'app-reject-dialog',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule],
  templateUrl: './reject-dialog.html',
  styleUrl: './reject-dialog.scss',
})
export class RejectDialog {
  data = inject<RejectDialogData>(MAT_DIALOG_DATA);
  private dialogRef = inject(MatDialogRef<RejectDialog, boolean>);

  confirm() {
    this.dialogRef.close(true);
  }
  cancel() {
    this.dialogRef.close(false);
  }
}
