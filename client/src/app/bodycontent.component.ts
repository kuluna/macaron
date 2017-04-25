import { Component } from '@angular/core';

@Component({
  selector: 'app-bodycontent',
  template:
    `
    <div fxLayout="row" fxLayoutAlign="center " style="margin:16px">
      <div fxFlex="0 1 1024px">
        <ng-content></ng-content>
      </div>
    </div>
    `
})
export class BodyContentComponent {}
