import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'

import { GisRoutingModule } from './gis-routing.module'
import { GisComponent } from './gis.component'
import { GisService } from './gis.service'

@NgModule({
  declarations: [GisComponent],
  imports: [CommonModule, GisRoutingModule],
  providers: [GisService],
})
export class GisModule {}
