import { Component } from '@angular/core';
import { NavBarComponent } from './core/nav-bar/nav-bar.component';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  imports: [NavBarComponent]
})
export class AppComponent {
  title = 'client';
}
