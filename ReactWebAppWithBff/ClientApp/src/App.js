import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Orders } from './components/Orders';
import { ProductListingWebComponent } from './components/ProductListingWebComponent';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
            <Route path='/orderlisting' component={Orders} />
            <Route path='/productcatalog' component={ProductListingWebComponent} />
      </Layout>
    );
  }
}
