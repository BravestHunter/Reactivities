import { createContext, useContext } from 'react'
import ActivityStore from './activityStore'
import ProfileStore from './profileStore'
import CommentStore from './commentsStore'

interface MainStore {
  profileStore: ProfileStore
  activityStore: ActivityStore
  commentStore: CommentStore
}

export const mainStore: MainStore = {
  profileStore: new ProfileStore(),
  activityStore: new ActivityStore(),
  commentStore: new CommentStore(),
}

export const MainStoreContext = createContext(mainStore)

export function useMainStore() {
  return useContext(MainStoreContext)
}
