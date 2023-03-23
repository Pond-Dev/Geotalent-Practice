import { Component } from '@angular/core'

import { AxAuthenticationService } from '@atlasx/core/authentication'
import { AxConfigurationService } from '@atlasx/core/configuration'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent {
  constructor(public configService: AxConfigurationService, public authService: AxAuthenticationService) {}
}
