const { createProxyMiddleware } = require('http-proxy-middleware');

const target = process.env.REACT_APP_BASE_API_URL?.replace(/\/api$/, '') || 'http://localhost:5000';

module.exports = function(app) {
  app.use(
    '/assets',
    createProxyMiddleware({
      target: target,
      changeOrigin: true,
    })
  );
};