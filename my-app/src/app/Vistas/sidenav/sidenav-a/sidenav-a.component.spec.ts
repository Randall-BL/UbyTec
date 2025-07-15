import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SidenavAComponent } from './sidenav-a.component';

describe('SidenavAComponent', () => {
  let component: SidenavAComponent;
  let fixture: ComponentFixture<SidenavAComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SidenavAComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SidenavAComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
