import { Grid, GridColumn } from "semantic-ui-react";
import ProfileHeader from "./ProfileHeader";
import ProfileContent from "./ProfileContent";
import { observer } from "mobx-react-lite";
import { useParams } from "react-router-dom";
import { useStore } from "../../app/stores/store";
import { useEffect } from "react";
import LoadingComponent from "../../app/layout/LoadingComponent";

export default observer(function ProfilePage() {
  const { username } = useParams<{ username: string }>();
  const { profileStore } = useStore();
  const { profile, loadingProfile, loadProfile } = profileStore;

  useEffect(() => {
    if (!username) {
      return;
    }

    loadProfile(username);
  }, [username, loadProfile]);

  if (loadingProfile) {
    return <LoadingComponent content="Loading profile..." />;
  }

  return (
    <Grid>
      <Grid.Column width={16}>
        {profile && <ProfileHeader profile={profile!} />}
        <ProfileContent />
      </Grid.Column>
    </Grid>
  );
});
