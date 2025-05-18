import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../../service/employee.service';

@Component({
  selector: 'app-employee',
  standalone: false,
  templateUrl: './employee.component.html',
  styleUrl: './employee.component.css'
})
export class EmployeeComponent implements OnInit{
  employees: any[]=[];
  deleteEmp=''
  errorMessage=''
  selectedFile:any
  
  constructor(private employeeservices: EmployeeService){}
  ngOnInit(): void {
    console.log("ngOnInit called")
    
    this.employeeservices.getEmployees().subscribe({
      next:(data)=>{
        
        this.employees=data;
        console.log(this.employees)

      },
      error:(error)=>{          
        console.error("error fetching employees",error)
        this.errorMessage="unable to fetch employee details"
      }
    }
      
    )
  }

  deleteEmpById(){
    if(!this.deleteEmp){
      this.errorMessage="Please Provide EmpId"
      return
    }
    this.employeeservices.deleteEmployee(this.deleteEmp)
    .subscribe({
      next:(data)=>{
        this.errorMessage="Deleted Employee Details"
        this.employees=this.employees.filter(emp=>emp.id!==Number(this.deleteEmp))
      },
      error:(error)=>{
        console.error("unable to delete employee", error)
        this.errorMessage="unable to delete employee details" + error
      }
    })
  }
  DownLoadEmployeeDetails(){
    this.employeeservices.DownloadEmployeeDetails().subscribe({
      next:(response)=>{
        const blob = new Blob([response],{type:'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'})
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href=url;
        a.download='EmployeeDetails.xlsx';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
      },
      error:(error)=>{
        console.error('Error downloading File',error);
        this.errorMessage="Error downloading File" + error
      }
    });
  }

  OnSelectedFile(event:any){
    const inputElement = event.target as HTMLInputElement
    if(inputElement.files && inputElement.files.length>0){
      this.selectedFile=inputElement.files[0];
    }
    else{
      console.warn("No file selected")
      this.selectedFile=null;
    }
  }
  UploadFile(){
    if(!this.selectedFile){
      this.errorMessage='Please upload File'
      return
    }
    const formData= new FormData();
    formData.append('file',this.selectedFile);
    this.employeeservices.UploadWorkerDetails(formData).subscribe({
      next:(response)=>{
        this.errorMessage='uploaded Excel Details'
      },
      error:(error)=>{
        this.errorMessage='error while uploading File'+error
      }
    })
  }
}
