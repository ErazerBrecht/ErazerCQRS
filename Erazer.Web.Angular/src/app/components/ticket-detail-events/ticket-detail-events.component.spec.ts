import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketDetailEventsComponent } from './ticket-detail-events.component';

describe('TicketDetailEventsComponent', () => {
  let component: TicketDetailEventsComponent;
  let fixture: ComponentFixture<TicketDetailEventsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TicketDetailEventsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketDetailEventsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
