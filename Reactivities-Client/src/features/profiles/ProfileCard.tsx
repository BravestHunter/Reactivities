import { observer } from "mobx-react-lite";
import React from "react";
import { Profile } from "../../app/models/profile";
import { Link } from "react-router-dom";
import { Card, Icon, Image } from "semantic-ui-react";

interface Props {
  profile: Profile;
}

export default observer(function ProfileCard(props: Props) {
  const { profile } = props;

  return (
    <Card as={Link} to={`/profiles/${profile.username}`}>
      <Image src={profile.image || "/assets/user.png"} />
      <Card.Content>
        <Card.Header>{profile.displayName}</Card.Header>
        <Card.Description>Bio goes here</Card.Description>
      </Card.Content>
      <Card.Content extra>
        <Icon name="user" />
        <span>20 followers</span>
      </Card.Content>
    </Card>
  );
});