import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators';
import { User } from '../_models/user';

// the account service is injectable, meaning it can be used in other components
@Injectable({
  providedIn: 'root'
})
//This Service will make a request to our API controller
export class AccountService {
  baseUrl = 'https://localhost:5001/api/';
  //we are creating an observable of type ReplaySubject for User.by convention we use $ sign to represent observables
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http:HttpClient) { }
  // we used the post method because we are sending user data as an object to our controller
  // we will get a reponse with a userDto(username and token) then we store it on the browser
  // - for the purpose of Persisting the login (i.e the client login info will be on the browser)
  // - for future use of the API like a session
  login(model: any){

    return this.http.post(this.baseUrl+'account/login',model).pipe(
      //the type of response will be a User interface from _modles
      map((response: User)=>{
        const user = response;
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  register(model: any){
    return this.http.post(this.baseUrl+'account/register', model).pipe(
      map((user: User)=>{
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
        return user;
      })
    )
  }

  setCurrentUser(user: User){
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

}
