import React, { Fragment, useEffect, useState } from "react";
import axios from "axios";
import { Button, Container, Header, List } from "semantic-ui-react";
import { Activity } from "../models/activity";
import NavBar from "./NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";

function App() {
  const [activities, setAtivities] = useState<Activity[]>([]);
  const [selectedActivity, setSelectedActivity] = useState<
    Activity | undefined
  >(undefined);

  useEffect(() => {
    axios
      .get<Activity[]>("http://localhost:5000/api/activities")
      .then((response) => {
        setAtivities(response.data);
      });
  }, []);

  function handleSelectedActivity(id: string) {
    setSelectedActivity(activities.find((a) => a.id === id));
  }

  function cancelSelectedActivity() {
    setSelectedActivity(undefined);
  }

  return (
    <Fragment>
      <NavBar />
      <Container style={{ marginTop: "7rem" }}>
        <ActivityDashboard
          activities={activities}
          selectedActivity={selectedActivity}
          selectActivity={handleSelectedActivity}
          cancelSelectedActivity={cancelSelectedActivity}
        />
      </Container>
    </Fragment>
  );
}

export default App;
