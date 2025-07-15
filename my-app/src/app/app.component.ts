import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {BienvenidaComponent} from "./Vistas/bienvenida/bienvenida.component";
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,BienvenidaComponent, CommonModule, FormsModule,HttpClientModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'UbyTEC';
}
