import { SyntheticEvent } from 'react'
import { Button, Reveal } from 'semantic-ui-react'
import { observer } from 'mobx-react-lite'
import { useMainStore } from '../../app/stores/mainStore'
import { useGlobalStore } from '../../app/stores/globalStore'
import { ProfileShort } from '../../app/models/profileShort'

interface Props {
  profile: ProfileShort
}

export default observer(function FollowButton(props: Props) {
  const { profile } = props
  const { userStore } = useGlobalStore()
  const { profileStore } = useMainStore()
  const { updateFollowing, loading } = profileStore

  if (userStore.user?.username === profile.username) {
    return null
  }

  function handleFollow(e: SyntheticEvent, username: string) {
    e.preventDefault()
    updateFollowing(username, !profile.following)
  }

  return (
    <Reveal animated="move">
      <Reveal.Content visible style={{ width: '100%' }}>
        <Button
          fluid
          color="teal"
          content={profile.following ? 'Following' : 'Not following'}
        />
      </Reveal.Content>
      <Reveal.Content hidden style={{ width: '100%' }}>
        <Button
          fluid
          basic
          color={profile.following ? 'red' : 'green'}
          content={profile.following ? 'Unfollow' : 'Follow'}
          loading={loading}
          onClick={(e) => handleFollow(e, profile.username)}
        />
      </Reveal.Content>
    </Reveal>
  )
})
