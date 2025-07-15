import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SidenavNComponent } from './sidenav-n.component';

describe('SidenavNComponent', () => {
  let component: SidenavNComponent;
  let fixture: ComponentFixture<SidenavNComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SidenavNComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SidenavNComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
