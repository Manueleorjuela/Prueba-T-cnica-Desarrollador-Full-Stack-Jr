import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Resultado } from '../../../Results/Risponse';
import { viewservice } from '../../../services/auth/usersviewservices/viewservices';

@Component({
  selector: 'app-usersview',
  standalone:true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './usersview.html',
  styleUrl: './usersview.css'
})
export class Usersview {
    viewForm: FormGroup;
    viewError: string= '';
    viewMessage: string = '';
    user: any = null;

    constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private deleteService: viewservice
  ) {
    this.viewForm = this.formBuilder.group({
      id: ['', [Validators.required]]
    });
  }
  VolverMenuOpciones(): void {
    this.router.navigate(['/usuario-logeado']);
  }

  view(): void {
    if (this.viewForm.valid) {
      this.viewError = '';
      this.viewMessage = '';

      const token = localStorage.getItem('token');
      if (!token) {
        this.viewError = 'No tienes sesión activa. Inicia sesión primero.';
        return;
      }
      this.deleteService.viewUser(this.viewForm.value.id, token).subscribe({
        next: (res: Resultado) => {
          if (res.success) {
            this.viewMessage = res.mensaje;
            this.viewError = '';
            this.viewForm.reset();
            this.user = res.datos; 
          } else {
            this.viewError = res.mensaje;
            this.viewMessage = '';
          }
        },
        error: (err) => {
          console.error(err);
          this.viewError = 'Error en el servidor o token inválido.';
          this.viewMessage = '';
        },
        complete: () => console.info('Petición finalizada')
      });
    } else {
      this.viewForm.markAllAsTouched();
      this.viewError = 'Por favor ingresa un ID válido.';
      this.viewMessage = '';
    }
  }
}

