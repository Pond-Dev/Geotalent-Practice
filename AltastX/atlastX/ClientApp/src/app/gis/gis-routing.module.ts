import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'

import { GisComponent } from './gis.component'

const routes: Routes = [
  {
    path: '',
    component: GisComponent,
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GisRoutingModule {}
