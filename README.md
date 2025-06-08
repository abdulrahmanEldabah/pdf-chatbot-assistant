# PDF Chatbot Assistant

A web application that allows users to upload PDF files and ask questions about their content using an AI model (Hugging Face FLAN-T5). The app consists of an Angular frontend and an ASP.NET Core backend.

## Features
- Upload PDF files and extract their text
- Ask questions about the uploaded PDF
- AI-powered answers using Hugging Face Inference API
- Modern, user-friendly chat interface
- Error handling and validation

## Technologies
- **Frontend:** Angular 7
- **Backend:** ASP.NET Core 9
- **AI:** Hugging Face Inference API (FLAN-T5)

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Node.js & npm](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli)

### Backend Setup
1. Navigate to the backend directory:
   ```sh
   cd PdfChatbotBackend
   ```
2. Restore dependencies:
   ```sh
   dotnet restore
   ```
3. Set your Hugging Face API token (do **not** commit this to source control!):
   - Edit `appsettings.json` and set:
     ```json
     "HuggingFace": {
       "ApiToken": "YOUR_HUGGING_FACE_API_TOKEN"
     }
     ```
   - Or set as an environment variable:
     ```sh
     set HuggingFace__ApiToken=YOUR_HUGGING_FACE_API_TOKEN
     ```
4. Run the backend:
   ```sh
   dotnet run
   ```
   The backend will be available at `https://localhost:5001`.

### Frontend Setup
1. Navigate to the frontend directory:
   ```sh
   cd pdf-chatbot-frontend
   ```
2. Install dependencies:
   ```sh
   npm install
   ```
3. Start the frontend:
   ```sh
   npm start
   ```
   The frontend will be available at `http://localhost:4200` (or as configured).

## API Documentation
- The backend uses Swagger for API documentation. Once running, visit:
  - `https://localhost:5001/swagger`

## Project Structure
```
PDF-Chatbot-Assistant/
  pdf-chatbot-frontend/   # Angular frontend
  PdfChatbotBackend/      # ASP.NET Core backend
```

## Environment Variables
- **HuggingFace__ApiToken**: Your Hugging Face API token (required for backend)

## Security
- **Never commit secrets** (API tokens, passwords) to source control.
- Use environment variables or secret managers for sensitive data.

## Contributing
Pull requests are welcome! For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](LICENSE)

---

## Further Documentation
- [Angular Documentation](https://angular.io/docs)
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [Hugging Face Inference API](https://huggingface.co/docs/api-inference/index) 