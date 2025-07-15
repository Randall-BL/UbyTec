import { Component } from '@angular/core';
import {Router} from '@angular/router';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-productos',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './productos.component.html',
  styleUrl: './productos.component.css'
})
export class ProductosComponent {
  nombre: string = '';

  constructor(
    private router: Router,
  ) {}

  backToAdmin() {
    this.router.navigate(['/sidenavN']);
  }

  Enviar() {
    console.log("Formulario enviado");

    const camposFaltantes = [];

    // Validar campos faltantes
    if (!this.nombre) camposFaltantes.push('Nombre');

    // Verificar si hay campos faltantes
    if (camposFaltantes.length > 0) {
      alert(`Por favor, complete los siguientes campos: ${camposFaltantes.join(', ')}`);
      return;
    }

    // Si todas las validaciones se cumplen, muestra un mensaje de éxito
    alert('Eliminación exitosa');
    this.router.navigate(['/sidenavN']);
  }
}
