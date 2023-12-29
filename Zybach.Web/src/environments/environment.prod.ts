export const environment = {
  production: true,
  staging: false,
  dev: false,
  mainAppApiUrl: "https://internalapi-zybach.aks-prod.sitkatech.com",
  createAccountRedirectUrl: "https://zybach.aks-prod.sitkatech.com/create-user-callback",

  geoserverMapServiceUrl: 'https://geoserver-zybach.aks-prod.sitkatech.com/geoserver/Zybach',
  boundingBoxLeft: -100.22425584641142,
  boundingBoxRight: -102.05544891242484,
  boundingBoxTop: 40.878401166400693,
  boundingBoxBottom: 41.73706831826739,

  allowOpenETSync: true,
  faviconFilename: "favicon.ico",
  geoOptixWebUrl: "https://tpnrd.geooptix.com",
  mapQuestApiUrl: "https://open.mapquestapi.com/nominatim/v1/search.php?key=gAtuAvFkArwZH61P1UVcRXseleJWEB7r",
  appInsightsInstrumentationKey: "b91b594a-1c51-461f-a892-07e4bd43aa35",
  GETEnvironmentUrl: "https://get-api.azure-api.net/GETAzureFunctionApi/",

  keystoneAuthConfiguration: {
    clientId: 'Zybach',
    issuer: 'https://identity.sitkatech.com',
    responseType: "code",
    disablePKCE: false,
    redirectUriRelative: "/signin-oidc",
    redirectUri: window.location.origin,
    scope: 'openid all_claims keystone',
    sessionChecksEnabled: true,
    logoutUrl: 'https://identity.sitkatech.com/account/logout',
    postLogoutRedirectUri: window.location.origin + '/',
    waitForTokenInMsec: 500
  }
};