import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../environments/environment';

export interface ChatResponse {
  success: boolean;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private apiUrl = `${environment.apiUrl}/api/chat`;

  constructor(private http: HttpClient) { }

  sendMessage(message: string): Observable<ChatResponse> {
    if (!message || message.trim().length === 0) {
      return throwError(() => new Error('Message cannot be empty'));
    }

    return this.http.post<ChatResponse>(`${this.apiUrl}/ask`, { question: message })
      .pipe(
        map(response => {
          if (!response.success) {
            throw new Error(response.message || 'Failed to get response');
          }
          return response;
        }),
        catchError(error => {
          console.error('Error sending message:', error);
          return throwError(() => new Error(error.error.message || 'Failed to send message. Please try again.'));
        })
      );
  }
} 