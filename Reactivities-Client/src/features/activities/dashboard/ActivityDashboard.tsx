import React from "react";
import { Grid } from "semantic-ui-react";
import { Activity } from "../../../app/models/activity";
import ActivityList from "./ActivityList";
import ActivityDetails from "../details/ActivityDetails";
import ActivityForm from "../form/ActivityForm";

interface Props {
  activities: Activity[];
  selectedActivity: Activity | undefined;
  selectActivity: (id: string) => void;
  cancelSelectedActivity: () => void;
  editMode: boolean;
  openForm: (id: string) => void;
  closeForm: () => void;
  createOfUpdate: (activity: Activity) => void;
  delete: (id: string) => void;
  submitting: boolean;
}

export default function ActivityDashboard(props: Props) {
  return (
    <Grid>
      <Grid.Column width="10">
        <ActivityList
          activities={props.activities}
          selectActivity={props.selectActivity}
          delete={props.delete}
          submitting={props.submitting}
        />
      </Grid.Column>
      <Grid.Column width="6">
        {props.selectedActivity && !props.editMode && (
          <ActivityDetails
            activity={props.selectedActivity}
            cancelSelectedActivity={props.cancelSelectedActivity}
            openForm={props.openForm}
          />
        )}
        {props.editMode && (
          <ActivityForm
            closeForm={props.closeForm}
            activity={props.selectedActivity}
            createOfUpdate={props.createOfUpdate}
            submitting={props.submitting}
          />
        )}
      </Grid.Column>
    </Grid>
  );
}
