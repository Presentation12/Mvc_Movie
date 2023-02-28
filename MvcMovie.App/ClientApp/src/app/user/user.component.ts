import { Component, OnInit } from '@angular/core';
import { SharedServiceService } from 'src/app/shared/shared-service.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

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
