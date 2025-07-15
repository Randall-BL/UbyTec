import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReporteHistorialComponent } from './reporte-historial.component';

describe('ReporteHistorialComponent', () => {
  let component: ReporteHistorialComponent;
  let fixture: ComponentFixture<ReporteHistorialComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReporteHistorialComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReporteHistorialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
