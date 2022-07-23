import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/message';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  //we get a paginated message from the server
  getMessages(pageNumber, pageSize, container){
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('Container',container); //appending key value pairs where 'Container' is key and the..
    
    //this get request might be confusing as it is deiffernt than the others ...the actual request is ..
    //..inside the paginationHelpers function library
    return getPaginatedResult<Message[]>(this.baseUrl + 'messages',params,this.http);

  }
  getMessageThread(username : string){
    return this.http.get<Message[]>(this.baseUrl + 'messages/thread/'+ username);
  }
  
  sendMessage(username: string, content: string){
    return this.http.post<Message>(this.baseUrl + 'messages', {recipientUsername:username , content});
  }

  deleteMessage(id : number){
    return this.http.delete(this.baseUrl + 'messages/' + id);
  }
}
