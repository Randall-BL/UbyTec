import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';
import { DetallesPedidoService } from '../../../services/detalles-pedido.service';
import { PedidoService } from '../../../services/pedido.service';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-gestion-pedidos',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule
  ],
  providers: [
    DetallesPedidoService,
    PedidoService,
    AuthService
  ],
  templateUrl: './gestion-pedidos.component.html',
  styleUrls: ['./gestion-pedidos.component.css']
})
export class GestionPedidosComponent implements OnInit {
  pedidos: any[] = [];

  constructor(
    private detallesPedidoService: DetallesPedidoService,
    private pedidoService: PedidoService,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const afiliado = this.authService.obtenerSesionAfiliado();
    if (afiliado) {
      this.cargarPedidos(afiliado.afiliadoID);
    }
  }

  cargarPedidos(afiliadoId: number): void {
    this.detallesPedidoService.getDetallesPedidosByAfiliadoId(afiliadoId).subscribe(
      (data) => {
        // Filtrar los pedidos que no están completados
        this.pedidos = data.filter((pedido: any) => pedido.estado !== 'Completado');
      },
      (error) => {
        console.error('Error al cargar pedidos', error);
      }
    );
  }

  marcarEnProceso(pedido: any): void {
    pedido.estado = 'En proceso';
    this.detallesPedidoService.updateDetallePedido(pedido.detalleID, pedido).subscribe(
      () => {
        console.log('Pedido marcado como En proceso');
        this.cargarPedidos(pedido.afiliadoID);
      },
      (error) => {
        console.error('Error al actualizar el estado del pedido', error);
      }
    );
  }

completarPedido(pedido: any): void {
    this.pedidoService.moverVentasCompletadas().subscribe({
      next: () => {
        alert('Pedido completado y movido con éxito.');
        const afiliado = this.authService.obtenerSesionAfiliado();
        if (afiliado) {
          this.cargarPedidos(afiliado.afiliadoID); // Recargar los pedidos
        }
      },
      error: (error) => {
        console.error('Alerta', error);
        alert('Pedido completado y movido con éxito.');
      }
    });
 }
}
