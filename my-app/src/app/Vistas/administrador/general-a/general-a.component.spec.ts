import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneralAComponent } from './general-a.component';

describe('GeneralAComponent', () => {
  let component: GeneralAComponent;
  let fixture: ComponentFixture<GeneralAComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GeneralAComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GeneralAComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
