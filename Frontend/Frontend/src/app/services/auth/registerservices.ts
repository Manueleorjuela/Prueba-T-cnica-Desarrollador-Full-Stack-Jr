import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http'; 
import { catchError, Observable, tap, throwError } from 'rxjs';
import { registerRequest,} from './registeresquest';
import { Resultado } from '../../Results/Risponse';

@Injectable({
  providedIn: 'root'
})
export class registerService {  
  constructor(private http: HttpClient) {}   
register(credentials: registerRequest): Observable<Resultado> {
   return this.http.post<Resultado>(
    'https://localhost:7076/API/Register',
    credentials 
  ).pipe(
    catchError(this.handleError));
  }
  private handleError(error:HttpErrorResponse){
    if (error.status == 0){
      console.error('Se ha producido un eror', error)

    }else{
      console.error('Backend  retorno código de estado ', error.status , error)
    } 
    return throwError  (()=> new Error('Algo falló. Por favor intente nuevamente'));  
  }
}

