import { Button, Header, Image, Item, Label, Segment } from 'semantic-ui-react'
import { Activity } from '../../../app/models/activity'
import { observer } from 'mobx-react-lite'
import { Link } from 'react-router-dom'
import { format } from 'date-fns'
import { useMainStore } from '../../../app/stores/mainStore'

const activityImageStyle = {
  filter: 'brightness(30%)',
}

const activityImageTextStyle = {
  position: 'absolute',
  bottom: '5%',
  left: '5%',
  width: '100%',
  height: 'auto',
  color: 'white',
}

interface Props {
  activity: Activity
}

export default observer(function ActivityDetailedHeader(props: Props) {
  const { activity } = props
  const {
    activityStore: { updateAttendance, updating, cancelActivityToggle },
  } = useMainStore()

  return (
    <Segment.Group>
      <Segment basic attached="top" style={{ padding: '0' }}>
        {activity.isCancelled && (
          <Label
            style={{ position: 'absolute', zIndex: 1000, left: -14, top: 20 }}
            ribbon
            color="red"
            content="Cancelled"
          />
        )}
        <Image
          src={`/assets/categoryImages/${activity.category}.jpg`}
          fluid
          style={activityImageStyle}
        />
        <Segment style={activityImageTextStyle} basic>
          <Item.Group>
            <Item>
              <Item.Content>
                <Header
                  size="huge"
                  content={activity.title}
                  style={{ color: 'white' }}
                />
                <p>{format(activity.date!, 'dd MMM yyyy')}</p>
                <p>
                  Hosted by{' '}
                  <strong>
                    <Link to={`/profiles/${activity.host?.username}`}>
                      {activity.host?.displayName}
                    </Link>
                  </strong>
                </p>
              </Item.Content>
            </Item>
          </Item.Group>
        </Segment>
      </Segment>
      <Segment clearing attached="bottom">
        {activity.isHost ? (
          <>
            <Button
              color={activity.isCancelled ? 'green' : 'red'}
              floated="left"
              basic
              content={activity.isCancelled ? 'Re-activate' : 'Cancel'}
              onClick={cancelActivityToggle}
              loading={updating}
            />
            <Button
              disabled={activity.isCancelled}
              as={Link}
              to={`/manage/${activity.id}`}
              color="orange"
              floated="right"
            >
              Manage Event
            </Button>
          </>
        ) : activity.isGoing ? (
          <Button loading={updating} onClick={updateAttendance}>
            Cancel attendance
          </Button>
        ) : (
          <Button
            disabled={activity.isCancelled}
            loading={updating}
            onClick={updateAttendance}
            color="teal"
          >
            Join Activity
          </Button>
        )}
      </Segment>
    </Segment.Group>
  )
})
