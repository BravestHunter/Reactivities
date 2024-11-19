import { createContext, useContext } from 'react'
import ActivityStore from './activityStore'
import ProfileStore from './profileStore'
import CommentStore from './commentsStore'

interface MainStore {
  activityStore: ActivityStore
  profileStore: ProfileStore
  commentStore: CommentStore
}

export function createMainStore(): MainStore {
  const activityStore = new ActivityStore()
  return {
    activityStore: activityStore,
    profileStore: new ProfileStore(activityStore),
    commentStore: new CommentStore(activityStore),
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
