import React, { Component } from 'react';

export class ProductListingWebComponent extends Component {

    constructor(props) {
        super(props);
        this.state = { loading: true };
    }

    componentDidMount() {
        var webComponentScript = document.getElementById("webcomponent")

        if (webComponentScript === null) {
            const script = document.createElement("script");
            script.id = "webcomponent";
            script.src = "/js-proxy-productlisting";
            script.async = true;
            script.onload = () => this.scriptLoaded();
            document.body.appendChild(script);
        }

        this.setState({ loading: false });
    }

    scriptLoaded() {
        console.log("External web component script loaded.");
    }

    renderExternalWebComponent() {
        return (
            <div>
                <product-listing id="product-listing" filter="latest" />
            </div>
        );
    }

    render() {

        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderExternalWebComponent();

        return (
            <div>
                <p>This component demonstrates rendering external web component.</p>
                {contents}
            </div>
        );
  }
}
