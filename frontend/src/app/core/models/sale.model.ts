export interface Sale {
  id?: string;
  storeBranch: string;
  customer: string;
  saleDate: Date;
  items: SaleItem[];
}

export interface SaleItem {
  id?: string;
  product: string;
  quantity: number;
  unitPrice: number;
} 