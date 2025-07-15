import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneralCComponent } from './general-c.component';

describe('GeneralCComponent', () => {
  let component: GeneralCComponent;
  let fixture: ComponentFixture<GeneralCComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GeneralCComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GeneralCComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
