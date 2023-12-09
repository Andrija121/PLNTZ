import React from "react";
import { NavBarTab } from "./nav-bar-tab";
import { useAuth0 } from "@auth0/auth0-react";

export const NavBarTabs = () => {
  const { isAuthenticated } = useAuth0();
  return (
    //Tbd
    <div className="nav-bar__tabs">
      <NavBarTab path="/profile" label="Profile" />
      {isAuthenticated && (
        <>
          <NavBarTab path="/public" label="Public" />
          <NavBarTab path="/protected" label="Protected" />
          <NavBarTab path="/admin" label="Admin" />
        </>
      )}
    </div>
  );
};
