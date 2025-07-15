import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DetallesPedidoService {
  private baseUrl = 'https://api3tecuby-ggdga2ahetfef0fr.canadacentral-01.azurewebsites.net/api/DetallesPedido'; // Cambia si el endpoint tiene otra URL base

  constructor(private http: HttpClient) {}

  // Obtener todos los detalles de pedido
  getDetallesPedidos(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}`);
  }

  // Obtener detalles de pedido por ClienteID
  getDetallesPedidosByClienteId(clienteId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/porCliente/${clienteId}`);
  }

  // Obtener detalles de pedido por AfiliadoID
  getDetallesPedidosByAfiliadoId(afiliadoId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/porAfiliado/${afiliadoId}`);
  }

  // Crear un nuevo detalle de pedido
  createDetallePedido(detallePedido: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}`, detallePedido);
  }

  // Actualizar un detalle de pedido por ID
  updateDetallePedido(detalleId: number, detallePedido: any): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/${detalleId}`, detallePedido);
  }

  // Eliminar un detalle de pedido por ID
  deleteDetallePedido(detalleId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${detalleId}`);
  }
}
