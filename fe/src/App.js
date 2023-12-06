import "./styles/App.css";
import React from "react";
import { Route, Routes } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import { PageLoader } from "./components/page-loader";
import { Profile } from "./pages/profile-page";
import { CallbackPage } from "./pages/callback-page";
import { AuthenticationGuard } from "./components/authentication-guard";
import { HomePage } from "./pages/home-page";

export const App = () => {
  const { isLoading } = useAuth0();

  if (isLoading) {
    return (
      <div className="page-layout">
        <PageLoader />
      </div>
    );
  }
  return (
    <Routes>
      <Route path="/" element={<HomePage />}></Route>
      <Route
        path="/profile"
        element={<AuthenticationGuard component={Profile} />}
      />
      <Route path="/callback" element={<CallbackPage />} />
      {/* <Route path="*" element={<NotFoundPage />} /> */}
      {/* <Route
        path="/protected"
        element={<AuthenticationGuard component={ProtectedPage} />}
      />
      <Route
        path="/admin"
        element={<AuthenticationGuard component={AdminPage} />}
      /> */}
    </Routes>
    //class name could be removed
    // <div className="App">
    //   <header></header>
    //   <LoginButton />
    //   <LogoutButton />
    //   <Profile />
    // </div>
  );
};
