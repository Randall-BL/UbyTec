import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ProductoService } from '../../../services/producto.service';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-gestion-productos',
  standalone: true,
  imports: [
    FormsModule, HttpClientModule
  ],
  templateUrl: './gestion-productos.component.html',
  styleUrls: ['./gestion-productos.component.css'],
  providers: [ProductoService]
})
export class GestionProductosComponent {
  nombre: string = '';
  categoria: string = '';
  foto: File | null = null;
  precio: number | null = null;

  constructor(
    private router: Router,
    private productoService: ProductoService,
    private authService: AuthService
  ) {
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.foto = file;
    }
  }

  backToAdmin() {
    this.router.navigate(['/sidenavN']);
  }

  eliminar() {
    this.router.navigate(['/productos']);
  }

  Enviar() {
    console.log("Formulario enviado");

    const camposFaltantes = [];

    // Validar campos faltantes
    if (!this.nombre) camposFaltantes.push('Nombre');
    if (!this.categoria) camposFaltantes.push('Categoría');
    if (!this.foto) camposFaltantes.push('Foto');
    if (!this.precio) camposFaltantes.push('Precio');

    if (camposFaltantes.length > 0) {
      alert(`Por favor, complete los siguientes campos: ${camposFaltantes.join(', ')}`);
      return;
    }

    // Obtener el ID del afiliado desde el AuthService
    const afiliadoID = Number(this.authService.obtenerSesionAfiliado()?.afiliadoID);

    if (isNaN(afiliadoID)) {
      alert('No se pudo obtener la sesión del afiliado. Por favor, inicie sesión de nuevo.');
      return;
    }

    // Crear el objeto del producto con el afiliadoID
    const producto = {
      afiliadoID: afiliadoID,
      nombreProducto: this.nombre,
      categoria: this.categoria,
      foto: this.foto?.name, // O manejar la carga del archivo si es necesario
      precio: this.precio
    };

    console.log("Datos del producto a enviar:", JSON.stringify(producto, null, 2));

    // Verificar si el producto ya existe
    this.productoService.buscarProductoPorAfiliadoYNombre(afiliadoID, this.nombre).subscribe({
      next: (productoExistente) => {
        if (productoExistente) {
          console.log("Producto encontrado. Procediendo a actualizar.");

          // Actualizar el producto
          productoExistente.categoria = this.categoria;
          productoExistente.foto = this.foto?.name;
          productoExistente.precio = this.precio;

          this.productoService.actualizarProducto(productoExistente.productoID, productoExistente).subscribe({
            next: response => {
              console.log('Producto actualizado:', response);
              alert('Producto actualizado exitosamente');
              this.router.navigate(['/sidenavN']);
            },
            error: error => {
              console.error('Error al actualizar producto:', error);
              alert('Ocurrió un error al actualizar el producto. Por favor, inténtelo de nuevo.');
            }
          });
        }
      },
      error: (error) => {
        if (error.status === 404) {
          console.log("Producto no encontrado. Procediendo a registrar un nuevo producto.");

          this.productoService.registrarProducto(producto).subscribe({
            next: response => {
              console.log('Producto registrado:', response);
              alert('Producto registrado exitosamente');
              this.router.navigate(['/sidenavN']);
            },
            error: error => {
              console.error('Error al registrar producto:', error);
              alert('Ocurrió un error al registrar el producto. Por favor, inténtelo de nuevo.');
            }
          });
        } else {
          console.error('Error al buscar producto:', error);
          alert('Ocurrió un error al buscar el producto. Por favor, inténtelo de nuevo.');
        }
      }
    });
  }
}
