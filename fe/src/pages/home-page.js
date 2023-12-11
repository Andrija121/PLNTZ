import React from "react";
import { PageLayout } from "../components/page-layout";
import Typed from "typed.js";

export const HomePage = () => {
  const el = React.useRef(null);
  React.useEffect(() => {
    const typed = new Typed(el.current, {
      strings: [
        "Welcome to <strong >Planets</strong>.",
        "Welcome to <strong class='colored'>PLNTZ</strong>.",
        "Welcome to <strong class='colored2'>Planetz</strong>.",
        "Solution created \n to <strong class='colored3'>connect people</strong>.",
      ],
      typeSpeed: 60,
      backSpeed: 50,
      onComplete: () => {
        setTimeout(() => {
          typed.stop();
          typed.reset();
          typed.start();
        }, 5000);
      },
    });
    return () => {
      typed.destroy();
    };
  }, []);
  return (
    <PageLayout>
      <div className="container">
        <h1>
          <span className="walkingText" ref={el}></span>
        </h1>
      </div>
    </PageLayout>
  );
};
