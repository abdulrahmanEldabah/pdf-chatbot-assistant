import { Component, OnInit } from '@angular/core';
import { ChatService, ChatResponse } from '../../services/chat.service';
import { PdfStateService } from '../../services/pdf-state.service';
import { Message } from '../../models/message.model';

@Component({
  selector: 'app-chat-interface',
  templateUrl: './chat-interface.component.html',
  styleUrls: ['./chat-interface.component.scss']
})
export class ChatInterfaceComponent implements OnInit {
  messages: Message[] = [];
  userMessage: string = '';
  isLoading: boolean = false;
  hasPdfUploaded: boolean = false;
  uploadedFileName: string = '';

  constructor(
    private chatService: ChatService,
    private pdfStateService: PdfStateService
  ) {}

  ngOnInit() {
    this.pdfStateService.hasPdfUploaded$.subscribe(uploaded => {
      this.hasPdfUploaded = uploaded;
    });

    this.pdfStateService.uploadedFileName$.subscribe(fileName => {
      this.uploadedFileName = fileName;
    });
  }

  sendMessage() {
    if (!this.userMessage.trim()) {
      return;
    }

    if (!this.hasPdfUploaded) {
      this.messages.push({
        content: 'Please upload a PDF file first.',
        isUser: false,
        timestamp: new Date()
      });
      return;
    }

    const message = this.userMessage;
    this.messages.push({
      content: message,
      isUser: true,
      timestamp: new Date()
    });
    this.userMessage = '';
    this.isLoading = true;

    this.chatService.sendMessage(message).subscribe({
      next: (response: ChatResponse) => {
        this.messages.push({
          content: response.message,
          isUser: false,
          timestamp: new Date()
        });
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error:', error);
        this.messages.push({
          content: error.message || 'Sorry, I encountered an error. Please try again.',
          isUser: false,
          timestamp: new Date()
        });
        this.isLoading = false;
      }
    });
  }
} 