import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-gestion-tipo-comercio',
  standalone: true,
  imports: [
    FormsModule, HttpClientModule
  ],
  templateUrl: './gestion-tipo-comercio.component.html',
  styleUrls: ['./gestion-tipo-comercio.component.css']
})
export class GestionTipoComercioComponent {
  tipo: string = '';

  constructor(
    private router: Router,
    private http: HttpClient // Inyectamos HttpClient para hacer la solicitud
  ) {}

  backToAdmin() {
    this.router.navigate(['/sidenavA']);
  }

  eliminar() {
    this.router.navigate(['/tipo']);
  }

  Enviar() {
    console.log("Formulario enviado");

    const camposFaltantes = [];

    // Validar campos faltantes
    if (!this.tipo) camposFaltantes.push('Tipo de Comercio');

    // Si hay campos faltantes, muestra una alerta
    if (camposFaltantes.length > 0) {
      alert(`Por favor, complete los siguientes campos: ${camposFaltantes.join(', ')}`);
      return;
    }

    // Si todas las validaciones se cumplen, hacemos la solicitud POST
    const tipoComercio = { NombreTipo: this.tipo }; // Creamos el objeto con el tipo de comercio

    this.http.post('https://api3tecuby-ggdga2ahetfef0fr.canadacentral-01.azurewebsites.net/api/tiposcomercio', tipoComercio) // URL de la API
      .subscribe(
        (response) => {
          console.log('Tipo de comercio registrado:', response);
          alert('Registro exitoso');
          this.router.navigate(['/sidenavA']); // Redirigimos a la página de administración
        },
        (error) => {
          console.error('Error al registrar tipo de comercio:', error);
          alert('Hubo un error al registrar el tipo de comercio');
        }
      );
  }
}
