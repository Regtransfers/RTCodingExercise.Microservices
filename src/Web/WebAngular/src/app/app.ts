import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PlateListComponent } from './components/plate-list/plate-list';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, PlateListComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('Regtransfers Plate Shop');
}
