import { Component, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { AgRendererComponent } from 'ag-grid-angular';

@Component({
  selector: 'grid-context-menu-renderer',
  templateUrl: './context-menu-renderer.component.html',
  styleUrls: ['./context-menu-renderer.component.scss']
})

export class ContextMenuRendererComponent implements AgRendererComponent {
  params: any;

  constructor(
    private ngZone: NgZone,
    private router: Router,
  ) { }

  agInit(params: any): void {
    if (params.value) {
      this.params = params;
      this.params.title = params.title || "Actions";
    }
  }

  refresh(params: any): boolean {
    return false;
  }
}
