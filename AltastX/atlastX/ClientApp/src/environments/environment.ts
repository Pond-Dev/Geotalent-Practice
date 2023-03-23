import { AxAuthenticationConfig } from '@atlasx/core/authentication'
import { ApplicationConfig } from '../app/core/config/application.config'

// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment: ApplicationConfig & AxAuthenticationConfig = {
  production: false,
  // webServiceUrl: 'https://localhost:5001',
  webServiceUrl: 'https://atlasx.cdg.co.th/axws-demo',

  // OAuth client information.
  clientId: 'atlasx-web-application',
  clientSecret: '02FZf68/Xff0hrSQgc7Je5CNgdjaSgltNU4gb3AbJ9A=',

  // When use full secure authentication, application are required
  // the callbackUri property.
  fullSecureAuthentication: true,
  callbackUri: 'http://localhost:4200/callback',

  // If not use full secure authentication, application are required
  // the loginUri property.
  // fullSecureAuthentication: false,
  // loginUri: 'http://localhost:4200/login'
}

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error'  // Included with Angular CLI.
