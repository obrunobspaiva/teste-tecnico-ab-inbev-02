import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/pages/login/login.component';
import { SalesListComponent } from './features/sales/pages/sales-list/sales-list.component';
import { SaleDetailComponent } from './features/sales/pages/sale-detail/sale-detail.component';
import { SaleFormComponent } from './features/sales/pages/sale-form/sale-form.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { 
    path: 'sales',
    canActivate: [authGuard],
    children: [
      { path: '', component: SalesListComponent },
      { path: 'new', component: SaleFormComponent },
      { path: ':id', component: SaleDetailComponent },
      { path: ':id/edit', component: SaleFormComponent }
    ]
  },
  { path: '', redirectTo: 'sales', pathMatch: 'full' }
];
