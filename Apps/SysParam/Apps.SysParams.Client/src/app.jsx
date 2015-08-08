import React from 'react';
import Routes from './routes.jsx';
import Router from 'react-router';
import http from 'http';

http.get({
  path: '/Account/GetToken'
}, (resp) => {
  console.log(resp);
});

Router.run(Routes, Router.HashLocation, (Root) => {
  React.render(<Root/>, document.getElementById('appRoot'));
});
