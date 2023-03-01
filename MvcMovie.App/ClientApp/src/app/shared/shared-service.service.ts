import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedServiceService {
  readonly APIUrl="https://localhost:7180"
  //headersAuth = new HttpHeaders().append("Authorization", `Bearer ${localStorage.getItem('token')}`);

  constructor(private http:HttpClient) { }

  //#region MOVIE
    //#region  GET
    getMovieDetails(idMovie:any){
      return this.http.get(this.APIUrl+'/Movies/Details/'+ idMovie)
    }

    getMovieCreate():Observable<any[]>{
      return this.http.get<any>(this.APIUrl+'/Movies/Create')
    }

    getMovieEdit(idMovie:any){
      return this.http.get(this.APIUrl+'/Movies/Edit/'+ idMovie)
    }
    //#endregion

    //#region POST
    getMovies(model:any){
      return this.http.post(this.APIUrl+'/Movies/Paginate', model)
    }

    postMovieCreate(newMovie:any){
      return this.http.post(this.APIUrl+'/Movies/Create', newMovie)
    }

    postMovieEdit(editMovie:any){
      return this.http.post(this.APIUrl+'/Movies/Edit', editMovie)
    }

    postMovieDelete(idMovie:any){
      return this.http.post(this.APIUrl+'/Movies/Delete/'+idMovie, null)
    }
    //#endregion
  //#endregion

  //#region GENRE
    //#region  GET
    getUsedGenres(){
      return this.http.get(this.APIUrl+'/Genre/UsedGenres')
    }
    //#endregion

    //#region POST
    getGenres(model:any){
      return this.http.post(this.APIUrl+'/Genre/Paginate', model)
    }

    postGenreCreate(newGenre:any){
      return this.http.post(this.APIUrl+'/Genre/Create', newGenre)
    }

    postGenreDelete(idGenre:any){
      return this.http.post(this.APIUrl+'/Genre/Delete/'+idGenre, null)
    }
    //#endregion
  //#endregion

  //#region FAVOURITE
    //#region  GET
    getFavouriteDetails(idFavourite:any){
      return this.http.get<any>(this.APIUrl+'/Favourite/Details', idFavourite)
    }
    //#endregion

    //#region POST
    getFavourites(model:any){
      return this.http.post(this.APIUrl+'/Favourite/Paginate', model)
    }

    postFavouriteUpdate(newFavourite:any){
      return this.http.post(this.APIUrl+'/Favourite/Update', newFavourite)
    }

    postFavouriteDelete(favourite:any){
      return this.http.post(this.APIUrl+'/Favourite/Delete', favourite)
    }
    //#endregion
  //#endregion

  //#region USER
    //#region  GET
    getUserIndex(){
      return this.http.get<any>(this.APIUrl+'/User/Index')
    }

    getUserProfile(){
      return this.http.get<any>(this.APIUrl+'/User/Profile')
    }

    getUserEdit(){
      return this.http.get<any>(this.APIUrl+'/User/Edit')
    }
    //#endregion

    //#region POST
    getUserByToken(data:any){
      return this.http.post(this.APIUrl+'/User/GetUserByToken', data)
    }

    postUserCreate(newUser:any){
      return this.http.post(this.APIUrl+'/User/Create', newUser)
    }

    postUserEdit(editUser:any){
      alert(editUser.Id + " // " + editUser.Name + " // " + editUser.Email + " // " + editUser.Password)
      return this.http.post(this.APIUrl+'/User/Edit', editUser)
    }

    postUserDelete(user:any){
      return this.http.post(this.APIUrl+'/User/Delete', user)
    }

    postLogin(user:any){
      return this.http.post(this.APIUrl+'/User/Login', user)
    }

    postLogout(){
      return this.http.post(this.APIUrl+'/User/Logout', null)
    }
    //#endregion
  //#endregion
}