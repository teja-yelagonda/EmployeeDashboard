import { Component, Input } from '@angular/core';
import { AuthService } from '../../auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-passwordreset',
  standalone: false,
  templateUrl: './passwordreset.component.html',
  styleUrl: './passwordreset.component.css'
})
export class PasswordresetComponent {
  @Input() username:string='';
  password='';
  confirmPassword=''
  errorMessage=''
  constructor(private authservice:AuthService, private router:Router){}

  Submit(){
    debugger
    if(!this.password || !this.confirmPassword){
      this.errorMessage="Please enter all Fields"
      return
    }
    if(this.password!=this.confirmPassword){
      this.errorMessage="Password did not match"
      return
    }

    this.authservice.PasswordReset({Email:this.username,Password:this.password})
    .subscribe({
      next:(response:any)=>{
        this.router.navigate(['/login'])

    },
    error:(error:any)=>{
      if(error.status==200){
        this.errorMessage="Password updated successfully"
      }
      else{
        this.errorMessage="something went wrong please try again"
      }
      console.log("failed to verify otp",error);
    }
    });
  }

}
