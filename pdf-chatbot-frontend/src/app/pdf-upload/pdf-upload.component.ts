import { Component, EventEmitter, Output } from '@angular/core';
import { PdfService } from '../services/pdf.service';
import { PDFUploadResponse } from '../models/pdf.model';
import { PdfStateService } from '../services/pdf-state.service';

@Component({
  selector: 'app-pdf-upload',
  templateUrl: './pdf-upload.component.html',
  styleUrls: ['./pdf-upload.component.scss']
})
export class PDFUploadComponent {
  @Output() fileUploaded = new EventEmitter<PDFUploadResponse>();
  
  selectedFile: File | null = null;
  isUploading = false;
  uploadError: string | null = null;
  uploadProgress = 0;

  constructor(
    private readonly pdfService: PdfService,
    private readonly pdfStateService: PdfStateService
  ) {}

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      if (file.type === 'application/pdf') {
        this.selectedFile = file;
        this.uploadError = null;
        this.uploadProgress = 0;
      } else {
        this.uploadError = 'Please select a PDF file';
        this.selectedFile = null;
      }
    }
  }

  uploadFile(): void {
    if (!this.selectedFile) return;

    this.isUploading = true;
    this.uploadError = null;
    this.uploadProgress = 0;

    this.pdfService.uploadPdf(this.selectedFile)
      .subscribe({
        next: (response: PDFUploadResponse) => {
          this.isUploading = false;
          this.uploadProgress = 100;
          this.fileUploaded.emit(response);
          this.selectedFile = null;
          this.pdfStateService.setPdfUploaded(true, response.fileName || '');
        },
        error: (error: Error) => {
          this.isUploading = false;
          this.uploadError = error.message || 'Failed to upload file. Please try again.';
          console.error('Upload error:', error);
          this.pdfStateService.setPdfUploaded(false, '');
        }
      });
  }

  clearSelection(): void {
    this.selectedFile = null;
    this.uploadError = null;
    this.uploadProgress = 0;
    this.pdfStateService.setPdfUploaded(false, '');
  }
} 