import { useAuth0 } from "@auth0/auth0-react";
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { PageLayout } from "../components/page-layout";
import { getProtectedResource } from "../services/message.service";
import { getUserWithAuthZId } from "../services/user.service";

export const LocationPage = () => {
  const [loading, setLoading] = useState(true);
  const { user, getAccessTokenSilently } = useAuth0();
  const navigate = useNavigate();

  useEffect(() => {
    let isMounted = true;

    const getLocationAndMessage = async () => {
      try {
        setLoading(true);

        const accessToken = await getAccessTokenSilently();

        // Fetch user data
        const { data: userData, error: userError } = await getUserWithAuthZId(
          accessToken,
          user.sub.replace(/^auth0\|/, "")
        );

        console.log("userData:", userData);
        console.log("userError:", userError);

        // Check if location is not set, redirect to profile page
        if (userData.city == null || userData.country == null) {
          console.error("Error fetching user data:", userError);
          setTimeout(() => {
            navigate("/profile"); // Redirect after a short delay
          }, 3500); // Adjust the delay as needed
          return;
        }

        // Fetch protected resource (message)
        const { error } = await getProtectedResource(accessToken);

        if (!isMounted) {
          return;
        }

        if (error) {
          // Display a message to set the location on the profile page
          setLoading(false);
        }
      } catch {
        console.log("error");
      }
    };

    getLocationAndMessage();

    return () => {
      isMounted = false;
    };
  }, [getAccessTokenSilently, navigate, user.sub]);

  return (
    <PageLayout>
      <div className="content-layout">
        <h1 id="page-title" className="content__title">
          Location page
        </h1>
        <div className="content__body">
          <p id="page-description">
            {loading
              ? "Loading..."
              : "You have set your location successfully ."}
          </p>
        </div>
      </div>
    </PageLayout>
  );
};
