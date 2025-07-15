import { Component } from '@angular/core';
import {Router, RouterLink, RouterOutlet} from "@angular/router";

@Component({
  selector: 'app-sidenav-a',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink
  ],
  templateUrl: './sidenav-a.component.html',
  styleUrl: './sidenav-a.component.css'
})
export class SidenavAComponent {

  constructor(private router: Router) {}

  toggleSidebar() {
    const sidebar = document.getElementById("sidebar");
    if (sidebar) {
      sidebar.classList.toggle("expand");
    }
  }
}
