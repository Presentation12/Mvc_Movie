import { Component, OnInit } from '@angular/core';
import { SharedServiceService } from 'src/app/shared/shared-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.css']
})
export class UserCreateComponent implements OnInit {

  constructor(private service: SharedServiceService, private router : Router) { }

  newUser={
    Name:"",
    Email:"",
    Password:""
  };

  ngOnInit(): void {
  }

  postUserCreate(): void {
    this.service.postUserCreate(this.newUser).subscribe(
      data => {
        alert("Success");
        this.router.navigate(['/login']);
      }, 
      error => alert("Error"))
  }

}
