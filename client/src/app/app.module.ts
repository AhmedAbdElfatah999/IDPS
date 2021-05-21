import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { CoreModule } from './core/core.module';
import { IDPSModule } from './idps/idps.module';
// PaginationModule
import { NgxPaginationModule } from 'ngx-pagination';
import { AlertifyService } from './account/alertify.service';
import { AuthService } from './account/auth.service';
import { AccountService } from './account/account.service';
import { MessagesResolver } from './account/messages.resolver';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [AppComponent],
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
