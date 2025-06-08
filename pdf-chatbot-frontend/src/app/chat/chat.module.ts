import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { ChatInterfaceComponent } from './chat-interface/chat-interface.component';

@NgModule({
  declarations: [
    ChatInterfaceComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule
  ],
  exports: [
    ChatInterfaceComponent
  ]
})
export class ChatModule { } 