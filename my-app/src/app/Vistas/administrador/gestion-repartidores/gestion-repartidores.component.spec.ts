import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionRepartidoresComponent } from './gestion-repartidores.component';

describe('GestionRepartidoresComponent', () => {
  let component: GestionRepartidoresComponent;
  let fixture: ComponentFixture<GestionRepartidoresComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GestionRepartidoresComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GestionRepartidoresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
