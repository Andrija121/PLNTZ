import { withAuthenticationRequired } from "@auth0/auth0-react";
import React from "react";
import { PageLoader } from "./page-loader";
import { useAuth0 } from "@auth0/auth0-react";
import { Navigate } from "react-router-dom";

export const AuthenticationGuard = ({ component, requiredRoles }) => {
  const { user } = useAuth0();
  const userRoles = user?.["/roles"] || [];
  const hasRequiredRoles = requiredRoles.every((role) =>
    userRoles.includes(role)
  );
  if (requiredRoles.length > 0 && !hasRequiredRoles) {
    // Redirect or render an error page
    return <Navigate to="/4042" />;
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
