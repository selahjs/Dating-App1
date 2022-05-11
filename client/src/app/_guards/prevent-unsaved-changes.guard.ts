import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root'
})
//This route guard only helps when we are inside the angular application(when we are moving b/n components)
//and it is applied on the app-routing-module level(not in component class)
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {
  canDeactivate( component: MemberEditComponent): boolean {
      if(component.editForm.dirty){
        return confirm('Are you sure you want to continue? Any unsaved chages will be lost!');
        //If the form is dirty the prompt will show up, otherwise canDeactivate from the component
      }
    return true;
  }
  
}
