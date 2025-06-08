import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PdfStateService {
  private hasPdfUploaded = new BehaviorSubject<boolean>(false);
  private uploadedFileName = new BehaviorSubject<string>('');
  
  hasPdfUploaded$ = this.hasPdfUploaded.asObservable();
  uploadedFileName$ = this.uploadedFileName.asObservable();

  setPdfUploaded(uploaded: boolean, fileName: string = '') {
    this.hasPdfUploaded.next(uploaded);
    this.uploadedFileName.next(fileName);
  }
} 