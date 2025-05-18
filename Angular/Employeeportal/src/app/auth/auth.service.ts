import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private authurl = "https://localhost:44372/api/auth"

  constructor(private http: HttpClient) { }

  //login api call
  login(credentials:{username: string;password:string}):Observable<any>{
    return this.http.post<any>(this.authurl+"/login",credentials);
  }
  Register(EmpDetails:{FirstName: string;LastName:string;DOB:Date|null;Gender:string;Email:string;PhoneNumber:string;Address:string;Password:string}): Observable<any> {
    return this.http.post<any>(this.authurl+"/Register",EmpDetails);
  }

  forgotpassword(username: string): Observable<any> {
    return this.http.get<any>(this.authurl+"/ForgotPassword",{params:{username}});
  }

  VerifyOTP(Details:{Email: string;OTP: string}): Observable<any> {
    return this.http.post<any>(this.authurl+"/ValidateOTP",Details);
  }

  PasswordReset(Details:{Email: string;Password: string}): Observable<any> {
    debugger
    return this.http.put<any>(this.authurl+"/UpdatePassword",Details);
  }

  //store token after login
  storeToken(token:string){
    localStorage.setItem('authToken',token)
  }

  //Retrive Token
  getToken(){
    return localStorage.getItem('authToken')
  }

  //Check if user is loged in
  isLoggedin(): boolean{
      return !!this.getToken()
    }

    //logout
    logout(){
      localStorage.removeItem('authToken')
    }

}
