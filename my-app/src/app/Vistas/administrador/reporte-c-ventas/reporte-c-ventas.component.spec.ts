import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReporteCVentasComponent } from './reporte-c-ventas.component';

describe('ReporteCVentasComponent', () => {
  let component: ReporteCVentasComponent;
  let fixture: ComponentFixture<ReporteCVentasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReporteCVentasComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReporteCVentasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
