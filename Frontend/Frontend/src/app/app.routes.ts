import { Routes } from '@angular/router';
import { Dashboard } from './pages/dashboard/dashboard';
import { Login } from './auth/login/login';
import { Users} from './auth/users/users';
import {Userslist} from './auth/users/userslist/userslist'
import {Usersnew} from './auth/users/usersnew/usersnew'
import { Usersdelete } from './auth/users/usersdelete/usersdelete';
import { Usersview } from './auth/users/usersview/usersview';
import { Usersedit } from './auth/users/usersedit/usersedit';
import{ Register} from './auth/register/register';

export const routes: Routes = [
{path: '', redirectTo: '/inicio', pathMatch: 'full'},
  { path: 'inicio', component: Dashboard },
  { path: 'iniciar-sesion', component: Login },
  {path: 'usuario-logeado', component: Users},
  {path: 'listar-usuarios', component: Userslist},
  {path: 'añadir-usuario', component: Usersnew} ,
  {path: 'eliminar-usuario', component: Usersdelete},
  {path: 'ver-usuario', component: Usersview},
  {path: 'editar-usuario', component:  Usersedit},
  {path: 'registrarse', component: Register}
];
