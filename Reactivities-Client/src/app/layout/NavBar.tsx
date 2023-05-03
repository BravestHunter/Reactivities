import React from "react";
import { Button, Container, Menu } from "semantic-ui-react";
import { useStore } from "../stores/store";

export default function NavBar() {
  const { activityStore } = useStore();

  return (
    <Menu inverted fixed="top" className="top-navbar">
      <Container>
        <Menu.Item header>
          <img
            src="/assets/logo.png"
            alt="logo"
            style={{ marginRight: "1rem" }}
          />
          <span>Reactivities</span>
        </Menu.Item>
        <Menu.Item name="Activities"></Menu.Item>
        <Menu.Item onClick={() => activityStore.openActivityForm()}>
          <Button positive content="Create Activity" />
        </Menu.Item>
      </Container>
    </Menu>
  );
}
