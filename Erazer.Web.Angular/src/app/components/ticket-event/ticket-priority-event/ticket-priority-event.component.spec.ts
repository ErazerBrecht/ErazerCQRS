import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketPriorityEventComponent } from './ticket-priority-event.component';

describe('TicketPriorityEventComponent', () => {
  let component: TicketPriorityEventComponent;
  let fixture: ComponentFixture<TicketPriorityEventComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TicketPriorityEventComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketPriorityEventComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
