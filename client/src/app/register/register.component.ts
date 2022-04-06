import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  //when ever a component containes a form we use a model to store the form inputs
 
  @Output() cancelRegister = new EventEmitter();
  model: any = {};

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
    
  }

  register(){
    this.accountService.register(this.model).subscribe(response=>{
      console.log(response);
      this.cancel();
    },error=>{
      console.log(error);
    })
  }
  cancel(){
    this.cancelRegister.emit(false);
  }

}
