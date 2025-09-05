import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.html',
  styleUrls: ['./navbar.css']
})
export class Navbar {
  constructor(private router: Router) {}

  

  irLogin() {
    this.router.navigate(['/iniciar-sesion']);
  }

  irRegistrarse() {
    this.router.navigate(['/registrarse']);
  }
}