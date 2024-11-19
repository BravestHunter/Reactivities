//import { useMemo } from 'react'
//import { createMainStore, MainStoreContext } from './mainStore'
//
//export const MainStoreProvider: React.FC<{ children: React.ReactNode }> = ({
//  children,
//}) => {
//  const store = useMemo(() => createMainStore(), [])
//  return (
//    <MainStoreContext.Provider value={store}>
//      {children}
//    </MainStoreContext.Provider>
//  )
//}
