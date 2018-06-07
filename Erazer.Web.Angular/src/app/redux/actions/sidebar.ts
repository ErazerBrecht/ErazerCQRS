import {Action} from "@ngrx/store";

export const TOGGLE_SIDEBAR = '[SideBar] Toggle';

export class ToggleSidebar implements Action {
    readonly type = TOGGLE_SIDEBAR;

    public constructor() {}
}

export type Actions =
    ToggleSidebar;