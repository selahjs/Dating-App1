import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages : Message[];
  pagination : Pagination;
  pageNumber =1;
  pageSize = 5;
  container = "Unread";
  loading = false;

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
    this.loadMessages()
  }

  loadMessages(){
    this.loading = true;
    this.messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe(response =>
      {
        this.messages = response.result;
        this.pagination = response.pagination;
        this.loading = false;
      })
  }

  deleteMessage(id: number){
    //we put an empty parenthesis because we don't get anything from delete
    this.messageService.deleteMessage(id).subscribe(()=>{
      //we also delete 1 message with the specific id from the local array
      this.messages.splice(this.messages.findIndex(m => m.id == id), 1);
    });
  }
  pageChanged(event :any){
    if(this.pageNumber !== event.page){//we should only change the pagenumber when its not the same with the current page
      this.pageNumber = event.page;
      this.loadMessages();
    }
    
  }

}
