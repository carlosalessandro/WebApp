import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MENU_CONFIG } from '../../config/menu.config';
import { MenuCategory } from '../../models/menu.model';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  menuCategories: MenuCategory[] = [];
  searchTerm = '';
  filteredCategories: MenuCategory[] = [];

  ngOnInit(): void {
    this.menuCategories = MENU_CONFIG;
    this.filteredCategories = [...this.menuCategories];
  }

  toggleCategory(category: MenuCategory): void {
    category.expanded = !category.expanded;
  }

  filterMenu(): void {
    if (!this.searchTerm.trim()) {
      this.filteredCategories = [...this.menuCategories];
      return;
    }

    const term = this.searchTerm.toLowerCase();
    this.filteredCategories = this.menuCategories
      .map(category => ({
        ...category,
        items: category.items.filter(item =>
          item.label.toLowerCase().includes(term) ||
          item.id.toLowerCase().includes(term)
        ),
        expanded: true
      }))
      .filter(category => category.items.length > 0);
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.filterMenu();
  }
}
