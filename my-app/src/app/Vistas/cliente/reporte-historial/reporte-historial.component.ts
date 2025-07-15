import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { PedidoService } from '../../../services/pedido.service';
import { FeedbackService } from '../../../services/feedback.service'; // Importar el servicio de feedback
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reporte-historial',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule
  ],
  providers: [
    PedidoService,
    FeedbackService, // Agregar el servicio de feedback como proveedor
    AuthService
  ],
  templateUrl: './reporte-historial.component.html',
  styleUrls: ['./reporte-historial.component.css']
})
export class ReporteHistorialComponent implements OnInit {
  pedidos: any[] = []; // Array para almacenar los pedidos obtenidos
  comentarios: { [key: number]: string } = {}; // Objeto para almacenar los comentarios por pedidoID
  totalVentas: number = 0; // Variable para el total de ventas

  constructor(
    private authService: AuthService,
    private pedidoService: PedidoService,
    private feedbackService: FeedbackService, // Servicio de feedback
    private router: Router
  ) {}

  ngOnInit(): void {
    const cliente = this.authService.obtenerSesionCliente();
    if (cliente) {
      this.cargarPedidos(cliente.clienteID);
    } else {
      alert('No se ha encontrado la sesión del cliente.');
      this.router.navigate(['/inicio-C/tipo1']);
    }
  }

  cargarPedidos(clienteId: number): void {
    this.pedidoService.getPedidosByClienteId(clienteId).subscribe({
      next: (data) => {
        this.pedidos = data;
        this.calcularTotalVentas();
        this.cargarComentarios(); // Cargar los comentarios relacionados con los pedidos
      },
      error: (error) => {
        console.error('Error al cargar los pedidos', error);
      }
    });
  }

  calcularTotalVentas(): void {
    this.totalVentas = this.pedidos.reduce((acc, pedido) => acc + pedido.total, 0);
  }
  cargarComentarios(): void {
    // Iterar sobre los pedidos y cargar los comentarios por PedidoID
    this.pedidos.forEach((pedido) => {
      this.feedbackService.getFeedbackByPedidoId(pedido.pedidoID).subscribe({
        next: (feedbacks) => {
          if (feedbacks.length > 0) {
            this.comentarios[pedido.pedidoID] = feedbacks[0].comentario; // Asignar el comentario
          } else {
            this.comentarios[pedido.pedidoID] = 'Sin comentario'; // Por defecto
          }
        },
        error: (error) => {
          console.error(`Error al cargar comentario para PedidoID ${pedido.pedidoID}:`, error);
          this.comentarios[pedido.pedidoID] = 'Sin comentario'; // Mensaje de error
        }
      });
    });
  }
}
