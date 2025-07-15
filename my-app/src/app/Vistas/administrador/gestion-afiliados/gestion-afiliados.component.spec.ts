import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionAfiliadosComponent } from './gestion-afiliados.component';

describe('GestionAfiliadosComponent', () => {
  let component: GestionAfiliadosComponent;
  let fixture: ComponentFixture<GestionAfiliadosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GestionAfiliadosComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GestionAfiliadosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
