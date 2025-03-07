import { observer } from 'mobx-react-lite'
import { Container, Header, Segment } from 'semantic-ui-react'
import { useGlobalStore } from '../../app/stores/globalStore'

export default observer(function ServerError() {
  const { commonStore } = useGlobalStore()

  return (
    <Container>
      <Header as="h1" content="Server error" />
      <Header sub as="h5" color="red" content={commonStore.error?.message} />
      {commonStore?.error?.details && (
        <Segment>
          <Header as="h4" content="Stack trace" color="teal" />
          <code style={{ marginTop: '10px' }}>{commonStore.error.details}</code>
        </Segment>
      )}
    </Container>
  )
})
