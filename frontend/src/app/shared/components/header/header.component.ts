import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../features/auth/services/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <header class="header">
      <nav>
        <a routerLink="/sales">Vendas</a>
        <button (click)="logout()">Sair</button>
      </nav>
    </header>
  `,
  styles: [`
    .header {
      background-color: #333;
      padding: 1rem;
      color: white;
    }

    nav {
      display: flex;
      justify-content: space-between;
      align-items: center;
      max-width: 1200px;
      margin: 0 auto;
    }

    a {
      color: white;
      text-decoration: none;
      padding: 0.5rem 1rem;
    }

    button {
      background: none;
      border: 1px solid white;
      color: white;
      padding: 0.5rem 1rem;
      cursor: pointer;
    }
  `]
})
export class HeaderComponent {
  constructor(private authService: AuthService) {}

  logout() {
    this.authService.logout();
  }
} 