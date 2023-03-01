import { Component, OnInit } from '@angular/core';
import { SharedServiceService } from 'src/app/shared/shared-service.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class MovieEditComponent implements OnInit {

  constructor(private service: SharedServiceService, private route : ActivatedRoute, private router : Router) { }

  GenreList: any=[];
  EditMovie:any={};

  ngOnInit(): void {
    this.getUsedGenres();
    this.getMovieDetails();
  }

  getUsedGenres(){
    this.service.getUsedGenres().subscribe(
    data => {
      this.GenreList=data;
    }, 
    error => alert("Error"))
  }

  getMovieDetails(){
    const idMovie = this.route.snapshot.paramMap.get('id');
    this.service.getMovieEdit(idMovie).subscribe(data => {
      this.EditMovie=data;

      // date format
      const releaseDate = new Date(this.EditMovie.releaseDate).toISOString().slice(0, 10);
      this.EditMovie.releaseDate = releaseDate;
    })
  }

  postMovieEdit(): void {
    this.EditMovie={
      Id: this.route.snapshot.paramMap.get('id'),
      Title:`${this.EditMovie.title}`,
      ReleaseDate:`${this.EditMovie.releaseDate}`,
      Genre:`${this.EditMovie.genre}`,
      Price:`${this.EditMovie.price}`,
      Rating:`${this.EditMovie.rating}`
    }
    
    this.service.postMovieEdit(this.EditMovie).subscribe(
      data => {
        this.router.navigate(['/movie/details/' + this.EditMovie.Id]);
      }, 
      error => alert("Error"))
  }

}
