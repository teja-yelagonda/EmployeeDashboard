import { Component } from '@angular/core';
import { EmployeeComponent } from './component/employee/employee.component';
import { AuthService } from './auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent {
  constructor(public authService:AuthService, private router:Router){
    
  }
  logout(){
    this.authService.logout();
    this.router.navigate(['/login'])
  }
}
