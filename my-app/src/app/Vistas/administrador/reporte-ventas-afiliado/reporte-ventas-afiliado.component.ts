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
  selector: 'app-reporte-ventas-afiliado',
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
  templateUrl: './reporte-ventas-afiliado.component.html',
  styleUrls: ['./reporte-ventas-afiliado.component.css']
})
export class ReporteVentasAfiliadoComponent implements OnInit {
  reportes: any[] = []; // Para almacenar los reportes de ventas por afiliado
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
    this.cargarReportes(); // Cargar los reportes cuando el componente se inicializa
  }

  cargarReportes(): void {
    this.pedidoService.getPedidosAgrupadosPorAfiliado().subscribe({
      next: (data) => {
        console.log('Datos recibidos del servidor:', data); // Verifica la estructura de los datos
        this.reportes = data;

        // Forzar actualización de la vista
        this.cdr.detectChanges();

        // Nombres de los afiliados
        const afiliados = this.reportes.map(
          (reporte: any[]) => reporte[0]?.nombreComercio || 'Desconocido'
        );

        // Totales de ventas por afiliado
        const totalesVentas = this.reportes.map((reporte: any[]) =>
          reporte.reduce((acc, pedido) => acc + pedido.total, 0)
        );

        // Crear gráfico de barras: Totales de ventas por afiliado
        this.createBarChart('myBarChart', afiliados, totalesVentas);

        // Total general de ventas
        const totalGeneral = totalesVentas.reduce((acc, total) => acc + total, 0);

        // Proporciones de ventas para el gráfico de pastel
        const proporcionesVentas = totalesVentas.map(
          (total) => (total / totalGeneral) * 100
        );

        // Crear gráfico de pastel: Proporciones de ventas
        this.createPieChart('myPieChart2', afiliados, proporcionesVentas);
      },
      error: (error) => {
        console.error('Error al cargar los reportes:', error);
      },
    });
  }

  // Calcular el total de ventas para cada afiliado
  calcularTotalVentas(reporte: any[]): number {
    return reporte.reduce((acc, pedido) => acc + pedido.total, 0);
  }

  // Calcular el monto que se le debe a UbyTec (5% del total de ventas)
  calcularMontoUbyTec(reporte: any[]): number {
    return this.calcularTotalVentas(reporte) * 0.05;
  }

  createBarChart(chartId: string, labels: string[], data: number[]) {
    const canvas = document.getElementById(chartId) as HTMLCanvasElement;
    if (canvas) {
      const ctx = canvas.getContext('2d');
      if (ctx) {
        new Chart(ctx, {
          type: 'bar',
          data: {
            labels: labels, // Nombres de los afiliados
            datasets: [
              {
                label: 'Total Ventas por Afiliado',
                data: data, // Totales de ventas
                backgroundColor: '#4e73df',
              },
            ],
          },
          options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
              x: {
                title: {
                  display: true,
                  text: 'Afiliados',
                },
              },
              y: {
                beginAtZero: true,
                title: {
                  display: true,
                  text: 'Total Ventas (₡)',
                },
              },
            },
          },
        });
      }
    } else {
      console.error(`Canvas element with id '${chartId}' not found.`);
    }
  }

  createPieChart(chartId: string, labels: string[], data: number[]) {
    const canvas = document.getElementById(chartId) as HTMLCanvasElement;
    if (canvas) {
      const ctx = canvas.getContext('2d');
      if (ctx) {
        new Chart(ctx, {
          type: 'pie',
          data: {
            labels: labels, // Nombres de los afiliados
            datasets: [
              {
                data: data, // Proporciones en porcentaje
                backgroundColor: [
                  '#4e73df',
                  '#1cc88a',
                  '#36b9cc',
                  '#f6c23e',
                  '#e74a3b',
                  '#858796',
                ],
              },
            ],
          },
          options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
              tooltip: {
                callbacks: {
                  label: (tooltipItem) => {
                    const value = tooltipItem.raw as number;
                    return `${value.toFixed(2)}%`;
                  },
                },
              },
              legend: {
                position: 'bottom',
              },
            },
          },
        });
      }
    } else {
      console.error(`Canvas element with id '${chartId}' not found.`);
    }
  }

  exportarPDF(): void {
    const doc = new jsPDF();
    const pageWidth = doc.internal.pageSize.getWidth();
    let currentY = 20; // Coordenada Y inicial

    // Título centrado
    doc.setFontSize(18);
    doc.text('UbyTec', pageWidth / 2, currentY, { align: 'center' });
    currentY += 10;

    doc.setFontSize(15);
    doc.text('Reporte Ventas por Afiliado', pageWidth / 2, currentY, { align: 'center' });
    currentY += 10;

    // Iterar sobre cada tabla en la página
    const tablas = document.querySelectorAll('.reportes table'); // Seleccionar todas las tablas con clase 'table'
    tablas.forEach((tabla, index) => {
      // Título para cada tabla
      const afiliadoTitulo = tabla.previousElementSibling?.textContent?.trim() || `Tabla ${index + 1}`;
      doc.setFontSize(14);
      doc.text(afiliadoTitulo, 10, currentY);
      currentY += 10;

      // Convertir la tabla HTML a datos que autoTable pueda procesar
      autoTable(doc, {
        html: tabla as HTMLTableElement, // Pasar la tabla directamente
        startY: currentY,
        styles: {
          cellPadding: 3,
          fontSize: 10,
          overflow: 'linebreak',
        },
        didDrawPage: (data) => {
          // @ts-ignore
          currentY = data.cursor.y + 10; // Actualizar la posición de Y
        },
      });
    });

    // Agregar información al final, justificada a la derecha
    doc.setFontSize(12);
    const rightMargin = pageWidth - 10; // Ajustar margen derecho
    doc.text(`Descargado por: ${this.administradorEmail || 'Desconocido'}`, rightMargin, currentY + 10, { align: 'right' });
    doc.text(`Fecha de descarga: ${new Date().toLocaleDateString()}`, rightMargin, currentY + 20, { align: 'right' });

    // Descargar el PDF
    doc.save(`Reporte_Ventas-Afiliado_${new Date().toLocaleDateString()}.pdf`);
  }


  backToAdmin() {
    this.router.navigate(['/sidenavA'], { skipLocationChange: false }).then(() => {
      window.location.reload(); // Recarga la página para renderizar completamente el menú lateral
    });
  }
}
