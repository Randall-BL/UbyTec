import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { AfiliadoService } from '../../../services/afiliado.service';
import { TiposComercioService } from '../../../services/tipos-comercio.service';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-gestion-negocio',
  standalone: true,
  imports: [
    FormsModule, HttpClientModule, CommonModule
  ],
  providers: [
    AfiliadoService,
    TiposComercioService,
    AuthService
  ],
  templateUrl: './gestion-negocio.component.html',
  styleUrls: ['./gestion-negocio.component.css']
})
export class GestionNegocioComponent implements OnInit {
  email: string = '';
  cedula: string = '';
  nombre: string = '';
  tipo: string = '';
  provincia: string = '';
  canton: string = '';
  distrito: string = '';
  sinpe: string = '';
  tiposComercio: string[] = [];

  constructor(
    private router: Router,
    private afiliadoService: AfiliadoService,
    private tiposComercioService: TiposComercioService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const sesionAfiliado = this.authService.obtenerSesionAfiliado();
    if (sesionAfiliado) {
      this.cedula = sesionAfiliado.numeroCedulaJuridica;
      this.cargarDatosAfiliado(this.cedula);
    } else {
      alert('No se pudo obtener la sesión del afiliado. Por favor, inicie sesión de nuevo.');
      this.router.navigate(['/inicio-N/tipo3']);
    }

    // Cargar los tipos de comercio
    this.tiposComercioService.getTiposComercio().subscribe({
      next: (data) => {
        // Extraer solo los nombres de tipoComercio
        this.tiposComercio = data.map((tipo: any) => tipo.nombreTipo);
        console.log('Nombres de tipos de comercio:', this.tiposComercio);
      },
      error: (error) => {
        console.error('Error al obtener tipos de comercio:', error);
      }
    });
  }

  cargarDatosAfiliado(cedula: string) {
    this.afiliadoService.obtenerAfiliadoPorCedulaJuridica(cedula).subscribe({
      next: (afiliado: any) => {
        this.nombre = afiliado.nombreComercio;
        this.tipo = afiliado.tipoComercio;
        this.provincia = afiliado.direccionProvincia;
        this.canton = afiliado.direccionCanton;
        this.distrito = afiliado.direccionDistrito;
        this.email = afiliado.correoElectronico;
        this.sinpe = afiliado.numeroSINPE;
      },
      error: (error) => {
        console.error('Error al cargar datos del afiliado:', error);
        alert('Ocurrió un error al cargar los datos del afiliado.');
      }
    });
  }

  formatearTelefonoSinpe() {
    let sinpeLimpio = this.sinpe.replace(/\D/g, ''); // Remover caracteres no numéricos

    if (sinpeLimpio.length > 8) {
      sinpeLimpio = sinpeLimpio.slice(0, 8);
    }

    if (sinpeLimpio.length > 4) {
      this.sinpe = `${sinpeLimpio.slice(0, 4)}-${sinpeLimpio.slice(4)}`;
    } else {
      this.sinpe = sinpeLimpio;
    }
  }


  modificarNegocio() {
    console.log('Formulario enviado para modificar el negocio');

    // Validar campos requeridos
    const camposFaltantes: string[] = [];
    if (!this.cedula) camposFaltantes.push('Número de Cédula');
    if (!this.nombre) camposFaltantes.push('Nombre');
    if (!this.tipo) camposFaltantes.push('Tipo de comercio');
    if (!this.provincia) camposFaltantes.push('Provincia');
    if (!this.canton) camposFaltantes.push('Cantón');
    if (!this.distrito) camposFaltantes.push('Distrito');
    if (!this.email) camposFaltantes.push('Correo');
    if (!this.sinpe) camposFaltantes.push('Número de SINPE');

    if (camposFaltantes.length > 0) {
      alert(`Por favor, complete los siguientes campos: ${camposFaltantes.join(', ')}`);
      return;
    }

    // Obtener los valores actuales del afiliado antes de la modificación
    this.afiliadoService.obtenerAfiliadoPorCedulaJuridica(this.cedula).subscribe({
      next: (afiliadoExistente: any) => {
        if (afiliadoExistente) {
          // Crear el objeto del negocio con los datos modificados y agregar los valores requeridos
          const afiliadoData = {
            numeroCedulaJuridica: this.cedula,
            nombreComercio: this.nombre,
            tipoComercio: this.tipo,
            direccionProvincia: this.provincia,
            direccionCanton: this.canton,
            direccionDistrito: this.distrito,
            correoElectronico: this.email,
            numeroSINPE: this.sinpe,
            estado: afiliadoExistente.estado, // Mantener el valor existente del estado
            administrador: afiliadoExistente.administrador // Mantener el valor existente del administrador
          };

          // Log para revisar qué datos se están enviando en el PUT
          console.log('Datos del afiliado enviados para modificación:', JSON.stringify(afiliadoData, null, 2));

          // Realizar la solicitud PUT
          this.afiliadoService.actualizarAfiliadoPorCedulaJuridica(this.cedula, afiliadoData).subscribe({
            next: () => {
              alert('Negocio modificado con éxito');
              this.router.navigate(['/sidenavN']);
            },
            error: (error) => {
              console.error('Error al modificar negocio:', error);
              console.log('Datos enviados:', afiliadoData);
              alert('Ocurrió un error al modificar el negocio. Por favor, inténtelo de nuevo.');
            }
          });
        }
      },
      error: (error) => {
        console.error('Error al obtener afiliado antes de modificar:', error);
        alert('No se pudo obtener el afiliado existente. Por favor, inténtelo de nuevo.');
      }
    });
  }

  backTobienvenida() {
    this.router.navigate(['/sidenavN']);
  }
}
