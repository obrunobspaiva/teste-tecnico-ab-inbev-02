import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { SalesService } from '../../services/sales.service';
import { Sale, SaleItem } from '../../../../core/models/sale.model';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';

@Component({
  selector: 'app-sale-detail',
  standalone: true,
  imports: [CommonModule, LoadingComponent],
  template: `
    <div class="sale-detail" *ngIf="!loading; else loadingTpl">
      <h2>Detalhes da Venda</h2>
      
      <div class="details" *ngIf="sale">
        <p><strong>Cliente:</strong> {{ sale.customer }}</p>
        <p><strong>Data:</strong> {{ sale.saleDate | date:'MM/dd/yyyy' }}</p>
        <p><strong>Total:</strong> {{ calculateSaleTotal() | currency:'BRL' }}</p>

        <h3>Itens</h3>
        <table>
          <thead>
            <tr>
              <th>Produto</th>
              <th>Quantidade</th>
              <th>Preço Unit.</th>
              <th>Total</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let item of sale.items">
              <td>{{ item.product }}</td>
              <td>{{ item.quantity }}</td>
              <td>{{ item.unitPrice | currency:'BRL' }}</td>
              <td>{{ calculateItemTotal(item) | currency:'BRL' }}</td>
            </tr>
          </tbody>
        </table>

        <div class="actions">
          <button (click)="onEdit()">Editar</button>
          <button (click)="onDelete()">Excluir</button>
          <button (click)="onBack()">Voltar</button>
        </div>
      </div>
    </div>

    <ng-template #loadingTpl>
      <app-loading></app-loading>
    </ng-template>
  `,
  styles: [`
    .sale-detail {
      padding: 2rem;
      max-width: 800px;
      margin: 0 auto;
    }

    table {
      width: 100%;
      border-collapse: collapse;
      margin: 1rem 0;
    }

    th, td {
      padding: 0.75rem;
      text-align: left;
      border-bottom: 1px solid #ddd;
    }

    .actions {
      margin-top: 2rem;
      display: flex;
      gap: 1rem;
    }

    button {
      padding: 0.5rem 1rem;
      background-color: #007bff;
      color: white;
      border: none;
      border-radius: 4px;
      cursor: pointer;
    }
  `]
})
export class SaleDetailComponent implements OnInit {
  sale?: Sale;
  loading = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private salesService: SalesService
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loading = true;
      this.salesService.getSaleById(id).subscribe({
        next: (response) => {
          console.log('Sale detail received:', response);
          this.sale = response;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading sale:', error);
          this.loading = false;
          alert('Erro ao carregar venda');
          this.router.navigate(['/sales']);
        }
      });
    }
  }

  onEdit() {
    this.router.navigate(['edit'], { relativeTo: this.route });
  }

  onDelete() {
    if (this.sale?.id && confirm('Tem certeza que deseja excluir esta venda?')) {
      this.salesService.deleteSale(this.sale.id).subscribe({
        next: () => this.router.navigate(['/sales']),
        error: () => alert('Erro ao excluir venda')
      });
    }
  }

  onBack() {
    this.router.navigate(['/sales']);
  }

  calculateItemTotal(item: SaleItem): number {
    return item.quantity * item.unitPrice;
  }

  calculateSaleTotal(): number {
    return this.sale?.items.reduce((sum, item) => 
      sum + this.calculateItemTotal(item), 0
    ) ?? 0;
  }
} 