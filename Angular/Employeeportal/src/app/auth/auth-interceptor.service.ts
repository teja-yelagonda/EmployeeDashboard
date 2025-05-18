import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptorService implements HttpInterceptor{
  constructor(private authservice:AuthService, private router:Router){}
  intercept(req: HttpRequest<any>, next: HttpHandler){
    const token =localStorage.getItem('authToken')

    let cloned=req;
    console.log(cloned)

    if(token){
      cloned=req.clone({
        setHeaders:{
          Authorization:`Bearer ${token}`
        }
      });
    }
    return next.handle(cloned).pipe(
      catchError((error:HttpErrorResponse)=>{
        if(error.status==401){
          this.authservice.logout()
          this.router.navigate(['/login'])
        }
        return throwError(()=>error);
      })
    );
    // return next.handle(req)
  }
}
