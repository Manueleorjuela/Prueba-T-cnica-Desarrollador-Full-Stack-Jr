import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Resultado } from '../../../Results/Risponse';
import { EditService } from '../../../services/auth/usereditservices/editservices';

@Component({
  selector: 'app-usersedit',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './usersedit.html',
  styleUrls: ['./usersedit.css']
})
export class Usersedit {
  editForm: FormGroup;
  editError: string = '';
  editMessage: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private editService: EditService
  ) {
    this.editForm = this.formBuilder.group({
      id: ['', Validators.required], // ID del usuario a editar
      email: ['', [Validators.required, Validators.email]],
      role: ['', [Validators.required, Validators.pattern(/^(admin|user)$/)]],
      name: ['', [Validators.required, Validators.pattern(/^[a-zA-Z\s]+$/)]],
      password: ['', Validators.required]
    });
  }

  VolverMenuOpciones(): void {
    this.router.navigate(['/usuario-logeado']);
  }

  editar(): void {
    if (this.editForm.valid) {
      this.editError = '';
      this.editMessage = '';

      const token = localStorage.getItem('token');
      if (!token) {
        this.editError = 'No tienes sesión activa. Inicia sesión primero.';
        return;
      }

      // Extraer id y demás datos
      const { id, ...userData } = this.editForm.value;

      this.editService.editUser(id, userData, token).subscribe({
        next: (res: Resultado) => {
          if (res.success) {
            this.editMessage = res.mensaje;
            this.editError = '';
            this.editForm.reset();
          } else {
            this.editError = res.mensaje;
            this.editMessage = '';
          }
        },
        error: (err) => {
          console.error(err);
          this.editError = 'Error en el servidor o token inválido.';
          this.editMessage = '';
        },
        complete: () => console.info('Petición finalizada')
      });
    } else {
      this.editForm.markAllAsTouched();
      this.editError = 'Por favor completa todos los campos correctamente.';
      this.editMessage = '';
    }
  }
}



