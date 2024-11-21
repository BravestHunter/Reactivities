import { ReactNode, useMemo } from 'react'
import { useGlobalStore } from './globalStore'
import { observer } from 'mobx-react-lite'
import ActivityStore, { ActivityStoreContext } from './activityStore'

export const ActivityStoreProvider: React.FC<{ children: ReactNode }> =
  observer(({ children }) => {
    const { userStore } = useGlobalStore()
    const { user } = userStore
    const store = useMemo(() => new ActivityStore(), [user])
    return (
      <ActivityStoreContext.Provider value={store}>
        {children}
      </ActivityStoreContext.Provider>
    )
  })
