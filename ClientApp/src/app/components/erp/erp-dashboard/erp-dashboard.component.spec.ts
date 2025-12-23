import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ErpDashboardComponent } from './erp-dashboard.component';

describe('ErpDashboardComponent', () => {
  let component: ErpDashboardComponent;
  let fixture: ComponentFixture<ErpDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ErpDashboardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ErpDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
