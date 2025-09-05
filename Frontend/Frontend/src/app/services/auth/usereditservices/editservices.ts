import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http'; 
import { catchError, Observable, throwError } from 'rxjs';
import { Resultado } from '../../../Results/Risponse';

@Injectable({
  providedIn: 'root'
})
export class EditService {  
  constructor(private http: HttpClient) {}   

  editUser(id: string, userData: any, token: string): Observable<Resultado> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.http.put<Resultado>(
      `https://localhost:7076/API/Loged/EditarUsuario/${id}`,
      userData, // Aquí van los datos del formulario
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
