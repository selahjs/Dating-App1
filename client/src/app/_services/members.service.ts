import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from'src/app/_models/member';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { take } from 'rxjs/operators';
import { User } from '../_models/user';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  // we are loading our users/members form the server api
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  memberCache = new Map(); //Map is a kind of dictionary that stores data in key value pair
  user: User;
  userParams : UserParams;

  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user =>{
      this.user = user; // we get the current user when logged in and  store it in the user varibale
      this.userParams = new UserParams(user); // we create a userParameter from the user. 
      //the userParam class addes additional prameters to theuser. and also changes the gender of the user to be sent to the api server
    })
   }

  getUserParams(){
    return this.userParams;
  }
  setUserParams(params: UserParams){
    this.userParams = params;
  }
  resetUserParams(){
    this.userParams = new UserParams(this.user);
    return this.userParams; 
  }

  getMembers(userParams: UserParams){
    //we are implementing caching here
    //in the below code we are getting a value from memeberCahe and check if there are stored caches of the same param
    var response = this.memberCache.get(Object.values(userParams).join('-'));
    if(response){
      return of(response);
    }
    //we are setting user parameters to send as query string
    let params = getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

    params = params.append("minAge", userParams.minAge.toString());
    params = params.append("maxAge", userParams.maxAge.toString());
    params = params.append("gender", userParams.gender);
    params = params.append("orderBy", userParams.orderBy);
    
    return getPaginatedResult<Member[]>(this.baseUrl + 'users',params, this.http)
      .pipe(map(response =>{
        //we are setting our response to the memberCache for every query we send to the api
        this.memberCache.set(Object.values(userParams).join('-'),response);
        return response;
      }))
  }

  
  getMember(username: string){
    const member = [...this.memberCache.values()]
      .reduce((arr, elem)=> arr.concat(elem.result),[])
      .find((member: Member)=>member.username === username);
    if(member){
      return of(member);
    }
    console.log(member);
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

    setMainPhoto(photoId: number){
      return this.http.put(this.baseUrl + 'users/set-main-photo/'+ photoId, {});
    }

    deletePhoto(photoId: number){
      return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
    }

    addLike(username: string){
      return this.http.post(this.baseUrl + 'likes/' + username,{});
    }

    getLikes(predicate: string, pageNumber, pageSize){
      //we are applying pagination to the list(likes) component
      let params = getPaginationHeaders(pageNumber, pageSize);
      params = params.append('predicate', predicate);
      //then we get a paginated list of users we like/ that like us
      return getPaginatedResult<Partial<Member[]>>(this.baseUrl + 'likes', params, this.http);
    }
    
    
}
