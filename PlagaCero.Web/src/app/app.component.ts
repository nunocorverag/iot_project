import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './site_header/header/header.component';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, NgFor, NgIf ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'PlagaCero.Web';

  plantas:any = [

    {
      color: "verde",
      temperatura: 32,
      humedad: 20,
      estado: "sano",
    },


    {
      color: "amarillo",
      temperatura: 40,
      humedad: 10,
      estado: "infectado",
    },


    {
      color: "amarillo",
      temperatura: 35,
      humedad: 18,
      estado: "riesgo",
    },
  ]

  

}
