import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Resultado } from '../../../Results/Risponse';
import { deleteservice } from '../../../services/auth/userdeleteservices/deleteservices';

@Component({
  selector: 'app-usersdelete',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './usersdelete.html',
  styleUrls: ['./usersdelete.css']
})
export class Usersdelete {
  deleteForm: FormGroup;
  deleteError: string = '';
  deleteMessage: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private deleteService: deleteservice
  ) {
    this.deleteForm = this.formBuilder.group({
      id: ['', [Validators.required]]
    });
  }

  VolverMenuOpciones(): void {
    this.router.navigate(['/usuario-logeado']);
  }

  eliminar(): void {
    if (this.deleteForm.valid) {
      this.deleteError = '';
      this.deleteMessage = '';

      const token = localStorage.getItem('token');
      if (!token) {
        this.deleteError = 'No tienes sesión activa. Inicia sesión primero.';
        return;
      }

      this.deleteService.deleteUser(this.deleteForm.value.id, token).subscribe({
        next: (res: Resultado) => {
          if (res.success) {
            this.deleteMessage = res.mensaje;
            this.deleteError = '';
            this.deleteForm.reset();
          } else {
            this.deleteError = res.mensaje;
            this.deleteMessage = '';
          }
        },
        error: (err) => {
          console.error(err);
          this.deleteError = 'Error en el servidor o token inválido.';
          this.deleteMessage = '';
        },
        complete: () => console.info('Petición finalizada')
      });
    } else {
      this.deleteForm.markAllAsTouched();
      this.deleteError = 'Por favor ingresa un ID válido.';
      this.deleteMessage = '';
    }
  }
}
