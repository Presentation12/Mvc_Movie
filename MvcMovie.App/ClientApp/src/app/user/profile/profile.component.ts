import { Component, OnInit } from '@angular/core';
import { SharedServiceService } from 'src/app/shared/shared-service.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class UserProfileComponent implements OnInit {

  constructor(private service: SharedServiceService) { }

  User:any={};

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
}
