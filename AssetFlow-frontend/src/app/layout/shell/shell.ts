import { Component, inject } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink, RouterOutlet } from '@angular/router';
import { MatAnchor } from "@angular/material/button";
import { Auth } from '../../core/auth/auth';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [RouterOutlet, RouterLink, MatSidenavModule, MatListModule, MatToolbarModule, MatAnchor],
  templateUrl: './shell.html',
  styleUrl: './shell.scss',
})
export class Shell {

  private auth = inject(Auth);

  logout(){
    this.auth.logout();
  }

  isAdmin = () => this.auth.getRole() === 'Admin';
}
