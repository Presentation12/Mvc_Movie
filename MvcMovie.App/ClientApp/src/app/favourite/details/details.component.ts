import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SharedServiceService } from 'src/app/shared/shared-service.service';

@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.css']
})
export class FavouriteDetailsComponent implements OnInit {

  constructor(private service: SharedServiceService, private route: ActivatedRoute) { }

  
  FavouriteMovie:any={};

  ngOnInit(): void {
    this.getMovieDetails();
  }

  getMovieDetails(){
    const idFavouriteMovie = this.route.snapshot.paramMap.get('id');
    this.service.getMovieDetails(idFavouriteMovie).subscribe(data => {
      this.FavouriteMovie=data;
    })
  }

}
