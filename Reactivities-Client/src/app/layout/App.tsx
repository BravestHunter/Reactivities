import { Fragment } from "react";
import { Container } from "semantic-ui-react";
import NavBar from "./NavBar";
import { observer } from "mobx-react-lite";
import { Outlet, useLocation } from "react-router-dom";
import HomePage from "../../features/home/HomePage";
import { ToastContainer } from "react-toastify";

function App() {
  const location = useLocation();

  if (location.pathname === "/") {
    return <HomePage />;
  }

  return (
    <Fragment>
      <ToastContainer position="bottom-right" hideProgressBar theme="colored" />
      <NavBar />
      <Container style={{ marginTop: "7rem" }}>
        <Outlet />
      </Container>
    </Fragment>
  );
}

export default observer(App);
