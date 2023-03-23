import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core'
import { ActivatedRoute } from '@angular/router'

import MapImageLayer from '@arcgis/core/layers/MapImageLayer'
import { AxAuthenticationService } from '@atlasx/core/authentication'

import { GisService } from './gis.service'

@Component({
  selector: 'app-gis',
  templateUrl: './gis.component.html',
  styleUrls: ['./gis.component.scss'],
})
export class GisComponent implements OnInit, AfterViewInit {
  @ViewChild('mapDiv', { static: true }) mapDiv!: ElementRef<HTMLDivElement>

  constructor(
    private route: ActivatedRoute,
    public gisService: GisService,
    public authService: AxAuthenticationService
  ) {}

  ngOnInit() {
    // Get functions from system id.
    const systemId: string = this.route.snapshot.data.systemId
    this.gisService.loadFunctions(systemId)
  }

  ngAfterViewInit(): void {
    // Create a new map.
    this.gisService.createMap(this.mapDiv.nativeElement)

    /*** For test only ***/
    const atlasxLayer = new MapImageLayer({
      url: 'https://appserver2.cdg.co.th/arcgis/rest/services/AtlasX/City/MapServer',
      id: 'AtlasXCity',
    })
    this.gisService.map.add(atlasxLayer)
  }
}
