import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';


@Injectable({
  providedIn: 'root'
})
export class SignupService {

  constructor(private http:HttpClient) { }

 
  readonly baseURL1 ='https://8080-effddfdaeebcfacbdcbaeadbebabcdebdca.project.examly.io/api/saveUser';
  readonly baseURL2 ='https://8080-effddfdaeebcfacbdcbaeadbebabcdebdca.project.examly.io/api/saveAdmin';

  addUser(Obj:any){
    return this.http.post<any>(this.baseURL1,Obj);
   }

   addAdmin(Obj:any){
    console.log(Obj,"Data");
    return this.http.post<any>(this.baseURL2,Obj);
   }
}
