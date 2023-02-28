import { Component, OnInit } from '@angular/core';
import { SharedServiceService } from 'src/app/shared/shared-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class UserEditComponent implements OnInit {

  constructor(private service: SharedServiceService, private router : Router) { }

  User:any={};
  EditUser: any={};
  // EditUser:any={
  //   Id:"",
  //   Name:"",
  //   Email:"",
  //   Password:""
  // };
  
  ngOnInit(): void {
    this.getUserByToken();
  }

  getUserByToken(){
    var token = localStorage.getItem('token');
    var data = {
      token: token
    };
    this.service.getUserByToken(data).subscribe(data => {
      this.User=data;
    })
  }

  postUserEdit(): void {
    this.EditUser={
      Id:`${this.User.id}`,
      Name:`${this.User.name}`,
      Email:`${this.User.email}`,
      Password: this.User.password !== "" ? `${this.User.password}` : undefined
    }

    this.service.postUserEdit(this.EditUser).subscribe(
      data => {
        alert("Success");
        this.router.navigate(['/user/profile']);
      }, 
      error => alert("Error"))
  }

}
