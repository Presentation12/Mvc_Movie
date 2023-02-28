import { Component, OnInit } from '@angular/core';
import { SharedServiceService } from 'src/app/shared/shared-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.css']
})
export class GenreCreateComponent implements OnInit {

  constructor(private service: SharedServiceService, private router : Router) { }

  newGenre={
    Name:""
  };

  ngOnInit(): void {
  }

  postGenreCreate(): void {
  this.service.postGenreCreate(this.newGenre).subscribe(
    data => {
      alert("Success");
      this.router.navigate(['/genre']);
    }, 
    error => alert("error"))
  }

}
