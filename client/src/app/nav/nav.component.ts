import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
  
  // we are injecting the AccountService inside this component 
  constructor(public accountService: AccountService, private router: Router,
    private toastr: ToastrService) { }

  ngOnInit(): void {
  }
  login()
  {
    return this.accountService.login(this.model).subscribe(response => {
      this.router.navigateByUrl('/members');
    })
  }
  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
  


}
