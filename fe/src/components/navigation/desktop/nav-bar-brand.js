import React from "react";
import { NavLink } from "react-router-dom";
import Logo from "../../../images/plntz-logo.png";

export const NavBarBrand = () => {
  return (
    <div className="nav-bar__brand">
      <NavLink to="/">
        <img
          className="nav-bar__logo"
          src={Logo}
          alt="Planets logo"
          width="85"
        />
      </NavLink>
    </div>
  );
};
