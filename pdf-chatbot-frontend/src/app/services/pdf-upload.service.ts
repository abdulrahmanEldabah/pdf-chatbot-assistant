import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PDFUploadService {
  private apiUrl = '/api'; // Replace with your actual API URL

  constructor(private http: HttpClient) {}

  uploadPDF(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post(`${this.apiUrl}/upload-pdf`, formData);
  }
} 