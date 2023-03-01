import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SharedServiceService } from 'src/app/shared/shared-service.service';

@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.css']
})
export class MovieDetailsComponent implements OnInit {

  constructor(private service: SharedServiceService, private route: ActivatedRoute) { }

  Movie:any={};

  ngOnInit(): void {
    this.getMovieDetails();
  }

  getMovieDetails(){
    const idMovie = this.route.snapshot.paramMap.get('id');
    this.service.getMovieDetails(idMovie).subscribe(data => {
      this.Movie=data;
    })
  }

}
