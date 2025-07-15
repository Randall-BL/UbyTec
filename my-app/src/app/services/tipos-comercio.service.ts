import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TiposComercioService {
  private baseUrl = 'https://api3tecuby-ggdga2ahetfef0fr.canadacentral-01.azurewebsites.net/api/TiposComercio'; // Cambia la URL según tu configuración

  constructor(private http: HttpClient) {}

  // Obtener todos los tipos de comercio
  getTiposComercio(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}`);
  }

  // Obtener los nombres de todos los tipos de comercio
  getNombresTiposComercio(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/nombres`);
  }

  // Obtener un tipo de comercio por ID
  getTipoComercioById(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/${id}`);
  }

  // Crear un nuevo tipo de comercio
  createTipoComercio(tipoComercio: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}`, tipoComercio);
  }

  // Actualizar un tipo de comercio por ID
  updateTipoComercio(id: number, tipoComercio: any): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, tipoComercio);
  }

  // Eliminar un tipo de comercio por ID
  deleteTipoComercioById(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  // Eliminar un tipo de comercio por nombre
  deleteTipoComercioByName(nombre: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/eliminarPorNombre/${nombre}`);
  }
}
