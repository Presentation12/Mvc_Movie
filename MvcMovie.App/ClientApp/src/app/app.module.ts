import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';

import { MovieComponent } from './movie/movie.component';
import { MovieDetailsComponent } from './movie/details/details.component';
import { MovieCreateComponent } from './movie/create/create.component';
import { MovieEditComponent } from './movie/edit/edit.component';
import { GenreComponent } from './genre/genre.component';
import { GenreCreateComponent } from './genre/create/create.component';
import { UserComponent } from './user/user.component';
import { UserProfileComponent } from './user/profile/profile.component';
import { UserCreateComponent } from './user/create/create.component';
import { UserEditComponent } from './user/edit/edit.component';
import { UserLoginComponent } from './user/login/login.component';
import { FavouriteComponent } from './favourite/favourite.component';
import { FavouriteDetailsComponent } from './favourite/details/details.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    MovieComponent,
    MovieDetailsComponent,
    MovieCreateComponent,
    MovieEditComponent,
    GenreComponent,
    GenreCreateComponent,
    UserComponent,
    UserProfileComponent,
    UserCreateComponent,
    UserEditComponent,
    UserLoginComponent,
    FavouriteComponent,
    FavouriteDetailsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    AppRoutingModule,
    HttpClientModule,
    FontAwesomeModule,
    FormsModule,

    RouterModule.forRoot([
      { path: '', component: UserLoginComponent, pathMatch: 'full' }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
