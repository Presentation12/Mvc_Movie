import { Component } from '@angular/core';
import { faMugHot } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';
  faCoffee = faMugHot;
}
