import {ChangeDetectorRef, Component, NgZone, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { PedidoService } from '../../../services/pedido.service';
import {Chart, registerables} from 'chart.js';
import { AuthService } from '../../../services/auth.service'; // Importar el AuthService
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

@Component({
  selector: 'app-reporte-c-ventas',
  standalone: true,
  imports: [
    FormsModule,
    HttpClientModule,
    CommonModule
  ],
  providers: [
    Router,
    PedidoService
  ],
  templateUrl: './reporte-c-ventas.component.html',
  styleUrls: ['./reporte-c-ventas.component.css']
})
export class ReporteCVentasComponent implements OnInit {
  reportes: any[] = [];
  administradorEmail: string | null = ''; // Variable para almacenar el email del administrador

  constructor(
    private router: Router,
    private pedidoService: PedidoService,
    private cdr: ChangeDetectorRef, // Inyectar ChangeDetectorRef
    private authService: AuthService, // Inyectar AuthService
    private ngZone: NgZone, // Inyectar NgZone
  ) {
    Chart.register(...registerables);
  }

  ngOnInit(): void {
    this.administradorEmail = this.authService.obtenerSesionAdmin(); // Obtener el email del administrador
    this.cargarReportes();
  }

  cargarReportes(): void {
    this.pedidoService.getVentasGenerales().subscribe({
      next: (data) => {
        console.log('Datos recibidos del servicio:', data); // Imprime los datos crudos
        this.reportes = data;

        // Forzar actualización de la vista
        this.cdr.detectChanges();

        // Agrupación para el gráfico de barras (Clientes)
        const ventasPorCliente = this.reportes.reduce((acc, reporte) => {
          acc[reporte.nombreCliente] = (acc[reporte.nombreCliente] || 0) + reporte.totalCompras;
          return acc;
        }, {} as Record<string, number>);

        const clientes = Object.keys(ventasPorCliente);
        const totalComprasClientes = Object.values(ventasPorCliente);

        // Crear gráfico de barras
        this.createBarChart('myBarChart', clientes, totalComprasClientes);

        // Agrupación para el gráfico de pastel (Comercios)
        const ventasPorComercio = this.reportes.reduce((acc, reporte) => {
          acc[reporte.nombreComercio] = (acc[reporte.nombreComercio] || 0) + reporte.totalCompras;
          return acc;
        }, {} as Record<string, number>);

        const comercios = Object.keys(ventasPorComercio);
        const totalVentasComercios = Object.values(ventasPorComercio);

        // Crear gráfico de pastel
        this.createPieChart('myPieChart2', comercios, totalVentasComercios);
      },
      error: (error) => {
        console.error('Error al cargar los reportes:', error);
      }
    });
  }

  createBarChart(chartId: string, labels: string[], data: unknown[]) {
    const canvas = document.getElementById(chartId) as HTMLCanvasElement;
    if (canvas) {
      const ctx = canvas.getContext('2d');
      if (ctx) {
        new Chart(ctx, {
          type: 'bar',
          data: {
            labels: labels,
            datasets: [{
              label: 'Total Compras por Cliente',
              data: data,
              backgroundColor: '#4e73df',
            }],
          },
          options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
              x: {
                title: {
                  display: true,
                  text: 'Clientes'
                }
              },
              y: {
                beginAtZero: true,
                title: {
                  display: true,
                  text: 'Total Compras (₡)'
                }
              }
            }
          },
        });
      }
    } else {
      console.error(`Canvas element with id '${chartId}' not found.`);
    }
  }

  createPieChart(chartId: string, labels: string[], data: unknown[]) {
    const canvas = document.getElementById(chartId) as HTMLCanvasElement;
    if (canvas) {
      const ctx = canvas.getContext('2d');
      if (ctx) {
        new Chart(ctx, {
          type: 'pie',
          data: {
            labels: labels,
            datasets: [{
              data: data,
              backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc', '#f6c23e', '#e74a3b'],
            }],
          },
          options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
              legend: {
                position: 'bottom'
              }
            }
          },
        });
      }
    } else {
      console.error(`Canvas element with id '${chartId}' not found.`);
    }
  }

  exportarPDF(): void {
    // Ejecutar la generación del PDF fuera del contexto de Angular
    this.ngZone.runOutsideAngular(() => {
      const doc = new jsPDF();

      const pageWidth = doc.internal.pageSize.getWidth();

      // Título y subtítulos centrados
      doc.setFontSize(18);
      doc.text('UbyTec', pageWidth / 2, 20, { align: 'center' });
      doc.setFontSize(15);
      doc.text('Reporte Consolidado de Ventas', pageWidth / 2, 30, { align: 'center' });

      const tablaDatos = this.reportes.map(reporte => [
        reporte.nombreCliente,
        reporte.nombreComercio,
        reporte.nombreRepartidor,
        `₡${reporte.totalCompras.toFixed(2)}`
      ]);

      autoTable(doc, {
        head: [['Cliente', 'Comercio', 'Repartidor', 'Total Compras']],
        body: tablaDatos,
        startY: 40,
        didDrawPage: (data) => {
          // @ts-ignore
          const finalY = data.cursor.y;
          doc.setFontSize(12);
          doc.text(`Descargado por: ${this.administradorEmail}`, pageWidth - 10, finalY + 10, { align: 'right' });
          doc.text(`Fecha de descarga: ${new Date().toLocaleDateString()}`, pageWidth - 10, finalY + 20, { align: 'right' });
        },
      });

      // Descargar el archivo
      doc.save(`Reporte_Consolidado_Ventas_${new Date().toLocaleDateString()}.pdf`);

      // Regresar al contexto de Angular para actualizar la UI
      this.ngZone.run(() => {
        console.log('PDF generado correctamente');
        this.cdr.detectChanges(); // Notifica a Angular sobre los cambios
      });
    });
  }

  backToAdmin() {
    this.router.navigate(['/sidenavA'], { skipLocationChange: false }).then(() => {
      window.location.reload(); // Recarga la página para renderizar completamente el menú lateral
    });
  }
}

