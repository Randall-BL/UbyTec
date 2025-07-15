import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PedidoService {
  private baseUrl = 'https://api3tecuby-ggdga2ahetfef0fr.canadacentral-01.azurewebsites.net/api/Pedido'; // Cambia si el endpoint tiene otra URL base

  constructor(private http: HttpClient) {}

  // Obtener todos los pedidos
  getPedidos(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}`);
  }

  // Obtener pedidos por ClienteID
  getPedidosByClienteId(clienteId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/cliente/${clienteId}`);
  }
  // Obtener reporte general de ventas
  getVentasGenerales(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/ventasGenerales`);
  }

  // Obtener pedidos por AfiliadoID
  getPedidosByAfiliadoId(afiliadoId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/afiliado/${afiliadoId}`);
  }

  // Obtener pedidos agrupados por Afiliado
  getPedidosAgrupadosPorAfiliado(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/porAfiliado`);
  }

  // Mover ventas completadas a Pedidos (POST para ejecutar el procedimiento almacenado)
  moverVentasCompletadas(): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/moverVentasCompletadas`, {});
  }

  // Obtener un pedido por ID
  getPedidoById(pedidoId: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/${pedidoId}`);
  }

  // Eliminar un pedido por ID
  deletePedido(pedidoId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${pedidoId}`);
  }
}
