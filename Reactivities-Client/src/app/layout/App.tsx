import React from "react";
import { Fragment, useEffect, useState } from "react";
import { Container } from "semantic-ui-react";
import { Activity } from "../models/activity";
import NavBar from "./NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import { v4 as uuid } from "uuid";
import agent from "../api/agent";
import LoadingComponent from "./LoadingComponent";

function App() {
  const [activities, setAtivities] = useState<Activity[]>([]);
  const [selectedActivity, setSelectedActivity] = useState<
    Activity | undefined
  >(undefined);
  const [editMode, setEditMode] = useState(false);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    agent.Activities.list().then((activities) => {
      activities = activities.map<Activity>((a) => {
        a.date = a.date.split("T")[0];
        return a;
      });

      setAtivities(activities);
      setLoading(false);
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

  async function handleCreateOfUpdateActivity(activity: Activity) {
    setSubmitting(true);
    if (activity.id) {
      await agent.Activities.update(activity);

      setAtivities([
        ...activities.filter((a) => a.id !== activity.id),
        activity,
      ]);
    } else {
      activity.id = uuid();

      await agent.Activities.create(activity);

      setAtivities([...activities, { ...activity, id: uuid() }]);
    }

    setEditMode(false);
    setSelectedActivity(activity);
    setSubmitting(false);
  }

  async function handleDeleteActivity(id: string) {
    setSubmitting(true);

    await agent.Activities.delete(id);

    setAtivities([...activities.filter((a) => a.id !== id)]);

    if (selectedActivity && selectedActivity.id === id) {
      setSelectedActivity(undefined);
    }

    setSubmitting(false);
  }

  if (loading) {
    return <LoadingComponent content="Loading app" />;
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
          submitting={submitting}
        />
      </Container>
    </Fragment>
  );
}

export default App;
