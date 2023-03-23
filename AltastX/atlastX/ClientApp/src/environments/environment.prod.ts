import { AxAuthenticationConfig } from '@atlasx/core/authentication'
import { ApplicationConfig } from '../app/core/config/application.config'


export const environment: ApplicationConfig & AxAuthenticationConfig = {
  production: true,
  webServiceUrl: 'https://atlasx.cdg.co.th/axws-demo',

  // OAuth client information.
  clientId: 'atlasx-web-application',
  clientSecret: '02FZf68/Xff0hrSQgc7Je5CNgdjaSgltNU4gb3AbJ9A=',

  // When use full secure authentication, application are required
  // the callbackUri property.
  fullSecureAuthentication: true,
  callbackUri: 'https://atlasx.cdg.co.th/axwa-demo/callback',

  // If not use full secure authentication, application are required
  // the loginUri property.
  // fullSecureAuthentication: false,
  // loginUri: 'https://atlasx.cdg.co.th/axwa-demo/login'
}
