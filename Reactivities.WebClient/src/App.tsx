import { globalStore, GlobalStoreContext } from './app/stores/globalStore'
import RouterRoot from './app/router/RouterRoot'
import { ToastContainer } from 'react-toastify'
import ModalContainer from './app/common/modals/ModalContainer'
import { ActivityStoreProvider } from './app/stores/ActivityStoreProvider'

function App() {
  return (
    <>
      <ModalContainer />
      <ToastContainer position="bottom-right" hideProgressBar theme="colored" />

      <GlobalStoreContext.Provider value={globalStore}>
        <ActivityStoreProvider>
          <RouterRoot />
        </ActivityStoreProvider>
      </GlobalStoreContext.Provider>
    </>
  )
}

export default App
