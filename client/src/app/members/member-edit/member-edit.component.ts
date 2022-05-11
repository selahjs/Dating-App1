import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm; //we are accessing the form
  member: Member;
  user: User;
  //the HostListener helps us access the browser event "window:beforeunload", This event enables 
  //-a web page to trigger a confirmation dialog asking the user if they really want to leave the page.
  @HostListener('window:beforeunload',['$event']) unloadNotification($event:any){
    if(this.editForm.dirty){
      $event.returnValue = true;
    }
  }

  constructor(private accountService: AccountService, private memberService: MembersService,
    private toastr: ToastrService) {
      //inorder to use the user we need to get it out of the currentuser observale inside the localstorage
      //and asign it to our user variable ... currentuser is crrently loggedin user
      this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);

   }

  ngOnInit(): void {
    this.loadMember()
  }

  loadMember(){
    this.memberService.getMember(this.user.username).subscribe(member =>{
      this.member = member;
    })
  }

  updateMember(){
    this.memberService.updateMember(this.member).subscribe(()=>{
      this.toastr.success('Profile Updated Successfully');
      this.editForm.reset(this.member); // reset it to the updated input and form is now not dirty
    });
    
  }

}
