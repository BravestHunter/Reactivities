import { Grid } from 'semantic-ui-react'
import ProfileHeader from './ProfileHeader'
import ProfileContent from './ProfileContent'
import { observer } from 'mobx-react-lite'
import { useParams } from 'react-router-dom'
import { useMainStore } from '../../app/stores/mainStore'
import { useEffect } from 'react'
import LoadingComponent from '../../app/layout/LoadingComponent'

export default observer(function ProfilePage() {
  const { username } = useParams<{ username: string }>()
  const { profileStore } = useMainStore()
  const { profile, loadingProfile, loadProfile, setActiveTab } = profileStore

  useEffect(() => {
    if (!username) {
      return
    }

    loadProfile(username)

    return () => {
      setActiveTab(0)
    }
  }, [username, loadProfile, setActiveTab])

  if (loadingProfile) {
    return <LoadingComponent content="Loading profile..." />
  }

  return (
    <Grid>
      <Grid.Column width={16}>
        {profile && (
          <>
            <ProfileHeader profile={profile!} />
            <ProfileContent profile={profile} />
          </>
        )}
      </Grid.Column>
    </Grid>
  )
})
