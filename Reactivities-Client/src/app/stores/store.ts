import { createContext, useContext } from "react";
import ActivityStore from "./activityStore";
import CommonStore from "./commonStore";
import UserStore from "./userStore";
import ModalStore from "./modalStore";

interface Store {
  userStore: UserStore;
  commonStore: CommonStore;
  activityStore: ActivityStore;
  modalStore: ModalStore;
}

export const store: Store = {
  userStore: new UserStore(),
  commonStore: new CommonStore(),
  activityStore: new ActivityStore(),
  modalStore: new ModalStore(),
};

export const StoreContext = createContext(store);

export function useStore() {
  return useContext(StoreContext);
}
