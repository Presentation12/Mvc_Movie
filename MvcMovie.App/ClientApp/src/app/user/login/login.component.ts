import { Component, OnInit } from '@angular/core';
import { SharedServiceService } from 'src/app/shared/shared-service.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class UserLoginComponent implements OnInit {

  constructor(private service: SharedServiceService, private router : Router) { }

  ngOnInit(): void {
  }

  account:any= {
    Email: "",
    Passord: ""
  };

  login(): void {
    this.service.postLogin(this.account).subscribe(
      data=>{
        localStorage.setItem('token', data.toString());
        this.router.navigateByUrl('/user').then(() =>{
          this.router.navigate([decodeURI('/user')]);
        });
      },
    error=>{
      alert('Login failed.');
    });
  }
}