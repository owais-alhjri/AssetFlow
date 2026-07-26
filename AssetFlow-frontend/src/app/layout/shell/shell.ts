import { Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { BreakpointObserver } from '@angular/cdk/layout';
import { map } from 'rxjs';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { Auth } from '../../core/auth/auth';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    MatSidenavModule,
    MatListModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
  ],
  templateUrl: './shell.html',
  styleUrl: './shell.scss',
})
export class Shell {
  private auth = inject(Auth);
  private breakpoints = inject(BreakpointObserver);

  // Drives the drawer's mode ('over' vs 'side') and the hamburger's visibility.
  // A media query can't do this on its own — sidenav mode is an input, not a
  // style. 900px rather than a phone breakpoint: a 240px drawer alongside the
  // assets table stops fitting well before the tablet boundary.
  isHandset = toSignal(
    this.breakpoints.observe('(max-width: 900px)').pipe(map((result) => result.matches)),
    { initialValue: false },
  );

  isAdmin = () => this.auth.getRole() === 'Admin';
  role = () => this.auth.getRole();

  logout() {
    this.auth.logout();
  }
}
