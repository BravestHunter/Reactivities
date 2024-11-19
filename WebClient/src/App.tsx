import { globalStore, GlobalStoreContext } from './app/stores/globalStore'
import RouterRoot from './app/router/RouterRoot'
import { ToastContainer } from 'react-toastify'
import ModalContainer from './app/common/modals/ModalContainer'
import { MainStoreProvider } from './app/stores/MainStoreProvider'

function App() {
  return (
    <>
      <ModalContainer />
      <ToastContainer position="bottom-right" hideProgressBar theme="colored" />

      <GlobalStoreContext.Provider value={globalStore}>
        <MainStoreProvider>
          <RouterRoot />
        </MainStoreProvider>
      </GlobalStoreContext.Provider>
    </>
  )
}

export default App
