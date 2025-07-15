import { Component } from '@angular/core';
import {FormsModule} from '@angular/forms';
import {Router} from '@angular/router';
import {HttpClientModule} from '@angular/common/http';
import {CommonModule} from '@angular/common';
import {ClienteService} from '../../../services/cliente.service';
import {AuthService} from '../../../services/auth.service';

@Component({
  selector: 'app-clientes',
  standalone: true,
  imports: [
    FormsModule, HttpClientModule, CommonModule
  ],
  providers: [
    ClienteService,
    AuthService // Añade AuthService a los proveedores
  ],
  templateUrl: './clientes.component.html',
  styleUrls: ['./clientes.component.css'] // Corregido: styleUrls (plural)
})
export class ClientesComponent {
  cedula: string = '';

  constructor(
    private router: Router,
    private clienteService: ClienteService,
    private authService: AuthService // Inyecta AuthService
  ) {}

  formatearCedula() {
    let cedulaLimpia = this.cedula.replace(/\D/g, '');
    if (cedulaLimpia.length > 9) {
      cedulaLimpia = cedulaLimpia.slice(0, 9);
    }
    if (cedulaLimpia.length > 5) {
      this.cedula = `${cedulaLimpia.slice(0, 1)}-${cedulaLimpia.slice(1, 5)}-${cedulaLimpia.slice(5)}`;
    } else if (cedulaLimpia.length > 1) {
      this.cedula = `${cedulaLimpia.slice(0, 1)}-${cedulaLimpia.slice(1)}`;
    } else {
      this.cedula = cedulaLimpia;
    }
  }

  backToCliente() {
    this.router.navigate(['/sidenavC']);
  }

  Enviar() {
    console.log('Formulario enviado');

    const camposFaltantes = [];
    if (!this.cedula) camposFaltantes.push('Número de Cédula');

    if (camposFaltantes.length > 0) {
      alert(`Por favor, complete los siguientes campos: ${camposFaltantes.join(', ')}`);
      return;
    }

    const cedulaLimpia = this.cedula.replace(/\D/g, '');
    if (cedulaLimpia.length !== 9) {
      alert('La cédula debe tener exactamente 9 dígitos.');
      return;
    }

    if (confirm('Está seguro de que desea eliminar su perfil? Esta acción no se puede deshacer.')) {
      this.clienteService.eliminarClientePorCedula(this.cedula).subscribe({
        next: () => {
          // Cierra la sesión del usuario
          this.authService.cerrarSesion();

          // Muestra el mensaje de agradecimiento
          alert('Gracias por utilizar nuestra aplicación. ¡Te esperamos pronto!');

          // Redirige a la ventana de bienvenida
          this.router.navigate(['']);
        },
        error: (error) => {
          console.error('Error al eliminar cliente:', error);
          alert('Ocurrió un error al eliminar el cliente. Por favor, inténtelo de nuevo.');
        }
      });
    }
  }
}
