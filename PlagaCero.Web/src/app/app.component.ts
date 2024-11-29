import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './site_header/header/header.component';
import { NgFor, NgIf } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, NgFor, NgIf, MatButtonModule, MatProgressSpinnerModule],
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
      link: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRvNUBH_oPcYGA7OBcm9tfVG8hcWbjTa6OrEg&s"
    },



  ]
}

