import { createContext, useContext } from 'react'
import ActivityStore from './activityStore'
import ProfileStore from './profileStore'
import CommentStore from './commentsStore'

interface Store {
  profileStore: ProfileStore
  activityStore: ActivityStore
  commentStore: CommentStore
}

export const store: Store = {
  profileStore: new ProfileStore(),
  activityStore: new ActivityStore(),
  commentStore: new CommentStore(),
}

export const StoreContext = createContext(store)

export function useStore() {
  return useContext(StoreContext)
}
