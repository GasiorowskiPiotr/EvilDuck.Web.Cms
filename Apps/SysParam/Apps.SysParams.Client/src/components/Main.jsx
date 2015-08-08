import React from 'react';
import { RouteHandler } from 'react-router';

export default class Main extends React.Component {
  constructor() {
    super();
    this.state = null;
  }

  render() {
    return (
      <div>
        <RouteHandler/>
      </div>
    );
  }
}
