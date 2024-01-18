import { withAuthenticationRequired } from "@auth0/auth0-react";
import React from "react";
import { PageLoader } from "./page-loader";
import { useAuth0 } from "@auth0/auth0-react";
//import { Route } from "react-router-dom";
import { NotFoundPage } from "../pages/not-found-page";

export const AuthenticationGuard = ({ component, requiredRoles }) => {
  const { user } = useAuth0();
  const userRoles = user?.["/roles"] || [];

  if (
    requiredRoles.length > 0 &&
    !requiredRoles.every((role) => userRoles.includes(role))
  ) {
    // Redirect users without the required roles to the NotFoundPage
    return <NotFoundPage />;
  }
  const Component = withAuthenticationRequired(component, {
    onRedirecting: () => (
      <div className="page-layout">
        <PageLoader />
      </div>
    ),
  });

  return <Component />;
};
