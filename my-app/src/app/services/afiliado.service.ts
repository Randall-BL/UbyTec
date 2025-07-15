import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AfiliadoService {
  private apiUrl = 'https://api3tecuby-ggdga2ahetfef0fr.canadacentral-01.azurewebsites.net/api/afiliados';

  constructor(private http: HttpClient, private authService: AuthService) {}

  // Método para registrar un nuevo afiliado
  registrarAfiliado(afiliado: any): Observable<any> {
    return this.http.post(`${this.apiUrl}`, afiliado);
  }

  // Método para iniciar sesión del afiliado usando correo y contraseña
  iniciarSesionAfiliado(correo: string, password: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/iniciarSesion`, {
      params: {
        correoElectronico: correo,
        password: password
      }
    });
  }

  // Método para manejar la sesión del afiliado
  iniciarSesionYGuardarSesion(correo: string, password: string): Observable<any> {
    return new Observable((observer) => {
      this.iniciarSesionAfiliado(correo, password).subscribe({
        next: (afiliado) => {
          this.authService.guardarSesionAfiliado(afiliado); // Guardar sesión del afiliado
          console.log("Sesión guardada para afiliado:", afiliado);
          observer.next(afiliado);
        },
        error: (error) => {
          observer.error(error);
        }
      });
    });
  }

  // Método para obtener afiliados con estado vacío
  obtenerAfiliadosConEstadoVacio(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/estadoVacio`);
  }


  // Método para obtener un afiliado por cédula jurídica
  obtenerAfiliadoPorCedulaJuridica(numeroCedulaJuridica: string): Observable<any> {
    // Mantener los guiones en la cédula jurídica
    const url = `${this.apiUrl}/buscarPorCedulaJuridica/${encodeURIComponent(numeroCedulaJuridica)}`;
    console.log("=== GET Request ===");
    console.log("URL:", url);
    console.log("Cédula jurídica enviada:", numeroCedulaJuridica);

    return this.http.get(url);
  }

  // Método para actualizar un afiliado por cédula jurídica
  actualizarAfiliadoPorCedulaJuridica(numeroCedulaJuridica: string, afiliadoData: any): Observable<any> {
    // Mantener los guiones en la cédula jurídica
    const url = `${this.apiUrl}/modificarPorCedulaJuridica/${encodeURIComponent(numeroCedulaJuridica)}`;
    console.log("=== PUT Request ===");
    console.log("URL:", url);
    console.log("Datos enviados para actualización:", JSON.stringify(afiliadoData, null, 2));

    return this.http.put(url, afiliadoData);
  }

  // Método para obtener afiliados con productos asociados
  getAfiliadosConProductos(): Observable<any[]> {
    const url = `${this.apiUrl}/productos`;
    return this.http.get<any[]>(url);
  }

  // Método para obtener afiliados por estado vacío (estado == "")
  obtenerAfiliadosSinEstado(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/estado`, {
      params: { estado: "" }
    });
  }

  // Método para aceptar un afiliado
  aceptarAfiliado(numeroCedulaJuridica: string): Observable<void> {
    const url = `${this.apiUrl}/aceptar/${numeroCedulaJuridica}`;
    console.log("=== ACEPTAR Afiliado ===");
    console.log("URL:", url);
    console.log("Cédula jurídica enviada para aceptar:", numeroCedulaJuridica);

    return this.http.put<void>(url, {});
  }

  // Método para rechazar un afiliado con comentario
  rechazarAfiliado(numeroCedulaJuridica: string, comentario: string): Observable<void> {
    const url = `${this.apiUrl}/rechazar/${numeroCedulaJuridica}`;
    console.log("=== RECHAZAR Afiliado ===");
    console.log("URL:", url);
    console.log("Cédula jurídica enviada para rechazar:", numeroCedulaJuridica);
    console.log("Comentario de rechazo:", comentario);

    // Asegurarse de que se envía el comentario como un string simple en el cuerpo
    return this.http.put<void>(url, JSON.stringify(comentario), {
      headers: {
        'Content-Type': 'application/json'
      }
    });
  }

  // Método para eliminar un afiliado por su número de cédula jurídica
  eliminarAfiliadoPorCedulaJuridica(numeroCedulaJuridica: string): Observable<void> {
    // Asegurarse de que el número de cédula jurídica se envíe con el formato correcto
    const cedulaFormateada = this.formatearCedula(numeroCedulaJuridica);
    const url = `${this.apiUrl}/eliminarPorCedulaJuridica/${encodeURIComponent(cedulaFormateada)}`;

    console.log("=== DELETE Request ===");
    console.log("URL:", url);
    console.log("Cédula enviada para eliminación:", cedulaFormateada);

    return this.http.delete<void>(url);
  }

  // Método para formatear la cédula con el formato 0-0000-0000
  private formatearCedula(cedula: string): string {
    // Asegurarse de que la cédula tenga una longitud de 9 caracteres antes de formatear
    if (cedula.length === 9) {
      return `${cedula.charAt(0)}-${cedula.slice(1, 5)}-${cedula.slice(5)}`;
    }
    return cedula;
  }

  getAfiliadosConProductosPorUbicacion(provincia: string, canton: string, distrito: string): Observable<any[]> {
    const url = `${this.apiUrl}/productosPorUbicacion`;
    return this.http.get<any[]>(url, { params: { provincia, canton, distrito } });
  }

  // Método para obtener productos de un negocio específico
  getProductosPorNegocio(negocioId: number): Observable<{ nombreComercio: string, productos: any[] }> {
    const url = `${this.apiUrl}/${negocioId}/productos`;
    return this.http.get<any>(url).pipe(
      map(response => ({
        nombreComercio: response.nombreComercio,
        productos: response.productos.map((producto: any) => ({
          id: producto.productoID,
          nombre: producto.nombreProducto,
          categoria: producto.categoria,
          foto: producto.foto,
          precio: producto.precio
        }))
      }))
    );
  }
}

