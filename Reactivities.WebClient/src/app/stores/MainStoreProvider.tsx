import { ReactNode, useMemo } from 'react'
import { createMainStore, MainStoreContext } from './mainStore'
import { useGlobalStore } from './globalStore'
import { observer } from 'mobx-react-lite'

export const MainStoreProvider: React.FC<{ children: ReactNode }> = observer(
  ({ children }) => {
    const { userStore } = useGlobalStore()
    const { user } = userStore
    const store = useMemo(() => createMainStore(), [user])
    return (
      <MainStoreContext.Provider value={store}>
        {children}
      </MainStoreContext.Provider>
    )
  }
)
