import {Component, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { AfiliadoService } from '../../../services/afiliado.service'; // Importar el servicio
import { TiposComercioService } from '../../../services/tipos-comercio.service'; // Importar el servicio
import {AdminService} from '../../../services/administrador.service';

@Component({
  selector: 'app-registro-n',
  standalone: true,
  imports: [
    FormsModule, HttpClientModule, CommonModule
  ],
  templateUrl: './registro-n.component.html',
  styleUrls: ['./registro-n.component.css'],
  providers: [
    AfiliadoService, // Añadir el servicio a los providers
    TiposComercioService, // Añadir el servicio a los providers
    AdminService
  ]
})
export class RegistroNComponent implements OnInit {
  email: string = '';
  cedula: string = '';
  nombre: string = '';
  tipo: string = '';
  provincia: string = '';
  canton: string = '';
  distrito: string = '';
  telefonos: string[] = [''];  // Array de teléfonos
  sinpe: string = '';
  admin: string = '';
  estado: string = '';
  tiposComercio: string[] = []; // Array para almacenar los tipos de comercio
  nombresAdministradores: string[] = [];

  constructor(
    private router: Router,
    private afiliadoService: AfiliadoService, // Inyectar el servicio
    private tiposComercioService: TiposComercioService, // Inyectar el servicio
    private adminService: AdminService
  ) {
  }

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

  // Método para formatear el número de teléfono
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

  // Función para formatear el número de teléfono en formato XXXX-XXXX
  formatearTelefono(index: number) {
    let telefonoLimpio = this.telefonos[index].replace(/\D/g, '');

    if (telefonoLimpio.length > 8) {
      telefonoLimpio = telefonoLimpio.slice(0, 8);
    }

    if (telefonoLimpio.length > 4) {
      this.telefonos[index] = `${telefonoLimpio.slice(0, 4)}-${telefonoLimpio.slice(4)}`;
    } else {
      this.telefonos[index] = telefonoLimpio;
    }
  }

  // Función para agregar un nuevo campo de teléfono
  agregarTelefono() {
    if (this.telefonos.length < 3) {
      this.telefonos.push('');
    } else {
      alert("Se permite un máximo de 3 números telefónicos.");
    }
  }

  // Función para remover el último campo de teléfono
  removerTelefono() {
    if (this.telefonos.length > 1) {
      this.telefonos.pop();
    } else {
      alert("Debe agregar al menos un número telefónico.");
    }
  }

  validarTelefonosUnicos(): boolean {
    const numerosUnicos = new Set(this.telefonos.map(telefono => telefono.replace(/\D/g, '')));
    if (numerosUnicos.size !== this.telefonos.length) {
      alert("Los números de teléfono deben ser únicos.");
      return false;
    }
    return true;
  }

  ngOnInit(): void {
    // Cargar los tipos de comercio
    this.tiposComercioService.getNombresTiposComercio().subscribe(
      (data) => {
        this.tiposComercio = data;
      },
      (error) => {
        console.error('Error al obtener tipos de comercio', error);
      }
    );

    // Cargar los nombres de los administradores
    this.adminService.obtenerNombresAdministradores().subscribe(
      (data) => {
        this.nombresAdministradores = data;
      },
      (error) => {
        console.error('Error al obtener nombres de administradores', error);
      }
    );
  }

  backTobienvenida() {
    this.router.navigate(['']);
  }

  Enviar() {
    console.log("Formulario enviado");
    const camposFaltantes = [];

    // Validar campos requeridos
    if (!this.cedula) camposFaltantes.push('Número de Cédula');
    if (!this.nombre) camposFaltantes.push('Nombre');
    if (!this.tipo) camposFaltantes.push('Tipo de comercio');
    if (!this.provincia) camposFaltantes.push('Provincia');
    if (!this.canton) camposFaltantes.push('Cantón');
    if (!this.distrito) camposFaltantes.push('Distrito');
    if (!this.email) camposFaltantes.push('Correo');
    if (!this.sinpe) camposFaltantes.push('Número de SINPE móvil');
    if (!this.admin) camposFaltantes.push('Administrador del comercio');

    // Verificar si hay campos faltantes
    if (camposFaltantes.length > 0) {
      alert(`Por favor, complete los siguientes campos: ${camposFaltantes.join(', ')}`);
      return;
    }

    // Validación de formato para cédula y SINPE
    const cedulaLimpia = this.cedula.replace(/\D/g, '');
    if (cedulaLimpia.length !== 9) {
      alert("La cédula debe tener exactamente 9 dígitos.");
      return;
    }

    // Validación de los teléfonos
    for (const telefono of this.telefonos) {
      const telefonoLimpio = telefono.replace(/\D/g, '');
      if (telefonoLimpio.length !== 8) {
        alert("Cada número de teléfono debe tener exactamente 8 dígitos.");
        return;
      }
    }

    const sinpeLimpio = this.sinpe.replace(/\D/g, '');
    if (sinpeLimpio.length !== 8) {
      alert("El número de SINPE debe tener exactamente 8 dígitos.");
      return;
    }

    // Validar que los números de teléfono sean únicos
    if (!this.validarTelefonosUnicos()) {
      return; // Si hay números duplicados, no continuar
    }

    // Verificar si el afiliado existe por su cédula jurídica
    this.afiliadoService.obtenerAfiliadoPorCedulaJuridica(this.cedula).subscribe({
      next: (afiliadoExistente: any) => {
        if (afiliadoExistente) {
          console.log("Afiliado encontrado. Procediendo a actualizar.");

          // Crear un objeto afiliado para actualizar
          const afiliadoData = {
            numeroCedulaJuridica: this.cedula,
            nombreComercio: this.nombre,
            tipoComercio: this.tipo,
            direccionProvincia: this.provincia,
            direccionCanton: this.canton,
            direccionDistrito: this.distrito,
            correoElectronico: this.email,
            numeroSINPE: this.sinpe,
            administrador: this.admin,
            estado: this.estado,
            telefonos: afiliadoExistente.telefonos.map((telefono: any, index: number) => ({
              telefonoAfiliadoID: telefono.telefonoAfiliadoID,
              afiliadoID: afiliadoExistente.afiliadoID,
              numeroTelefono: this.telefonos[index] ? this.telefonos[index] : telefono.numeroTelefono
            })),
          };

          console.log("Datos del afiliado enviados para actualización:", JSON.stringify(afiliadoData, null, 2));

          this.afiliadoService.actualizarAfiliadoPorCedulaJuridica(this.cedula, afiliadoData).subscribe({
            next: () => {
              alert('Afiliado actualizado con éxito');
              this.router.navigate(['']);
            },
            error: (error) => {
              console.error('Error al actualizar afiliado:', error);
              alert('Ocurrió un error al actualizar el afiliado. Por favor, inténtelo de nuevo.');
            }
          });
        }
      },
      error: (error) => {
        if (error.status === 404) {
          console.log("Afiliado no encontrado. Procediendo a registrar un nuevo afiliado.");

          // Crear un objeto afiliado para registrar
          const afiliadoData = {
            numeroCedulaJuridica: this.cedula,
            nombreComercio: this.nombre,
            tipoComercio: this.tipo,
            direccionProvincia: this.provincia,
            direccionCanton: this.canton,
            direccionDistrito: this.distrito,
            correoElectronico: this.email,
            numeroSINPE: this.sinpe,
            administrador: this.admin,
            estado: this.estado,
            telefonos: this.telefonos.map((telefono: string) => ({
              telefonoAfiliadoID: 0, // ID inicial para nuevos registros
              afiliadoID: 0, // ID inicial, será asignado por la base de datos
              numeroTelefono: telefono
            })),
          };

          console.log("Datos del afiliado enviados para registro:", JSON.stringify(afiliadoData, null, 2));

          this.afiliadoService.registrarAfiliado(afiliadoData).subscribe({
            next: () => {
              alert('Afiliado registrado con éxito');
              this.router.navigate(['']);
            },
            error: (err) => {
              console.error('Error al registrar afiliado:', err);
              alert('Ocurrió un error al registrar el afiliado. Por favor, inténtelo de nuevo.');
            }
          });
        } else {
          console.error('Error al buscar afiliado:', error);
          alert('Ocurrió un error al buscar el afiliado. Por favor, inténtelo de nuevo.');
        }
      }
    });
  }
}
