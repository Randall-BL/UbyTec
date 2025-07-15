import { Component } from '@angular/core';
import {FormsModule} from '@angular/forms';
import {Router} from '@angular/router';
import {CommonModule, NgForOf} from '@angular/common';
import { TiposComercioService } from '../../../services/tipos-comercio.service';
import {HttpClientModule} from '@angular/common/http';
import {AfiliadoService} from '../../../services/afiliado.service'; // Importar el servicio

@Component({
  selector: 'app-tipo-comercio',
  standalone: true,
  imports: [
    FormsModule, HttpClientModule, CommonModule
  ],
  providers: [
    TiposComercioService // Añadir el servicio a los providers
  ],
  templateUrl: './tipo-comercio.component.html',
  styleUrl: './tipo-comercio.component.css'
})
export class TipoComercioComponent {
  tipo: string = '';
  tiposComercio: string[] = []; // Array para almacenar los tipos de comercio

  constructor(
    private router: Router,
    private tiposComercioService: TiposComercioService // Inyectar el servicio
  ) {}

  backToAdmin() {
    this.router.navigate(['/sidenavA']);
  }

  ngOnInit(): void {
    // Cargar los tipos de comercio cuando el componente se inicializa
    this.tiposComercioService.getNombresTiposComercio().subscribe(
      (data) => {
        this.tiposComercio = data; // Asignar los tipos de comercio al array
      },
      (error) => {
        console.error('Error al obtener tipos de comercio', error);
      }
    );
  }

  Enviar() {
    if (!this.tipo) {
      alert('Seleccione un tipo de comercio para eliminar.');
      return;
    }

    // Llama al servicio para eliminar el tipo de comercio seleccionado
    this.tiposComercioService.deleteTipoComercioByName(this.tipo).subscribe(
      () => {
        alert('Tipo de Comercio eliminado con éxito.');
        this.router.navigate(['/sidenavA']);
      },
      (error) => {
        console.error('Error al eliminar el tipo de comercio:', error);
        alert('Error al eliminar el tipo de comercio. Por favor, intente de nuevo.');
      }
    );
  }
}

