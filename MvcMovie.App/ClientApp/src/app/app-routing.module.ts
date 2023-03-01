import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

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

const routes: Routes = [
    {path: 'movie', component:MovieComponent},
    {path: 'movie/details/:id', component:MovieDetailsComponent},
    {path: 'movie/create', component:MovieCreateComponent},
    {path: 'movie/edit/:id', component:MovieEditComponent},
    {path: 'genre', component:GenreComponent},
    {path: 'genre/create', component:GenreCreateComponent},
    {path: 'user', component:UserComponent},
    {path: 'user/profile', component:UserProfileComponent},
    {path: 'user/create', component:UserCreateComponent},
    {path: 'user/edit', component:UserEditComponent},
    {path: 'login', component:UserLoginComponent},
    {path: 'favourite', component:FavouriteComponent},
    {path: 'favourite/details/:id', component:FavouriteDetailsComponent},
    
  ];

  @NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
  })
  export class AppRoutingModule { }