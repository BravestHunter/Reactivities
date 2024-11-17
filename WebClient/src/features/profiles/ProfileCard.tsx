import { observer } from 'mobx-react-lite'
import { Link } from 'react-router-dom'
import { Card, Icon, Image } from 'semantic-ui-react'
import FollowButton from './FollowButton'
import ProfileShort from '../../app/models/profileShort'

interface Props {
  profile: ProfileShort
}

export default observer(function ProfileCard(props: Props) {
  const { profile } = props

  function truncate(str: string | undefined) {
    if (str) {
      return str.length > 40 ? str.substring(0, 37) + '...' : str
    }
  }

  return (
    <Card as={Link} to={`/profiles/${profile.username}`}>
      <Image src={profile.profilePhotoUrl || '/assets/user.png'} />
      <Card.Content>
        <Card.Header>{profile.displayName}</Card.Header>
        <Card.Description>{truncate(profile.bio)}</Card.Description>
      </Card.Content>
      <Card.Content extra>
        <Icon name="user" />
        <span>{profile.followersCount} followers</span>
      </Card.Content>
      <FollowButton profile={profile} />
    </Card>
  )
})
