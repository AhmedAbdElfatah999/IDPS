import { Component, Input, OnInit } from '@angular/core';
import { tap } from 'rxjs/operators';
import { AccountService } from '../account.service';
import { AlertifyService } from '../alertify.service';
import { AuthService } from '../auth.service';
import { Message } from '../../shared/models/message';

@Component({
  selector: 'app-messages-for-user',
  templateUrl: './messages-for-user.component.html',
  styleUrls: ['./messages-for-user.component.scss']
})
export class MessagesForUserComponent implements OnInit {

  @Input() recipientId: string;
  messages: Message[];
  newMessage: any = {};

  constructor(private userService: AccountService, private authService: AuthService,
      private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadMessages();
  }

  loadMessages() {
    const currentUserId = this.userService.User.id;
    this.userService.getMessageThread(this.userService.User.id, this.recipientId)
      .pipe(
        tap(messages => {
          for (let i = 0; i < messages.length; i++) {
            if (messages[i].isRead === false && messages[i].recipientId === currentUserId) {
              this.userService.markAsRead(currentUserId, messages[i].id);
            }
          }
        })
      )
      .subscribe(messages => {
        this.messages = messages;
    }, error => {
      this.alertify.error(error);
    });
  }

  sendMessage() {
    this.newMessage.recipientId = this.recipientId;
    this.userService.sendMessage(this.userService.User.id, this.newMessage)
      .subscribe((message: Message) => {
        this.messages.unshift(message);
        this.newMessage.content = '';
    }, error => {
      this.alertify.error(error);
    });
  }

}
