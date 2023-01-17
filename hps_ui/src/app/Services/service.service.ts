import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ServiceService {

  constructor(private http: HttpClient) { }

  getAllInfo(){
    return this.http.get('https://localhost:44388/api/Info');
  }
  insertInfo(requestBody: any){
    return this.http.post('https://localhost:44388/api/Info/CreateInfo', requestBody);
  }
  insertInfo2(input: any, fileToUpload: File){
    const formData = new FormData();
    formData.append('resume', fileToUpload, fileToUpload.name);
    formData.append('name', input?.name);
    formData.append('country', input?.country);
    formData.append('city', input?.city);
    formData.append('languages', input?.languages);
    formData.append('dateofbirth', input?.dateofbirth);
    return this.http.post('https://localhost:44388/api/Info/CreateInfo', formData);
  }
  updateInfo(requestBody: any){
    return this.http.put('https://localhost:44388/api/Info/UpdateInfo', requestBody);
  }
  delete(requestBody: any){
    return this.http.delete('https://localhost:44388/api/Info/DeleteInfo', requestBody);
  }
}
