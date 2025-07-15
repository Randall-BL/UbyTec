import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CarritoService {
  private productosCarrito: any[] = [];
  private negocioId: number | null = null;
  private pedidoActual: any = null;

  agregarProducto(producto: any, negocioId: number): boolean {
    if (this.negocioId && this.negocioId !== negocioId) {
      alert("Solo puedes añadir productos del mismo negocio.");
      return false; // No se agregó el producto
    }

    if (!this.negocioId) {
      this.negocioId = negocioId; // Almacenar el ID del negocio si es el primer producto
    }

    const productoExistente = this.productosCarrito.find(p => p.id === producto.id);
    if (productoExistente) {
      productoExistente.cantidad += 1;
    } else {
      this.productosCarrito.push({ ...producto, cantidad: 1 });
    }

    return true; // Producto agregado con éxito
  }

  obtenerProductos(): any[] {
    return this.productosCarrito;
  }

  obtenerNegocioId(): number | null {
    return this.negocioId;
  }

  actualizarProducto(productoActualizado: any) {
    const index = this.productosCarrito.findIndex(p => p.id === productoActualizado.id);
    if (index !== -1) {
      this.productosCarrito[index] = productoActualizado;
    }
  }

  eliminarProducto(producto: any) {
    this.productosCarrito = this.productosCarrito.filter(p => p.id !== producto.id);
    if (this.productosCarrito.length === 0) {
      this.negocioId = null; // Restablecer el ID del negocio si el carrito está vacío
    }
  }

  vaciarCarrito() {
    this.productosCarrito = [];
    this.negocioId = null;
  }

  guardarPedido(pedido: any) {
    this.pedidoActual = pedido;
  }

  obtenerPedido(): any {
    return this.pedidoActual;
  }
}

