import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReporteVentasAfiliadoComponent } from './reporte-ventas-afiliado.component';

describe('ReporteVentasAfiliadoComponent', () => {
  let component: ReporteVentasAfiliadoComponent;
  let fixture: ComponentFixture<ReporteVentasAfiliadoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReporteVentasAfiliadoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReporteVentasAfiliadoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
