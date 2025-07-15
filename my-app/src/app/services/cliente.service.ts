import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ClienteService {
  // URL de la API 
  private apiUrl = 'https://api3tecuby-ggdga2ahetfef0fr.canadacentral-01.azurewebsites.net/api/clientes';

  constructor(private http: HttpClient) {}

  // Método para registrar un nuevo cliente
  registrarCliente(cliente: any): Observable<any> {
    return this.http.post(this.apiUrl, cliente);
  }

  // Método para obtener un cliente por nombre de usuario
  obtenerClientePorUsuario(usuario: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/byusername/${usuario}`);
  }

  // Método para actualizar un cliente por número de cédula
  actualizarClientePorCedula(numeroCedula: string, cliente: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/bycedula/${numeroCedula}`, cliente);
  }

  // Método para eliminar un cliente por número de cédula
  eliminarClientePorCedula(numeroCedula: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/bycedula/${numeroCedula}`);
  }
}
