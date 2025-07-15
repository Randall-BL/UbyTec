import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TipoComercioComponent } from './tipo-comercio.component';

describe('TipoComercioComponent', () => {
  let component: TipoComercioComponent;
  let fixture: ComponentFixture<TipoComercioComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TipoComercioComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TipoComercioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
