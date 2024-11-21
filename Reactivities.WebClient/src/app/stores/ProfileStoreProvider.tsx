import { ReactNode, useMemo } from 'react'
import { observer } from 'mobx-react-lite'
import ProfileStore, { ProfileStoreContext } from './profileStore'
import { useActivityStore } from './activityStore'

export const ProfileStoreProvider: React.FC<{ children: ReactNode }> = observer(
  ({ children }) => {
    const activityStore = useActivityStore()
    const store = useMemo(() => {
      console.log('Created profile store')
      return new ProfileStore(activityStore)
    }, [activityStore])
    return (
      <ProfileStoreContext.Provider value={store}>
        {children}
      </ProfileStoreContext.Provider>
    )
  }
)
