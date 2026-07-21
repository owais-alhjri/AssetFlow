import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-approve-dialog',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  templateUrl: './approve-dialog.html',
  styleUrl: './approve-dialog.scss',
})
export class ApproveDialog {

  private fb =  inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<ApproveDialog>);

  form = this.fb.nonNullable.group({
    employeeNumber: ['', [Validators.required, Validators.maxLength(50)]],
    firstName: ['', [Validators.required, Validators.maxLength(50)]],
    lastName: ['',[ Validators.required, Validators.maxLength(50)]],
    department: ['', [Validators.required, Validators.maxLength(50)]],
    jobTitle: ['', Validators.maxLength(50)],
    hireDate: this.fb.control<Date | null>(null, Validators.required),
  })

  get employeeNumber(){return this.form.get('employeeNumber')!;}
  get firstName(){return this.form.get('firstName')!;}
  get lastName(){return this.form.get('lastName')!;}
  get department(){return this.form.get('department')!;}
  get jobTitle(){return this.form.get('jobTitle')!;}
  get hireDate(){return this.form.get('hireDate')!;}

  save(): void{
    if(this.form.invalid){
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();
    const d = value.hireDate;
    const hireDate = d
      ? `${d.getFullYear()}-${String(d.getMonth()+1).padStart(2,'0')}-${String(d.getDate()).padStart(2,'0')}`
      : null;
    const dto = {
      ...value,
      hireDate
    };

    this.dialogRef.close(dto);
  }
  cancel(): void{
    this.dialogRef.close();
  }

}
