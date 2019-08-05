import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {VacationRequestComponent} from "./vacation-request/vacation-request.component";
import {AccountManagementComponent} from "./account-management/account-management.component";
import {LoginComponent} from "./login/login.component";
import {InboxComponent} from "./inbox/inbox.component";
import {OutboxComponent} from "./outbox/outbox.component";
import {ReceiversComponent} from "./receivers/receivers.component";
import {ProcessDefinition} from "./service/models";
import {ProcessDefinitionListComponent} from "./process-definition-list/process-definition-list.component";

const routes: Routes = [
  {path: '', pathMatch: 'full', redirectTo: 'vacation'},
  {path: 'vacation', component: VacationRequestComponent},
  {path: 'login', component: LoginComponent},
  {path: 'account', component: AccountManagementComponent},
  {path: 'inbox', component: InboxComponent},
  {path: 'outbox', component: OutboxComponent},
  {path: 'receivers', component: ReceiversComponent},
  {path: 'processDefinitions', component: ProcessDefinitionListComponent},

];

@NgModule({
  imports: [RouterModule.forRoot(routes, {useHash: true})],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
