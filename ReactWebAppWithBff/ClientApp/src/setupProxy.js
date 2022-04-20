const createProxyMiddleware = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:30207';

const context =  [
    "/orders",
    "/products",
    "/product-listing-webcomponent-proxy",
    "/signin-oidc",
    "/login?redirecturi=/",
    "/logout?redirecturi=/",
    "/signout-callback-oidc",
    "/js-proxy-productlisting"
];

module.exports = function(app) {
  const appProxy = createProxyMiddleware(context, {
    target: target,
    secure: false
  });

  app.use(appProxy);
};
