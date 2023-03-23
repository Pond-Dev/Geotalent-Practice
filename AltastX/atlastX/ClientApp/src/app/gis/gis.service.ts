import { Injectable } from '@angular/core'

import Map from '@arcgis/core/Map'
import MapView from '@arcgis/core/views/MapView'
import { AxConfigurationService, AxFunctionAttributes } from '@atlasx/core/configuration'

@Injectable()
export class GisService {
  /**
   * Function collection of GIS system. Call the `loadFunctions` method before use it.
   */
  functions: AxFunctionAttributes[] = []

  /**
   * Standard function collection of GIS system. Call the `loadFunctions` method before use it.
   */
  standardTools: AxFunctionAttributes[] = []

  /**
   * The map object. Call the `createMap` method before use it.
   */
  map: Map | null = null

  /**
   * The view object. Call the `createMap` method before use it.
   */
  mapView: MapView | null = null

  constructor(private configService: AxConfigurationService) {}

  /**
   * Load functions and standard tools. Call this method once time
   * before use the `functions` and `standardTools` properties.
   * @param systemId The system id of GIS system.
   */
  loadFunctions(systemId: string) {
    this.configService.functions.getFunctions(systemId).forEach((func) => {
      if (!func.FUNCTION_ID.startsWith('STD')) {
        this.functions.push(func)
      } else {
        this.standardTools.push(func)
      }
    })
  }

  /**
   * Create a new map and stored `map` and `view` to this service.
   * @param node Node representing the DOM element containing the view.
   */
  createMap(node: HTMLDivElement) {
    this.map = new Map({
      basemap: 'topo-vector',
    })

    console.log('node', node)

    this.mapView = new MapView({
      container: node,
      map: this.map,
      center: [100.5433989, 13.7029924], // longitude, latitude
      zoom: 20,
      padding: {
        right: 256,
      },
    })
  }
}
