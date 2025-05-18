import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeComponent } from './component/employee/employee.component';
import { LoginComponent } from './component/login/login.component';
import { AuthGuard } from './auth/auth.guard';
import { RegistrationComponent } from './component/registration/registration.component';
import { ForgotpasswordComponent } from './component/forgotpassword/forgotpassword.component';
import { PasswordresetComponent } from './component/passwordreset/passwordreset.component';

const routes: Routes = [
  {path: 'login',component:LoginComponent},
  {path:'employees',component:EmployeeComponent,canActivate:[AuthGuard]},
  {path:'register',component:RegistrationComponent},
  {path:'forgot',component:ForgotpasswordComponent},
  {path:'passwordreset',component:PasswordresetComponent},
  {path:'**',redirectTo:'login'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
