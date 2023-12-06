import React from "react";
import ReactDOM from "react-dom/client";
import { Auth0Provider, useAuth0 } from "@auth0/auth0-react";
import "./styles/index.css";
import reportWebVitals from "./reportWebVitals";
import { BrowserRouter } from "react-router-dom";
import { App } from "./App";
import { Auth0ProviderWithNavigate } from "./auth0-provider-with-navigate";

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <React.StrictMode>
    <BrowserRouter>
      <Auth0ProviderWithNavigate>
        <App></App>
      </Auth0ProviderWithNavigate>
    </BrowserRouter>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
