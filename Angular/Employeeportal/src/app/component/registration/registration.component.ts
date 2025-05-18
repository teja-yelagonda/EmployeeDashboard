import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-registration',
  standalone: false,
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.css'
})
export class RegistrationComponent implements OnInit{
  constructor(private authservice:AuthService,private router:Router){}
  FirstName=''
  LastName=''
  DOB:Date |null=null
  Gender=''
  Email=''
  PhoneNumber=''
  Address=''
  password=''
  ConfirmPassword=''
  errorMessage=''
  ngOnInit(): void {
    
  }
  Register(){
    this.authservice.Register({FirstName:this.FirstName,LastName:this.LastName,DOB:this.DOB,Gender:this.Gender,Email:this.Email,PhoneNumber:this.PhoneNumber,Address:this.Address,Password:this.password}).subscribe({
      next:(response)=>{
        console.log(response)
      this.router.navigate(['/login'])
    },
    error:(error)=>{
      if(error.status==400){
        
        this.errorMessage="username already existt"
      }
      else if(error.status==200){
        this.errorMessage="Registration successfull"
      }
      else{
        this.errorMessage="something went wrong please try again"
      }
      console.log("failed to Register",error);
    }
    });
  }

  Login(){
    this.router.navigate(['/login'])
  }

  dateFilter=(date:Date|null):boolean=>{
    const today = new Date();
    return date? date<=today:true;
  };

}
