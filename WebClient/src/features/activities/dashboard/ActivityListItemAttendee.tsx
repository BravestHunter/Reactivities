import { observer } from 'mobx-react-lite'
import { Image, List, Popup } from 'semantic-ui-react'
import { Link } from 'react-router-dom'
import ProfileCard from '../../profiles/ProfileCard'
import Attendee from '../../../app/models/profileShort'

interface Props {
  attendees: Attendee[]
}

export default observer(function ActivityListItemAttendee(props: Props) {
  const { attendees } = props

  const styles = {
    borderColor: 'orange',
    borderWidth: 2,
  }

  return (
    <List horizontal>
      {attendees.map((attendee) => (
        <Popup
          key={attendee.username}
          hoverable
          trigger={
            <List.Item
              key={attendee.username}
              as={Link}
              to={`/profiles/${attendee.username}`}
            >
              <Image
                size="mini"
                circular
                src={attendee.profilePhotoUrl || '/assets/user.png'}
                bordered
                style={attendee.following ? styles : null}
              />
            </List.Item>
          }
        >
          <Popup.Content>
            <ProfileCard profile={attendee} />
          </Popup.Content>
        </Popup>
      ))}
    </List>
  )
})
