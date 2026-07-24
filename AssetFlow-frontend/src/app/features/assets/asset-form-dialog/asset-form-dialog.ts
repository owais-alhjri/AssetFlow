import { Asset, AssetCondition } from './../../../shared/models/asset.model';
import { Component, inject, signal, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { CategoriesServices } from '../../../core/services/CategoriesServices';
import { Category } from '../../../shared/models/category.model';

@Component({
  selector: 'app-asset-form-dialog',
  standalone: true,
  imports: [
        ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatDatepickerModule, MatNativeDateModule, MatButtonModule,
  ],
  templateUrl: './asset-form-dialog.html',
  styleUrl: './asset-form-dialog.scss',
})
export class AssetFormDialog implements OnInit {
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<AssetFormDialog>);
  private data = inject<{asset?: Asset}>(MAT_DIALOG_DATA, {optional: true});
  private categoryServices = inject(CategoriesServices);

  categories = signal<Category[]>([]);
  conditions = Object.keys(AssetCondition);
  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(){
    this.categoryServices.getCategories().subscribe({
      next:(res)=> this.categories.set(res),
      error:()=>this.categories.set([]),
    })
  }
  loadConditions(){
    this.conditions;
  }
  isEdit = !!this.data?.asset;

  form = this.fb.nonNullable.group({
    tag: ['', [Validators.required, Validators.maxLength(50)]],
    name: ['', [Validators.required, Validators.maxLength(200)]],
    serialNumber: ['', [Validators.required, Validators.maxLength(100)]],
    categoryId: ['', Validators.required],
    condition: ['', Validators.required],
    purchaseDate: [null as Date | null, Validators.required],
    purchasePrice: [null as number | null, [Validators.required, Validators.min(0)]],
    warrantyExpiryDate: [null as Date | null],
    nextMaintenanceDate: [null as Date | null],
    location: ['', Validators.maxLength(200)],
    notes: ['', Validators.maxLength(1000)],
  });

  get tag(){return this.form.get('tag')!;}
  get name(){return this.form.get('name')!;}
  get serialNumber(){return this.form.get('serialNumber')!;}
  get categoryId(){return this.form.get('categoryId')!;}
  get condition(){return this.form.get('condition')!;}
  get purchaseDate(){return this.form.get('purchaseDate')!;}
  get purchasePrice(){return this.form.get('purchasePrice')!;}
  get warrantyExpiryDate(){return this.form.get('warrantyExpiryDate')!;}
  get nextMaintenanceDate(){return this.form.get('nextMaintenanceDate')!;}
  get location(){return this.form.get('location')!;}
  get notes(){return this.form.get('notes')!;}

  constructor() {
    if (this.isEdit) {
      const a = this.data!.asset!;
      ['tag', 'serialNumber', 'categoryId', 'purchaseDate', 'purchasePrice']
        .forEach(f => this.form.get(f)!.disable());
      this.form.patchValue({
        tag: a.tag, name: a.name, serialNumber: a.serialNumber,
        categoryId: a.categoryId, condition: a.condition, purchasePrice: a.purchasePrice,
        purchaseDate: new Date(a.purchaseDate),
        warrantyExpiryDate: a.warrantyExpiryDate ? new Date(a.warrantyExpiryDate) : null,
        nextMaintenanceDate: a.nextMaintenanceDate ? new Date(a.nextMaintenanceDate) : null,
        location: a.location ?? '', notes: a.notes ?? '',
      });
    }
  }

  private toDateString(d: Date | null): string | null{
    return d
          ? `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`
      : null;
  }

  save(): void{
    if(this.form.invalid){
      this.form.markAllAsTouched();
      return;
    }

    const v = this.form.getRawValue();

    if(this.isEdit){
      const dto = {
        name: v.name,
        condition: v.condition,
        warrantyExpiryDate: this.toDateString(v.warrantyExpiryDate),
        nextMaintenanceDate: this.toDateString(v.nextMaintenanceDate),
        location: v.location || null,
        notes: v.notes || null,
      };
      this.dialogRef.close({mode: 'edit', id: this.data!.asset!.id, dto});
    }else{
      const dto = {
        tag: v.tag,
        name: v.name,
        serialNumber: v.serialNumber,
        categoryId: v.categoryId,
        condition: v.condition,
        purchaseDate: this.toDateString(v.purchaseDate)!,
        purchasePrice: v.purchasePrice!,
        currency: 'OMR',
        warrantyExpiryDate: this.toDateString(v.warrantyExpiryDate),
        nextMaintenanceDate: this.toDateString(v.nextMaintenanceDate),
        location: v.location || null,
        notes: v.notes || null,
      };
      this.dialogRef.close({mode: 'create', dto});
    }

  }
  cancel(): void{
    this.dialogRef.close();
  }
}
