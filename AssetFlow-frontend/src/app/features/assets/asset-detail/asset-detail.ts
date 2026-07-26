import { Component, inject, OnInit, signal } from '@angular/core';
import { AssetsServices } from '../../../core/services/assetsServices';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Asset } from '../../../shared/models/asset.model';
import { DatePipe, DecimalPipe } from '@angular/common';
import { MatCard } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-asset-detail',
  standalone: true,
  imports: [
    DatePipe,
    DecimalPipe,
    RouterLink,
    MatCard,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './asset-detail.html',
  styleUrl: './asset-detail.scss',
})
export class AssetDetail implements OnInit {
  private assetServices = inject(AssetsServices);
  private route = inject(ActivatedRoute);

  assetId: string = '';
  asset = signal<Asset | null>(null);
  isLoading = signal(false);
  // Previously absent entirely: a failed fetch left asset() null forever and
  // the page rendered its labels with nothing after them, with no way to tell
  // "still loading" from "this asset doesn't exist" from "the request failed".
  errorMessage = signal<string | null>(null);
  notFound = signal(false);

  ngOnInit(): void {
    this.loadAssetById();
  }

  loadAssetById() {
    this.assetId = this.route.snapshot.paramMap.get('id') || '';
    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.notFound.set(false);

    this.assetServices.getAssetById(this.assetId).subscribe({
      next: (res) => {
        this.asset.set(res);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.isLoading.set(false);
        // A deleted or mistyped id is a different problem from a broken
        // request, and only one of them is worth offering a retry for.
        if (err?.status === 404) {
          this.notFound.set(true);
        } else {
          this.errorMessage.set('Could not load this asset. Please try again.');
        }
      },
    });
  }
}
