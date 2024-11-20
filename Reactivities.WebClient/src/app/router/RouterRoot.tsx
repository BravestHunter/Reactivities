import { RouterProvider } from 'react-router-dom'
import { useGlobalStore } from '../stores/globalStore'
import { observer } from 'mobx-react-lite'
import { useEffect } from 'react'
import LoadingComponent from '../layout/LoadingComponent'
import { router } from './Routes'

export default observer(function RouterRoot() {
  const { commonStore, userStore } = useGlobalStore()

  useEffect(() => {
    if (commonStore.token) {
      userStore.getUser().finally(() => commonStore.setAppLoaded())
    } else {
      commonStore.setAppLoaded()
    }
  }, [commonStore, userStore])

  if (!commonStore.appLoaded) {
    return <LoadingComponent content="Loading app..." />
  }

  return <RouterProvider router={router} />
})
