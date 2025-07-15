import { Component } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { FormsModule } from "@angular/forms";
import { HttpClientModule } from '@angular/common/http';
import { AuthService} from '../../../services/auth.service';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-inicio-a',
  standalone: true,
  imports: [FormsModule, HttpClientModule],
  providers: [AuthService],  // Añadir el proveedor ClienteService
  templateUrl: './inicio-a.component.html',
  styleUrls: ['./inicio-a.component.css'],
})
export class InicioAComponent {
  tipoUsuario: string = '';
  email: string = '';
  password: string = '';

  passwordVisible: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService // Inyectar el servicio AuthService
  ) {
  }

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
      // Validar si el correo y la contraseña son correctos
      if (this.email === 'admin' && this.password === 'admin123') {
        alert('Inicio de sesión exitoso');

        this.authService.guardarSesionAdmin(this.email); // Guardar solo el email del administrador
        this.router.navigate(['/sidenavA']);
      } else {
        alert('Correo o contraseña incorrectos');
      }
    }
  }
}
