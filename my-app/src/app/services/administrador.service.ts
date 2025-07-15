import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private baseUrl = 'https://api3tecuby-ggdga2ahetfef0fr.canadacentral-01.azurewebsites.net/api/administradores';

  constructor(private http: HttpClient) { }

  obtenerAdministradorPorCedula(numeroCedula: string): Observable<any> {
    // Ajustar la URL según el formato correcto del endpoint
    const url = `${this.baseUrl}/buscarPorNumeroCedula/${encodeURIComponent(numeroCedula)}`;
    console.log("=== GET Request ===");
    console.log("URL:", url);
    console.log("Cédula enviada:", numeroCedula);

    return this.http.get(url);
  }

  actualizarAdministradorPorCedula(numeroCedula: string, adminData: any): Observable<any> {
    // Ajustar la URL según el formato correcto del endpoint
    const url = `${this.baseUrl}/modificarPorNumeroCedula/${encodeURIComponent(numeroCedula)}`;
    console.log("=== PUT Request ===");
    console.log("URL:", url);
    console.log("Datos enviados para actualización:", JSON.stringify(adminData, null, 2));

    return this.http.put(url, adminData);
  }

  registrarAdministrador(adminData: any): Observable<any> {
    const url = `${this.baseUrl}`;
    console.log("=== POST Request ===");
    console.log("URL:", url);
    console.log("Datos enviados para registro:", JSON.stringify(adminData, null, 2));

    return this.http.post(url, adminData);
  }

  eliminarAdministradorPorCedula(numeroCedula: string): Observable<void> {
    const url = `${this.baseUrl}/eliminarPorNumeroCedula/${encodeURIComponent(numeroCedula)}`;
    console.log("=== DELETE Request ===");
    console.log("URL:", url);
    console.log("Cédula enviada para eliminación:", numeroCedula);

    return this.http.delete<void>(url);
  }

  obtenerNombresAdministradores(): Observable<string[]> {
    const url = `${this.baseUrl}/nombres-completos`;
    return this.http.get<string[]>(url);
  }

}
