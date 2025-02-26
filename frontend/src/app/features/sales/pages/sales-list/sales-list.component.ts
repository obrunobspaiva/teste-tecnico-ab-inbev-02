import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SalesService } from '../../services/sales.service';
import { Sale } from '../../../../core/models/sale.model';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';

@Component({
  selector: 'app-sales-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, LoadingComponent],
  template: `
    <div class="sales-container">
      <div class="header">
        <h1>Vendas</h1>
        <button routerLink="/sales/new">Nova Venda</button>
      </div>

      <div class="filters">
        <input 
          type="text" 
          [(ngModel)]="filters.customer"
          placeholder="Buscar por cliente"
          (ngModelChange)="loadSales()">
          
        <input 
          type="date" 
          [(ngModel)]="filters.startDate"
          (ngModelChange)="onDateChange($event, 'start')">
          
        <input 
          type="date" 
          [(ngModel)]="filters.endDate"
          (ngModelChange)="onDateChange($event, 'end')">
      </div>

      <app-loading *ngIf="loading"></app-loading>

      <table *ngIf="!loading">
        <thead>
          <tr>
            <th>Cliente</th>
            <th>Data</th>
            <th>Total</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let sale of sales">
            <td>{{ sale.customer }}</td>
            <td>{{ sale.saleDate | date:'MM/dd/yyyy' }}</td>
            <td>{{ calculateTotal(sale) | currency:'BRL' }}</td>
            <td>
              <button [routerLink]="['/sales', sale.id]">Ver</button>
              <button [routerLink]="['/sales', sale.id, 'edit']">Editar</button>
              <button (click)="deleteSale(sale.id!)">Excluir</button>
            </td>
          </tr>
        </tbody>
      </table>

      <div class="pagination" *ngIf="totalPages > 1">
        <button 
          [disabled]="currentPage === 1"
          (click)="changePage(currentPage - 1)">
          Anterior
        </button>
        
        <span>Página {{ currentPage }} de {{ totalPages }}</span>
        
        <button 
          [disabled]="currentPage === totalPages"
          (click)="changePage(currentPage + 1)">
          Próxima
        </button>
      </div>
    </div>
  `,
  styles: [`
    .sales-container {
      padding: 2rem;
    }

    .header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 2rem;
    }

    .filters {
      display: flex;
      gap: 1rem;
      margin-bottom: 1rem;
    }

    table {
      width: 100%;
      border-collapse: collapse;
    }

    th, td {
      padding: 0.75rem;
      text-align: left;
      border-bottom: 1px solid #ddd;
    }

    .pagination {
      display: flex;
      justify-content: center;
      align-items: center;
      gap: 1rem;
      margin-top: 2rem;
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
export class SalesListComponent implements OnInit {
  sales: Sale[] = [];
  loading = false;
  currentPage = 1;
  totalPages = 1;
  filters = {
    customer: '',
    startDate: '',
    endDate: ''
  };

  constructor(private salesService: SalesService) {}

  ngOnInit() {
    this.loadSales();
  }

  loadSales() {
    this.loading = true;
    this.salesService.getSales(
      this.currentPage,
      10,
      this.filters.customer,
      this.filters.startDate ? new Date(this.filters.startDate) : null,
      this.filters.endDate ? new Date(this.filters.endDate) : null
    ).subscribe({
      next: (response) => {
        console.log('Received sales:', response);
        this.sales = response.data || [];
        this.totalPages = response.totalPages;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading sales:', error);
        this.loading = false;
      }
    });
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadSales();
  }

  deleteSale(id: string) {
    if (confirm('Tem certeza que deseja excluir esta venda?')) {
      this.salesService.deleteSale(id).subscribe({
        next: () => this.loadSales(),
        error: () => alert('Erro ao excluir venda')
      });
    }
  }

  calculateTotal(sale: Sale): number {
    return sale.items.reduce((total, item) => total + (item.quantity * item.unitPrice), 0);
  }

  onDateChange(event: string, type: 'start' | 'end') {
    if (type === 'start') {
      this.filters.startDate = event;
    } else {
      this.filters.endDate = event;
    }
    this.loadSales();
  }
} 