import { RouterProvider } from 'react-router-dom'
import { router } from './app/router/Routes'
import './App.css'
import { globalStore, GlobalStoreContext } from './app/stores/globalStore'

function App() {
  return (
    <>
      <GlobalStoreContext.Provider value={globalStore}>
        <RouterProvider router={router} />
      </GlobalStoreContext.Provider>
    </>
  )
}

export default App
