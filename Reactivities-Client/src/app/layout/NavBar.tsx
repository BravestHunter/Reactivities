import React from "react";
import { Button, Container, Menu } from "semantic-ui-react";

interface Props {
  openForm: () => void;
}

export default function NavBar(props: Props) {
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
        <Menu.Item onClick={props.openForm}>
          <Button positive content="Create Activity" />
        </Menu.Item>
      </Container>
    </Menu>
  );
}
