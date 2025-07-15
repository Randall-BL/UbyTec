import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionTipoComercioComponent } from './gestion-tipo-comercio.component';

describe('GestionTipoComercioComponent', () => {
  let component: GestionTipoComercioComponent;
  let fixture: ComponentFixture<GestionTipoComercioComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GestionTipoComercioComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GestionTipoComercioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
