import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../auth/auth.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit{
  loginForm!:FormGroup;
  errorMessage=''
  constructor(private fb: FormBuilder, private authservice: AuthService, private router:Router){}

  ngOnInit(): void {
    this.authservice.logout()
    this.loginForm=this.fb.group({
      username:['',[Validators.required,Validators.email]],
      password:['',
      [Validators.required,
      Validators.minLength(8),
      Validators.pattern('^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*]).{8,}$')]
    ]
    })
  }

  login(){
    if(!this.loginForm.value.username || !this.loginForm.value.password){
      this.errorMessage='Please fill all the Required details';
      return;
    }
    if(this.loginForm.invalid){
      this.errorMessage='Please enter valid details';
      return;
    }
    

    const credentials=this.loginForm.value;
    console.log(credentials)
    this.authservice.login(credentials)
    .subscribe({
      next:(response)=>{
      this.authservice.storeToken(response.token);
      
      this.router.navigate(['/employees'])
    },
    error:(error)=>{
      console.log("failed to login",error);
      this.errorMessage="Invalid Credentials Please try again."
    }
    });
  }

  get username(){
    return this.loginForm.get('username');
  }

  get password(){
    return this.loginForm.get('password');
  }

  Forgot(){
    this.router.navigate(['/forgot'])
  }

  Register(){
    this.router.navigate(['/register'])
  }

}
