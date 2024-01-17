import { useAuth0 } from "@auth0/auth0-react";
import React, { useEffect } from "react";
import { PageLayout } from "../components/page-layout";
import { getAdminResource } from "../services/message.service";

export const AdminPage = () => {
  const { getAccessTokenSilently } = useAuth0();

  useEffect(() => {
    let isMounted = true;

    const getMessage = async () => {
      const accessToken = await getAccessTokenSilently();
      const { data, error } = await getAdminResource(accessToken);

      if (!isMounted) {
        return;
      }

      if (data) {
        return;
      }

      if (error) {
        return;
      }
    };

    getMessage();

    return () => {
      isMounted = false;
    };
  }, [getAccessTokenSilently]);

  return (
    <PageLayout>
      <div className="content-layout">
        <h1 id="page-title" className="content__title">
          Admin Page
        </h1>
        <div className="content__body">
          <p id="page-description">
            <span>
              This confirms that only Admin can retrieve this page and an{" "}
              <strong>admin message</strong> from an external API.
            </span>
          </p>
          <code> Hi you are an Admin, welcome back !</code>
        </div>
      </div>
    </PageLayout>
  );
};
