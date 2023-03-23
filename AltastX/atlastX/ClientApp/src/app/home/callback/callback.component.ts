import { Component, OnInit } from '@angular/core'

import { AxAuthenticationService } from '@atlasx/core/authentication'

@Component({
  selector: 'app-callback',
  template: `<p>Please wait, validating your sign in.</p>`,
  styles: ['app-callback {  display: block; }'],
})
export class CallbackComponent implements OnInit {
  constructor(private authService: AxAuthenticationService) {}

  ngOnInit(): void {
    this.authService.authorizeCallback()
  }
}
