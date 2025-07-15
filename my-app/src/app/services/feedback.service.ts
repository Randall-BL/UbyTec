import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FeedbackService {
  private baseUrl = 'https://apifeedbacktec-hdgeazc9ewewbzhc.canadacentral-01.azurewebsites.net/api/Feedback'; // Cambia la URL base según sea necesario

  constructor(private http: HttpClient) {}

  // Obtener todos los feedbacks
  getFeedbacks(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}`);
  }

// Obtener feedback por PedidoID
getFeedbackByPedidoId(pedidoId: number): Observable<any[]> {
  return this.http.get<any[]>(`${this.baseUrl}/pedido/${pedidoId}`);
}

  // Crear un nuevo feedback
  createFeedback(feedback: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}`, feedback);
  }

  // Actualizar un feedback existente por ID
  updateFeedback(feedbackId: number, feedback: any): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${feedbackId}`, feedback);
  }

  // Eliminar un feedback por ID
  deleteFeedback(feedbackId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${feedbackId}`);
  }
}
