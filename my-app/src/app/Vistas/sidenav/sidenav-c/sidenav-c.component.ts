import { Component } from '@angular/core';
import {Router, RouterLink, RouterOutlet} from "@angular/router";

@Component({
  selector: 'app-sidenav-a',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink
  ],
  templateUrl: './sidenav-c.component.html',
  styleUrl: './sidenav-c.component.css'
})
export class SidenavCComponent {

  constructor(private router: Router) {}

  toggleSidebar() {
    const sidebar = document.getElementById("sidebar");
    if (sidebar) {
      sidebar.classList.toggle("expand");
    }
  }
}
