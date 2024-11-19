import { globalStore, GlobalStoreContext } from './app/stores/globalStore'
import RouterRoot from './app/router/RouterRoot'
import { ToastContainer } from 'react-toastify'
import ModalContainer from './app/common/modals/ModalContainer'

function App() {
  return (
    <>
      <ModalContainer />
      <ToastContainer position="bottom-right" hideProgressBar theme="colored" />

      <GlobalStoreContext.Provider value={globalStore}>
        <RouterRoot />
      </GlobalStoreContext.Provider>
    </>
  )
}

export default App
