import { Component, NgZone, OnInit } from '@angular/core';
import { SharedServiceService } from 'src/app/shared/shared-service.service';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-genre',
  templateUrl: './genre.component.html',
  styleUrls: ['./genre.component.css']
})
export class GenreComponent implements OnInit {

  constructor(private service: SharedServiceService, private ngZone: NgZone) { }
  faTrash = faTrash;

  GenresList:any=[];
  model = {
    offset: 0,
    limit: 0,
    search: [{
      name: "Genre",
      value: "" //document.getElementById('genre-filter')
    }]
  };

  idGenre:any={};

  page: number = 1;
  pageSize: number = 2;

  ngOnInit(): void {
    this.getGenresPaginate();
  }

  getPaginatedGenres() {
    const startIndex = (this.page - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    return this.GenresList.slice(startIndex, endIndex);
  }

  getGenresPaginate(){

    const searchGenre = this.model.search.find(s => s.name === 'Genre');
    if (searchGenre) {
      searchGenre.value = (document.getElementById('genre-filter') as HTMLInputElement).value;
    }

    this.service.getGenres(this.model).subscribe(
      (data :any) => {
        this.ngZone.run(() => {
          this.GenresList = data.rows;
        });
      }
      ,error =>{
        alert(error.message);
    });
  }

  deleteGenre(){
    this.service.postGenreDelete(this.idGenre).subscribe(
      data => this.getGenresPaginate(), 
      error => alert("Error")
    )
  }

  selectedGenreId(currentId : any){
    this.idGenre = currentId;
  }

}
