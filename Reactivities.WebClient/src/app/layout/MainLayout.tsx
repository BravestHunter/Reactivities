import { Container } from 'semantic-ui-react'
import NavBar from './NavBar'
import { observer } from 'mobx-react-lite'
import { Outlet, ScrollRestoration } from 'react-router-dom'
import './styles.css'

function MainLayout() {
  return (
    <>
      <ScrollRestoration />

      <NavBar />
      <Container style={{ marginTop: '7rem' }}>
        <Outlet />
      </Container>
    </>
  )
}

export default observer(MainLayout)
