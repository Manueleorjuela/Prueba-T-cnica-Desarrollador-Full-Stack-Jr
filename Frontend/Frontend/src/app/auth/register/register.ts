import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { registerService } from '../../services/auth/registerservices';
import { registerRequest } from '../../services/auth/registeresquest';
import { Resultado } from '../../Results/Risponse';


@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, CommonModule], 
  standalone: true,
  templateUrl: './register.html',
  styleUrls: ['./register.css']
})
export class Register {
  registerForm: FormGroup;
  registerError: string = '';
  registerMessage: string = '';
  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private registerService: registerService,

  ) {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
      name: ['', [Validators.required, Validators.pattern(/^[a-zA-Z\s]+$/)]]
    });
  }
  VolverInicio() {
    this.router.navigate(['/inicio']);
  }
   register(): void {
    if (this.registerForm.valid) {
      this.registerError = '';
      this.registerMessage = '';

      this.registerService.register(this.registerForm.value as registerRequest).subscribe({
        next: (userData: Resultado) => {
          if (userData.success === true) {
            this.registerMessage = userData.mensaje;
            this.registerError = ''; 
            this.registerForm.reset();
          } else {
            this.registerError = userData.mensaje;
            this.registerMessage = '';
          
          }
        },
        error: (errorData) => {
          console.error(errorData);
          this.registerError = 'Error en el servidor. Intenta más tarde.';
          this.registerMessage = ''; 
        },
        complete: () => {
          console.info("Registro completo");
        }
      });
    } else {
      this.registerForm.markAllAsTouched();
      this.registerError = 'Por favor, ingresa los datos correctamente.';
      this.registerMessage = ''; 
    }
  }
}
