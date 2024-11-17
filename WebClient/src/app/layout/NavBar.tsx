import { Button, Container, Dropdown, Image, Menu } from 'semantic-ui-react'
import { Link, NavLink } from 'react-router-dom'
import { observer } from 'mobx-react-lite'
import { useGlobalStore } from '../stores/globalStore'

export default observer(function NavBar() {
  const {
    userStore: { user, logout },
  } = useGlobalStore()

  return (
    <Menu inverted fixed="top" className="top-navbar">
      <Container>
        <Menu.Item as={NavLink} to="/" header>
          <img
            src="/assets/logo.png"
            alt="logo"
            style={{ marginRight: '1rem' }}
          />
          <span>Reactivities</span>
        </Menu.Item>
        <Menu.Item as={NavLink} to="/activities" name="Activities"></Menu.Item>
        <Menu.Item as={NavLink} to="/errors" name="Errors"></Menu.Item>
        <Menu.Item>
          <Button
            as={NavLink}
            to="/createActivity"
            positive
            content="Create Activity"
          />
        </Menu.Item>
        <Menu.Item position="right">
          <Image
            src={user?.profilePhotoUrl || '/assets/user.png'}
            avatar
            spaced="right"
          />
          <Dropdown pointing="top left" text={user?.displayName}>
            <Dropdown.Menu>
              <Dropdown.Item
                as={Link}
                to={`/profiles/${user?.username}`}
                text="Profile"
                icon="user"
              />
              <Dropdown.Item onClick={logout} text="Logout" icon="power" />
            </Dropdown.Menu>
          </Dropdown>
        </Menu.Item>
      </Container>
    </Menu>
  )
})
