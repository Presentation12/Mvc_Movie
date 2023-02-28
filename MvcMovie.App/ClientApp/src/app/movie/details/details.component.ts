import { Component, OnInit } from '@angular/core';
import { SharedServiceService } from 'src/app/shared/shared-service.service';

@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.css']
})
export class MovieDetailsComponent implements OnInit {

  constructor(private service: SharedServiceService) { }

  Movie:any={};

  ngOnInit(): void {
    this.getMovieDetails();
  }

  getMovieDetails(){
    var idMovie = (document.getElementById('id-movie') as HTMLInputElement).value
    alert(idMovie);
    this.service.getMovieDetails(idMovie).subscribe(data => {
      this.Movie=data;
    })
  }

}
