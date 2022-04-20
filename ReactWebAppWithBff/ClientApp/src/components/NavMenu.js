import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor (props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  render () {
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
          <Container>
            <NavbarBrand tag={Link} to="/">ReactWebAppWithBff</NavbarBrand>
            <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
              <ul className="navbar-nav flex-grow">
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                </NavItem>
                <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/orderlisting">Orders (API request through BFF)</NavLink>
                </NavItem>
                <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/productcatalog">Product catalog (Web Component script through BFF)</NavLink>
                </NavItem>
                <NavItem>
                    <NavLink as="a" href="https://localhost:7135/login?redirecturi=/" className="text-dark">Log in</NavLink>
                </NavItem>
                <NavItem>
                    <NavLink as="a" href="https://localhost:7135/logout?redirecturi=/"className="text-dark">Log out</NavLink>
                </NavItem>
               
              </ul>
            </Collapse>
          </Container>
        </Navbar>
      </header>
    );
  }
}
