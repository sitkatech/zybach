export const environment = {
  production: false,
  staging: false,
  dev: true,
  mainAppApiUrl: "https://host.docker.internal:8712",
  createAccountUrl: "https://host.docker.internal:7112/Account/Register?ClientID=Zybach&RedirectUrl=",
  createAccountRedirectUrl: "https://zybach.localhost.sitkatech.com:8713/create-user-callback",

  geoserverMapServiceUrl: "http://localhost:7615/geoserver/Zybach",

  boundingBoxLeft: -100.22425584641142,
  boundingBoxRight: -102.05544891242484,
  boundingBoxTop: 40.878401166400693,
  boundingBoxBottom: 41.73706831826739,
  
  allowOpenETSync: true,

  faviconFilename: "favicon.ico",
  geoOptixWebUrl: "https://tpnrd.geooptix.com",
  mapQuestApiUrl: "https://open.mapquestapi.com/nominatim/v1/search.php?key=gAtuAvFkArwZH61P1UVcRXseleJWEB7r",
  appInsightsInstrumentationKey: null,
  GETEnvironmentUrl: null,

  keystoneAuthConfiguration: {
    clientId: "Zybach",
    issuer: "https://host.docker.internal:7112",
    responseType: "code",
    disablePKCE: false,
    redirectUriRelative: "/",
    scope: "openid profile offline_access keystone",
    sessionChecksEnabled: false,
    logoutUrl: "https://host.docker.internal:7112/Account/logout",
    postLogoutRedirectUri: "/",
    waitForTokenInMsec: 500
  }
};