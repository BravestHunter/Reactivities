import React from "react";
import { Fragment, useEffect, useState } from "react";
import { Container } from "semantic-ui-react";
import { Activity } from "../models/activity";
import NavBar from "./NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import { v4 as uuid } from "uuid";
import agent from "../api/agent";

function App() {
  const [activities, setAtivities] = useState<Activity[]>([]);
  const [selectedActivity, setSelectedActivity] = useState<
    Activity | undefined
  >(undefined);
  const [editMode, setEditMode] = useState(false);

  useEffect(() => {
    agent.Activities.list().then((activities) => {
      activities = activities.map<Activity>((a) => {
        a.date = a.date.split("T")[0];
        return a;
      });

      setAtivities(activities);
    });
  }, []);

  function handleSelectedActivity(id: string) {
    setSelectedActivity(activities.find((a) => a.id === id));
  }

  function cancelSelectedActivity() {
    setSelectedActivity(undefined);
  }

  function handleFormOpen(id?: string) {
    id ? handleSelectedActivity(id) : cancelSelectedActivity();
    setEditMode(true);
  }

  function handleFormClose() {
    setEditMode(false);
  }

  function handleCreateOfUpdateActivity(activity: Activity) {
    if (activity.id) {
      setAtivities([
        ...activities.filter((a) => a.id !== activity.id),
        activity,
      ]);
    } else {
      setAtivities([...activities, { ...activity, id: uuid() }]);
    }
    setEditMode(false);
    setSelectedActivity(activity);
  }

  function handleDeleteActivity(id: string) {
    setAtivities([...activities.filter((a) => a.id !== id)]);

    if (selectedActivity && selectedActivity.id === id) {
      setSelectedActivity(undefined);
    }
  }

  return (
    <Fragment>
      <NavBar openForm={handleFormOpen} />
      <Container style={{ marginTop: "7rem" }}>
        <ActivityDashboard
          activities={activities}
          selectedActivity={selectedActivity}
          selectActivity={handleSelectedActivity}
          cancelSelectedActivity={cancelSelectedActivity}
          editMode={editMode}
          openForm={handleFormOpen}
          closeForm={handleFormClose}
          createOfUpdate={handleCreateOfUpdateActivity}
          delete={handleDeleteActivity}
        />
      </Container>
    </Fragment>
  );
}

export default App;
