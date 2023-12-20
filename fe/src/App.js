import React from "react";
import { Route, Routes } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import { PageLoader } from "./components/page-loader";
import { Profile } from "./pages/profile-page";
import { CallbackPage } from "./pages/callback-page";
import { AuthenticationGuard } from "./components/authentication-guard";
import { HomePage } from "./pages/home-page";
import { AdminPage } from "./pages/admin-page";
import { ProtectedPage } from "./pages/protected-page";
import { UserServicePage } from "./pages/user-service.page";
import { NotFoundPage } from "./pages/not-found-page";

export const App = () => {
  const { isLoading } = useAuth0();

  if (isLoading) {
    return (
      <div className="page-layout">
        <PageLoader />
      </div>
    );
  }
  const adminRequiredRoles = ["admin"];
  //const protectedRequiredRoles = ["admin", "client"];
  return (
    <Routes>
      <Route path="/" element={<HomePage />}></Route>
      <Route
        path="/users"
        element={
          <AuthenticationGuard
            component={UserServicePage}
            requiredRoles={adminRequiredRoles}
          />
        }
      ></Route>
      <Route
        path="/profile"
        element={<AuthenticationGuard component={Profile} requiredRoles={[]} />}
      />
      <Route path="/callback" element={<CallbackPage />} />
      <Route path="*" element={<NotFoundPage />} />
      <Route
        path="/protected"
        element={
          <AuthenticationGuard
            component={ProtectedPage}
            requiredRoles={adminRequiredRoles}
          />
        }
      />
      <Route
        path="/admin"
        element={
          <AuthenticationGuard
            component={AdminPage}
            requiredRoles={adminRequiredRoles}
          />
        }
      />
    </Routes>
  );
};
