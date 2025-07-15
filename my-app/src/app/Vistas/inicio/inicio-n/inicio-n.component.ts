import { Component } from '@angular/core';
import { FormsModule } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClientModule } from '@angular/common/http';
import { AfiliadoService } from '../../../services/afiliado.service'; // Importar el servicio
import { AuthService } from '../../../services/auth.service'; // Importar el AuthService

@Component({
  selector: 'app-inicio-n',
  standalone: true,
  imports: [FormsModule, HttpClientModule],
  templateUrl: './inicio-n.component.html',
  styleUrls: ['./inicio-n.component.css'],
  providers: [AfiliadoService, AuthService] // Añadir ambos servicios al proveedor
})
export class InicioNComponent {
  email: string = '';
  password: string = '';
  passwordVisible: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private afiliadoService: AfiliadoService, // Inyectar el servicio de Afiliado
    private authService: AuthService // Inyectar el AuthService para manejar la sesión
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

    if (!this.email) camposFaltantes.push('Correo');
    if (!this.password) camposFaltantes.push('Contraseña');

    if (camposFaltantes.length > 0) {
      alert(`Por favor, complete los siguientes campos: ${camposFaltantes.join(', ')}`);
    } else {
      // Llamar al servicio para validar el inicio de sesión
      this.afiliadoService.iniciarSesionAfiliado(this.email, this.password).subscribe({
        next: response => {
          // Si la autenticación es exitosa, guardar la sesión del afiliado
          this.authService.guardarSesionAfiliado(response);
          console.log('Sesión del afiliado guardada:', response); // Agregar un print para verificar la sesión guardada
          alert('Inicio de sesión exitoso');
          this.router.navigate(['/sidenavN']);
        },
        error: error => {
          // Si ocurre un error (por ejemplo, credenciales incorrectas)
          console.error('Error al iniciar sesión:', error);
          if (error.status === 404) {
            alert('Correo electrónico no encontrado. Por favor, verifique e intente de nuevo.');
          } else if (error.status === 401) {
            alert('Contraseña incorrecta. Por favor, verifique e intente de nuevo.');
          } else {
            alert('Ocurrió un error al intentar iniciar sesión. Por favor, inténtelo de nuevo.');
          }
        }
      });
    }
  }
}
