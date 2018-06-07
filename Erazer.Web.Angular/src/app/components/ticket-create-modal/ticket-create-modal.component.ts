import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'ticket-create-modal',
  templateUrl: './ticket-create-modal.component.html',
  styleUrls: ['./ticket-create-modal.component.css']
})
export class TicketCreateModalComponent {

  @Input() loaded: boolean;
  @Output() redirect: EventEmitter<string> = new EventEmitter();

  public visible = false;
  public visibleAnimate = false;

  public show(): void {
    this.visible = true;
    setTimeout(() => this.visibleAnimate = true, 100);
  }

  public hide(): void {
    this.visibleAnimate = false;
    setTimeout(() => this.visible = false, 300);
  }

  public onContainerClicked(event: MouseEvent): void {
    if ((<HTMLElement>event.target).classList.contains('modal')) {
      this.hide();
    }
  }

  public redirectToTicket(){
    if(this.loaded)
      this.redirect.emit();
  }

}
