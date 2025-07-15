import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InicioNComponent } from './inicio-n.component';

describe('InicioNComponent', () => {
  let component: InicioNComponent;
  let fixture: ComponentFixture<InicioNComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InicioNComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InicioNComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
