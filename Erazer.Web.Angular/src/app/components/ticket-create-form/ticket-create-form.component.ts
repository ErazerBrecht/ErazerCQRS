import { Component, ChangeDetectionStrategy, OnInit, Input, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { CreateTicket } from '../../entities/write/createTicket';
import { LOCALE } from '../../configuration/config'
import { IOption } from '../../configuration/ioption';
import * as moment from 'moment';

@Component({
  selector: 'ticket-create-form',
  templateUrl: './ticket-create-form.component.html',
  styleUrls: ['./ticket-create-form.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TicketCreateFormComponent implements OnInit {
  @ViewChild("file") fileInput: ElementRef;
  @Output() onSave = new EventEmitter<CreateTicket>();
  typeOptions: Array<IOption>;
  ticketCreateForm: FormGroup;

  constructor(private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.typeOptions = [{ id: 'AAAA', title: 'TODO1' }, { id: 'BBBB', title: 'TODO2' }]       // TODO TypeOptions    
    this.ticketCreateForm = this.formBuilder.group({
      creator: [{ value: 'Brecht Carlier', disabled: true }],
      date: [{ value: moment().locale(LOCALE).format('L'), disabled: true }],
      title: ['', Validators.required],
      description: ['', Validators.required],
      priority: ['', Validators.required],
      type: ['', Validators.required]
    });
  }

  onfileChange(files: FileList) {
    let validated = false;
    const validedFiles = Array.from(files);

    if (validedFiles.some(f => f.size > 15728640)) {
      const file = validedFiles.find(f => f.size > 15728640);
      alert("FILE TO BIG " + file.name);
    }
    else if (validedFiles.some(f => f.type !== "image/png" && f.type !== "image/jpg" && f.type !== "image/jpeg")) {
      const file = validedFiles.find(f => f.type !== "image/png" && f.type !== "image/jpg" && f.type !== "image/jpeg");
      alert("INCORRECT FILE " + file.name);
    }
    else {
      validated = true;
    }

    if (!validated)
      this.fileInput.nativeElement.value = "";
  }

  onSubmit() {
    const formValues = this.ticketCreateForm.getRawValue();
    const files = this.fileInput.nativeElement.files as FileList;
    this.onSave.emit(new CreateTicket(formValues.title, formValues.description, formValues.priority, files));
  }
}
