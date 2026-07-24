export interface Asset {
  id: string;
  tag: string;
  name: string;
  serialNumber: string;
  categoryId: string;
  categoryName: string;
  statusId: string;
  statusName: string;
  condition: string;
  purchaseDate: string;
  purchasePrice: number;
  currency: string;
  warrantyExpiryDate?: string | null;
  nextMaintenanceDate?: string | null;
  location?: string | null;
  notes?: string | null;
  createdAt: string;
  updatedAt?: string | null;
}

export interface CreateAssetRequest {
  tag: string;
  name: string;
  serialNumber: string;
  categoryId: string;
  condition: string;
  purchaseDate: string;
  purchasePrice: number;
  currency: string;
  warrantyExpiryDate?: string | null;
  nextMaintenanceDate?: string | null;
  location?: string | null;
  notes?: string | null;
}

export interface UpdateAssetRequest {
  name: string;
  condition: string;
  warrantyExpiryDate?: string | null;
  nextMaintenanceDate?: string | null;
  location?: string | null;
  notes?: string | null;
}
