import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketCreateModalComponent } from './ticket-create-modal.component';

describe('TicketCreateModalComponent', () => {
  let component: TicketCreateModalComponent;
  let fixture: ComponentFixture<TicketCreateModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TicketCreateModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketCreateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
