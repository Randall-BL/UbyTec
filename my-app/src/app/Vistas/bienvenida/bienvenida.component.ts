import { Component } from '@angular/core';
import {FormsModule} from "@angular/forms";
import {Router, RouterOutlet} from '@angular/router';

@Component({
  selector: 'app-bienvenida',
  standalone: true,
  imports: [
    FormsModule,
    RouterOutlet
  ],
  templateUrl: './bienvenida.component.html',
  styleUrl: './bienvenida.component.css'
})
export class BienvenidaComponent {
  tipoUsuarioR: string = '';
  tipoUsuario: string = '';

  constructor(private router: Router) {}

  // Función para alternar la selección de los radiobuttons
  // Función para seleccionar el tipo de usuario para registro
  selectTipoUsuarioR(value: string) {
    this.tipoUsuarioR = this.tipoUsuarioR === value ? '' : value;
    // Deselecciona los radiobuttons de inicio de sesión al seleccionar uno de registro
    this.tipoUsuario = '';
  }

  // Función para seleccionar el tipo de usuario para inicio de sesión
  selectTipoUsuario(value: string) {
    this.tipoUsuario = this.tipoUsuario === value ? '' : value;
    // Deselecciona los radiobuttons de registro al seleccionar uno de inicio de sesión
    this.tipoUsuarioR = '';
  }

  navigateToinicio() {
    if (this.tipoUsuario === '1') {
      this.router.navigate(['/inicio-C/tipo1']);
    } else if (this.tipoUsuario === '2') {
      this.router.navigate(['/inicio-A/tipo2']);
    } else if (this.tipoUsuario === '3') {
      this.router.navigate(['/inicio-N/tipo3']);
    } else {
      alert('Por favor, selecciona un tipo de usuario para iniciar sesión.');
    }
  }
  navigateToregistro()
  {
    if (this.tipoUsuarioR === '1') {
      this.router.navigate(['/registro-C/tipo1']);
    } else if (this.tipoUsuarioR === '3') {
      this.router.navigate(['/registro-N/tipo3']);
    } else {
      alert('Por favor, selecciona un tipo de usuario para registrarte.');
    }
  }
}
