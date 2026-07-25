import { Component, inject, OnInit, signal } from '@angular/core';
import { AssetsServices } from '../../../core/services/assetsServices';
import { ActivatedRoute } from '@angular/router';
import { Asset } from '../../../shared/models/asset.model';
import { DatePipe } from '@angular/common';
@Component({
  selector: 'app-asset-detail',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './asset-detail.html',
  styleUrl: './asset-detail.scss',
})
export class AssetDetail implements OnInit {
  private assetServices = inject(AssetsServices);
  private route = inject(ActivatedRoute);

  assetId: string = '';
  asset = signal<Asset | null>(null);
  ngOnInit(): void {
    this.loadAssetById();
  }

  loadAssetById(){
    this.assetId = this.route.snapshot.paramMap.get('id') || '';

    this.assetServices.getAssetById(this.assetId).subscribe({
      next: (res) =>{
        this.asset.set(res)
      }
    });
  }
}
