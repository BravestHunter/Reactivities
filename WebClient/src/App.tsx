import { StoreContext, store } from './app/stores/store'
import { RouterProvider } from 'react-router-dom'
import { router } from './app/router/Routes'
import './App.css'
import { globalStore, GlobalStoreContext } from './app/stores/globalStore'

function App() {
  return (
    <>
      <GlobalStoreContext.Provider value={globalStore}>
        <StoreContext.Provider value={store}>
          <RouterProvider router={router} />
        </StoreContext.Provider>
      </GlobalStoreContext.Provider>
    </>
  )
}

export default App
