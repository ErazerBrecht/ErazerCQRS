import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketCreateFormComponent } from './ticket-create-form.component';

describe('TicketCreateFormComponent', () => {
  let component: TicketCreateFormComponent;
  let fixture: ComponentFixture<TicketCreateFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TicketCreateFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketCreateFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
