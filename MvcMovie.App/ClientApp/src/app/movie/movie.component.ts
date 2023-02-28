import { Component, OnInit } from '@angular/core';
import { SharedServiceService } from '../shared/shared-service.service';

@Component({
  selector: 'app-movie',
  templateUrl: './movie.component.html',
  styleUrls: ['./movie.component.css']
})
export class MovieComponent implements OnInit {

  constructor(private service: SharedServiceService) { }

  ngOnInit(): void {
    //this.refreshMovies();
  }

  MoviesList: any = [];

  // refreshMovies() {
  //   this.service.getMovies().subscribe(data => {
  //     this.MoviesList = data;
  //   })
  // }

}
