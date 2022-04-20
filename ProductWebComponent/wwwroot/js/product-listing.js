class ProductListing extends HTMLElement {
    async connectedCallback() {
        const products = await this.getProducts();
        console.log("Filter was",this.getAttribute("filter"));
        this.renderProducts(products);
    }

    async getProducts() {
        console.log("Fetching products...");
        const res = await fetch("/products")
        const products = await res.json();
        console.log("Products found", products);
        return products;
    }

    async renderProducts(products) {
        this.innerHTML = '<strong>Products Web component</strong>';
        var pre = document.createElement('pre');
        pre.textContent = JSON.stringify(products, 0, 2);
        this.appendChild(pre);
    }
}

customElements.define('product-listing', ProductListing);