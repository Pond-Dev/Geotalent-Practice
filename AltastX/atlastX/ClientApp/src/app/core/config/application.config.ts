export interface ApplicationConfig {
  /**
   * Build option environment. In `environment.prod.ts` file should be set `true` only.
   */
  production?: boolean

  /**
   * The url address of back-end (ServerApp).
   *
   * Example on `development` use https://localhost:5003
   * or on `production` use https://atlasx.cdg.co.th/axwa-demo
   */
  // backEndUrl?: string

  /**
   * The url address of web service (AtlasX Web Service).
   *
   * Example on `development` use https://localhost:5001
   * or on `production` use https://atlasx.cdg.co.th/axws-demo
   */
  webServiceUrl?: string

  /**
   * The url address of direct login to old AtlasX.
   */
  directAccessUrl?: string
}
