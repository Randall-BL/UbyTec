import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly sessionKeyAfiliado = 'afiliadoSession';
  private readonly sessionKeyCliente = 'clienteSession';
  private readonly sessionKeyAdmin = 'adminSession';

  constructor() {}

  // Métodos para gestionar la sesión del afiliado
  guardarSesionAfiliado(afiliado: any): void {
    localStorage.setItem(this.sessionKeyAfiliado, JSON.stringify(afiliado));
  }

  obtenerSesionAfiliado(): any {
    const sessionData = localStorage.getItem(this.sessionKeyAfiliado);
    return sessionData ? JSON.parse(sessionData) : null;
  }

  cerrarSesionAfiliado(): void {
    localStorage.removeItem(this.sessionKeyAfiliado);
  }

  // Métodos para gestionar la sesión del cliente
  guardarSesionCliente(cliente: any): void {
    localStorage.setItem(this.sessionKeyCliente, JSON.stringify(cliente));
  }

  obtenerSesionCliente(): any {
    const sessionData = localStorage.getItem(this.sessionKeyCliente);
    return sessionData ? JSON.parse(sessionData) : null;
  }

  cerrarSesion(): void {
    // Elimina la sesión del cliente, token o cualquier dato almacenado
    localStorage.removeItem('sesionCliente');
    sessionStorage.clear();
  }

  cerrarSesionCliente(): void {
    localStorage.removeItem(this.sessionKeyCliente);
  }

  // Método para verificar si cualquier tipo de usuario está autenticado
  estaAutenticado(): boolean {
    return this.obtenerSesionAfiliado() !== null || this.obtenerSesionCliente() !== null;
  }

  // Métodos para gestionar la sesión del administrador
  guardarSesionAdmin(email: string): void {
    localStorage.setItem(this.sessionKeyAdmin, email);
  }

  obtenerSesionAdmin(): string | null {
    return localStorage.getItem(this.sessionKeyAdmin);
  }

  cerrarSesionAdmin(): void {
    localStorage.removeItem(this.sessionKeyAdmin);
  }

  // Método para determinar el tipo de usuario autenticado
  tipoUsuarioAutenticado(): 'afiliado' | 'cliente' | 'administrador' | null {
    if (this.obtenerSesionAfiliado() !== null) {
      return 'afiliado';
    } else if (this.obtenerSesionCliente() !== null) {
      return 'cliente';
    }
    return null;
  }
}
