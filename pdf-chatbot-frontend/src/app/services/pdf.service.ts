import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { PDFUploadResponse, PDFErrorResponse } from '../models/pdf.model';

@Injectable({
  providedIn: 'root'
})
export class PdfService {
  private readonly apiUrl = `${environment.apiUrl}/api/pdf`;

  constructor(private readonly http: HttpClient) {}

  uploadPdf(file: File): Observable<PDFUploadResponse> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<PDFUploadResponse>(`${this.apiUrl}/upload`, formData)
      .pipe(
        map(response => ({
          ...response,
          uploadDate: new Date(response.uploadDate)
        })),
        catchError(this.handleError)
      );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An error occurred while processing the PDF';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = error.error.message;
    } else {
      // Server-side error
      const errorResponse = error.error as PDFErrorResponse;
      errorMessage = errorResponse.message || errorMessage;
    }

    return throwError(() => new Error(errorMessage));
  }
} 