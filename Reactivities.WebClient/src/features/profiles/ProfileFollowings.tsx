import { observer } from 'mobx-react-lite'
import { Card, Grid, Header, Tab } from 'semantic-ui-react'
import ProfileCard from './ProfileCard'
import { useProfileStore } from '../../app/stores/profileStore'

export default observer(function ProfileFollowings() {
  const profileStore = useProfileStore()
  const { profile, followings, loadingFollowings, activeTab } = profileStore

  return (
    <Tab.Pane loading={loadingFollowings}>
      <Grid>
        <Grid.Column width={16}>
          <Header
            floated="left"
            icon="user"
            content={
              activeTab === 3
                ? `People following ${profile?.displayName}`
                : `People ${profile?.displayName} is following`
            }
          />
        </Grid.Column>
        <Grid.Column width={16}>
          <Card.Group itemsPerRow={4}>
            {followings.map((profile) => (
              <ProfileCard key={profile.username} profile={profile} />
            ))}
          </Card.Group>
        </Grid.Column>
      </Grid>
    </Tab.Pane>
  )
})
