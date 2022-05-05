import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from'src/app/_models/member';
const httOptions = {
  
}

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  // we are loading our users/members form the server api
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }
  getMembers(){
    //we send our token through every request in httpOptions
    return this.http.get<Member[]>(this.baseUrl + 'users');
  }
  getMember(username: string){
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }
}
