import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {VacationRequestComponent} from "./vacation-request/vacation-request.component";
import {AccountManagementComponent} from "./account-management/account-management.component";
import {LoginComponent} from "./login/login.component";
import {InboxComponent} from "./inbox/inbox.component";
import {OutboxComponent} from "./outbox/outbox.component";

const routes: Routes = [
  {path: '', pathMatch: 'full', redirectTo: 'vacation'},
  {path: 'vacation', component: VacationRequestComponent},
  {path: 'login', component: LoginComponent},
  {path: 'account', component: AccountManagementComponent},
  {path: 'inbox', component: InboxComponent},
  {path: 'outbox', component: OutboxComponent},

];

@NgModule({
  imports: [RouterModule.forRoot(routes, {useHash: true})],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
