import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductoService {
  private apiUrl = 'https://api3tecuby-ggdga2ahetfef0fr.canadacentral-01.azurewebsites.net/api/productos';

  constructor(private http: HttpClient) {}

  // Método para registrar un nuevo producto
  registrarProducto(producto: any): Observable<any> {
    return this.http.post(`${this.apiUrl}`, producto);
  }

  // Método para buscar un producto por afiliadoID y nombreProducto
    buscarProductoPorAfiliadoYNombre(afiliadoID: number, nombreProducto: string): Observable<any> {
        return this.http.get(`${this.apiUrl}/buscarPorAfiliadoYNombre`, {
        params: {
            afiliadoID: afiliadoID.toString(),
            nombreProducto: nombreProducto
        }
        });
    }
  

  // Método para actualizar un producto existente
  actualizarProducto(productoID: number, producto: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${productoID}`, producto);
  }
}
