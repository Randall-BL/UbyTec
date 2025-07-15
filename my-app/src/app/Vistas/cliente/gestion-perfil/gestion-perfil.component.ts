import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { ClienteService } from '../../../services/cliente.service';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-gestion-perfil',
  standalone: true,
  imports: [
    FormsModule, HttpClientModule, CommonModule
  ],
  providers: [
    ClienteService,
    AuthService
  ],
  templateUrl: './gestion-perfil.component.html',
  styleUrls: ['./gestion-perfil.component.css']
})
export class GestionPerfilComponent implements OnInit {
  usuario: string = '';
  cedula: string = '';
  nombre: string = '';
  apellido1: string = '';
  apellido2: string = '';
  provincia: string = '';
  canton: string = '';
  distrito: string = '';

  constructor(
    private router: Router,
    private clienteService: ClienteService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const sesionCliente = this.authService.obtenerSesionCliente();
    if (sesionCliente) {
      this.usuario = sesionCliente.usuario; // Asegúrate de obtener el usuario desde la sesión
      this.cedula = sesionCliente.numeroCedula;
      this.cargarDatosCliente(this.usuario);
    } else {
      alert('No se pudo obtener la sesión del cliente. Por favor, inicie sesión de nuevo.');
      this.router.navigate(['/inicio-N/tipo1']);
    }
  }

  cargarDatosCliente(cedula: string) {
    this.clienteService.obtenerClientePorUsuario(this.usuario).subscribe({
      next: (cliente: any) => {
        this.cedula = cliente.numeroCedula;
        this.nombre = cliente.nombre;
        this.apellido1 = cliente.apellidos.split(' ')[0];
        this.apellido2 = cliente.apellidos.split(' ')[1] || '';
        this.provincia = cliente.direccionProvincia;
        this.canton = cliente.direccionCanton;
        this.distrito = cliente.direccionDistrito;
        this.usuario = cliente.usuario;
      },
      error: (error) => {
      }
    });
  }

  modificarInfo() {
    console.log('Formulario enviado para modificar el cliente');

    // Validaciones de los campos requeridos
    const camposFaltantes = [];
    if (!this.cedula) camposFaltantes.push('Número de Cédula');
    if (!this.nombre) camposFaltantes.push('Nombre');
    if (!this.apellido1) camposFaltantes.push('Primer Apellido');
    if (!this.apellido2) camposFaltantes.push('Segundo Apellido');
    if (!this.provincia) camposFaltantes.push('Provincia');
    if (!this.canton) camposFaltantes.push('Cantón');
    if (!this.distrito) camposFaltantes.push('Distrito');

    if (camposFaltantes.length > 0) {
      alert(`Por favor, complete los siguientes campos: ${camposFaltantes.join(', ')}`);
      return;
    }

    // Crear el objeto del cliente con los datos modificados
    const clienteData = {
      numeroCedula: this.cedula,
      nombre: this.nombre,
      apellidos: `${this.apellido1} ${this.apellido2}`,
      direccionProvincia: this.provincia,
      direccionCanton: this.canton,
      direccionDistrito: this.distrito
    };

    // Log para revisar qué datos se están enviando en el PUT
    console.log('Datos del cliente enviados para modificación:', JSON.stringify(clienteData, null, 2));

    // Realizar la solicitud PUT
    this.clienteService.actualizarClientePorCedula(this.cedula, clienteData).subscribe({
      next: () => {
        alert('Cliente modificado con éxito');
        this.router.navigate(['/sidenavC']);
      },
      error: (error) => {
        console.error('Error al modificar cliente:', error);
        alert('Ocurrió un error al modificar el cliente. Por favor, inténtelo de nuevo.');
      }
    });
  }

  eliminar() {
    this.router.navigate(['/clientes']);
  }

  backTobienvenida() {
    this.router.navigate(['/sidenavC']);
  }
}
