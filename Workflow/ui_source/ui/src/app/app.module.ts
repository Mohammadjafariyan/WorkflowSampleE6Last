import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {FormlyModule} from "@ngx-formly/core";
import {FormlyBootstrapModule} from "@ngx-formly/bootstrap";
import { VacationRequestComponent } from './vacation-request/vacation-request.component';
import {HttpClientModule} from "@angular/common/http";
import { AccountManagementComponent } from './account-management/account-management.component';
import { LoginComponent } from './login/login.component';
import { InboxComponent } from './inbox/inbox.component';
import {OutboxComponent} from "./outbox/outbox.component";
import { ProcessDefinitionListComponent } from './process-definition-list/process-definition-list.component';
import { ReceiversComponent } from './receivers/receivers.component';

@NgModule({
  declarations: [
    AppComponent,
    VacationRequestComponent,
    AccountManagementComponent,
    LoginComponent,
    InboxComponent,
    OutboxComponent,
    ProcessDefinitionListComponent,
    ReceiversComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormlyModule.forRoot(),

    /**
     * - Bootstrap:    FormlyBootstrapModule
     * - Material2:    FormlyMaterialModule
     * - Ionic:        FormlyIonicModule
     * - PrimeNG:      FormlyPrimeNGModule
     * - Kendo:        FormlyKendoModule
     * - NativeScript: FormlyNativescriptModule
     */
    FormlyBootstrapModule,
    FormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
