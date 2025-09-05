import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http'; 
import { catchError, Observable, throwError } from 'rxjs';
import { Resultado } from '../../../Results/Risponse';

@Injectable({
  providedIn: 'root'
})
export class deleteservice {  
  constructor(private http: HttpClient) {}   

  deleteUser(id: string, token: string): Observable<Resultado> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.delete<Resultado>(
      `https://localhost:7076/API/Admin/Eliminar_Usuario/${id}`,
      { headers }
    ).pipe(
      catchError(this.handleError)
    );
  }
  private handleError(error: HttpErrorResponse) {
    if (error.status === 0) {
      console.error('Se ha producido un error:', error);
    } else {
      console.error('Backend retornó código de estado', error.status, error);
    }
    return throwError(() => new Error('Algo falló. Por favor intente nuevamente'));
  }
}
