import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { PresenceService } from './presence.service';

// the account service is injectable, meaning it can be used in other components
@Injectable({
  providedIn: 'root'
})
//This Service will make a request to our API controller in the dotnet server
export class AccountService {
  baseUrl = environment.apiUrl;
  //we are creating an observable of type ReplaySubject for User.
  //by convention we use $ sign to represent observables
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();
  constructor(private http:HttpClient, private presence: PresenceService) { }
  // we used the post method because we are sending user data as an object to our controller.
  // we will get a reponse with a userDto(username and token) then we store it on the browser
  // - for the purpose of Persisting the login (i.e the client login info will be on the client/browser)
  // - for future use of the API like a session
  login(model: any){

    return this.http.post(this.baseUrl+'account/login',model).pipe(
      //the type of response will be a User interface from _modles
      //we are sending a post request to the login endpoint
      //then we use .pipe to manipulate(save the response to localstorage) the response
      map((response: User)=>{
        const user = response;
        if(user){
          this.setCurrentUser(user);

        }
      })
    );
  }

  register(model: any){
    return this.http.post(this.baseUrl+'account/register', model).pipe(
      map((user: User)=>{
        if(user){
          this.setCurrentUser(user);
          this.presence.createHubConncetion(user);
        }
        return user;
      })
    )
  }

  setCurrentUser(user: User){
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    //when we get back the token from the server the role comes either in array(for memebers with multiple roles) or just a string(for memebers with single role)
    //either way we save it as an array in the client
    Array.isArray(roles)? user.roles = roles: user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.presence.stopHubConnection();
  }

  getDecodedToken(token){
    return JSON.parse(atob(token.split(".")[1])); 
    //the atob method will convert the token to string(the payload is not encrypted soo..)
    //the token comes in 3 part object header,payload & signature. the part we are interested in the second index 
    //...ie z payload because it containes our role
  }

}
