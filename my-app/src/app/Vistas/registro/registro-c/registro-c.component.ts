import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { ClienteService } from '../../../services/cliente.service'; // Importar el servicio

@Component({
  selector: 'app-registro-c',
  standalone: true,
  imports: [FormsModule, HttpClientModule, CommonModule],
  providers: [ClienteService], // Añadir el proveedor ClienteService
  templateUrl: './registro-c.component.html',
  styleUrls: ['./registro-c.component.css'],
})
export class RegistroCComponent {
  usuario: string = '';
  password: string = '';
  cedula: string = '';
  nombre: string = '';
  apellido1: string = '';
  apellido2: string = '';
  provincia: string = '';
  canton: string = '';
  distrito: string = '';
  edad: number | null = null;
  fechaNacimiento: string = '';
  telefono: string = '';

  passwordVisible: boolean = false;
  mostrarModal: boolean = false;

  constructor(
    private router: Router,
    private clienteService: ClienteService // Inyectar el servicio ClienteService
  ) {}

  // Método para alternar la visibilidad de la contraseña
  alternarVisibilidadpassword() {
    this.passwordVisible = !this.passwordVisible;
    const Entradapassword = document.getElementById('password') as HTMLInputElement;
    Entradapassword.type = this.passwordVisible ? 'text' : 'password';
  }

  // Método para calcular la edad en base a la fecha de nacimiento
  calcularEdad() {
    const today = new Date();
    const birthDate = new Date(this.fechaNacimiento);
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDifference = today.getMonth() - birthDate.getMonth();
    if (monthDifference < 0 || (monthDifference === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    this.edad = age;
  }

  // Método para formatear el número de cédula
  formatearCedula() {
    let cedulaLimpia = this.cedula.replace(/\D/g, ''); // Remover caracteres no numéricos

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

  // Método para formatear el número de teléfono
  formatearTelefono() {
    let telefonoLimpio = this.telefono.replace(/\D/g, ''); // Remover caracteres no numéricos

    if (telefonoLimpio.length > 8) {
      telefonoLimpio = telefonoLimpio.slice(0, 8);
    }

    if (telefonoLimpio.length > 4) {
      this.telefono = `${telefonoLimpio.slice(0, 4)}-${telefonoLimpio.slice(4)}`;
    } else {
      this.telefono = telefonoLimpio;
    }
  }

  // Método para mostrar los términos y condiciones
  mostrarCondiciones() {
    this.mostrarModal = true;
  }

  // Método para cerrar el modal
  cerrarModal() {
    this.mostrarModal = false;
  }

  // Método para navegar a la pantalla de bienvenida
  backTobienvenida() {
    this.router.navigate(['']);
  }

  // Método para enviar el formulario y registrar al cliente
  Enviar() {
    console.log("Formulario enviado");

    // Validaciones de los campos requeridos
    const camposFaltantes = [];
    if (!this.cedula) camposFaltantes.push('Número de Cédula');
    if (!this.nombre) camposFaltantes.push('Nombre');
    if (!this.apellido1) camposFaltantes.push('Primer Apellido');
    if (!this.apellido2) camposFaltantes.push('Segundo Apellido');
    if (!this.provincia) camposFaltantes.push('Provincia');
    if (!this.canton) camposFaltantes.push('Cantón');
    if (!this.distrito) camposFaltantes.push('Distrito');
    if (!this.fechaNacimiento) camposFaltantes.push('Fecha de Nacimiento');
    if (!this.telefono) camposFaltantes.push('Teléfono');
    if (!this.usuario) camposFaltantes.push('Usuario');
    if (!this.password) camposFaltantes.push('Contraseña');

    if (camposFaltantes.length > 0) {
      alert(`Por favor, complete los siguientes campos: ${camposFaltantes.join(', ')}`);
      return;
    }

    // Validar que la cédula tenga exactamente 9 dígitos numéricos
    const cedulaLimpia = this.cedula.replace(/\D/g, '');
    if (cedulaLimpia.length !== 9) {
      alert("La cédula debe tener exactamente 9 dígitos.");
      return;
    }

    // Validar que el número de teléfono tenga exactamente 8 dígitos numéricos
    const telefonoLimpio = this.telefono.replace(/\D/g, '');
    if (telefonoLimpio.length !== 8) {
      alert("El número de teléfono debe tener exactamente 8 dígitos.");
      return;
    }

    // Validar que la contraseña cumpla con las condiciones de seguridad
    const regexPassword = /^(?=.*[A-Z])(?=(.*[a-z]){4,})(?=(.*\d){3,})[A-Za-z\d]{8,16}$/;
    if (!regexPassword.test(this.password)) {
      alert('La contraseña no cumple con las condiciones requeridas: mínimo una letra mayúscula, cuatro letras minúsculas, tres dígitos numéricos y entre 8 y 16 caracteres.');
      return;
    }

    // Crear el objeto cliente para enviar a la API
    const cliente = {
      numeroCedula: this.cedula,
      nombre: this.nombre,
      apellidos: `${this.apellido1} ${this.apellido2}`,
      direccionProvincia: this.provincia,
      direccionCanton: this.canton,
      direccionDistrito: this.distrito,
      fechaNacimiento: this.fechaNacimiento,
      telefono: this.telefono,
      usuario: this.usuario,
      passwordHash: btoa(this.password) // Convertir la contraseña a base64
    };

    // Usar el servicio para registrar el cliente
    this.clienteService.registrarCliente(cliente)
      .subscribe({
        next: (response) => {
          alert('Registro exitoso');
          this.router.navigate(['']);
        },
        error: (error) => {
          console.error('Error al registrar el cliente:', error);
          alert('Ocurrió un error durante el registro. Por favor, inténtelo de nuevo más tarde.');
        }
      });
  }
}
