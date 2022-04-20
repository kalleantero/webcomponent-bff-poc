import React, { Component } from 'react';

export class Orders extends Component {
    static displayName = Orders.name;

  constructor(props) {
    super(props);
    this.state = { orders: [], loading: true };
  }

  componentDidMount() {
      this.getOrders();
  }

    static renderOrders(orders) {
        return (
            <div>
        <strong>Orders from API</strong>
                <pre>{JSON.stringify(orders, 0, 2)}</pre>
                </div>
    );
  }

    async getOrders() {
        const response = await fetch('orders');
        const data = await response.json();
        this.setState({ orders: data, loading: false });
    }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
        : Orders.renderOrders(this.state.orders);

    return (
      <div>
        <p>This component demonstrates fetching data from the API.</p>
        {contents}
      </div>
    );
  }

 
}
