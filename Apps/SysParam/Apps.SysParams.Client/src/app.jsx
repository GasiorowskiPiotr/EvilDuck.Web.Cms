import React from 'react';
import Routes from './routes.jsx';
import Router from 'react-router';
import $ from 'jquery';

if(window.location.toString().indexOf('access_token') < 0) {
  var hash = window.location.hash;
  window.location = '/Account/Authorize?client_id=web&response_type=token&state=sysparam&redirect_uri=' + encodeURIComponent(window.location.toString().replace(hash, ''));
} else {

  var hash = window.location.hash;
  var items = hash.split('&');
  var obj = { };
  for(var i = 0; i < items.length; i++) {
    var kv = items[i].split('=');
    obj[kv[0]] = kv[1];
  }

  $.ajaxSetup({
    headers: {
      'Authorization': 'Bearer ' + obj['#access_token']
    }
  });

  console.log('Ajax modified');

  window.location.hash = '';

  Router.run(Routes, Router.HashLocation, (Root) => {
    React.render(<Root/>, document.getElementById('appRoot'));
  });
}
