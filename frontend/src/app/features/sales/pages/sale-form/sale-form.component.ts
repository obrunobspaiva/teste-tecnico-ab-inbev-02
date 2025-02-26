import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SalesService } from '../../services/sales.service';
import { Sale, SaleItem } from '../../../../core/models/sale.model';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';

@Component({
  selector: 'app-sale-form',
  standalone: true,
  imports: [CommonModule, FormsModule, LoadingComponent],
  template: `
    <div class="sale-form">
      <h2>{{ isEditing ? 'Editar' : 'Nova' }} Venda</h2>
      
      <form (ngSubmit)="onSubmit()" #saleForm="ngForm">
        <div class="form-group">
          <label for="customer">Cliente:</label>
          <input 
            type="text" 
            id="customer" 
            name="customer"
            [(ngModel)]="sale.customer" 
            required>
        </div>

        <div class="form-group">
          <label for="date">Data:</label>
          <input 
            type="date" 
            id="date" 
            name="date"
            [ngModel]="formatDateISO(sale.saleDate)"
            (ngModelChange)="sale.saleDate = parseDate($event)"
            [placeholder]="'dd/MM/yyyy'"
            required>
        </div>

        <h3>Itens</h3>
        <div class="items">
          <div *ngFor="let item of sale.items; let i = index" class="item">
            <div class="form-group">
              <label>Produto:</label>
              <input 
                type="text" 
                [(ngModel)]="item.product" 
                [name]="'product' + i"
                required>
            </div>
            
            <div class="form-group">
              <label>Quantidade:</label>
              <input 
                type="number" 
                [(ngModel)]="item.quantity" 
                [name]="'quantity' + i"
                (change)="calculateItemTotal(item)"
                required>
            </div>
            
            <div class="form-group">
              <label>Preço Unit.:</label>
              <input 
                type="number" 
                [(ngModel)]="item.unitPrice" 
                [name]="'price' + i"
                (change)="calculateItemTotal(item)"
                required>
            </div>

            <button type="button" (click)="removeItem(i)">Remover</button>
          </div>
        </div>

        <button type="button" (click)="addItem()">Adicionar Item</button>

        <div class="total">
          <strong>Total: {{ calculateSaleTotal() | currency:'BRL' }}</strong>
        </div>

        <div class="actions">
          <button type="submit" [disabled]="!saleForm.form.valid">Salvar</button>
          <button type="button" (click)="onCancel()">Cancelar</button>
        </div>
      </form>
    </div>
  `,
  styles: [`
    .sale-form {
      padding: 2rem;
      max-width: 800px;
      margin: 0 auto;
    }

    .form-group {
      margin-bottom: 1rem;
    }

    label {
      display: block;
      margin-bottom: 0.5rem;
    }

    input {
      width: 100%;
      padding: 0.5rem;
      border: 1px solid #ddd;
      border-radius: 4px;
    }

    .items {
      margin: 1rem 0;
    }

    .item {
      display: grid;
      grid-template-columns: 2fr 1fr 1fr auto;
      gap: 1rem;
      margin-bottom: 1rem;
      padding: 1rem;
      border: 1px solid #ddd;
      border-radius: 4px;
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

    button:disabled {
      background-color: #ccc;
    }
  `]
})
export class SaleFormComponent implements OnInit {
  isEditing = false;
  loading = false;
  sale: Sale = {
    id: undefined,
    customer: '',
    saleDate: new Date(),
    storeBranch: 'Main Branch',
    items: []
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private salesService: SalesService
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditing = true;
      this.salesService.getSaleById(id).subscribe({
        next: (sale) => {
          this.sale = {
            ...sale,
            saleDate: new Date(sale.saleDate)
          };
          this.loading = false;
        },
        error: () => {
          this.loading = false;
          this.router.navigate(['/sales']);
        }
      });
    } else {
      this.addItem();
    }
  }

  addItem() {
    this.sale.items.push({
      id: undefined,
      product: '',
      quantity: 0,
      unitPrice: 0
    });
  }

  removeItem(index: number) {
    this.sale.items.splice(index, 1);
    this.calculateSaleTotal();
  }

  calculateItemTotal(item: SaleItem): number {
    return item.quantity * item.unitPrice;
  }

  calculateSaleTotal(): number {
    return this.sale.items.reduce((sum, item) => 
      sum + this.calculateItemTotal(item), 0
    );
  }

  onSubmit() {
    if (this.isEditing && this.sale.id) {
      this.loading = true;
      this.salesService.updateSale(this.sale.id, this.sale).subscribe({
        next: () => {
          this.loading = false;
          this.router.navigate(['/sales']);
        },
        error: (error) => {
          console.error('Error updating sale:', error);
          this.loading = false;
          alert('Erro ao atualizar venda');
        }
      });
    } else {
      this.salesService.createSale(this.sale).subscribe({
        next: () => this.router.navigate(['/sales']),
        error: () => alert('Erro ao criar venda')
      });
    }
  }

  onCancel() {
    this.router.navigate(['/sales']);
  }

  formatDateISO(date: Date): string {
    if (!date) return '';
    const d = new Date(date);
    return d.toISOString().split('T')[0];
  }

  parseDate(dateStr: string): Date {
    if (!dateStr) return new Date();
    const [year, month, day] = dateStr.split('-').map(Number);
    return new Date(year, month - 1, day);
  }
} 