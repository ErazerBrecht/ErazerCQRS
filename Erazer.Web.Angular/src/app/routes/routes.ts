import { RouterModule } from "@angular/router";

import { AllTicketsComponent } from "../containers/all-tickets/all-tickets.component";
import { CreateTicketComponent } from "../containers/create-ticket/create-ticket.component";
import { DetailTicketComponent } from "../containers/detail-ticket/detail-ticket.component";
import { WelcomeComponent } from "../components/welcome/welcome.component";

import { TicketDetailGuard } from "./guards/ticketDetail.guard";

export const routes = [
    { path: "", redirectTo: "/tickets", pathMatch: "full" },
    { path: "about", component: WelcomeComponent },
    { path: "tickets", component: AllTicketsComponent },
    { path: "create", component: CreateTicketComponent },
    { path: "tickets/detail/:id", component: DetailTicketComponent, canActivate: [TicketDetailGuard]},
    { path: "**", redirectTo: "/tickets", pathMatch: 'full'},
];
export const routing = RouterModule.forRoot(routes);