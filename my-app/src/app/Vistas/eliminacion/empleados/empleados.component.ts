import { Component } from '@angular/core';
import {FormsModule} from '@angular/forms';
import {Router} from '@angular/router';
import {AdminService} from '../../../services/administrador.service';
import {HttpClientModule} from '@angular/common/http';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-empleados',
  standalone: true,
  imports: [
    FormsModule, HttpClientModule, CommonModule
  ],
  providers: [
    AdminService,
  ],
  templateUrl: './empleados.component.html',
  styleUrl: './empleados.component.css'
})
export class EmpleadosComponent {
  cedula: string = '';

  constructor(
    private router: Router,
    private adminService: AdminService
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

  backToAdmin() {
    this.router.navigate(['/sidenavA']);
  }

  Enviar() {
    console.log("Formulario enviado");

    const camposFaltantes = [];

    if (!this.cedula) camposFaltantes.push('Número de Cédula Jurídica');

    if (camposFaltantes.length > 0) {
      alert(`Por favor, complete los siguientes campos: ${camposFaltantes.join(', ')}`);
      return;
    }

    const cedulaLimpia = this.cedula.replace(/\D/g, '');
    if (cedulaLimpia.length !== 9) {
      alert("La cédula debe tener exactamente 9 dígitos.");
      return;
    }

    // Llama al servicio para eliminar el administrador
    this.adminService.eliminarAdministradorPorCedula(cedulaLimpia).subscribe(
      () => {
        alert('Eliminación de Empleado Administrador exitosa');
        this.router.navigate(['/sidenavA']);
      },
      (error) => {
        console.error('Error al eliminar administrador:', error);
        alert('Error al eliminar el administrador. Por favor, intente de nuevo.');
      }
    );
  }
}
