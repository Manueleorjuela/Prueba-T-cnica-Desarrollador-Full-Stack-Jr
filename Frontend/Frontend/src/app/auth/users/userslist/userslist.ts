import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { listservices } from '../../../services/auth/userlistservices/listservices';
import { Resultado } from '../../../Results/Risponse';

@Component({
  selector: 'app-userslist',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './userslist.html',
  styleUrls: ['./userslist.css']
})
export class Userslist {
  listMessage: string = '';
  listError: string = '';
  users: any[] = []; 
  
  constructor(
    private router: Router,
    private userService: listservices
  ) {}

    ngOnInit(): void {
    this.listusers();
  }

  VolverMenuOpciones() {

    this.router.navigate(['/usuario-logeado']);
  }

  listusers(): void {
    const token = localStorage.getItem('token'); 
    
    if (!token) {
      this.listError = 'No tienes sesión activa. Inicia sesión primero.';
      return;
    }
    this.userService.listUsers(token).subscribe({
      next: (userData: Resultado) => {
        if (userData.success === true) {
          this.listMessage = userData.mensaje;
          this.users = userData.datos; 
          this.listError = '';
        } else {
          this.listError = userData.mensaje;
          this.users = [];
        }
      },
      error: (err) => {
        this.listError = 'Error en el servidor o token inválido';
        console.error(err);
      },
      complete: () => {
        console.info('Petición finalizada');
      }
    });
  }
}
