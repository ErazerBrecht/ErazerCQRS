import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketDetailBasicComponent } from './ticket-detail-basic.component';

describe('TicketDetailBasicComponent', () => {
  let component: TicketDetailBasicComponent;
  let fixture: ComponentFixture<TicketDetailBasicComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TicketDetailBasicComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketDetailBasicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
