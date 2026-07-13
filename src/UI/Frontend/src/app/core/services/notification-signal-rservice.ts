import { inject, Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { NotificationDto } from '../models/notification-dto.interface';
import { TokenService } from './token-service';

@Injectable({
  providedIn: 'root',
})
export class NotificationSignalRService {
  private connection: signalR.HubConnection;
  private tokenService = inject(TokenService);

  private notificationReceived = new Subject<NotificationDto>();

  notificationReceived$ = this.notificationReceived.asObservable();

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:5001/hubs/notifications', {
        accessTokenFactory: () => localStorage.getItem('accessToken') ?? ''
      })
      .withAutomaticReconnect()
      .build();

    this.registerHandlers();
  }

  private registerHandlers(): void {
    this.connection.on('ReceiveNotification', (notification: NotificationDto) => {
      this.notificationReceived.next(notification);
    });
  }

  async startConnection(): Promise<void> {
    if (this.connection.state === signalR.HubConnectionState.Connected)
      return;

    try {
      await this.connection.start();
      console.log('SignalR Connected');
    } catch (error) {
      console.error('SignalR Connection Error:', error);
    }

    // await this.connection.start();
  }

  async stopConnection(): Promise<void> {
    await this.connection.stop();
  }
}
