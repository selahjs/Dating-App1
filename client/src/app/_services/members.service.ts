import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from'src/app/_models/member';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  // we are loading our users/members form the server api
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  constructor(private http: HttpClient) { }

  getMembers(){
    //we send our token through every request in httpOptions
    //we are getting all the users (their username and token)
    if(this.members.length > 0) return of(this.members); //return if there are already members in the array
    //and not make the api request, ie the members are already saved in the client.
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map(members=>{ //store the member's state on the members array 
        this.members = members;
        return members;
      })
    )
  }
  getMember(username: string){
    //we are getting a single user (username and token), the get method only needs a url.
    const member = this.members.find(x => x.username === username);
    if(member !== undefined) return of(member); //return if you find the user (as an observable)
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member){
    //the put method takes a url and a body as an input, notice the url and the member are separated by a comma
    //the put request doesn't return a member for us. so we update the members array while we are sending
    //-the api request. we assign the updated member by going to it's index.
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(()=>{
        const index = this.members.indexOf(member);// we find the index of the member in members
        this.members[index] = member; //we assign the update of member to the old member.
      })
    );
    }
}
