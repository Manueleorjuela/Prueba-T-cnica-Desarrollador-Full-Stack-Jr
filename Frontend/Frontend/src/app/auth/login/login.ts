import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Loginservice } from '../../services/auth/loginservices';
import { LoginRequest } from '../../services/auth/loginrequest';
import { Resultado } from '../../Results/Risponse';


@Component({
  selector: 'app-login',
  standalone: true, 
  imports: [ReactiveFormsModule, CommonModule], 
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class Login {
  loginForm: FormGroup;
  loginError: string = '';
  loginMessage: string = '';
  

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private loginService: Loginservice,

  ) {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }

    VolverInicio() {
    this.router.navigate(['/inicio']);
  }

  login(): void {
    if (this.loginForm.valid) {
      this.loginError = '';
      this.loginMessage = '';

      this.loginService.login(this.loginForm.value as LoginRequest).subscribe({
        next: (userData: Resultado) => {
          if (userData.success === true) {
            this.loginMessage = userData.mensaje;
            this.loginError = ''; 
            localStorage.setItem('token', userData.datos);
            setTimeout(() => {
            this.router.navigateByUrl('/usuario-logeado');
            this.loginForm.reset(); },2000);
          } else {
            this.loginError = userData.mensaje;
            this.loginMessage = '';
          }
        },
        error: (errorData) => {
          console.error(errorData);
          this.loginError = 'Error en el servidor. Intenta más tarde.';
          this.loginMessage = ''; 
        },
        complete: () => {
          console.info("Login completo");
        }
      });
    } else {
      this.loginForm.markAllAsTouched();
      this.loginError = 'Por favor, ingresa los datos correctamente.';
      this.loginMessage = ''; 
    }
  }

}
