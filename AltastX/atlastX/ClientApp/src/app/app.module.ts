import { NgModule } from '@angular/core'
import { BrowserModule } from '@angular/platform-browser'

import { AxAuthenticationModule } from '@atlasx/core/authentication'
import { AxConfigurationModule } from '@atlasx/core/configuration'
import { AxWebServiceUrl } from '@atlasx/core/http-service'

import { environment } from '../environments/environment'
import { AppRoutingModule } from './app-routing.module'
import { AppComponent } from './app.component'
import { ArcgisjsapiProvider } from './gis/argisjsapi-provider'

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,

    // Required register, if application use AtlasX configuration pattern.
    // It will load configuration before application initial startup.
    AxConfigurationModule,

    // Required register, if application use authentication.
    AxAuthenticationModule.forRoot(environment),
  ],
  providers: [
    // Required register, if application use AxAuthenticationModule or AxConfigurationModule.
    { provide: AxWebServiceUrl, useValue: environment.webServiceUrl },

    // Requried register, if application use ArcGIS API for JavaScript.
    ArcgisjsapiProvider,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
