import { createContext, useContext } from 'react'
import ActivityStore from './activityStore'
import ProfileStore from './profileStore'

interface MainStore {
  activityStore: ActivityStore
  profileStore: ProfileStore
}

export function createMainStore(): MainStore {
  const activityStore = new ActivityStore()
  return {
    activityStore: activityStore,
    profileStore: new ProfileStore(activityStore),
  }
}

export const MainStoreContext = createContext<MainStore | undefined>(undefined)

export const useMainStore = (): MainStore => {
  const context = useContext(MainStoreContext)
  if (!context) {
    throw new Error('useMainStore must be used within a MainStoreProvider')
  }
  return context
}
