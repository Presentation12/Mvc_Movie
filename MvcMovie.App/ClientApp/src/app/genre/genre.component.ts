import { Component, OnInit } from '@angular/core';
import { SharedServiceService } from 'src/app/shared/shared-service.service';

@Component({
  selector: 'app-genre',
  templateUrl: './genre.component.html',
  styleUrls: ['./genre.component.css']
})
export class GenreComponent implements OnInit {

  constructor(private service: SharedServiceService) { }

  GenresList:any=[];

  ngOnInit(): void {
    this.getGenres();
  }

  getGenres(){
    var model = {

    };
    this.service.getGenres(model).subscribe(data => {
      this.GenresList=data;
    });
  }

}
