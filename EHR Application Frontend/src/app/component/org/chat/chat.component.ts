import { Component, inject, OnInit } from '@angular/core';
import { initializeApp } from 'firebase/app';
import { getDatabase, ref, set, onValue, child, push } from 'firebase/database';
import { firebaseConfig } from '../../../../enviorment';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../navbar/navbar.component';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtService } from '../../../core/utility/jwt.service';

const app = initializeApp(firebaseConfig);
const db = getDatabase(app);
@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css',
})
export class ChatComponent implements OnInit {
  senderId: string = '';
  receiverId: string = '';
  message: string = '';
  messages: any[] = [];
  isStarted: boolean = false;

  onClickStartBtn() {
    console.log('Sender: ', this.senderId);
    console.log('Receiver: ', this.receiverId);

    if (this.senderId && this.receiverId) {
      this.isStarted = true;
      this.listenForMessages(this.senderId, this.receiverId);
    }
  }
  sendMessage(receiverId: string, message: string): void {
    console.log('recei', receiverId, 'senderId', this.senderId);
    const messageId = push(
      ref(db, `messages/${this.senderId}_${receiverId}`)
    ).key;
    if (messageId) {
      const messagePayload = {
        senderId: this.senderId,
        senderName: this.senderName, // Use senderName here
        receiverId: receiverId,
        message: message,
        timestamp: Date.now(),
      };
      console.log('Message Payload', messagePayload);
      const messageRefSenderReceiver = ref(
        db,
        `messages/${this.senderId}_${receiverId}/${messageId}`
      );
      set(messageRefSenderReceiver, messagePayload);

      const messageRefReceiverSender = ref(
        db,
        `messages/${receiverId}_${this.senderId}/${messageId}`
      );
      set(messageRefReceiverSender, messagePayload);
    }
  }

  // Listen for messages from both sender-receiver and receiver-sender paths
  listenForMessages(senderId: string, receiverId: string): void {
    const messagesRefSenderReceiver = ref(
      db,
      `messages/${senderId}_${receiverId}`
    );
    const messagesRefReceiverSender = ref(
      db,
      `messages/${receiverId}_${senderId}`
    );

    // Listen for messages from the sender-receiver path
    onValue(messagesRefSenderReceiver, (snapshot) => {
      const data = snapshot.val();
      if (data) {
        // Combine the messages and sort them by timestamp
        this.messages = Object.values(data).sort(
          (a: any, b: any) => a.timestamp - b.timestamp
        );
        console.log('Messages (Sender -> Receiver): ', this.messages);
      }
    });

    // Listen for messages from the receiver-sender path
    onValue(messagesRefReceiverSender, (snapshot) => {
      const data = snapshot.val();
      if (data) {
        // Combine the messages and sort them by timestamp
        this.messages = Object.values(data).sort(
          (a: any, b: any) => a.timestamp - b.timestamp
        );
        console.log('Messages (Receiver -> Sender): ', this.messages);
        console.log('getting the message for sender name', this.messages);
      }
    });
  }

  currentUserId: number = 0;
  senderName: string = '';
  private jwtService = inject(JwtService);
  private route = inject(ActivatedRoute);

  ngOnInit() {
    this.receiverId = this.route.snapshot.paramMap.get('id') || '';
    this.senderId = this.jwtService.getUserId();
    this.senderName = this.jwtService.getFirstName();
    this.listenForMessages(this.senderId, this.receiverId);
  }
}
