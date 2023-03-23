import { APP_INITIALIZER, Provider } from '@angular/core'

import config from '@arcgis/core/config'

export const ArcgisjsapiProvider: Provider = {
  provide: APP_INITIALIZER,
  useFactory: () => {
    return () => {
      config.assetsPath = `${document.baseURI}assets`
    }
  },
  multi: true,
}
