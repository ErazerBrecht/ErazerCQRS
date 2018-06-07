import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';

import { AppComponent } from './app.component';

// Routing
import { routing } from './routes/routes';

// Guards
import { TicketDetailGuard } from './routes/guards/ticketDetail.guard';

// Containers (Smart - components)
import { NavbarComponent } from './containers/navbar/navbar.component';
import { AllTicketsComponent } from './containers/all-tickets/all-tickets.component';
import { SidebarComponent } from './containers/sidebar/sidebar.component';
import { CreateTicketComponent } from './containers/create-ticket/create-ticket.component';
import { DetailTicketComponent } from './containers/detail-ticket/detail-ticket.component';

// Services
import { AllTicketsService } from './containers/all-tickets/all-tickets.service';
import { CreateTicketService } from './containers/create-ticket/create-ticket.service';
import { DetailTicketService } from './containers/detail-ticket/detail-ticket.service';

// Presential Components (Dumb - components)
import { TicketListComponent } from './components/ticket-list/ticket-list.component';
import { WelcomeComponent } from './components/welcome/welcome.component';
import { TicketCreateFormComponent } from './components/ticket-create-form/ticket-create-form.component';
import { TicketCreateModalComponent } from './components/ticket-create-modal/ticket-create-modal.component';
import { FormGroupTextboxComponent } from './components/form-group-textbox/form-group-textbox.component';
import { FormGroupRadioComponent } from './components/form-group-radio/form-group-radio.component';
import { FormGroupSelectComponent } from './components/form-group-select/form-group-select.component';
import { FormGroupMarkdownEditorComponent } from './components/form-group-markdown-editor/form-group-markdown-editor.component';
import { PrioritySelectorComponent } from './components/priority-selector/priority-selector.component';
import { CommentBoxComponent } from './components/comment-box/comment-box.component';
import { MarkdownViewerComponent } from './components/markdown-viewer/markdown-viewer.component';
import { FooterComponent } from './components/footer/footer.component';
import { TicketEventComponent } from './components/ticket-event/ticket-event.component';
import { TicketPriorityEventComponent } from './components/ticket-event/ticket-priority-event/ticket-priority-event.component';
import { TicketStatusEventComponent } from './components/ticket-event/ticket-status-event/ticket-status-event.component';
import { TicketCommentEventComponent } from './components/ticket-event/ticket-comment-event/ticket-comment-event.component';
import { TicketCreatedEventComponent } from './components/ticket-event/ticket-created-event/ticket-created-event.component';
import { TicketDetailBasicComponent } from './components/ticket-detail-basic/ticket-detail-basic.component';
import { TicketDetailEventsComponent } from './components/ticket-detail-events/ticket-detail-events.component';
import { FileComponentComponent } from './components/file-component/file-component.component';

// State-managment REDUX
import { StoreModule } from '@ngrx/store';
import { rootReducer } from './redux/reducers/root.reducer';
import { LimitToPipe } from './pipes/limit-to.pipe';

// Realtime
import { RealTime } from "./common/realtime";

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    AllTicketsComponent,
    WelcomeComponent,
    SidebarComponent,
    TicketListComponent,
    LimitToPipe,
    CreateTicketComponent,
    TicketCreateFormComponent,
    TicketCreateModalComponent,    
    FormGroupTextboxComponent,
    FormGroupRadioComponent,
    PrioritySelectorComponent,
    FormGroupSelectComponent,
    FormGroupMarkdownEditorComponent,
    DetailTicketComponent,
    CommentBoxComponent,
    FooterComponent,
    TicketEventComponent,
    TicketStatusEventComponent,
    TicketCommentEventComponent,
    TicketDetailBasicComponent,
    TicketDetailEventsComponent,
    TicketPriorityEventComponent,
    MarkdownViewerComponent,
    TicketCreatedEventComponent,
    FileComponentComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    routing,
    StoreModule.forRoot(rootReducer),
    StoreDevtoolsModule.instrument({
      maxAge: 50
    })
  ],
  providers: [AllTicketsService, CreateTicketService, DetailTicketService, TicketDetailGuard, RealTime],
  bootstrap: [AppComponent]
})
export class AppModule { }