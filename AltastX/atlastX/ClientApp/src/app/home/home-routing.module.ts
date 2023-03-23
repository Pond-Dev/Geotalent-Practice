import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'

import { AxLoginGuard } from '@atlasx/core/authentication'

import { CallbackComponent } from './callback/callback.component'
import { HomeComponent } from './home.component'
import { LoginComponent } from './login/login.component'

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    // If you would like to required authenticate this route, uncomment the `canActivate`
    // to enable authentication before navigates to route.
    // , canActivate: [AxAuthenticationGuard] // <--- Enable HERE
  },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [AxLoginGuard],
  },
  {
    path: 'callback',
    component: CallbackComponent,
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HomeRoutingModule {}
