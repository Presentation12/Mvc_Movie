import { Component,NgZone, OnInit } from '@angular/core';
import { SharedServiceService } from '../shared/shared-service.service';
import { faEye } from '@fortawesome/free-solid-svg-icons';
import { faTrash } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-favourite',
  templateUrl: './favourite.component.html',
  styleUrls: ['./favourite.component.css']
})
export class FavouriteComponent implements OnInit {

  constructor(private service: SharedServiceService, private ngZone: NgZone) { }
  faEye = faEye;
  faTrash = faTrash;

  GenresList:any=[];
  FavouriteMoviesList:any=[];
  model = {
    offset: 0,
    limit: 0,
    search: [{
      name: "Genre",
      value: ""
    },
    {
      name: "Title",
      value: ""
    },
    {
      name: "Token",
      value: ""
    }]
  };

  idFavouriteMovie:any={};

  page: number = 1;
  pageSize: number = 2;

  ngOnInit(): void {
    this.refreshFavouriteMovies();
    this.refreshGenres();
  }

  getPaginatedMovies() {
    const startIndex = (this.page - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    return this.FavouriteMoviesList.slice(startIndex, endIndex);
  }

  refreshFavouriteMovies() {
    var token = localStorage.getItem('token');
    const tokenX = this.model.search.find(s => s.name === 'Token');
    if (tokenX && token) {
      tokenX.value = token;
    }

    const searchGenre = this.model.search.find(s => s.name === 'Genre');
    if (searchGenre) {
      searchGenre.value = (document.getElementById('genre-filter') as HTMLInputElement).value;
    }

    const searchTitle = this.model.search.find(s => s.name === 'Title');
    if (searchTitle) {
      searchTitle.value = (document.getElementById('title-filter') as HTMLInputElement).value;
    }
    
    this.service.getFavourites(this.model).subscribe(
      (data :any) => {
        this.ngZone.run(() => {
          this.FavouriteMoviesList = data.rows;
          console.log(this.FavouriteMoviesList);
        });
      }
      ,error =>{
        alert(error.message);
    });
  }

  refreshGenres(){
    this.service.getUsedGenres().subscribe(
      data => {
        this.GenresList=data;
      }, 
      error => alert("Error"))
  }

  deleteFavouriteMovie(){
    var token = localStorage.getItem('token');
    var data = {
      token: token,
      idMovie: this.idFavouriteMovie
    };
    this.service.postFavouriteDelete(data).subscribe(
      data => this.refreshFavouriteMovies(), 
      error => alert("Error")
    )
  }

  selectedFavouriteMovieId(currentId : any){
    this.idFavouriteMovie = currentId;
  }

}
