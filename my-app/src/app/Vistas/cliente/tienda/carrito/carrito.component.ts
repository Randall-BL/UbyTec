import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CarritoService } from '../../../../services/carrito.service';
import { AuthService } from '../../../../services/auth.service';
import { AfiliadoService } from '../../../../services/afiliado.service';
import { DetallesPedidoService } from '../../../../services/detalles-pedido.service'; // Servicio de DetallesPedido

@Component({
  selector: 'app-carrito',
  standalone: true,
  imports: [FormsModule, HttpClientModule, CommonModule],
  templateUrl: './carrito.component.html',
  styleUrls: ['./carrito.component.css'],
  providers: [AfiliadoService, AuthService, DetallesPedidoService]
})
export class CarritoComponent implements OnInit {
  productosCarrito: any[] = [];
  negocioId: number | null = null;

  mostrarModalTarjeta: boolean = false;
  numeroTarjeta: string = '';
  fechaExpiracion: string = '';
  cvv: string = '';

  constructor(
    private router: Router,
    private carritoService: CarritoService,
    private detallesPedidoService: DetallesPedidoService, // Servicio inyectado
    private authService: AuthService // Servicio para obtener el cliente logueado
  ) {}

  ngOnInit() {
    this.productosCarrito = this.carritoService.obtenerProductos();
    this.negocioId = this.carritoService.obtenerNegocioId();
  }

  calcularTotal(producto: any): number {
    return producto.precio * producto.cantidad;
  }

  obtenerPrecioSubTotal(): number {
    return this.productosCarrito.reduce((total, producto) => total + this.calcularTotal(producto), 0);
  }

  obtenerPrecioFinal(): number {
    const total = this.productosCarrito.reduce((total, producto) => total + this.calcularTotal(producto), 0);
    const costoExtra = total * 0.05; // Costo adicional del 5%
    return total + costoExtra;
  }

  actualizarCantidad(producto: any) {
    if (producto.cantidad <= 0) {
      const confirmacion = confirm(`¿Deseas eliminar ${producto.nombre} del carrito?`);
      if (confirmacion) {
        this.eliminarProducto(producto);
      } else {
        producto.cantidad = 1; // Si cancela, resetea la cantidad a 1
      }
    } else {
      this.carritoService.actualizarProducto(producto);
    }
  }

  eliminarProducto(producto: any) {
    this.carritoService.eliminarProducto(producto);
    this.productosCarrito = this.carritoService.obtenerProductos();
  }

  agregarMasArticulos() {
    if (this.negocioId) {
      this.router.navigate(['/sidenavC/tienda']);
    }
  }

  abrirModalTarjeta() {
    this.mostrarModalTarjeta = true;
  }

  cerrarModalTarjeta() {
    this.mostrarModalTarjeta = false;
  }

  formatearNumeroTarjeta() {
    this.numeroTarjeta = this.numeroTarjeta
      .replace(/\D/g, '') // Eliminar caracteres no numéricos
      .replace(/(\d{4})(?=\d)/g, '$1 '); // Añadir espacio cada 4 dígitos
  }

  formatearFechaExpiracion() {
    this.fechaExpiracion = this.fechaExpiracion
      .replace(/\D/g, '') // Eliminar caracteres no numéricos
      .replace(/(\d{2})(\d+)/, '$1/$2') // Insertar "/" después de los primeros dos dígitos
      .substring(0, 7); // Limitar a 7 caracteres (MM/AAAA)
  }

  confirmarPedido() {
    if (this.validarDatosPago()) {
      const clienteSesion = this.authService.obtenerSesionCliente();
      if (!clienteSesion || !clienteSesion.clienteID) {
        alert('No se pudo obtener la información del cliente logueado.');
        return;
      }

      const detallePedido = {
        afiliadoID: this.negocioId, // Corresponde al afiliadoID
        clienteID: clienteSesion.clienteID, // Obtener ID del cliente logueado
        repartidorID: null, // Se asignará automáticamente en el backend
        estado: 'Recibido', // Estado inicial
        productos: this.productosCarrito.map(producto => ({
          productoID: producto.id, // Asegúrate de que cada producto tenga un ID
          cantidad: producto.cantidad,
          precio: producto.precio
        }))
      };

      // Enviar el detalle del pedido a la API
      this.detallesPedidoService.createDetallePedido(detallePedido).subscribe({
        next: (response: any) => {
          const totalAPagar = this.obtenerPrecioFinal(); // Calcula el monto total del pedido

          alert(`Pedido confirmado exitosamente. Monto total: ${totalAPagar.toFixed(2)}`);
          this.carritoService.vaciarCarrito(); // Vaciar el carrito
          this.mostrarModalTarjeta = false;

          // Guardar el detalle del pedido en el servicio (opcional)
          this.carritoService.guardarPedido({ ...response, total: totalAPagar });

          // Redirigir al componente de detalle del pedido
          this.router.navigate(['/sidenavC/pedidos-activos'], { queryParams: { pedidoID: response.detalleID } });
        },
        error: (err) => {
          if (err.status === 409) {
            // Manejo del error cuando no hay repartidores disponibles
            alert('No hay repartidores disponibles en este momento. Por favor, inténtelo más tarde.');
          } else if (err.status === 400) {
            // Manejo de otros errores específicos del backend
            alert(`Error: ${err.error}`);
          } else {
            // Error genérico
            console.error('Error al confirmar el pedido:', err);
            alert('Hubo un error al confirmar el pedido. Por favor, inténtalo de nuevo.');
          }
        }
      });
    }
  }




  validarDatosPago(): boolean {
    const tarjetaRegex = /^\d{16}$/;
    const tarjetaSinEspacios = this.numeroTarjeta.replace(/\s/g, '');

    if (!tarjetaRegex.test(tarjetaSinEspacios)) {
      alert('El número de tarjeta debe contener exactamente 16 dígitos.');
      return false;
    }

    const fechaExpiracionRegex = /^(0[1-9]|1[0-2])\/(\d{4})$/;
    const match = this.fechaExpiracion.match(fechaExpiracionRegex);

    if (!match) {
      alert('Por favor, ingrese una fecha de expiración válida en el formato MM/AAAA.');
      return false;
    }

    const mes = parseInt(match[1], 10);
    const anio = parseInt(match[2], 10);
    const anioActual = new Date().getFullYear();

    if (anio < anioActual || (anio === anioActual && mes < new Date().getMonth() + 1)) {
      alert('La fecha de expiración debe ser posterior a la fecha actual.');
      return false;
    }

    if (anio <= 2024) {
      alert('El año de expiración debe ser mayor al 2024.');
      return false;
    }

    const cvvRegex = /^\d{3}$/;
    if (!cvvRegex.test(this.cvv)) {
      alert('El CVV debe contener exactamente 3 dígitos.');
      return false;
    }

    return true;
  }

  carritoVacio(): boolean {
    return this.productosCarrito.length === 0;
  }

  backToTienda() {
    if (!this.carritoVacio()) {
      const confirmacion = confirm('¿Estás seguro de que deseas cancelar el pedido?');
      if (confirmacion) {
        this.carritoService.vaciarCarrito();
      }
    }
    this.router.navigate(['/sidenavC/tienda']);
  }
}
