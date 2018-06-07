import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketStatusEventComponent } from './ticket-status-event.component';

describe('TicketStatusEventComponent', () => {
  let component: TicketStatusEventComponent;
  let fixture: ComponentFixture<TicketStatusEventComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TicketStatusEventComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketStatusEventComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
