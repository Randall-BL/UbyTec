import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistroNComponent } from './registro-n.component';

describe('RegistroNComponent', () => {
  let component: RegistroNComponent;
  let fixture: ComponentFixture<RegistroNComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegistroNComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegistroNComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
