import { Component, OnInit } from '@angular/core';
import { SharedServiceService } from 'src/app/shared/shared-service.service';
import { Router } from '@angular/router';
import { faMugHot } from '@fortawesome/free-solid-svg-icons';
import { faEye } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class UserLoginComponent implements OnInit {

  constructor(private service: SharedServiceService, private router : Router) { }
  faCoffee = faMugHot;
  faEye = faEye;
  
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

  togglePasswordVisibility() {
    var password = document.getElementById('password') as HTMLInputElement;
    if (password.type === 'password') {
      password.type = 'text';
    } else {
      password.type = 'password';
    }
  }

}