import { createContext, useContext } from 'react'
import UserStore from './userStore'
import CommonStore from './commonStore'
import ModalStore from './modalStore'

interface GlobalStore {
  userStore: UserStore
  commonStore: CommonStore
  modalStore: ModalStore
}

export const globalStore: GlobalStore = {
  userStore: new UserStore(),
  commonStore: new CommonStore(),
  modalStore: new ModalStore(),
}

export const GlobalStoreContext = createContext(globalStore)

export function useGlobalStore() {
  return useContext(GlobalStoreContext)
}
