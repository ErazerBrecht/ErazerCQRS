import { AppState } from './appState';
import { DataState } from './dataState';

export interface State {
  readonly app: AppState;
  readonly data: DataState;
}