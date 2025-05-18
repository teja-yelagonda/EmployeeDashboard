import { Component, Input } from '@angular/core';
import { AuthService } from '../../auth/auth.service';
import { Router } from '@angular/router';
// import { FormBuilder, FormGroup,Validator } from '@angular/forms';

@Component({
  selector: 'app-forgotpassword',
  standalone: false,
  templateUrl: './forgotpassword.component.html',
  styleUrl: './forgotpassword.component.css'
})
export class ForgotpasswordComponent {
  // loginForm!:FormGroup;
  errorMessage=''
  showOtpInput:boolean=false
  OTP=''
  isOTPVerified:boolean=false;
  @Input() username:string ='';

  constructor(private authservice:AuthService, private router:Router){}

  submit(){
    if(!this.username){
      this.errorMessage="Please enter username"
      return
    }

    this.authservice.forgotpassword(this.username)
    .subscribe({
      next:(response)=>{
        this.showOtpInput=true

    },
    error:(error)=>{
      if(error.status==200){
        this.errorMessage="OTP sent to registered Mail address"
      }
      else{
        this.errorMessage="something went wrong please try again"
      }
      console.log("failed to send otp",error);
    }
    });
  }

  submitOtp(){
    console.log(this.username)
    if(!this.OTP){
      this.errorMessage="Please enter OTP"
      return
    }

    this.authservice.VerifyOTP({Email:this.username,OTP:this.OTP})
    .subscribe({
      next:(response)=>{
      this.isOTPVerified=true

    },
    error:(error)=>{
      debugger
      if(error.status==200){
        this.errorMessage="OTP validation successfull"
      }
      else{
        this.errorMessage="something went wrong please try again"
      }
      console.log("failed to verify otp",error);
    }
    });
  }


}
