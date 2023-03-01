import { Component, OnInit } from '@angular/core';
import { SharedServiceService } from 'src/app/shared/shared-service.service';
import { Router } from '@angular/router';
import { faMugHot } from '@fortawesome/free-solid-svg-icons';
import { faEye } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.css']
})
export class UserCreateComponent implements OnInit {

  constructor(private service: SharedServiceService, private router : Router) { }
  faCoffee = faMugHot;
  faEye = faEye;
  
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

  togglePasswordVisibility() {
    var password = document.getElementById('password') as HTMLInputElement;
    if (password.type === 'password') {
      password.type = 'text';
    } else {
      password.type = 'password';
    }
  }

}
