import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiurl="https://localhost:44372/api/Employee"
  constructor(private http: HttpClient) { 

  }
  getEmployees(): Observable<any[]> {    
    return this.http.get<any[]>(this.apiurl+"/GetEmployees")
  }

  deleteEmployee(empId:string): Observable<any> {
    return this.http.delete(this.apiurl+"/DeleteEmployee/"+empId);
  }
  DownloadEmployeeDetails():Observable<any> {
    return this.http.get(this.apiurl+"/DownloadEmployeeDetails/",{responseType:'blob'});
  }
  UploadWorkerDetails(formData:FormData):Observable<any> {
    return this.http.post(this.apiurl+"/UploadDetailsFromExcel/",formData);
  }
}
