import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SidenavCComponent } from './sidenav-c.component';

describe('SidenavCComponent', () => {
  let component: SidenavCComponent;
  let fixture: ComponentFixture<SidenavCComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SidenavCComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SidenavCComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
