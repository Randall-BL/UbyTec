import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import {CommonModule, NgForOf, NgIf} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import {AfiliadoService} from '../../../../services/afiliado.service';
import {AuthService} from '../../../../services/auth.service';
import {CarritoService} from '../../../../services/carrito.service';

@Component({
  selector: 'app-negocio-detalle',
  standalone: true,
  imports: [FormsModule, HttpClientModule, NgForOf, RouterLink, NgIf, CommonModule],
  providers: [
    AfiliadoService,
    AuthService
  ],
  templateUrl: './negocio-detalle.component.html',
  styleUrl: './negocio-detalle.component.css'
})
export class NegocioDetalleComponent implements OnInit {
  productos: any[] = [];
  nombreComercio: string = '';
  negocioId: number | null = null;

  constructor(
    private route: ActivatedRoute,
    private afiliadoService: AfiliadoService,
    private carritoService: CarritoService,
    private router: Router
  ) {}

  ngOnInit() {
    // Obtener el ID del negocio de la ruta
    this.negocioId = Number(this.route.snapshot.paramMap.get('id'));

    if (this.negocioId) {
      // Obtener los productos del negocio
      this.afiliadoService.getProductosPorNegocio(this.negocioId).subscribe(
        (data: { nombreComercio: string; productos: any[] }) => {
          this.nombreComercio = data.nombreComercio;
          this.productos = data.productos.map((producto: any) => ({
            id: producto.id,
            nombre: producto.nombre,
            categoria: producto.categoria,
            precio: producto.precio
          }));
        },
        (error: any) => {
          console.error('Error al obtener productos:', error);
        }
      );
    } else {
      console.error('No se proporcionó un ID de negocio válido.');
    }
  }

  anadirAlCarrito(producto: any) {
    if (this.negocioId !== null) {
      const productoAgregado = this.carritoService.agregarProducto(producto, this.negocioId);
      if (productoAgregado) {
        alert(`Producto ${producto.nombre} añadido al carrito.`);
        this.router.navigate(['/carrito']);
      }
    } else {
      alert('Error: No se pudo añadir el producto al carrito.');
    }
  }

  volverATienda() {
    this.router.navigate(['/sidenavC/tienda']); // Redirige a la tienda
  }

}
