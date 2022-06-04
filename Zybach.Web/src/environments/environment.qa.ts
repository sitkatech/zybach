// export const environment = {
//   production: false,
//   staging: true,
//   dev: false,
//   apiHostName: 'zybach-api.qa.sycan.sitkatech.com',
//   createAccountUrl: "https://qa.keystone.sitkatech.com/Authentication/Register?RedirectUrl=",
//   createAccountRedirectUrl: "https://zybach.qa.sycan.sitkatech.com/create-user-callback",
//   allowTrading: false,

//   keystoneSupportBaseUrl: "https://qa.keystone.sitkatech.com/Authentication",

//   keystoneAuthConfiguration: {
//     clientId: 'Zybach',
//     issuer: 'https://qa.keystone.sitkatech.com/core',
//     redirectUri: window.location.origin + '/login-callback',
//     scope: 'openid all_claims keystone',
//     sessionChecksEnabled: true,
//     logoutUrl: 'https://qa.keystone.sitkatech.com/core/connect/endsession',
//     postLogoutRedirectUri: window.location.origin + '/',
//     waitForTokenInMsec: 500
//   },
//   geoserverMapServiceUrl: 'https://zybach-geoserver.qa.sycan.sitkatech.com/geoserver/Zybach'
// };


import { DynamicEnvironment } from './dynamic-environment';
class Environment extends DynamicEnvironment {

  constructor() {
    super(false);
  }
}

export const environment = new Environment();