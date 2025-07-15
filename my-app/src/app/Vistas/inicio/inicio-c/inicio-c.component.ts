import { Component } from '@angular/core';
import { FormsModule } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClientModule } from '@angular/common/http';
import { ClienteService } from '../../../services/cliente.service'; // Importar el servicio ClienteService
import {AuthService} from '../../../services/auth.service';

@Component({
  selector: 'app-inicio-c',
  standalone: true,
  imports: [FormsModule, HttpClientModule],
  providers: [ClienteService, AuthService],  // Añadir el proveedor ClienteService
  templateUrl: './inicio-c.component.html',
  styleUrls: ['./inicio-c.component.css'],
})
export class InicioCComponent {
  tipoUsuario: string = '';
  usuario: string = '';
  password: string = '';

  passwordVisible: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private clienteService: ClienteService, // Inyectar el servicio ClienteService
    private authService: AuthService // Inyectar el servicio AuthService
  ) {}

  alternarVisibilidadpassword() {
    this.passwordVisible = !this.passwordVisible;
    const Entradapassword = document.getElementById('password') as HTMLInputElement;
    Entradapassword.type = this.passwordVisible ? 'text' : 'password';
  }

  backTobienvenida() {
    this.router.navigate(['']);
  }

  Enviar() {
    const camposFaltantes = [];

    if (!this.usuario) camposFaltantes.push('Usuario');
    if (!this.password) camposFaltantes.push('Contraseña');

    if (camposFaltantes.length > 0) {
      alert(`Por favor, complete los siguientes campos: ${camposFaltantes.join(', ')}`);
      return;
    }

    // Lógica de autenticación: buscar usuario por nombre de usuario y comparar la contraseña
    this.clienteService.obtenerClientePorUsuario(this.usuario).subscribe({
      next: (cliente) => {
        if (!cliente) {
          alert('Usuario no encontrado.');
        } else {
          // Convertir la contraseña ingresada en base64 para compararla con la contraseña encriptada
          const passwordHash = btoa(this.password);

          if (passwordHash === cliente.passwordHash) {
            alert('Inicio de sesión exitoso');
            this.authService.guardarSesionCliente(cliente); // Guardar sesión del cliente
            this.router.navigate(['/sidenavC']);
          } else {
            // Manejo explícito de error si las contraseñas no coinciden
            alert('Contraseña incorrecta.');
          }
        }
      },
      error: (error) => {
        // Manejo de error si no se puede buscar al usuario
        console.error('Error al buscar el cliente:', error);
        alert('Ocurrió un error al intentar iniciar sesión. Por favor, inténtelo de nuevo más tarde.');
      }
    });
  }
}
