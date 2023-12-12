import React from "react";
import { NavBarTab } from "./nav-bar-tab";
import { useAuth0 } from "@auth0/auth0-react";

export const NavBarTabs = () => {
  const { isAuthenticated, user } = useAuth0();
  const userRoles = user?.["/roles"] || [];

  const isAdmin = userRoles.includes("admin");
  //const isClient = userRoles.includes("client");
  return (
    //Tbd
    <div className="nav-bar__tabs">
      <NavBarTab path="/profile" label="Profile" />
      {isAuthenticated && isAdmin && (
        <>
          <NavBarTab path="/users" label="Users" />
          <NavBarTab path="/protected" label="Protected" />
          <NavBarTab path="/admin" label="Admin" />
        </>
      )}
    </div>
  );
};
