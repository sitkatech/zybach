// export const environment = {
//   production: true,
//   staging: false,
//   dev: false,
//   apiHostName: 'api-rrbwatertrading.sitkatech.com',
//   createAccountUrl: "https://keystone.sitkatech.com/Authentication/Register?RedirectUrl=",
//   createAccountRedirectUrl: "https://waterbudget.rrbwsd.com/create-user-callback",
//   allowTrading: true,

//   keystoneSupportBaseUrl: "https://keystone.sitkatech.com/Authentication",

//   keystoneAuthConfiguration: {
//     clientId: 'Fresca',
//     issuer: 'https://keystone.sitkatech.com/core',
//     redirectUri: window.location.origin + '/login-callback',
//     scope: 'openid all_claims keystone',
//     sessionChecksEnabled: true,
//     logoutUrl: 'https://keystone.sitkatech.com/core/connect/endsession',
//     postLogoutRedirectUri: window.location.origin + '/',
//     waitForTokenInMsec: 500
//   },
//   geoserverMapServiceUrl: 'https://fresca-geoserver.yachats.sitkatech.com/geoserver/Fresca'
// };


import { DynamicEnvironment } from './dynamic-environment';
class Environment extends DynamicEnvironment {

  constructor() {
    super(true);
  }
}

export const environment = new Environment();