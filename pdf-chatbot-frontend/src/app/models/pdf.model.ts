export interface PDFUploadResponse {
  fileId: string;
  fileName: string;
  extractedText: string;
  pageCount: number;
  uploadDate: Date;
}

export interface PDFErrorResponse {
  error: string;
  message: string;
  statusCode: number;
} 