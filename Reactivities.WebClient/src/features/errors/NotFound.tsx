import { Link } from 'react-router-dom'
import { Button, Header, Icon, Segment } from 'semantic-ui-react'
import { useGlobalStore } from '../../app/stores/globalStore'

export default function NotFound() {
  const { userStore } = useGlobalStore()
  const { isLoggedIn } = userStore

  return (
    <Segment placeholder>
      <Header icon>
        <Icon name="search" />
        <span>
          Oops - we've looked everywhere but could not find what you are looking
          for!
        </span>
      </Header>
      <Segment.Inline>
        {isLoggedIn ? (
          <Button as={Link} to="/activities">
            Return to activities page
          </Button>
        ) : (
          <Button as={Link} to="/">
            Return to home page
          </Button>
        )}
      </Segment.Inline>
    </Segment>
  )
}
