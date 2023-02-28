import { Component, OnInit } from '@angular/core';
import { SharedServiceService } from 'src/app/shared/shared-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.css']
})
export class MovieCreateComponent implements OnInit {

  constructor(private service: SharedServiceService, private router : Router) { }

  allGenres: any=[];
  newMovie={
    Title:"",
    ReleaseDate:"",
    Genre:"",
    Price:"",
    Rating:""
  };

  ngOnInit(): void {
    this.getUsedGenres();
  }

  getUsedGenres(){
    this.service.getUsedGenres().subscribe(
    data => {
      this.allGenres=data;
    }, 
    error => alert("Error"))
  }

  postMovieCreate(): void {
    this.service.postMovieCreate(this.newMovie).subscribe(
      data => {
        alert("Success");
        this.router.navigate(['/movie']);
      }, 
      error => alert("Error"))
  }

}
