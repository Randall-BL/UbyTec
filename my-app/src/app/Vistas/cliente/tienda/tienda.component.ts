import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import {AfiliadoService} from '../../../services/afiliado.service';
import {CommonModule, NgForOf, NgIf} from '@angular/common';
import { CarritoService } from '../../../services/carrito.service';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-tienda',
  standalone: true,
  imports: [FormsModule, HttpClientModule, NgForOf, RouterLink, NgIf, CommonModule],
  templateUrl: './tienda.component.html',
  styleUrl: './tienda.component.css',
  providers: [
    AfiliadoService,
    AuthService
  ]
})
export class TiendaComponent implements OnInit {
  negocios: any[] = [];
  provincia: string = '';
  canton: string = '';
  distrito: string = '';

  constructor(
    private router: Router,
    private afiliadosService: AfiliadoService,
    private authService: AuthService
  ) {
  }

  ngOnInit() {
    const cliente = this.authService.obtenerSesionCliente();
    if (cliente) {
      this.provincia = cliente.direccionProvincia;
      this.canton = cliente.direccionCanton;
      this.distrito = cliente.direccionDistrito;

      this.afiliadosService.getAfiliadosConProductosPorUbicacion(this.provincia, this.canton, this.distrito).subscribe(
        (data) => {
          this.negocios = data.map((afiliado: any) => ({
            id: afiliado.afiliadoID,
            Nombre: afiliado.nombreComercio,
            Provincia: afiliado.direccionProvincia,
            Canton: afiliado.direccionCanton,
            Distrito: afiliado.direccionDistrito,
            Telefono1: afiliado.telefono1,
          }));
        },
        (error) => {
          console.error('Error al obtener los negocios:', error);
        }
      );
    } else {
      console.error('No hay un cliente autenticado.');
    }
  }

  verDetallesNegocio(id: number) {
    console.log("Navegando al detalle del negocio con ID:", id);
    this.router.navigate(['/negocio-detalle', id]); // Navega a la página de detalle con el ID del negocio
  }
}
