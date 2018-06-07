import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from '../../redux/state/appState';
import { State } from '../../redux/state/state';
import { ToggleSidebar } from '../../redux/actions/sidebar';

@Component({
  selector: 'sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  isCollapsed$ = this.store.select((state: State) => state.app.isCollapsed);  

  constructor(private store: Store<State>) { }

  ngOnInit() {
  }

  toggle() {
    this.store.dispatch(new ToggleSidebar());
  }

}
