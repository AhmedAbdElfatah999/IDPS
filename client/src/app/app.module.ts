import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { CoreModule } from './core/core.module';
import { IDPSModule } from './idps/idps.module';
import { NO_ERRORS_SCHEMA } from '@angular/compiler';
// PaginationModule
import { NgxPaginationModule } from 'ngx-pagination';
import { AlertifyService } from './account/alertify.service';
import { AuthService } from './account/auth.service';
import { AccountService } from './account/account.service';
import { MessagesResolver } from './account/messages.resolver';
import { MessagesComponent } from './messages/messages.component';
import { FormsModule } from '@angular/forms';
import { PatientMessagesComponent } from './patient-messages/patient-messages.component';

@NgModule({
  declarations: [AppComponent, MessagesComponent, PatientMessagesComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    CoreModule,
    IDPSModule,
    NgxPaginationModule,
    FormsModule
  ],
  providers: [
    AuthService,
    AlertifyService,
    AccountService,
    MessagesResolver
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
