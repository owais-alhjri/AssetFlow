import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { AssetsServices } from '../../../core/services/assetsServices';
import { Component, inject, OnInit, signal } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import {
  MatCard,
  MatCardContent,
  MatCardHeader,
  MatCardTitle,
  MatCardSubtitle,
} from '@angular/material/card';
import { Asset } from '../../../shared/models/asset.model';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CategoriesServices } from '../../../core/services/CategoriesServices';
import { Category } from '../../../shared/models/category.model';
import { Auth } from '../../../core/auth/auth';
import { RouterLink } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { AssetFormDialog } from '../asset-form-dialog/asset-form-dialog';

@Component({
  selector: 'app-assets',
  standalone: true,
  imports: [
    MatTableModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatPaginatorModule,
    MatSelectModule,
    MatInputModule,
    MatCard,
    MatCardContent,
    MatCardHeader,
    MatCardTitle,
    MatCardSubtitle,
    MatButtonModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    RouterLink,
  ],
  templateUrl: './assets.html',
  styleUrl: './assets.scss',
})
export class Assets implements OnInit {
  private assetServices = inject(AssetsServices);
  private categoryServices = inject(CategoriesServices);
  private auth = inject(Auth);
  private dialog = inject(MatDialog);

  isAdmin = () => this.auth.getRole() === 'Admin';

  assets = signal<Asset[]>([]);
  categories = signal<Category[]>([]);
  totalAssets = signal(0);
  isLoading = signal(false);
  errorMessage = signal<string | null>(null);

  page = signal(1);
  pageSize = signal(10);
  search = signal('');
  status = signal<string | undefined>(undefined);
  categoryId = signal<string | undefined>(undefined);

  searchControl = new FormControl('');
  statusControl = new FormControl<string | undefined>(undefined);
  categoryControl = new FormControl<string | undefined>(undefined);

  color(status: string): string {
    if (status === 'Available') return 'lightgreen';
    if (status === 'Assigned') return 'lightblue';
    if (status === 'Retired') return 'lightgray';
    return status;
  }

  constructor() {
    this.searchControl.valueChanges
      .pipe(debounceTime(400), distinctUntilChanged(), takeUntilDestroyed())
      .subscribe((value) => {
        this.search.set(value ?? '');
        this.page.set(1);
        this.loadAssets();
      });

    this.statusControl.valueChanges.pipe(takeUntilDestroyed()).subscribe((value) => {
      this.status.set(value ?? undefined);
      this.page.set(1);
      this.loadAssets();
    });

    this.categoryControl.valueChanges.pipe(takeUntilDestroyed()).subscribe((value) => {
      this.categoryId.set(value ?? undefined);
      this.page.set(1);
      this.loadAssets();
    });
  }

  ngOnInit() {
    this.loadAssets();
    this.loadCategories();
  }

  onPageChange(e: PageEvent) {
    this.page.set(e.pageIndex + 1);
    this.pageSize.set(e.pageSize);
    this.loadAssets();
  }

  loadCategories() {
    this.categoryServices.getCategories().subscribe({
      next: (res) => this.categories.set(res),
      error: () => this.categories.set([]),
    });
  }

  loadAssets() {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.assetServices
      .getAssets({
        pageNumber: this.page(),
        pageSize: this.pageSize(),
        search: this.search(),
        status: this.status(),
        categoryId: this.categoryId(),
      })
      .subscribe({
        next: (res) => {
          this.assets.set(res.items);
          this.totalAssets.set(res.totalCount);
          this.pageSize.set(res.pageSize);
          this.isLoading.set(false);
        },
        error: () => {
          this.errorMessage.set('Failed to load assets. Please try again');
          this.isLoading.set(false);
        },
      });
  }

  openAssetDialog(asset?: Asset) {
    const dialogRef = this.dialog.open(AssetFormDialog, {
      width: '600px',
      data: asset ? {asset} : undefined,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if(!result) return;
      const call =
        result.mode === 'create'
          ? this.assetServices.createAsset(result.dto)
          : this.assetServices.updateAsset(result.id, result.dto);
      call.subscribe({
        next: () => this.loadAssets(),
        error: () => this.errorMessage.set('Could not create the asset. Please try again'),
      });
    });
  }
}
