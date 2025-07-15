import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { AfiliadoService } from '../../../services/afiliado.service';

@Component({
  selector: 'app-administracion-afiliaciones',
  standalone: true,
  imports: [
    FormsModule, HttpClientModule, CommonModule
  ],
  providers: [
    AfiliadoService,
  ],
  templateUrl: './administracion-afiliaciones.component.html',
  styleUrls: ['./administracion-afiliaciones.component.css']
})
export class AdministracionAfiliacionesComponent {
  negocios: any[] = [];
  mostrarModalRechazo: boolean = false;
  numeroCedulaJuridicaRechazo: string = ''; // Para almacenar el ID del negocio a rechazar
  comentarioRechazo: string = ''; // Para almacenar el comentario de rechazo
  noHaySolicitudes: boolean = false; // Indicador para saber si no hay solicitudes disponibles
  mensajeNoSolicitudes: string = ''; // Mensaje para indicar que no hay solicitudes
  mensajeErrorGeneral: string = ''; // Mensaje para indicar errores de conectividad o generales

  constructor(private afiliadosService: AfiliadoService, private router: Router) {}

  ngOnInit() {
    // Cargar afiliados según el estado vacío ""
    this.cargarAfiliadosSinEstado();
  }

  // Método para cargar afiliados por estado vacío ("")
  cargarAfiliadosSinEstado() {
    this.afiliadosService.obtenerAfiliadosConEstadoVacio().subscribe(
      (response: any) => {
        if (Array.isArray(response) && response.length > 0) {
          // Hay afiliados con estado vacío
          this.noHaySolicitudes = false;
          this.mensajeNoSolicitudes = '';
          this.mensajeErrorGeneral = '';
          this.negocios = response.map((afiliado: any) => ({
            nombreComercio: afiliado.nombreComercio,
            numeroCedulaJuridica: afiliado.numeroCedulaJuridica,
            tipoComercio: afiliado.tipoComercio,
            correoElectronico: afiliado.correoElectronico,
          }));
        } else {
          // En el caso de que la respuesta sea un array vacío
          this.noHaySolicitudes = true;
          this.mensajeNoSolicitudes = 'No hay solicitudes por el momento.';
          this.negocios = [];
        }
      },
      (error) => {
        if (error.status === 404 && error.error === 'No se encontraron afiliados con el estado vacío.') {
          // Si obtenemos un error 404 indicando que no hay afiliados
          this.noHaySolicitudes = true;
          this.mensajeNoSolicitudes = 'No hay solicitudes por el momento.';
          this.negocios = [];
        } else if (error.status === 0 && error.statusText === 'Unknown Error') {
          // Manejar el caso de error desconocido, típicamente problemas de conexión
          this.mensajeErrorGeneral = 'Error de conexión: no se pudo comunicar con el servidor. Intente nuevamente más tarde.';
          this.negocios = [];
        } else {
          // Manejar otros errores inesperados
          console.error('Error al obtener afiliados:', error);
          this.mensajeErrorGeneral = 'Ocurrió un error al obtener los afiliados. Intente nuevamente más tarde.';
        }
      }
    );
  }

  mostrarFormularioRechazo(numeroCedulaJuridica: string) {
    this.numeroCedulaJuridicaRechazo = numeroCedulaJuridica;
    this.mostrarModalRechazo = true;
  }

  cerrarFormularioRechazo() {
    this.mostrarModalRechazo = false;
    this.comentarioRechazo = '';
  }

  aceptarAfiliacion(numeroCedulaJuridica: string) {
    const negocio = this.negocios.find(n => n.numeroCedulaJuridica === numeroCedulaJuridica);
    this.afiliadosService.aceptarAfiliado(numeroCedulaJuridica).subscribe(
      () => {
        alert('Solicitud actualizada');
        this.router.navigate(['/sidenavA/general-a']); // Redirigir después de aceptar
      },
      (error) => {
        console.error('No hubo error', error);
        alert('Solicitud actualizada');
        this.router.navigate(['/sidenavA/general-a']); // Redirigir después de aceptar
      }
    );
  }

  rechazarAfiliacion() {
    if (!this.comentarioRechazo.trim()) {
      alert('El comentario de rechazo no puede estar vacío.');
      return;
    }
    const negocio = this.negocios.find(n => n.numeroCedulaJuridica === this.numeroCedulaJuridicaRechazo);
    this.afiliadosService.rechazarAfiliado(this.numeroCedulaJuridicaRechazo, this.comentarioRechazo).subscribe(
      () => {
        alert('Solicitud actualizada');
        this.router.navigate(['/sidenavA/general-a']); // Redirigir después de rechazar
      },
      (error) => {
        console.error('No hubo error', error);
        alert('Solicitud actualizada');
        this.router.navigate(['/sidenavA/general-a']); // Redirigir después de aceptar
      }
    );
  }
}
