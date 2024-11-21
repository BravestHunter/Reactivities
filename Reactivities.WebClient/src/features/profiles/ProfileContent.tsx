import { Tab } from 'semantic-ui-react'
import ProfilePhotos from './ProfilePhotos'
import { Profile } from '../../app/models/profile'
import { observer } from 'mobx-react-lite'
import ProfileAbout from './ProfileAbout'
import ProfileFollowings from './ProfileFollowings'
import ProfileActivities from './ProfileActivities'
import { useProfileStore } from '../../app/stores/profileStore'

interface Props {
  profile: Profile
}

export default observer(function ProfileContent(props: Props) {
  const { profile } = props
  const profileStore = useProfileStore()

  const panes = [
    { menuItem: 'About', render: () => <ProfileAbout /> },
    { menuItem: 'Photos', render: () => <ProfilePhotos profile={profile} /> },
    { menuItem: 'Events', render: () => <ProfileActivities /> },
    {
      menuItem: 'Followers',
      render: () => <ProfileFollowings />,
    },
    {
      menuItem: 'Following',
      render: () => <ProfileFollowings />,
    },
  ]

  return (
    <Tab
      menu={{ fluid: true, vertical: true }}
      menuPosition="right"
      panes={panes}
      onTabChange={(_, data) => profileStore.setActiveTab(data.activeIndex!)}
    />
  )
})