import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Resultado } from '../../../Results/Risponse';
import { Newservice } from '../../../services/auth/usernewservices/newservices';
import { newRequest } from '../../../services/auth/usernewservices/newrequest';

@Component({
  selector: 'app-usersnew',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './usersnew.html',
  styleUrls: ['./usersnew.css']
})
export class Usersnew {
  NewForm: FormGroup;
  NewError: string = '';
  NewMessage: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private AddService: Newservice,
  ) {
    this.NewForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      role: ['', [Validators.required, Validators.pattern(/^(admin|user)$/)]],
      password: ['', Validators.required],
      name: ['', [Validators.required, Validators.pattern(/^[a-zA-Z\s]+$/)]]
    });
  }

  VolverMenuOpciones(): void {
    this.router.navigate(['/usuario-logeado']);
  }

  add(): void {
    if (this.NewForm.valid) {
      this.NewError = '';
      this.NewMessage = '';

      const token = localStorage.getItem('token');
      if (!token) {
        this.NewError = 'No tienes sesión activa. Inicia sesión primero.';
        return;
      }

      this.AddService.addUser(this.NewForm.value as newRequest, token).subscribe({
        next: (userData: Resultado) => {
          if (userData.success) {
            this.NewMessage = userData.mensaje;
            this.NewError = ''; 
            this.NewForm.reset(); 
          } else {
            this.NewError = userData.mensaje;
            this.NewMessage = '';
          }
        },
        error: (errorData) => {
          console.error(errorData);
          this.NewError = errorData.error?.mensaje || 'Error en el servidor o token inválido.';
          this.NewMessage = ''; 
        },
        complete: () => {
          console.info("Petición finalizada");
        }
      });
    } else {
      this.NewForm.markAllAsTouched();
      this.NewError = 'Por favor, ingresa los datos correctamente.';
      this.NewMessage = ''; 
    }
  }
}
