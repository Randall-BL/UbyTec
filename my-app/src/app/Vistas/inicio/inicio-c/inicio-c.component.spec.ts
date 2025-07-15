import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InicioCComponent } from './inicio-c.component';

describe('InicioCComponent', () => {
  let component: InicioCComponent;
  let fixture: ComponentFixture<InicioCComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InicioCComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InicioCComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
