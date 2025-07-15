import { Component } from '@angular/core';
import {FormsModule} from '@angular/forms';
import {Router} from '@angular/router';
import {HttpClientModule} from '@angular/common/http';
import {CommonModule} from '@angular/common';
import {RepartidorService} from '../../../services/repartidor.service';

@Component({
  selector: 'app-repartidores',
  standalone: true,
  imports: [
    FormsModule, HttpClientModule, CommonModule
  ],
  providers: [
    RepartidorService,
  ],
  templateUrl: './repartidores.component.html',
  styleUrl: './repartidores.component.css'
})
export class RepartidoresComponent {
  usuario: string = '';

  constructor(
    private router: Router,
    private repartidorService: RepartidorService
  ) {}

  backToAdmin() {
    this.router.navigate(['/sidenavA']);
  }

  Enviar() {
    console.log("Formulario enviado");

    const camposFaltantes = [];

    // Validar campos faltantes
    if (!this.usuario) camposFaltantes.push('Usuario');

    if (camposFaltantes.length > 0) {
      alert(`Por favor, complete los siguientes campos: ${camposFaltantes.join(', ')}`);
      return;
    }

    this.repartidorService.eliminarRepartidorPorUsuario(this.usuario).subscribe({
      next: () => {
        alert('Repartidor eliminado con éxito');
        this.router.navigate(['/sidenavA']);
      },
      error: (error) => {
        console.error('Error al eliminar repartidor:', error);
        alert('Ocurrió un error al eliminar el repartidor. Por favor, inténtelo de nuevo.');
      }
    });
  }
}
