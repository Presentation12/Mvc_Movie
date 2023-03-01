import { Component } from '@angular/core';
import { faMugHot } from '@fortawesome/free-solid-svg-icons';
import { faFilm } from '@fortawesome/free-solid-svg-icons';
import { faWrench } from '@fortawesome/free-solid-svg-icons';
import { faBars } from '@fortawesome/free-solid-svg-icons';
import { faHeart } from '@fortawesome/free-solid-svg-icons';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { faSignOutAlt } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';
  faCoffee = faMugHot;
  faFilm = faFilm;
  faWrench = faWrench;
  faBars = faBars;
  faHeart = faHeart;
  faUser = faUser;
  faSignOutAlt = faSignOutAlt;
}
