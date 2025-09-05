import { Component } from '@angular/core';
import { Router } from '@angular/router';


@Component({
  selector: 'app-users',
  imports: [],
  standalone: true,
  templateUrl: './users.html',
  styleUrls: ['./users.css']
})
export class Users {
  constructor(private router: Router) {}

 listarUsuarios() {
    this.router.navigate(['/listar-usuarios']);
  }

  CerrarSesion() {
    localStorage.clear();
    this.router.navigate(['/inicio']);
  }

  verUsuario() {
     this.router.navigate(['/ver-usuario']);
  }

  crearUsuario() {
    this.router.navigate(['/añadir-usuario']);
  }

  editarUsuario() {
    this.router.navigate(['/editar-usuario']);
  }

  eliminarUsuario() {
     this.router.navigate(['/eliminar-usuario']);
  }
}