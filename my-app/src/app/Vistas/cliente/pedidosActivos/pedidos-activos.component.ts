import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';
import { DetallesPedidoService } from '../../../services/detalles-pedido.service';
import { PedidoService } from '../../../services/pedido.service';
import { AuthService } from '../../../services/auth.service';
import { FeedbackService } from '../../../services/feedback.service';
import { FormsModule } from '@angular/forms'; // Importar FormsModule

@Component({
  selector: 'app-pedidos-activos',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule // Agregar FormsModule
  ],
  providers: [
    DetallesPedidoService,
    PedidoService,
    AuthService,
    FeedbackService
  ],
  templateUrl: './pedidos-activos.component.html',
  styleUrls: ['./pedidos-activos.component.css']
})
export class PedidosActivosComponent implements OnInit {
  pedidos: any[] = [];
  mostrarModalRechazo: boolean = false;
  mostrarComentario: boolean = false;
  comentario: string = '';
  ultimoPedido: any | null = null;

  constructor(
    private detallesPedidoService: DetallesPedidoService,
    private pedidoService: PedidoService,
    private feedbackService: FeedbackService,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const cliente = this.authService.obtenerSesionCliente();
    if (cliente) {
      this.cargarPedidos(cliente.clienteID); // Cargar pedidos activos del cliente
    }
  }

  cargarPedidos(clienteId: number): void {
    this.detallesPedidoService.getDetallesPedidosByClienteId(clienteId).subscribe(
      (data) => {
        this.pedidos = data;
        console.log('Pedidos cargados:', this.pedidos);
      },
      (error) => {
        console.error('Error al cargar pedidos activos', error);
      }
    );
  }

  calcularTotal(pedido: any): number {
    return pedido.productos.reduce((acc: number, prod: any) => acc + prod.precio * prod.cantidad, 0);
  }

  cerrarFormularioRechazo() {
    this.mostrarModalRechazo = false;
    this.comentario = '';
  }

  marcarComoCompletado(pedido: any): void {
    pedido.estado = 'En proceso';
    this.detallesPedidoService.updateDetallePedido(pedido.detalleID, pedido).subscribe(
      () => {
        this.pedidoService.moverVentasCompletadas().subscribe({
          next: () => {
            alert('Pedido completado y movido con éxito.');
          },
          error: (error) => {
            console.error('Alerta', error);
            alert('Pedido completado y movido con éxito.');

            // Obtener el último pedido después de mover
            const cliente = this.authService.obtenerSesionCliente();
            if (cliente) {
              this.pedidoService.getPedidosByClienteId(cliente.clienteID).subscribe(
                (pedidos) => {
                  if (pedidos.length > 0) {
                    const ultimoPedido = pedidos[pedidos.length - 1];
                    this.mostrarModalRechazo = true;
                    this.ultimoPedido = ultimoPedido;
                    console.log('Último pedido obtenido para comentario:', ultimoPedido);
                  } else {
                    console.warn('No se encontraron pedidos para comentar.');
                  }
                },
                (getError) => {
                  console.error('Error al obtener pedidos por cliente ID', getError);
                }
              );
            }
          }
        });
      },
      (updateError) => {
        console.error('Error al actualizar el estado del pedido', updateError);
        alert('Error al marcar el pedido como completado.');
      }
    );
  }

  enviarComentario(): void {
    if (!this.comentario.trim()) {
      alert('Debe ingresar un comentario antes de enviarlo.');
      return;
    }

    if (this.ultimoPedido) {
      const feedback = {
        pedidoID: this.ultimoPedido.pedidoID,
        clienteID: this.ultimoPedido.clienteID,
        afiliadoID: this.ultimoPedido.afiliadoID,
        repartidorID: this.ultimoPedido.repartidorID,
        cantidadProducto: this.ultimoPedido.cantidadProducto,
        nombreProducto: this.ultimoPedido.nombreProducto,
        montoProducto: this.ultimoPedido.montoProducto,
        total: this.ultimoPedido.total,
        nombreCliente: this.ultimoPedido.nombreCliente,
        nombreComercio: this.ultimoPedido.nombreComercio,
        nombreRepartidor: this.ultimoPedido.nombreRepartidor,
        comentario: this.comentario
      };

      this.feedbackService.createFeedback(feedback).subscribe({
        next: () => {
          alert('Comentario enviado con éxito.');
          this.mostrarComentario = false;
          this.comentario = '';
          this.router.navigateByUrl('/sidenavC/tienda', { skipLocationChange: true }).then(() => {
            this.router.navigate(['/sidenavC/tienda']); // Redirigir a la ruta correcta
          });
        },
        error: (error) => {
          console.error('Error al enviar comentario', error);
          alert('Error al enviar el comentario.');
        }
      });
    }
  }
}
