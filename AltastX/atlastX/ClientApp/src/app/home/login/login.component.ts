import { Component, OnInit } from '@angular/core'
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms'

import { AxAuthenticationService } from '@atlasx/core/authentication'

interface LoginForm {
  username: FormControl<string>
  password: FormControl<string>
}

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup<LoginForm>

  constructor(private fb: FormBuilder, private authService: AxAuthenticationService) {}

  ngOnInit(): void {
    // Create form group with required validation.
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    })
  }

  signIn() {
    // Mark form controls as dirty.
    for (const key in this.loginForm.controls) {
      if (this.loginForm.contains(key)) {
        this.loginForm.controls[key].markAsDirty()
        this.loginForm.controls[key].updateValueAndValidity()
      }
    }

    if (this.loginForm.valid) {
      const formValues = this.loginForm.getRawValue()
      this.authService
        .completeAuthorizationPasswordRequest(formValues.username, formValues.password)
        .then(() => {
          this.authService.authorizeCallback()
        })
        .catch(() => {
          window.alert(`User doesn't exist, or the username or password is incorrect.`)
        })
    } else {
      window.alert('Username and Password cannot be empty.')
    }
  }
}
