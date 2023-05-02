import React from "react";
import { Activity } from "../../../app/models/activity";
import { Button, Card, Icon, Image } from "semantic-ui-react";

interface Props {
  activity: Activity;
  cancelSelectedActivity: () => void;
}

export default function ActivityDetails(props: Props) {
  return (
    <Card fluid>
      <Image
        src={`/assets/categoryImages/${props.activity.category}.jpg`}
        wrapped
        ui={false}
      />
      <Card.Content>
        <Card.Header>Matthew</Card.Header>
        <Card.Meta>
          <span>{props.activity.date}</span>
        </Card.Meta>
        <Card.Description>{props.activity.description}</Card.Description>
      </Card.Content>
      <Card.Content extra>
        <Button.Group widths="2">
          <Button basic color="blue" content="Edit" />
          <Button
            onClick={props.cancelSelectedActivity}
            basic
            color="grey"
            content="Cancel"
          />
        </Button.Group>
      </Card.Content>
    </Card>
  );
}
