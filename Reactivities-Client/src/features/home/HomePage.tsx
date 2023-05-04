import { Link } from "react-router-dom";
import { Container } from "semantic-ui-react";

export default function HomePage() {
  return (
    <Container style={{ marginTop: "7rem" }}>
      <h1>Home Page</h1>
      <h3>
        Go to <Link to="/activities">activities</Link>
      </h3>
    </Container>
  );
}