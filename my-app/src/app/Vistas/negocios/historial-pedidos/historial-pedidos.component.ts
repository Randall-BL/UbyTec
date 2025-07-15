import { Component } from '@angular/core';
import {AuthService} from '../../../services/auth.service';
import {PedidoService} from '../../../services/pedido.service';
import {Router} from '@angular/router';
import {CommonModule} from '@angular/common';
import {HttpClientModule} from '@angular/common/http';

@Component({
  selector: 'app-historial-pedidos',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule
  ],
  providers: [
    PedidoService,
    AuthService
  ],
  templateUrl: './historial-pedidos.component.html',
  styleUrl: './historial-pedidos.component.css'
})
export class HistorialPedidosComponent {pedidos: any[] = []; // Array para almacenar los pedidos obtenidos
  totalVentas: number = 0; // Variable para el total de ventas

  constructor(
    private authService: AuthService,  // Servicio de autenticación
    private pedidoService: PedidoService, // Servicio de pedidos
    private router: Router
  ) {}

  ngOnInit(): void {
    // Obtener el afiliadoID desde el AuthService
    const afiliado = this.authService.obtenerSesionAfiliado();
    if (afiliado) {
      // Llamar al servicio de pedidos con el afiliadoID
      this.cargarPedidos(afiliado.afiliadoID);
    } else {
      alert("No se ha encontrado la sesión del cliente.");
      this.router.navigate(['/inicio-N/tipo3']); // Redirigir si no hay sesión
    }
  }

  cargarPedidos(afiliadoID: number): void {
    this.pedidoService.getPedidosByAfiliadoId(afiliadoID).subscribe({
      next: (data) => {
        this.pedidos = data;
        this.calcularTotalVentas(); // Calcular el total de ventas después de cargar los pedidos
      },
      error: (error) => {
        console.error('Error al cargar los pedidos', error);
      }
    });
  }

  // Calcular el total de ventas
  calcularTotalVentas(): void {
    this.totalVentas = this.pedidos.reduce((acc, pedido) => acc + pedido.total, 0);
  }
}
