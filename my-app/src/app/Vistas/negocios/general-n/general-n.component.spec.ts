import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneralNComponent } from './general-n.component';

describe('GeneralNComponent', () => {
  let component: GeneralNComponent;
  let fixture: ComponentFixture<GeneralNComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GeneralNComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GeneralNComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
