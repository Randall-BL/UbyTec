import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class RepartidorService {
  private apiUrl = 'https://api3tecuby-ggdga2ahetfef0fr.canadacentral-01.azurewebsites.net/api/repartidores';

  constructor(private http: HttpClient, private authService: AuthService) {}

  // Método para registrar un nuevo repartidor
  registrarRepartidor(repartidor: any): Observable<any> {
    return this.http.post(`${this.apiUrl}`, repartidor);
  }

  // Método para obtener un repartidor por usuario
  obtenerRepartidorPorUsuario(usuario: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/buscarPorUsuario/${usuario}`);
  }

  // Método para actualizar un repartidor por usuario
  actualizarRepartidorPorUsuario(usuario: string, repartidor: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/modificarPorUsuario/${usuario}`, repartidor);
  }

  // Método para buscar o registrar un repartidor
  buscarORegistrarRepartidor(repartidor: any): Observable<any> {
    return this.obtenerRepartidorPorUsuario(repartidor.usuario).pipe(
      switchMap((repartidorExistente) => {
        if (repartidorExistente) {
          // Si el repartidor existe, actualizarlo
          return this.actualizarRepartidorPorUsuario(repartidor.usuario, repartidor).pipe(
            map(() => 'Repartidor actualizado con éxito')
          );
        } else {
          return of(''); // Retornar vacío si no existe, para manejar en el siguiente catchError
        }
      }),
      catchError((error) => {
        if (error.status === 404) {
          // Si no se encuentra el repartidor, proceder a registrar uno nuevo
          return this.registrarRepartidor(repartidor).pipe(
            map(() => 'Repartidor registrado con éxito')
          );
        } else {
          throw error;
        }
      })
    );
  }

  eliminarRepartidorPorUsuario(usuario: string): Observable<void> {
    const url = `${this.apiUrl}/eliminarPorUsuario/${encodeURIComponent(usuario)}`;
    console.log("=== DELETE Request ===");
    console.log("URL:", url);
    console.log("Usuario enviado para eliminación:", usuario);

    return this.http.delete<void>(url);
  }
}
