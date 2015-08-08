import React from 'react';
import { Route, DefaultRoute } from 'react-router';
import Home from './components/Home.jsx';
import Main from './components/Main.jsx';
import Add from './components/Add.jsx';

var routes = (
  <Route path="/" handler={Main}>
    <DefaultRoute handler={Home} name="Home" />
    <Route name="add" handler={Add} />
  </Route>
);

module.exports = routes;
