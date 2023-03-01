import { Component,NgZone, OnInit } from '@angular/core';
import { SharedServiceService } from '../shared/shared-service.service';
import { faEye } from '@fortawesome/free-solid-svg-icons';
import { faPen } from '@fortawesome/free-solid-svg-icons';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { faHeart } from '@fortawesome/free-solid-svg-icons';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-movie',
  templateUrl: './movie.component.html',
  styleUrls: ['./movie.component.css']
})
export class MovieComponent implements OnInit {

  constructor(private service: SharedServiceService, private route : ActivatedRoute, private ngZone: NgZone, private router: Router) { }
  faEye = faEye;
  faPen = faPen;
  faTrash = faTrash;
  faHeart = faHeart;

  GenresList:any=[];
  MoviesList:any=[];
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
    }]
  };

  idMovie:any={};

  page: number = 1;
  pageSize: number = 2;

  ngOnInit(): void {
    this.refreshMovies();
    this.refreshGenres();
  }

  getPaginatedMovies() {
    const startIndex = (this.page - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    return this.MoviesList.slice(startIndex, endIndex);
  }

  refreshMovies() {
    const searchGenre = this.model.search.find(s => s.name === 'Genre');
    if (searchGenre) {
      searchGenre.value = (document.getElementById('genre-filter') as HTMLInputElement).value;
    }

    const searchTitle = this.model.search.find(s => s.name === 'Title');
    if (searchTitle) {
      searchTitle.value = (document.getElementById('title-filter') as HTMLInputElement).value;
    }
    
    this.service.getMovies(this.model).subscribe(
      (data :any) => {
        this.ngZone.run(() => {
          this.MoviesList = data.rows;
          console.log(this.MoviesList);
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

  deleteMovie(){
    this.service.postMovieDelete(this.idMovie).subscribe(
      data => this.refreshMovies(), 
      error => alert("Error")
    )
  }

  selectedMovieId(currentId : any){
    this.idMovie = currentId;
  }

  addFavourite(idFavouriteMovie : any){
    var token = localStorage.getItem('token');
    var data = {
      token: token,
      idMovie: idFavouriteMovie
    };
    this.service.postFavouriteUpdate(data).subscribe(
      data => this.router.navigate(['/favourite']), 
      error => alert("Error")
    )
  }
}
